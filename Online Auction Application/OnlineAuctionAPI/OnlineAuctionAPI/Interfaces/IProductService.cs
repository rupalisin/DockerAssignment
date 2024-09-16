using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(int id);
        Task SellProductAsync(int productId);
        Task<IEnumerable<Product>> GetProductsByUserIdAsync(string userId);
        Task<IEnumerable<Product>> GetBoughtProductsByUserIdAsync(string userId);
    }
}