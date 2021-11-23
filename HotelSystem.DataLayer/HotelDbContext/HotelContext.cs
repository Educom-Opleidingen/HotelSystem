using HotelSystem.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelSystem.HotelDbContext
{
    public class HotelContext : DbContext
    {
        private readonly string _connectionString;

        public DbSet<Client> Clients { get; set; }
        public DbSet<Room> Rooms { get; set; }

        public HotelContext() : this(@"Server=(localdb)\mssqllocaldb;Database=HotelDB;Trusted_Connection=True;")
        {

        }

        public HotelContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonConfig());
            modelBuilder.ApplyConfiguration(new ClientConfig());
            modelBuilder.ApplyConfiguration(new RoomConfig());
        }
    }
}