using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalyseVisitorsTool.Models
{
    public class ServerLogFile
    {

        public ServerLogFile()
        {
            this.LocationID = -1;
        }

        public int ID { get; set; }
        public DateTime TimeStamp { get; set; }
        public string ServerIP { get; set; }
        public string Method { get; set; }
        public string UriStem { get; set; }
        public string UriQuery { get; set; }
        public string ServerPort { get; set; }
        public string Username { get; set; }
        public string ClientIP { get; set; }
        public string ClientUserAgent { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string Win32Status { get; set; }
        public string TimeTaken { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        [NotMapped]
        public int LocationID { get; set; }
        [NotMapped]
        public int Visits { get; set; }
    }
}
