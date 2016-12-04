using AnalyseVisitorsTool.Data;
using AnalyseVisitorsTool.Models;
using AnalyseVisitorsTool.Abstract;
using System.Linq;

namespace AnalyseVisitorsTool.Repositories
{
    public class ServerLogRepository : Repository<ServerLogFile>, IServerLogRepository
    {
        public ServerLogRepository(LogDbContext context) : base(context)
        {
        }

        public bool DoesEntryAlreadyExist(ServerLogFile file)
        {
            bool alreadyexists = false;
            alreadyexists = this.context.Set<ServerLogFile>().Where(l => l.TimeStamp.Equals(file.TimeStamp) && l.TimeTaken.Equals(file.TimeTaken)).Count() > 0;
            return alreadyexists;
        }

        public bool IsIPAdressAlreadyComplete(string ipaddress)
        {
            bool iscomplete = this.context.Set<ServerLogFile>().Where(l => l.ClientIP.Equals(ipaddress) && !string.IsNullOrEmpty(l.City)).Count() > 0;
            return iscomplete;
        }
        public IPLocation GetIPLocationOfIPAddress(string ipaddress)
        {
            var iplocation = new IPLocation();
            var log = this.context.Set<ServerLogFile>().Where(l => l.ClientIP.Equals(ipaddress) && !string.IsNullOrEmpty(l.City)).First();
            iplocation.cityName = log.City;
            iplocation.countryName = log.Country;
            iplocation.longitude = log.Longitude;
            iplocation.latitude = log.Latitude;
            return iplocation;
        }
    }
}
