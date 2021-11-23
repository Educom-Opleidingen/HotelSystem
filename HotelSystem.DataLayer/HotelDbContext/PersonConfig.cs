using HotelSystem.DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelSystem.HotelDbContext
{
    public class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasKey(person => person.Id);
            builder.Property(person => person.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(person => person.LastName).IsRequired().HasMaxLength(50);
            builder.Property(person => person.Birthdate).HasColumnType("datetime2").IsRequired(false);

            builder.ToTable("People");
        }
    }
}