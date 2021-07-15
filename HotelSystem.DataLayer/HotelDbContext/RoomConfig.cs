using HotelSystem.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.HotelDbContext
{
    public class RoomConfig : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.HasKey(room => room.RoomId);
            builder.Property(room => room.Number).IsRequired().HasMaxLength(5);
            builder.Property(room => room.Type).IsRequired();

            builder.ToTable("Rooms");
        }
    }
}