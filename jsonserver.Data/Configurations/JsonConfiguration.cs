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

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(10);

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.Jsons)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
