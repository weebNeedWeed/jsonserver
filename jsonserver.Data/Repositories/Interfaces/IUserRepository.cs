﻿using jsonserver.Data.Entities;
using System.Threading.Tasks;

namespace jsonserver.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByEmailAsync(string email);
        Task<User> AddAsync(User user);
    }
}