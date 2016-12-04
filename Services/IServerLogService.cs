using System.Collections.Generic;
using System.Threading.Tasks;
using AnalyseVisitorsTool.Data.Enums;
using AnalyseVisitorsTool.Models;

namespace AnalyseVisitorsTool.Services
{
    public interface IServerLogService
    {
        void GetServerLogFiles(string path);
        Task SetLocation(ServerLogFile file);
        void BuildServerLogDatabaseEntries();
        IEnumerable<ServerLogFile> SetLocationIDs(IEnumerable<ServerLogFile> files);
        IEnumerable<ServerLogFile> GetMostVisits(VisitType stats = VisitType.IPAddress);
        void UpdateMissingLocations(List<ServerLogFile> logs);
    }
}
