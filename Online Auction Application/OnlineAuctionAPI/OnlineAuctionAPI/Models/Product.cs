using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace OnlineAuctionAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal StartingPrice { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int AuctionDuration { get; set; } // In hours
        public DateTime AuctionStartTime { get; set; }
        public DateTime AuctionEndTime { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ReservedPrice { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public ICollection<Bid> Bids { get; set; }

        public bool Sold { get; set; }
        public decimal CurrentHighestBid { get; set; }


        public void StartAuction()
        {
            AuctionStartTime = DateTime.UtcNow;
            AuctionEndTime = AuctionStartTime.AddHours(AuctionDuration);
        }
    }
}
