using jsonserver.Data.Entities;
using jsonserver.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace jsonserver.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly JsonServerContext _context;

        public UserRepository(JsonServerContext context)
        {
            _context = context;
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public Task<User> GetByUserNameAsync(string userName)
        {
            return _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task AddAsync(User user)
        {
            try
            {
                await _context.AddAsync(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<User> GetByApiKeyAsync(string apiKey)
        {
            return _context.Users.Include(x => x.Jsons).FirstOrDefaultAsync(x => x.ApiKey == apiKey);
        }
    }
}
