using IpInfoAPI.Models;

namespace IpInfoAPI.Repositories
{
    public interface IIpAddressRepo
    {
        /// <summary>
        /// Returns an IpAddress object from context, given the address in string
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        IpAddress GetIpAddress(string ipAddress);
        /// <summary>
        /// Returns a List of IpAddress objects (row), starting from (.Skip) RowsPerPage*Page
        /// </summary>
        /// <param name="RowsPerPage"></param>
        /// <param name="Page"></param>
        /// <returns></returns>
        List<IpAddress> GetIps(int RowsPerPage, int Page);
        /// <summary>
        /// Updates an IP Address if it exists and returns status code 200 if successful
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> UpdateIpAddress (IpAddress ipAddress);
    }
}
