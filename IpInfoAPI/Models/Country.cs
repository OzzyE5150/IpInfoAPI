using Microsoft.AspNetCore.Mvc;

namespace IpInfoAPI.Models
{
    public partial class Country
    {
        public Country()
        {
            IpAddressCollection = new HashSet<IpAddress>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TwoLetterCode { get; set; } = null!;
        public string ThreeLetterCode { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<IpAddress> IpAddressCollection { get;set; }
    }
}
