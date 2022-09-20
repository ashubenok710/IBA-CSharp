﻿using AuthenticationServer.DbContexts;
using AuthenticationServer.Models;
using Microsoft.EntityFrameworkCore;
using CinemaPortal.Identity.Models;

namespace CinemaPortal.Identity.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _context;

        public UserRepository(AuthDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfile> GetByEmailAsync(string email)
        {
            return await _context.UserProfile.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}