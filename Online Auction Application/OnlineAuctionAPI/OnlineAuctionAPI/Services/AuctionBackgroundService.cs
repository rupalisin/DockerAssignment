using OnlineAuctionAPI.Interfaces;

namespace OnlineAuctionAPI.Services
{
    public class AuctionBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public AuctionBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckAndSellProductsAsync();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Check every minute
            }
        }

        private async Task CheckAndSellProductsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
                var products = await productService.GetAllProductsAsync();

                var now = DateTime.UtcNow;
                foreach (var product in products)
                {
                    if (!product.Sold && product.AuctionEndTime <= now)
                    {
                        await productService.SellProductAsync(product.Id);
                    }
                }
            }
        }
    }
}