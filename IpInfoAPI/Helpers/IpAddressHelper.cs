using IpInfoAPI.Models;
using IpInfoAPI.Repositories;

using Microsoft.Extensions.Caching.Memory;

namespace IpInfoAPI.Helpers
{
    public class IpAddressHelper
    {
        private CountryRepo countryRepo;
        private IpAddressRepo addressRepo;
        private IMemoryCache _cache;

        /// <summary>
        /// Set repository contexts and cache
        /// </summary>
        /// <param name="cache"></param>
        /// <exception cref="ArgumentNullException">Throws if cache is null</exception>
        public IpAddressHelper(IMemoryCache cache)
        {
            this.countryRepo = new(new Models.DbContext());
            this.addressRepo = new(new Models.DbContext());
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        /// <summary>
        /// Updates the IP Addresses using ip2c
        /// </summary>
        /// <returns></returns>
        public async Task UpdateIpAddresses()
        {
            List<IpAddress> ipAddresses = new();
            int size = 100;
            do
            {
                int p = 0;
                ipAddresses.AddRange(addressRepo.GetIps(size, p));
                Console.WriteLine("UPDATING {0} ADDRESSES", ipAddresses.Count);
                foreach (IpAddress ip in ipAddresses)
                {
                    HttpClient httpClient = new();
                    string[] response = (await httpClient.GetStringAsync($"https://ip2c.org/?ip={ip.Ip}")).Split(";");
                    string threeLetterCodeUpdate = response[2];
                    var key = "Countries:" + ip.Ip;
                    Country? countryUpdate = null;
                    try
                    {
                        countryUpdate = countryRepo.GetCountryByThreeLetterCode(threeLetterCodeUpdate);
                        if (ip.CountryId != countryUpdate.Id)
                        {
                            ip.CountryId = countryUpdate.Id;
                            Console.WriteLine("UPDATING IP: " + ip.Ip);
                            await addressRepo.UpdateIpAddress(ip);
                            _cache.Remove(key);
                        }
                    }
                    catch (Exception)
                    {
                        Country countryAdd = new()
                        {
                            TwoLetterCode = response[1],
                            ThreeLetterCode = response[2],
                            Name = response[3]
                        };
                        Console.WriteLine("ADDING COUNTRY");
                        Country countryNew = await countryRepo.InsertNewCountry(countryAdd);
                        ip.Country = countryNew;
                        ip.CountryId = countryNew.Id;
                        Console.WriteLine("UPDATING IP: " + ip.Ip);
                        await addressRepo.UpdateIpAddress(ip);
                        _cache.Remove(key);
                    }
                }
                p++;
            } while (ipAddresses.Count > size);
            Console.WriteLine("DONE");
        }
    }
}
