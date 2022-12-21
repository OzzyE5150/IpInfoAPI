using IpInfoAPI.Models;

namespace IpInfoAPI.Repositories
{
    public interface ICountryRepo
    {
        /// <summary>
        /// Returns List of Countries
        /// </summary>
        /// <returns></returns>
        List<Country> GetCountries();
        /// <summary>
        /// Returns a Country given an IP Address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        Country GetCountryByIp(string ipAddress);
        /// <summary>
        /// Returns a Country given a Three Letter Code
        /// </summary>
        /// <param name="threeLetterCode"></param>
        /// <returns></returns>
        Country GetCountryByThreeLetterCode(string threeLetterCode);
        /// <summary>
        /// Inserts a new Country
        /// </summary>
        /// <param name="country">New Country with Name, TwoLetterCode, ThreeLetterCode</param>
        /// <returns></returns>
        Task<Country> InsertNewCountry(Country country);
    }
}
