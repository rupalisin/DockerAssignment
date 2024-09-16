using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Data;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.User).Include(p => p.Bids).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Include(p => p.User).Include(p => p.Bids)
                                          .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByUserIdAsync(string userId)
        {
            return await _context.Products
                                 .Where(p => p.UserId == userId)
                                 .Include(p => p.Bids)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetBoughtProductsByUserIdAsync(string userId)
        {
            return await _context.Products
                                 .Where(p => p.Bids.Any(b => b.UserId == userId) && p.Sold)
                                 .Include(p => p.Bids)
                                 .ToListAsync();
        }


        public async Task AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
