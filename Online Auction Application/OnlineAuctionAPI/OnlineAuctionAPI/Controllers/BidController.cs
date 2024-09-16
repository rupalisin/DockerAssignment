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
    public class BidController : ControllerBase
    {
        private readonly IBidService _bidService;

        public BidController(IBidService bidService)
        {
            _bidService = bidService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bid>>> GetAllBids()
        {
            var bids = await _bidService.GetAllBidsAsync();
            return Ok(bids);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Bid>> GetBidById(int id)
        {
            var bid = await _bidService.GetBidByIdAsync(id);
            if (bid == null)
                return NotFound();

            return Ok(bid);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddBid([FromBody] BidDto bidDto)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var userId = userIdClaim.Value;

            var bid = new Bid
            {
                Amount = bidDto.Amount,
                ProductId = bidDto.ProductId,
                UserId = userId,
                BidTime = DateTime.UtcNow
            };

            var result = await _bidService.AddBidAsync(bid);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return Ok(new { message = result.Message });
        }

        [Authorize]
        [HttpGet("user-bids")]
        public async Task<ActionResult<IEnumerable<Bid>>> GetBidsByUser()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;

            var bids = await _bidService.GetBidsByUserIdAsync(userId);
            return Ok(bids);
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBid(int id, [FromBody] Bid bid)
        {
            if (id != bid.Id)
                return BadRequest();

            await _bidService.UpdateBidAsync(bid);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBid(int id)
        {
            await _bidService.DeleteBidAsync(id);
            return NoContent();
        }
    }
}


