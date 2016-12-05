using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AnalyseVisitorsTool.Abstract;
using AnalyseVisitorsTool.Data.Enums;
using AnalyseVisitorsTool.Models;
using Newtonsoft.Json;

namespace AnalyseVisitorsTool.Services
{
    public class ServerLogService : IServerLogService
    {
        private List<IPLocation> locationsduringruntime;
        private IServerLogRepository _serverlogrepository;
        private IIPLocationRepository _iplocationrepository;
        private ISettingsRepository _settingsrepository;
        private string apikey;

        public ServerLogService(IServerLogRepository serverlogrepository, IIPLocationRepository iplocationrepository,
                                ISettingsRepository settingsrepository)
        {
            this.locationsduringruntime = new List<IPLocation>();
            this._serverlogrepository = serverlogrepository;
            this._iplocationrepository = iplocationrepository;
            this._settingsrepository = settingsrepository;
            this.apikey = this._settingsrepository.GetAll().OrderByDescending(s => s.ID).First().IPLocationAPIKey;
        }

        public async Task SetLocation(ServerLogFile file)
        {
            bool locationalreadyset = false;
            var iplocation = new IPLocation();
            if (this.locationsduringruntime.Where(l => l.ipAddress.Equals(file.ClientIP)).Count() > 0) {
                iplocation = this.locationsduringruntime.Where(l => l.ipAddress.Equals(file.ClientIP)).First();
                locationalreadyset = true;
            }
            if (!locationalreadyset) {
                if (this._iplocationrepository.Find(l => l.ipAddress.Equals(file.ClientIP)).Count() > 0) {
                    iplocation = this._iplocationrepository.Find(l => l.ipAddress.Equals(file.ClientIP)).First();
                    locationalreadyset = true;
                }
                if (!locationalreadyset && this._serverlogrepository.IsIPAdressAlreadyComplete(file.ClientIP)) {
                    iplocation = this._serverlogrepository.GetIPLocationOfIPAddress(file.ClientIP);
                    locationalreadyset = true;
                }
            }
            if (!file.ClientIP.Contains(":") && !file.ClientIP.StartsWith("192.168") && !file.ClientIP.StartsWith("127.0.0.1") && !locationalreadyset) {
                var client = new HttpClient();
                try
                {
                    client.BaseAddress = new Uri("http://api.ipinfodb.com");
                    var response = await client.GetAsync(string.Format("/v3/ip-city/?key={0}&ip={1}&format=json", apikey, file.ClientIP));
                    response.EnsureSuccessStatusCode();
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    iplocation = JsonConvert.DeserializeObject<IPLocation>(stringResponse);
                    this.locationsduringruntime.Add(iplocation);
                    this._iplocationrepository.Add(iplocation);
                }
                catch (System.Net.Http.HttpRequestException ex) {
                    Debug.WriteLine(ex.Message + " Can not get IP: " + file.ClientIP);
                }
                catch (System.Exception ex)
                {  
                    Debug.WriteLine(ex.Message);
                }
            }
            file.City = iplocation.cityName ?? "";
            file.Country = iplocation.countryName ?? "";
            file.Longitude = iplocation.longitude ?? "";
            file.Latitude = iplocation.latitude ?? "";
            if (!this._serverlogrepository.DoesEntryAlreadyExist(file)) {
                this._serverlogrepository.Add(file);
            }
        }

        public void GetServerLogFiles(string path)
        {
            var filenames = Directory.GetFiles(path);
            var logfiles = new List<ServerLogFile>();
            try {
                foreach (var filename in filenames) {
                    var files = File.ReadAllLines(filename);
                    foreach (var file in files) {
                        if (!file.StartsWith("#")) {
                            var logfile = new ServerLogFile();
                            var splittedfile = file.Split(' ');
                            if (!splittedfile[8].Contains(":") && !splittedfile[8].StartsWith("192.168") && !splittedfile[8].StartsWith("127.0.0.1")) {
                                logfile.TimeStamp = Convert.ToDateTime(splittedfile[0] + " " + splittedfile[1]);
                                logfile.ServerIP = splittedfile[2];
                                logfile.Method = splittedfile[3];
                                logfile.UriQuery = splittedfile[4];
                                logfile.UriStem = splittedfile[5];
                                logfile.ServerPort = splittedfile[6];
                                logfile.Username = splittedfile[7];
                                logfile.ClientIP = splittedfile[8];
                                logfile.ClientUserAgent = splittedfile[9];
                                logfile.Status = splittedfile[10];
                                logfile.SubStatus = splittedfile[11];
                                logfile.Win32Status = splittedfile[12];
                                logfile.TimeTaken = splittedfile[13];
                                if (this._serverlogrepository.Find(l => 
                                                                    l.ClientIP.Equals(logfile.ClientIP) 
                                                                    && l.ClientUserAgent.Equals(logfile.ClientUserAgent)
                                                                    && l.TimeStamp.Equals(logfile.TimeStamp)
                                                                    && l.Method.Equals(logfile.Method)
                                                                    && l.TimeTaken.Equals(logfile.TimeTaken)
                                                                    && l.UriQuery.Equals(logfile.UriQuery)
                                                                    && l.Status.Equals(logfile.Status)
                                                                    && l.SubStatus.Equals(logfile.SubStatus))
                                                                    .Count() == 0) {
                                    this.SetLocation(logfile).Wait();
                                }
                            }
                        }
                    }
                }
            } catch {
            }
        }

