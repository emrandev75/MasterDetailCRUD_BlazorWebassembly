using Microsoft.EntityFrameworkCore;
using work_01.Shared.Models;

namespace work_01.Server
{
    public class TourDbContext : DbContext
    {
        public TourDbContext(DbContextOptions<TourDbContext> options) : base(options) { }
        public DbSet<Customer> Customers { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<BookingItem> BookingItems { get; set; } = default!;
        public DbSet<Spot> Spots { get; set; } = default!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingItem>().HasKey(o => new { o.BookingID, o.SpotID });
        }
    }
}
