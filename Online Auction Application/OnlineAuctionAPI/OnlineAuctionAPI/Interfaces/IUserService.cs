using OnlineAuctionAPI.Models;

namespace OnlineAuctionAPI.Interfaces
{
    public interface IUserService
    {
        Task<string> AuthenticateAsync(string email, string password);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<bool> SuspendUserAsync(string id);

    }
}
