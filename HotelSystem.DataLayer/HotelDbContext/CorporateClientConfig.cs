using HotelSystem.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.HotelDbContext
{
    public class CorporateClientConfig : IEntityTypeConfiguration<CorporateClient>
    {
        public void Configure(EntityTypeBuilder<CorporateClient> builder)
        {
            builder.Property(corporateClient => corporateClient.Account).IsRequired(false).HasMaxLength(20);

        }
    }
}
