using System;
using System.Data.Entity;
using System.Data.SQLite;
using SQLite.CodeFirst;
using Barcoded_Warehouse_Stock_Tracking.Entities;

namespace Barcoded_Warehouse_Stock_Tracking.DataAccess
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext() : base(GetConnection(), true)
        {
            try
            {
                Database.ExecuteSqlCommand(@"
                    CREATE TABLE IF NOT EXISTS Logs (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Level TEXT,
                        Action TEXT,
                        Details TEXT,
                        UserId INTEGER,
                        Timestamp TEXT
                    );
                    CREATE TABLE IF NOT EXISTS StockMovements (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        ProductId INTEGER NOT NULL,
                        BarcodeSnapshot TEXT,
                        Type TEXT,
                        Quantity INTEGER NOT NULL,
                        Reason TEXT,
                        RefType TEXT,
                        RefId INTEGER,
                        CreatedByUserId INTEGER,
                        CreatedAt TEXT
                    );
                ");

                // Varsayılan onarma kodumuz tabloyu daha evvel RefId olmadan yarattıysa diye güncelleme atılır
                try { Database.ExecuteSqlCommand("ALTER TABLE StockMovements ADD COLUMN RefId INTEGER;"); } catch { }
            }
            catch { }
        }

        private static SQLiteConnection GetConnection()
        {
            // We read the connection string from App.config or fallback
            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["WarehouseDb"]?.ConnectionString;
            if (string.IsNullOrEmpty(connStr))
                connStr = "Data Source=warehouse.db;Version=3;";
            return new SQLiteConnection(connStr);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Initialize SQLite DB using CodeFirst
            var sqliteConnectionInitializer = new SqliteCreateDatabaseIfNotExists<WarehouseContext>(modelBuilder);
            System.Data.Entity.Database.SetInitializer(sqliteConnectionInitializer);
            
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StockMovement> StockMovements { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CustomerTransaction> CustomerTransactions { get; set; }
        public DbSet<CustomerCollection> CustomerCollections { get; set; }
        public DbSet<SaleReturn> SaleReturns { get; set; }
        public DbSet<SaleReturnItem> SaleReturnItems { get; set; }
        public DbSet<Log> Logs { get; set; }
    }
}
