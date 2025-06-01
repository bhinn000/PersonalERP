using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalERP.Entity
{
    public class PayingOffCredit : DateTimeEntity
    {
        public int PaymentMethod { get; set; }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalBillPaid { get; set; }
        public decimal TotalBillRemaining { get; set; }
        public virtual Customer Customer { get; set; }
        //public int OrgId { get; set; }
        //public Organization Organization { get; set; }
        public int? BankId { get; set; }
    }
}
