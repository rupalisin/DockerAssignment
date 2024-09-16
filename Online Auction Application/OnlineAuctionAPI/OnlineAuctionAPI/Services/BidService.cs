using OnlineAuctionAPI.DTOs;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Services
{
    public class BidService : IBidService
    {
        private readonly IBidRepository _bidRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public BidService(IBidRepository bidRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _bidRepository = bidRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<Bid>> GetAllBidsAsync()
        {
            return await _bidRepository.GetAllBidsAsync();
        }

        public async Task<Bid> GetBidByIdAsync(int id)
        {
            return await _bidRepository.GetBidByIdAsync(id);
        }

        public async Task<IEnumerable<Bid>> GetBidsByUserIdAsync(string userId)
        {
            return await _bidRepository.GetBidsByUserIdAsync(userId);
        }


        public async Task<BidResponse> AddBidAsync(Bid bid)
        {
            var user = await _userRepository.GetUserByIdAsync(bid.UserId);
            if (user == null)
            {
                return new BidResponse { Success = false, Message = "User not found." };
            }

            if (user.IsSuspended)
            {
                return new BidResponse { Success = false, Message = "User is suspended." };
            }

            var product = await _productRepository.GetProductByIdAsync(bid.ProductId);
            if (product == null)
            {
                throw new ArgumentException("Invalid product ID.");
            }

            if (DateTime.UtcNow >= product.AuctionEndTime)
            {
                return new BidResponse { Success = false, Message = "Auction has ended." };
            }

            var currentHighestBid = await _bidRepository.GetCurrentHighestBidAsync(bid.ProductId);

            if (bid.Amount < product.StartingPrice || bid.Amount <= currentHighestBid)
            {
                return new BidResponse { Success = false, Message = "Bid amount is too low." };
            }

            product.CurrentHighestBid = bid.Amount;
            product.Bids.Add(bid);

            await _bidRepository.AddBidAsync(bid);
            return new BidResponse { Success = true, Message = "Bid placed successfully." };
        }

        public async Task UpdateBidAsync(Bid bid)
        {
            await _bidRepository.UpdateBidAsync(bid);
        }

        public async Task DeleteBidAsync(int id)
        {
            await _bidRepository.DeleteBidAsync(id);
        }
    }
}