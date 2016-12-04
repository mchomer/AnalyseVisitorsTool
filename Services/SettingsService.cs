using AnalyseVisitorsTool.Abstract;
using AnalyseVisitorsTool.Models;

namespace AnalyseVisitorsTool.Services
{
    public class SettingsService : ISettingsService
    {
        private ISettingsRepository _settingsrepository;

        public SettingsService(ISettingsRepository settingsrepository)
        {
            this._settingsrepository = settingsrepository;
        }

        public void UpdateSettings(Settings setup)
        {
            this._settingsrepository.Add(setup);
            this._settingsrepository.Save();
        }
    }
}
