using IpInfoAPI.Responses;
using Newtonsoft.Json;
using System.Text;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task TestGetCountryInfo()
        {
            // Arrange
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5127")
            };
            string ipAddress = "44.255.255.254";

            // Act
            var response = await client.GetAsync($"/{ipAddress}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var countryInfo = JsonConvert.DeserializeObject<CountryByIpRes>(responseString);
            Assert.Equal("US", countryInfo.TwoLetterCode);
            Assert.Equal("United States", countryInfo.Name);
        }
        [Fact]
        public async Task TestPostCountryCode()
        {
            // Arrange
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5127")
            };
            var countryCode = new string[] { "GR" };
            var content = new StringContent(JsonConvert.SerializeObject(countryCode), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/reports", content);

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var countryInfo = JsonConvert.DeserializeObject<IEnumerable<CountryRes>>(responseString);
            Assert.Single(countryInfo);
            Assert.Contains(countryInfo, c => c.CountryName == "Greece");
        }

    }
}