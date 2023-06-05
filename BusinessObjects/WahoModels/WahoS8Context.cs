using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BusinessObjects.WahoModels
{
    public partial class WahoS8Context : DbContext
    {
        public WahoS8Context()
        {
        }

        public WahoS8Context(DbContextOptions<WahoS8Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillDetail> BillDetails { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<InventorySheet> InventorySheets { get; set; }
        public virtual DbSet<InventorySheetDetail> InventorySheetDetails { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Oder> Oders { get; set; }
        public virtual DbSet<OderDetail> OderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ReturnOrder> ReturnOrders { get; set; }
        public virtual DbSet<ReturnOrderProduct> ReturnOrderProducts { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<SubCategory> SubCategories { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<WahoInformation> WahoInformations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =ADMIN\\DUC; database = WahoS8;uid=sa;pwd=12345;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>(entity =>
            {
                entity.ToTable("Bill");

                entity.Property(e => e.BillId).HasColumnName("billID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BillStatus)
                    .HasMaxLength(50)
                    .HasColumnName("billStatus");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Descriptions)
                    .HasMaxLength(100)
                    .HasColumnName("descriptions");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("total");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Bill__customerID__45F365D3");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK__Bill__userName__44FF419A");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.Bills)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__Bill__wahoID__440B1D61");
            });

            modelBuilder.Entity<BillDetail>(entity =>
            {
                entity.HasKey(e => new { e.BillId, e.ProductId })
                    .HasName("PK__BillDeta__DF41323758EA39D3");

                entity.Property(e => e.BillId).HasColumnName("billID");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Bill)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.BillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillDetai__billI__48CFD27E");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.BillDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__BillDetai__produ__49C3F6B7");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("categoryName");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .HasColumnName("description");

                entity.Property(e => e.HaveDate).HasColumnName("haveDate");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Adress)
                    .HasMaxLength(50)
                    .HasColumnName("adress");

                entity.Property(e => e.CustomerName)
                    .HasMaxLength(150)
                    .HasColumnName("customerName");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .HasColumnName("email");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(50)
                    .HasColumnName("taxCode");

                entity.Property(e => e.TypeOfCustomer).HasColumnName("typeOfCustomer");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__Customers__wahoI__412EB0B6");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.UserName)
                    .HasName("PK__Employee__66DCF95D26E6DBDD");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .HasColumnName("address");

                entity.Property(e => e.Dob).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.EmployeeName)
                    .HasMaxLength(150)
                    .HasColumnName("employeeName");

                entity.Property(e => e.HireDate)
                    .HasColumnType("date")
                    .HasColumnName("hireDate");

                entity.Property(e => e.Note)
                    .HasMaxLength(250)
                    .HasColumnName("note");

                entity.Property(e => e.Password)
                    .HasMaxLength(50)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .HasColumnName("title");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__Employees__wahoI__36B12243");
            });

            modelBuilder.Entity<InventorySheet>(entity =>
            {
                entity.Property(e => e.InventorySheetId).HasColumnName("inventorySheetID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .HasColumnName("description");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.InventorySheets)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK__Inventory__userN__3A81B327");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.InventorySheets)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__Inventory__wahoI__398D8EEE");
            });

            modelBuilder.Entity<InventorySheetDetail>(entity =>
            {
                entity.HasKey(e => new { e.InventorySheetId, e.ProductId })
                    .HasName("PK__Inventor__30B6C9918379354D");

                entity.ToTable("InventorySheetDetail");

                entity.Property(e => e.InventorySheetId).HasColumnName("inventorySheetID");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.CurNwareHouse).HasColumnName("curNWareHouse");

                entity.HasOne(d => d.InventorySheet)
                    .WithMany(p => p.InventorySheetDetails)
                    .HasForeignKey(d => d.InventorySheetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__inven__3D5E1FD2");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.InventorySheetDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Inventory__produ__3E52440B");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.ToTable("Location");

                entity.Property(e => e.LocationId).HasColumnName("locationID");

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Oder>(entity =>
            {
                entity.Property(e => e.OderId).HasColumnName("oderID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Cod)
                    .HasMaxLength(10)
                    .HasColumnName("cod")
                    .IsFixedLength();

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.Deposit)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("deposit");

                entity.Property(e => e.EstimatedDate)
                    .HasColumnType("date")
                    .HasColumnName("estimatedDate");

                entity.Property(e => e.OderState)
                    .HasMaxLength(50)
                    .HasColumnName("oderState");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("date")
                    .HasColumnName("orderDate");

                entity.Property(e => e.Region)
                    .HasMaxLength(150)
                    .HasColumnName("region");

                entity.Property(e => e.ShipperId).HasColumnName("shipperID");

                entity.Property(e => e.Total)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("total");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Oders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Oders__customerI__5070F446");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.Oders)
                    .HasForeignKey(d => d.ShipperId)
                    .HasConstraintName("FK__Oders__shipperID__5165187F");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.Oders)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK__Oders__userName__4F7CD00D");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.Oders)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__Oders__wahoID__52593CB8");
            });

            modelBuilder.Entity<OderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OderId, e.ProductId })
                    .HasName("PK__OderDeta__1FC5D59E8A2151C3");

                entity.Property(e => e.OderId).HasColumnName("oderID");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Oder)
                    .WithMany(p => p.OderDetails)
                    .HasForeignKey(d => d.OderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OderDetai__oderI__5535A963");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.OderDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__OderDetai__produ__5629CD9C");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.DateOfManufacture)
                    .HasColumnType("date")
                    .HasColumnName("dateOfManufacture");

                entity.Property(e => e.Description)
                    .HasMaxLength(150)
                    .HasColumnName("description");

                entity.Property(e => e.Expiry)
                    .HasColumnType("date")
                    .HasColumnName("expiry");

                entity.Property(e => e.HaveDate).HasColumnName("haveDate");

                entity.Property(e => e.ImportPrice).HasColumnName("importPrice");

                entity.Property(e => e.InventoryLevelMax).HasColumnName("inventoryLevelMax");

                entity.Property(e => e.InventoryLevelMin).HasColumnName("inventoryLevelMin");

                entity.Property(e => e.LocationId).HasColumnName("locationID");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("productName");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.SubCategoryId).HasColumnName("subCategoryID");

                entity.Property(e => e.SupplierId).HasColumnName("supplierID");

                entity.Property(e => e.Trademark)
                    .HasMaxLength(50)
                    .HasColumnName("trademark");

                entity.Property(e => e.Unit)
                    .HasMaxLength(50)
                    .HasColumnName("unit");

                entity.Property(e => e.UnitPrice).HasColumnName("unitPrice");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("FK__Products__locati__30F848ED");

                entity.HasOne(d => d.SubCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SubCategoryId)
                    .HasConstraintName("FK__Products__subCat__31EC6D26");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .HasConstraintName("FK__Products__suppli__33D4B598");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__Products__wahoID__32E0915F");
            });

            modelBuilder.Entity<ReturnOrder>(entity =>
            {
                entity.Property(e => e.ReturnOrderId).HasColumnName("returnOrderID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.BillId).HasColumnName("billID");

                entity.Property(e => e.CustomerId).HasColumnName("customerID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.PaidCustomer)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("paidCustomer");

                entity.Property(e => e.PayCustomer)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("payCustomer");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .HasColumnName("userName");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.ReturnOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__ReturnOrd__custo__59FA5E80");

                entity.HasOne(d => d.UserNameNavigation)
                    .WithMany(p => p.ReturnOrders)
                    .HasForeignKey(d => d.UserName)
                    .HasConstraintName("FK__ReturnOrd__userN__59063A47");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.ReturnOrders)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__ReturnOrd__wahoI__5AEE82B9");
            });

            modelBuilder.Entity<ReturnOrderProduct>(entity =>
            {
                entity.HasKey(e => new { e.ReturnOrderId, e.ProductId })
                    .HasName("PK__ReturnOr__A5B843729E48CFE6");

                entity.ToTable("ReturnOrderProduct");

                entity.Property(e => e.ReturnOrderId).HasColumnName("returnOrderID");

                entity.Property(e => e.ProductId).HasColumnName("productID");

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ReturnOrderProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReturnOrd__produ__5DCAEF64");

                entity.HasOne(d => d.ReturnOrder)
                    .WithMany(p => p.ReturnOrderProducts)
                    .HasForeignKey(d => d.ReturnOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReturnOrd__retur__5EBF139D");
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.Property(e => e.ShipperId).HasColumnName("shipperID");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.ShipperName)
                    .HasMaxLength(50)
                    .HasColumnName("shipperName");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.Shippers)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__Shippers__wahoID__4CA06362");
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.Property(e => e.SubCategoryId).HasColumnName("subCategoryID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Description)
                    .HasMaxLength(250)
                    .HasColumnName("description");

                entity.Property(e => e.SubCategoryName)
                    .HasMaxLength(250)
                    .HasColumnName("subCategoryName");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.SubCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__SubCatego__categ__29572725");
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.Property(e => e.SupplierId).HasColumnName("supplierID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Address)
                    .HasMaxLength(150)
                    .HasColumnName("address");

                entity.Property(e => e.Branch)
                    .HasMaxLength(50)
                    .HasColumnName("branch");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(150)
                    .HasColumnName("companyName");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.TaxCode)
                    .HasMaxLength(50)
                    .HasColumnName("taxCode");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.HasOne(d => d.Waho)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.WahoId)
                    .HasConstraintName("FK__Suppliers__wahoI__2C3393D0");
            });

            modelBuilder.Entity<WahoInformation>(entity =>
            {
                entity.HasKey(e => e.WahoId)
                    .HasName("PK__WahoInfo__088B57FE5B709045");

                entity.ToTable("WahoInformation");

                entity.Property(e => e.WahoId).HasColumnName("wahoID");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.Address)
                    .HasMaxLength(250)
                    .HasColumnName("address");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Email)
                    .HasMaxLength(150)
                    .HasColumnName("email");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.WahoName)
                    .HasMaxLength(250)
                    .HasColumnName("wahoName");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.WahoInformations)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__WahoInfor__categ__267ABA7A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
