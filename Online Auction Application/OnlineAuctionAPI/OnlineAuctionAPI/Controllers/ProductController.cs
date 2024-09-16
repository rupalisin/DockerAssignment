using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineAuctionAPI.DTOs;
using OnlineAuctionAPI.Interfaces;
using OnlineAuctionAPI.Models;
using System.Security.Claims;

namespace OnlineAuctionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto productDto)
        {
            // Retrieve user ID from token
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                StartingPrice = productDto.StartingPrice,
                AuctionDuration = productDto.AuctionDuration,
                Category = productDto.Category,
                ReservedPrice = productDto.ReservedPrice,
                UserId = userIdClaim.Value, 
                Bids = new List<Bid>()
            };

            await _productService.AddProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        [Authorize]
        [HttpGet("user-products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;

            var products = await _productService.GetProductsByUserIdAsync(userId);
            return Ok(products);
        }

        [Authorize]
        [HttpGet("bought-products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetBoughtProductsByUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;

            var products = await _productService.GetBoughtProductsByUserIdAsync(userId);
            return Ok(products);
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            if (id != product.Id)
                return BadRequest();

            await _productService.UpdateProductAsync(product);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
    }
}