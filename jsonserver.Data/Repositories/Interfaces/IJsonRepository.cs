using jsonserver.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace jsonserver.Data.Repositories.Interfaces
{
    public interface IJsonRepository
    {
        Task<Json> GetByNameAsync(string name);

        Task AddAsync(Json json);

        Task<List<Json>> GetAllAsync();

        Task DeleteByIdAsync(int jsonId);

        Task EditNameAsync(int jsonId, string name);
    }
}
