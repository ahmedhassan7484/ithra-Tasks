using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IthraDbContext _context;

        public UserRepository(IthraDbContext context)
        {
            _context = context;
        }
        public  Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var hashedPassword = HashPassword(password);
            return Task.FromResult(user.PasswordHash == hashedPassword);
        }

        public async Task<ApplicationUser> CreateAsync(ApplicationUser user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.PasswordHash = HashPassword(user.PasswordHash); 

            await _context.Users.AddAsync(user);
            //await _context.SaveChangesAsync();

            return user;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<ApplicationUser?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<ApplicationUser?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public  Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            var roles = new List<string> { user.Role.ToString() };
            return Task.FromResult<IList<string>>(roles);
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return Task.FromResult(user.Role.ToString().Equals(role, StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            user.UpdatedAt = DateTime.UtcNow;
            _context.Users.Update(user);
            //await _context.SaveChangesAsync();
        }
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
