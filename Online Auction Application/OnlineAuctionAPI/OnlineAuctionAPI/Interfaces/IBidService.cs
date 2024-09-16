using OnlineAuctionAPI.DTOs;
using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Interfaces
{
    public interface IBidService
    {
        Task<IEnumerable<Bid>> GetAllBidsAsync();
        Task<Bid> GetBidByIdAsync(int id);
        Task<BidResponse> AddBidAsync(Bid bid);
        Task UpdateBidAsync(Bid bid);
        Task DeleteBidAsync(int id);
        Task<IEnumerable<Bid>> GetBidsByUserIdAsync(string userId);
    }
}