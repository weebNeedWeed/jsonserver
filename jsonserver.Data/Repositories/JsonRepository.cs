using jsonserver.Data.Entities;
using jsonserver.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jsonserver.Data.Repositories
{
    public class JsonRepository: IJsonRepository
    {
        private readonly JsonServerContext _context;

        public JsonRepository(JsonServerContext context)
        {
            _context = context;
        }

        public Task<Json> GetByNameAsync(string name)
        {
            return _context.Jsons.FirstOrDefaultAsync(x => x.Name == name);
        }

        public async Task AddAsync(Json json)
        {
            try
            {
                await _context.Jsons.AddAsync(json);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<Json>> GetAllAsync()
        {
            return _context.Jsons.ToListAsync();
        }

        public async Task DeleteByIdAsync(int jsonId)
        {
            Json json = await _context.Jsons.FirstOrDefaultAsync(x => x.JsonId == jsonId);
            
            if(json != null)
            {
                _context.Remove(json);
                await _context.SaveChangesAsync();
            }
        }

        public async Task EditNameAsync(int jsonId, string name)
        {
            Json json = await _context.Jsons.FirstOrDefaultAsync(x => x.JsonId == jsonId);

            json.Name = name;

            await _context.SaveChangesAsync();
        }
    }
}
