using HotelSystem.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.HotelDbContext
{
    public class PrivateClientConfig : IEntityTypeConfiguration<PrivateClient>
    {
        public void Configure(EntityTypeBuilder<PrivateClient> builder)
        {
            builder.Property(privateClient => privateClient.EmailAddress).IsRequired(false).HasMaxLength(50);
            builder.Property(privateClient => privateClient.PhoneNumber).IsRequired(false).HasMaxLength(20);

        }
    }
}
