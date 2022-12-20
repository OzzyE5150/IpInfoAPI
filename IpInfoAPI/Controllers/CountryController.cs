using IpInfoAPI.Helpers;
using IpInfoAPI.Models;
using IpInfoAPI.Repositories;
using IpInfoAPI.Responses;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace IpInfoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : ControllerBase
    {
        public static IMemoryCache _memoryCache;
        private readonly CountryRepo countryRepo;
        private readonly IpAddressRepo ipAddressRepo;
        public CountryController(IMemoryCache? cache)
        {
            this.countryRepo = new(new Models.DbContext());
            this.ipAddressRepo = new(new Models.DbContext());
            _memoryCache = cache?? throw new ArgumentNullException(nameof(cache));
        }
        [HttpGet("/{ip}")]
        public async Task<CountryByIpRes> GetByIp(string ip)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                .SetPriority(CacheItemPriority.Normal);
            var key = "Countries:" + ip;
            Console.WriteLine("FETCHING FROM CACHE");
            if(_memoryCache.TryGetValue(key, out Country country)) {
                Console.WriteLine("FOUND IN CACHE");
            }
            else
            {
                Console.WriteLine("NOT FOUND IN CACHE. USING DATABASE");
                country = countryRepo.GetCountryByIp(ip);
                _memoryCache.Set(key, country, memoryCacheEntryOptions);
            }
            HttpClient httpClient = new();
            string[] response = (await httpClient.GetStringAsync($"https://ip2c.org/?ip={ip}")).Split(";");
            string threeLetterCodeUpdate = response[2];
            if(threeLetterCodeUpdate != null)
            {
                IpAddress ipAddress = ipAddressRepo.GetIpAddress(ip);
                Country? countryUpdate = null;
                try
                {
                    countryUpdate = countryRepo.GetCountryByThreeLetterCode(threeLetterCodeUpdate);
                    if(ipAddress.CountryId != countryUpdate.Id)
                    {
                        Console.WriteLine("COUNTRY EXISTS");
                        ipAddress.CountryId= countryUpdate.Id;
                        Console.WriteLine("UPDATING IP");
                        await ipAddressRepo.UpdateIpAddress(ipAddress);
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
                    Console.WriteLine("ADDING NEW");
                    Country countryNew = await countryRepo.InsertNewCountry(countryAdd);
                    ipAddress.Country= countryNew;
                    ipAddress.CountryId= countryNew.Id;
                    Console.WriteLine("UPDATING IP");
                    await ipAddressRepo.UpdateIpAddress(ipAddress);
                    _memoryCache.Set(key, countryNew, memoryCacheEntryOptions);
                }
            }
            return new CountryByIpRes()
            {
                Name = country.Name,
                TwoLetterCode = country.TwoLetterCode,
                ThreeLetterCode = country.ThreeLetterCode,
            };
        }
        [HttpPost("/reports")]
        public async Task<IEnumerable<CountryRes>?> GetCountry(string[]? TwoLetterList)
        {
            return await SqlCommandHelper.QueryCountryRes(TwoLetterList);
        }
    }
}
