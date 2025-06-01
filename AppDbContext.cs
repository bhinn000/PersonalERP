using Microsoft.EntityFrameworkCore;
using PersonalERP.Entity;

namespace PersonalERP
{
    public class AppDbContext: DbContext
    {
        public DbSet<CraftsOrder> CraftsOrders { get; set; }
        public DbSet<ArtPiece> ArtPieces { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BuyPayment> BuyPayments { get; set; }
        public DbSet<BillPaymentCredit> BillPaymentCredits { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options) 
        {

        }
    }
}
