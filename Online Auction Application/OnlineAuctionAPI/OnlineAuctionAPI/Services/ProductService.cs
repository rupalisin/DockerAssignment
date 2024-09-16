using Microsoft.EntityFrameworkCore;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBidRepository _bidRepository;

        public ProductService(IProductRepository productRepository, IBidRepository bidRepository)
        {
            _productRepository = productRepository;
            _bidRepository = bidRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllProductsAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetProductsByUserIdAsync(string userId)
        {
            return await _productRepository.GetProductsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Product>> GetBoughtProductsByUserIdAsync(string userId)
        {
            return await _productRepository.GetBoughtProductsByUserIdAsync(userId);
        }


        public async Task AddProductAsync(Product product)
        {
            product.StartAuction();
            product.CurrentHighestBid = 0;
            await _productRepository.AddProductAsync(product);
        }

        public async Task SellProductAsync(int productId)
        {
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null || product.Sold)
                return;

            var highestBid = await _bidRepository.GetHighestBidAsync(productId);
            if (highestBid != null && highestBid.Amount >= product.ReservedPrice)
            {
                product.Sold = true;
                await _productRepository.UpdateProductAsync(product);
            }
        }


        public async Task UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateProductAsync(product);
        }

        public async Task DeleteProductAsync(int id)
        {

            var product = await _productRepository.GetProductByIdAsync(id);

            if (product != null)
            {
                var bids = await _bidRepository.GetBidsByProductIdAsync(id);
                foreach (var bid in bids)
                {
                    await _bidRepository.DeleteBidAsync(bid.Id);
                }

            
            
            }
            await _productRepository.DeleteProductAsync(id);
        }
    }
}