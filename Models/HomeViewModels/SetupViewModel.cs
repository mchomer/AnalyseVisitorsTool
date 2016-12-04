namespace AnalyseVisitorsTool.Models.HomeViewModels
{
    public class SetupViewModel
    {
        public string LogFilesFolder { get; set; }

        public SetupViewModel()
        {
            this.LogFilesFolder = string.Empty;
        }
    }
}
