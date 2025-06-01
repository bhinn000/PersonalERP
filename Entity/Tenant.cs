//namespace PersonalERP.Entity
//{
//    public class Organization : DateTimeEntity
//    {
//        public Organization()
//        {
//            BillPaymentCredit=new HashSet<BillPaymentCredit>();
//            PayingOffCredits=new HashSet<PayingOffCredit>();
//            Customers=new HashSet<Customer>();

//        }
//        public int Id { get; set; }
//        public string Name { get; set; } = null!;
//        public string OrgCode { get; set; } = null!;
//        //public string? GoogleLogoPath { get; set; }
//        //public string? GoogleBannerPath { get; set; }
//        //public string? Logo { get; set; }
//        //public string? Banner { get; set; }
//        //public string? TagLine { get; set; }
//        //public string Address1 { get; set; } = null!;
//        //public string? Address2 { get; set; }
//        //public string Tel { get; set; } = null!;
//        //public string? ContactNo { get; set; }
//        public string Email { get; set; } = null!;
//        //public string? Website { get; set; }
//        //public string? AdminEmail { get; set; }
//        //public bool IsActive { get; set; }
//        //public string? AboutUs { get; set; }
//        //public bool IsPaid { get; set; } = false;
//        //public bool IsFestive { get; set; } = false;
//        //public bool IsApproved { get; set; } = false;
//        //public bool HasOMS { get; set; } = false;
//        //public bool HasSmsBalance { get; set; } = false;
//        //public bool HasPaymentMethod { get; set; } = false;
//        //public bool HasSalesScreen { get; set; } = false;
//        //public string? FaceBookLink { get; set; }
//        //public string? InstagramLink { get; set; }
//        //public int? ThemeId { get; set; } // Foreign key to Themes
//        //public string? CloudR2ImagePath { get; set; }
//        //public string? CloudR2BannerImagePath { get; set; }
//        //public decimal VATPercentage { get; set; } = 0;
//        //public bool DisableBotInCutsomer { get; set; } = false;
//        //public bool HasPOS { get; set; } = false;
//        //public decimal ServiceChargePercentage { get; set; } = 0;
//        //public string PrintPort { get; set; } = null!;
//        //public virtual Themes? Theme { get; set; }
//        //public virtual ICollection<OrganizationTradingHour> TradingHours { get; set; }
//        ////public virtual OrgService? Service { get; set; }
//        //public virtual ICollection<OrganizationUser> Users { get; set; }
//        //public virtual ICollection<MenuCategory> MenuCategories { get; set; }
//        //public virtual ICollection<MenuItem> MenuItems { get; set; }
//        //public virtual ICollection<UserMessage> UserMessages { get; set; }
//        //public virtual ICollection<MenuOrder>? MenuOrders { get; set; }
//        //public virtual GoogleProcessingOrganization GoogleProcessingOrganization { get; set; }
//        //public virtual ICollection<CloudR2ProcessingOrg> CloudR2ProcessingOrg { get; set; }
//        //public virtual ICollection<CloudR2ProcessingOrgBanner> CloudR2ProcessingOrgBanner { get; set; }
//        //public virtual ICollection<Discounts> Discounts { get; set; }
//        //public virtual ICollection<MenuIdentity> MenuIdentities { get; set; }
//        //public virtual ICollection<Purchase> Purchases { get; set; }
//        //public virtual ICollection<ExpensesCategory> ExpensesCategories { get; set; }//one org can have multiple expensescategories
//        //public virtual ICollection<ExpTxn> ExpTxns { get; set; }
//        //public virtual ICollection<ExpensesHeader> ExpensesHeaders { get; set; }
//        //public virtual ICollection<Bank> Banks { get; set; }
//        //public virtual ICollection<CounterCashTransaction> CounterCashTransactions { get; set; }
//        //public virtual ICollection<Supplier> Suppliers { get; set; }
//        //public virtual ICollection<SupplierPaymentLog> SupplierPaymentLogs { get; set; }
//        public virtual ICollection<BillPaymentCredit> BillPaymentCredit { get; set; }
//        public virtual ICollection<PayingOffCredit> PayingOffCredits { get; set; }
//        public virtual ICollection<Customer> Customers { get; set; }
//    }
//}

