using IpInfoAPI.Models;

namespace IpInfoAPI.Repositories
{
    public interface IIpAddressRepo
    {
        IpAddress GetIpAddress(string ipAddress);
        List<IpAddress> GetIps(int RowsPerPage, int Page);
        Task<HttpResponseMessage> UpdateIpAddress (IpAddress ipAddress);
    }
}
