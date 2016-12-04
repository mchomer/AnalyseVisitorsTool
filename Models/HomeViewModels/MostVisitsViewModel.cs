using System.Collections.Generic;

namespace AnalyseVisitorsTool.Models.HomeViewModels
{
    public class MostVisitsViewModel
    {
        public IEnumerable<ServerLogFile> LogFiles { get; set; }
        public List<DropDownEntry> VisitTypes { get; set; }
        public string FilterValue { get; set; }

        public MostVisitsViewModel()
        {
            this.initVisitTypes();
        }

        private void initVisitTypes()
        {
            this.VisitTypes = new List<DropDownEntry>();
            this.VisitTypes.Add(new DropDownEntry("IP-Adresse", "0"));
            this.VisitTypes.Add(new DropDownEntry("Land", "1"));
            this.VisitTypes.Add(new DropDownEntry("Stadt", "2"));
            this.VisitTypes.Add(new DropDownEntry("Datum", "3"));
        }
    }
}
