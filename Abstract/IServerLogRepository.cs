using AnalyseVisitorsTool.Models;

namespace AnalyseVisitorsTool.Abstract
{
    public interface IServerLogRepository : IRepository<ServerLogFile>
    {
        bool DoesEntryAlreadyExist(ServerLogFile file);
        bool IsIPAdressAlreadyComplete(string ipaddress);
        IPLocation GetIPLocationOfIPAddress(string ipaddress);
    }
}
