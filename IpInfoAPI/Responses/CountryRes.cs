using Microsoft.Identity.Client;

namespace IpInfoAPI.Responses
{
    public partial class CountryRes
    {
        public string CountryName { get; set; } = null!;
        public int AddressessCount { get; set; } = 0!;
        public DateTime? LastAddressUpdated { get; set; } = null!;
    }
}
