using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Data;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bid>> GetAllBidsAsync()
        {
            return await _context.Bids.Include(b => b.User).Include(b => b.Product).ToListAsync();
        }

        public async Task<Bid> GetBidByIdAsync(int id)
        {
            return await _context.Bids.Include(b => b.User).Include(b => b.Product)
                                      .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<decimal> GetCurrentHighestBidAsync(int productId)
        {
            return await _context.Bids
                .Where(b => b.ProductId == productId)
                .OrderByDescending(b => b.Amount)
                .Select(b => b.Amount)
                .FirstOrDefaultAsync();
        }

        public async Task AddBidAsync(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Bid>> GetBidsByUserIdAsync(string userId)
        {
            return await _context.Bids
                                 .Where(b => b.UserId == userId)
                                 .Include(b => b.Product)
                                 .ToListAsync();
        }


        public async Task<Bid> GetHighestBidAsync(int productId)
        {
            return await _context.Bids
                                 .Where(b => b.ProductId == productId)
                                 .OrderByDescending(b => b.Amount)
                                 .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Bid>> GetBidsByProductIdAsync(int productId)
        {
            return await _context.Bids.Where(b => b.ProductId == productId).ToListAsync();
        }


        public async Task UpdateBidAsync(Bid bid)
        {
            _context.Entry(bid).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBidAsync(int id)
        {
            var bid = await _context.Bids.FindAsync(id);
            if (bid != null)
            {
                _context.Bids.Remove(bid);
                await _context.SaveChangesAsync();
            }
        }
    }
}