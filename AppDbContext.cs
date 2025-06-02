using Microsoft.EntityFrameworkCore;
using PersonalERP.Entity;

namespace PersonalERP
{
    public class AppDbContext: DbContext
    {
        public DbSet<CraftsOrder> CraftsOrders { get; set; }
        public DbSet<ArtPiece> ArtPieces { get; set; }
        public DbSet<Customer> Customers { get; set; }
        //public DbSet<BuyPayment> BuyPayments { get; set; }
        public DbSet<BillPaymentCredit> BillPaymentCredits { get; set; }
        public DbSet<PayingOffCredit> PayingOffCredits { get; set; }
        //public DbSet<DateTimeEntity> DateTimeEntities { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ArtPiece>()
                .HasOne(a => a.CraftsOrder)
                .WithOne(c => c.Art)
                .HasForeignKey<ArtPiece>(a => a.CraftsOrderId)
                 .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<CraftsOrder>()
                .HasOne(c => c.Customer)
                .WithMany(cu => cu.CraftsOrders)
                .HasForeignKey(cu => cu.CustomerId)
                 .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BillPaymentCredit>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.BillPaymentCredits)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BillPaymentCredit>()
                .HasOne(b => b.CraftsOrder)
                .WithOne(c => c.BillPaymentCredit)
                .HasForeignKey<BillPaymentCredit>(b => b.CraftsOrderId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PayingOffCredit>()
                .HasOne(p => p.BillPaymentCredit)
                .WithMany(b => b.PayingOffCredits)
                .HasForeignKey(p => p.BPId)
               .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
