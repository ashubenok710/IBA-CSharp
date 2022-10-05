using CinemaPortal.Web.Models;

namespace CinemaPortal.Web.Data;

public interface IUserRepository
{
    Task<UserProfile> CreateAsync(UserProfile userProfile);
    Task<UserProfile> GetByEmailAsync(string email);
    Task<UserProfile> GetByIdAsync(int id);
}
