using jsonserver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace jsonserver.Data
{
    public class JsonServerContext: DbContext
    {
        public JsonServerContext(DbContextOptions<JsonServerContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
