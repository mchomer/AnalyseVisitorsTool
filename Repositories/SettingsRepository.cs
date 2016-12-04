using AnalyseVisitorsTool.Abstract;
using AnalyseVisitorsTool.Data;
using AnalyseVisitorsTool.Models;

namespace AnalyseVisitorsTool.Repositories
{
    public class SettingsRepository : Repository<Settings>, ISettingsRepository
    {
        public SettingsRepository(LogDbContext context) : base(context)
        {
        }
    }
}
