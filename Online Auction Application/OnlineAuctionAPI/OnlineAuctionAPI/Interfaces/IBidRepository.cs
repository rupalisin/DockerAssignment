using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Interfaces
{
    public interface IBidRepository
    {
        Task<IEnumerable<Bid>> GetAllBidsAsync();
        Task<Bid> GetBidByIdAsync(int id);
        Task AddBidAsync(Bid bid);
        Task UpdateBidAsync(Bid bid);
        Task DeleteBidAsync(int id);
        Task<decimal> GetCurrentHighestBidAsync(int productId);
        Task<Bid> GetHighestBidAsync(int productId);
        Task<IEnumerable<Bid>> GetBidsByUserIdAsync(string userId);

        Task<IEnumerable<Bid>> GetBidsByProductIdAsync(int productId);

    }
}
