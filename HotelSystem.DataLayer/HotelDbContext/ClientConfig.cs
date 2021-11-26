using HotelSystem.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.HotelDbContext
{
    public class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasOne(client => client.Room).WithMany(room => room.Clients).OnDelete(DeleteBehavior.Cascade);

            //builder.ToTable("Clients");
            
        }
    }
}