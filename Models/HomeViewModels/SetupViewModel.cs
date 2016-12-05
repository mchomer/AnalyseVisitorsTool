namespace AnalyseVisitorsTool.Models.HomeViewModels
{
    public class SetupViewModel
    {
        public string LogFilesFolder { get; set; }
        public string GoogleMapsAPIKey { get; set; }
        public string IPLocationAPIKey { get; set; }

        public SetupViewModel()
        {
            this.LogFilesFolder = string.Empty;
            this.GoogleMapsAPIKey = string.Empty;
            this.IPLocationAPIKey = string.Empty;
        }
    }
}
