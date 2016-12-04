using System;
using System.Globalization;
using System.Linq;
using AnalyseVisitorsTool.Abstract;
using AnalyseVisitorsTool.Data.Enums;
using AnalyseVisitorsTool.Models;
using AnalyseVisitorsTool.Models.HomeViewModels;
using AnalyseVisitorsTool.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyseVisitorsTool.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private IServerLogService _serverlogservice;
        private IServerLogRepository _serverlogrepository;
        private IIPLocationRepository _iplocationrepository;
        private ISettingsRepository _settingsrepository;
        private ISettingsService _settingsservice;
        
        public HomeController(IServerLogService serverlogservice, IServerLogRepository serverlogrepository,
                                IIPLocationRepository iplocationrepository, ISettingsRepository settingsrepository,
                                ISettingsService settingsservice)
        {
            this._serverlogservice = serverlogservice;    
            this._serverlogrepository = serverlogrepository;
            this._iplocationrepository = iplocationrepository;
            this._settingsrepository = settingsrepository;
            this._settingsservice = settingsservice;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var ivm = new IndexViewModel();
            ivm.ServerLogFiles = this._serverlogrepository.GetAll().OrderByDescending(s => s.TimeStamp).Take(200);
            ivm.ServerLogFiles = this._serverlogservice.SetLocationIDs(ivm.ServerLogFiles);
            return View(ivm);
        }

        [HttpGet]
        public IActionResult Locations()
        {
            var lvm = new LocationsViewModel();
            lvm.IPLocations = this._iplocationrepository.GetAll().OrderBy(l => l.countryName).ThenBy(l => l.cityName);
            lvm.Cities = this._iplocationrepository.GetAll().Select(c => c.cityName).Distinct().OrderBy(c => c);
            lvm.Countries = this._iplocationrepository.GetAll().Select(c => c.countryName).Distinct().OrderBy(c => c);
            return View(lvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Locations(LocationsViewModel lvm)
        {
            lvm.IPLocations = this._iplocationrepository.Find(l => l.countryName.Equals(lvm.Country)).OrderBy(l => l.countryName).ThenBy(l => l.cityName);
            lvm.Cities = this._iplocationrepository.GetAll().Select(c => c.cityName).Distinct().OrderBy(c => c);
            lvm.Countries = this._iplocationrepository.GetAll().Select(c => c.countryName).Distinct().OrderBy(c => c);
            return View(lvm);
        }

        [HttpGet]
        public IActionResult Location(int id)
        {
            var lvm = new LocationViewModel();
            lvm.CurrentLocation = this._iplocationrepository.Get(l => l.ID.Equals(id));
            lvm.ServerLogFiles = this._serverlogrepository.Find(l => l.ClientIP.Equals(lvm.CurrentLocation.ipAddress)).OrderByDescending(l => l.TimeStamp);
            return View(lvm);
        }

        public IActionResult AnalyseServerLogFiles()
        {
            this._serverlogservice.BuildServerLogDatabaseEntries();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult MostVisits()
        {
            var mvvm = new MostVisitsViewModel();
            mvvm.LogFiles = this._serverlogservice.GetMostVisits();
            mvvm.LogFiles = this._serverlogservice.SetLocationIDs(mvvm.LogFiles);
            return View(mvvm);
        }

        [HttpPost]
        public IActionResult MostVisits(MostVisitsViewModel mvvm)
        {
            mvvm.LogFiles = this._serverlogservice.GetMostVisits((VisitType)Convert.ToInt32(mvvm.FilterValue));
            mvvm.LogFiles = this._serverlogservice.SetLocationIDs(mvvm.LogFiles);
            return View(mvvm);
        }

        [HttpGet]
        public IActionResult VisitsToday()
        {
            var ivm = new IndexViewModel();
            ivm.ServerLogFiles = this._serverlogrepository.Find(l => l.TimeStamp.ToString("dd.MM.yyyy")
                                    .Equals(DateTime.Now.ToString("dd.MM.yyyy")))
                                    .OrderByDescending(l => l.TimeStamp).ToList();
            ivm.ServerLogFiles = this._serverlogservice.SetLocationIDs(ivm.ServerLogFiles);
            return View(ivm);
        }

        [HttpPost]
        public IActionResult VisitsToday(IndexViewModel ivm)
        {
            IFormatProvider culture = new CultureInfo("de-DE");
            var date = Convert.ToDateTime(ivm.FilterDate, culture);
            ivm.ServerLogFiles = this._serverlogrepository.Find(l => l.TimeStamp.ToString("dd.MM.yyyy")
                                    .Equals(date.ToString("dd.MM.yyyy")))
                                    .OrderByDescending(l => l.TimeStamp).ToList();
            ivm.ServerLogFiles = this._serverlogservice.SetLocationIDs(ivm.ServerLogFiles);
            return View(ivm);
        }



        [HttpGet]
        public IActionResult MissingIPLocations()
        {
            var ivm = new IndexViewModel();
            ivm.ServerLogFiles = this._serverlogrepository.Find(l => string.IsNullOrEmpty(l.City))
                                    .OrderByDescending(l => l.TimeStamp).ToList();
            ivm.ServerLogFiles = this._serverlogservice.SetLocationIDs(ivm.ServerLogFiles);
            ivm.ServerLogFiles = this._serverlogrepository.Find(l => l.LocationID < 1)
                                    .OrderByDescending(l => l.TimeStamp).ToList();
            ivm.TotalEntries = ivm.ServerLogFiles.Select(l => l.ClientIP).Distinct().Count();
            return View(ivm);
        }

        public IActionResult UpdateMissingLocations()
        {
            var logs = this._serverlogrepository.Find(l => string.IsNullOrEmpty(l.City))
                                    .OrderByDescending(l => l.TimeStamp).ToList();
            logs = this._serverlogservice.SetLocationIDs(logs).ToList();
            logs = this._serverlogrepository.Find(l => l.LocationID < 1)
                                    .OrderByDescending(l => l.TimeStamp).ToList();
            this._serverlogservice.UpdateMissingLocations(logs);
            return RedirectToAction("MissingIPLocations");
        }

        [HttpGet]
        public IActionResult Setup()
        {
            var svm = new SetupViewModel();
            if (this._settingsrepository.GetAll().Count() > 0) {
                svm.LogFilesFolder = this._settingsrepository.GetAll().OrderByDescending(l => l.ID).First().ServerLogFilesFolder;
            }
            return View(svm);
        }

        [HttpPost]
        public IActionResult Setup(SetupViewModel svm)
        {
            var setup = new Settings();
            setup.ServerLogFilesFolder = svm.LogFilesFolder;
            this._settingsservice.UpdateSettings(setup);
            return View(svm);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
