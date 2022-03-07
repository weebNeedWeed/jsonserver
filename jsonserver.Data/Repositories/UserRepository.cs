using jsonserver.Data.Entities;
using jsonserver.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace jsonserver.Data.Repositories
{
    public class UserRepository: IUserRepository
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

        public async Task<User> AddAsync(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
