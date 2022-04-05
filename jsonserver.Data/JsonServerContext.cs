using jsonserver.Data.Configurations;
using jsonserver.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace jsonserver.Data
{
    public class JsonServerContext: DbContext
    {
        public JsonServerContext(DbContextOptions<JsonServerContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new JsonConfiguration());
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Json> Jsons { get; set; }
    }
}
