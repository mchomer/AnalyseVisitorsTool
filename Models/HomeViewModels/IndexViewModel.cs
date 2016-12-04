using System;
using System.Collections.Generic;

namespace AnalyseVisitorsTool.Models.HomeViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<ServerLogFile> ServerLogFiles { get; set; }
        public int TotalEntries { get; set; }
        public string FilterDate { get; set; }

        public IndexViewModel()
        {
            this.FilterDate = DateTime.Now.ToString("dd.MM.yyyy");
        }
    }
}