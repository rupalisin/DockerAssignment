using System.ComponentModel.DataAnnotations;

namespace OnlineAuctionAPI.Models
{
    public class User
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]

        public string Email { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        public string Role { get; set; } // "Admin" or "User"

        public bool IsSuspended { get; set; } = false;
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
