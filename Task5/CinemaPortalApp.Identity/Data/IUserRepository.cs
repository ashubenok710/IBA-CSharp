using AuthenticationServer.Models;
using CinemaPortal.Identity.Models;

namespace CinemaPortal.Identity.Data;

public interface IUserRepository
{
    public Task<UserProfile> GetByEmailAsync(string email);
}
