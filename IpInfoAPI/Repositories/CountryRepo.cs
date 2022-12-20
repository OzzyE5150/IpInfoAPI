using IpInfoAPI.Models;

namespace IpInfoAPI.Repositories
{
    public class CountryRepo : ICountryRepo
    {
        private DbContext context;
        public CountryRepo(DbContext context)
        {
            this.context = context;
        }
        public List<Country> GetCountries()
        {
            return context.Countries.ToList();
        }
        public Country GetCountryByIp(string ip)
        {
            var result = context.Countries.Join(
                context.IpAddresses,
                country => country.Id,
                ipAddress => ipAddress.CountryId,
                (country, IpAddresses) => new { country, address = IpAddresses })
                .Where(res => res.address.Ip == ip).FirstOrDefault();
            return result.country;
        }
        public Country GetCountryByThreeLetterCode(string ThreeLetterCode)
        {

            Country country = context.Countries.Where(country => country.ThreeLetterCode == ThreeLetterCode).First();
            return country;
        }

        public async Task<Country> InsertNewCountry(Country newCountry)
        {
            var countryToInsert = new Country()
            {
                ThreeLetterCode = newCountry.ThreeLetterCode,
                TwoLetterCode = newCountry.TwoLetterCode,
                Name = newCountry.Name
            };
            await context.Countries.AddAsync(countryToInsert);
            await context.SaveChangesAsync();

            return countryToInsert;
        }
    }
}
