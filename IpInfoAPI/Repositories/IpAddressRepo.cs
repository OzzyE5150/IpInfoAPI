using IpInfoAPI.Models;

namespace IpInfoAPI.Repositories
{
    public class IpAddressRepo : IIpAddressRepo
    {
        private DbContext context;
        public IpAddressRepo(DbContext context)
        {
            this.context = context;
        }
        public IpAddress GetIpAddress(string ipAddress)
        {
            IpAddress? address = context.IpAddresses.Where(address => address.Ip == ipAddress).First();
            return address;
        }
        public List<IpAddress> GetIps(int rows, int page)
        {
            List<IpAddress> ips = context.IpAddresses
                .Skip(rows*page)
                .Take(rows)
                .ToList();
            return ips;
        }
        public async Task<HttpResponseMessage> UpdateIpAddress(IpAddress ipAddress)
        {
            IpAddress exists = await context.IpAddresses.FindAsync(ipAddress.Id);
            if(exists != null)
            {
                exists.CountryId = (ipAddress.CountryId != null)? ipAddress.CountryId : exists.CountryId;
                await context.SaveChangesAsync();
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
