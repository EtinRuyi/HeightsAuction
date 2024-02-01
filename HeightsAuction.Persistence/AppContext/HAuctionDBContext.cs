using HeightsAuction.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HeightsAuction.Persistence.AppContext
{
    public class HAuctionDBContext : IdentityDbContext<AppUser>
    {
        public HAuctionDBContext(DbContextOptions<HAuctionDBContext> options) : base(options) { }

        public DbSet<Bid> Bids { get; set; }
        public DbSet<BiddingRoom> BiddingRooms { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<BidNotification> BidNotifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bids)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