        public void UpdateMissingLocations(List<ServerLogFile> logs)
        {
            foreach (var log in logs) {
                this._serverlogrepository.Remove(log);
                this.SetLocation(log).Wait();
            }
            this._serverlogrepository.Save();
        }

        public void BuildServerLogDatabaseEntries()
        {
            var logsfolder = string.Empty;
            if (this._settingsrepository.GetAll().Count() > 0) {
                logsfolder = this._settingsrepository.GetAll().OrderByDescending(l => l.ID).First().ServerLogFilesFolder;
            }
            if (Directory.Exists(logsfolder)) {
                var folders = Directory.GetDirectories(logsfolder);
                foreach (var folder in folders) {
                    this.GetServerLogFiles(folder);
                }
                this._serverlogrepository.Save();
                this._iplocationrepository.Save();
            }
        }

        public IEnumerable<ServerLogFile> SetLocationIDs(IEnumerable<ServerLogFile> files) 
        {
            foreach (var file in files) {
                if (this._iplocationrepository.Find(l => l.ipAddress.Equals(file.ClientIP)).Count() > 0) {
                    file.LocationID = this._iplocationrepository.Get(l => l.ipAddress.Equals(file.ClientIP)).ID;
                } else {
                    file.LocationID = -1;
                }
            }
            return files;
        }

        public IEnumerable<ServerLogFile> GetMostVisits(VisitType visittype = VisitType.IPAddress) 
        {
            var logfiles = this._serverlogrepository.GetAll();
            var iplocations = this._iplocationrepository.GetAll();
            var results = new List<ServerLogFile>();
            switch (visittype) {
                case VisitType.IPAddress:
                    results =   (from l in logfiles
                        join i in iplocations
                        on l.ClientIP equals i.ipAddress
                        into g
                        select l).ToList();
                    results =   (from l in results
                                group l by l.ClientIP
                                into g
                                select new ServerLogFile
                                {
                                    ID = g.First().ID, ClientIP = g.First().ClientIP, City = g.First().City, 
                                    Country = g.First().Country, TimeStamp = g.OrderByDescending(l => l.TimeStamp).First().TimeStamp, Visits = g.Count()
                                }).OrderByDescending(l => l.Visits).ToList();
                    break;
                case VisitType.City:
                    results =   (from l in logfiles
                        join i in iplocations
                        on l.City equals i.cityName
                        into g
                        select l).ToList();
                    results =   (from l in results
                                group l by l.City
                                into g
                                select new ServerLogFile
                                {
                                    ID = g.First().ID, ClientIP = g.First().ClientIP, City = g.First().City, 
                                    Country = g.First().Country, TimeStamp = g.OrderByDescending(l => l.TimeStamp).First().TimeStamp, Visits = g.Count()
                                }).OrderByDescending(l => l.Visits).ToList();
                    break;
                case VisitType.Country:
                    results =   (from l in logfiles
                        join i in iplocations
                        on l.Country equals i.countryName
                        into g
                        select l).ToList();
                    results =   (from l in results
                                group l by l.Country
                                into g
                                select new ServerLogFile
                                {
                                    ID = g.OrderByDescending(l => l.Visits).First().ID, ClientIP = g.First().ClientIP, City = g.OrderByDescending(l => l.Visits).First().City, 
                                    Country = g.First().Country, TimeStamp = g.OrderByDescending(l => l.TimeStamp).First().TimeStamp, Visits = g.Count()
                                }).OrderByDescending(l => l.Visits).ToList();
                    break;
                case VisitType.Date:
                    results =   (from l in logfiles
                                group l by l.TimeStamp.ToString("dd.MM.yyyy")
                                into g
                                select new ServerLogFile
                                {
                                    ID = g.OrderByDescending(l => l.Visits).First().ID, ClientIP = g.OrderByDescending(l => l.Visits).First().ClientIP, City = g.OrderByDescending(l => l.Visits).First().City, 
                                    Country = g.OrderByDescending(l => l.Visits).First().Country, TimeStamp = g.OrderByDescending(l => l.Visits).First().TimeStamp, Visits = g.Count()
                                }).OrderByDescending(l => l.Visits).ToList();
                    break;
            }
            return results;
        }
    }
}
