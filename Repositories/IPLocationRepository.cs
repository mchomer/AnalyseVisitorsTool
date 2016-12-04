using AnalyseVisitorsTool.Abstract;
using AnalyseVisitorsTool.Data;
using AnalyseVisitorsTool.Models;

namespace AnalyseVisitorsTool.Repositories
{
    public class IPLocationRepository : Repository<IPLocation>, IIPLocationRepository
    {
        public IPLocationRepository(LogDbContext context) : base(context)
        {
        }
    }
}
