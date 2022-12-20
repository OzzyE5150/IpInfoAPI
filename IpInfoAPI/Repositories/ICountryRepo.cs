using IpInfoAPI.Models;

namespace IpInfoAPI.Repositories
{
    public interface ICountryRepo
    {
        List<Country> GetCountries();
        Country GetCountryByIp(string ipAddress);
        Country GetCountryByThreeLetterCode(string threeLetterCode);
        Task<Country> InsertNewCountry(Country country);
    }
}
