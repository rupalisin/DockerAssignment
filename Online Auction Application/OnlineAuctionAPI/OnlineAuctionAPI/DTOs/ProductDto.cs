namespace OnlineAuctionAPI.DTOs
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal StartingPrice { get; set; }
        public int AuctionDuration { get; set; } // In hours
        public string Category { get; set; }
        public decimal ReservedPrice { get; set; }
    }
}