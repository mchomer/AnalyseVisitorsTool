using System.Collections.Generic;

namespace AnalyseVisitorsTool.Models.HomeViewModels
{
    public class LocationsViewModel
    {
        public IEnumerable<IPLocation> IPLocations { get; set; }
        public IEnumerable<string> Cities { get; set; }
        public IEnumerable<string> Countries { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
