using System.Collections.Generic;

namespace AnalyseVisitorsTool.Models.HomeViewModels
{
    public class LocationViewModel
    {
        public IPLocation CurrentLocation { get; set; }
        public IEnumerable<ServerLogFile> ServerLogFiles { get; set; }
    }
}
