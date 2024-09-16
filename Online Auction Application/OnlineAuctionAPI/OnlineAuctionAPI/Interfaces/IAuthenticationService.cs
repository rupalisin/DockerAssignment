using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateJwtToken(User user);
    }
}
