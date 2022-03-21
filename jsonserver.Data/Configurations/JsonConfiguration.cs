using jsonserver.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace jsonserver.Data.Configurations
{
    public class JsonConfiguration : IEntityTypeConfiguration<Json>
    {
        public void Configure(EntityTypeBuilder<Json> builder)
        {
            builder.HasKey(x => x.JsonId);

            builder.Property(x => x.Content)
                .IsRequired()
                .HasDefaultValue("[]");

            builder.Property(x => x.UserId)
                .IsRequired();

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Jsons)
                .HasForeignKey(x => x.UserId);
        }
    }
}
