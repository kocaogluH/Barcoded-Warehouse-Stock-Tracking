using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public static class Database
    {
        private static readonly string ConnectionString =
            ConfigurationManager.ConnectionStrings["WarehouseDb"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static void EnsureDatabase()
        {
            // Önce master'a bağlanıp veritabanını oluştur
            var masterConnStr = ConnectionString.Replace("Initial Catalog=BarcodedWarehouseDb", "Initial Catalog=master");
            using (var conn = new SqlConnection(masterConnStr))
            {
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'BarcodedWarehouseDb')
BEGIN
    CREATE DATABASE [BarcodedWarehouseDb]
END";
                cmd.ExecuteNonQuery();
            }

            // Şimdi asıl veritabanına bağlanıp tabloları oluştur
            using (var conn = GetConnection())
            {
                conn.Open();

                // Products tablosu
                var cmd = conn.CreateCommand();
                cmd.CommandText = @"
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Products')
BEGIN
    CREATE TABLE Products (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Barcode NVARCHAR(100) NOT NULL UNIQUE,
        Name NVARCHAR(200) NOT NULL,
        UnitPrice DECIMAL(18,2) NOT NULL
    )
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'StockMovements')
BEGIN
    CREATE TABLE StockMovements (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Barcode NVARCHAR(100) NOT NULL,
        Quantity INT NOT NULL,
        Type NVARCHAR(10) NOT NULL,
        Date DATETIME NOT NULL
    )
END
";
                cmd.ExecuteNonQuery();
            }
        }

        public static DataTable GetProducts()
        {
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Barcode, Name, UnitPrice FROM Products ORDER BY Name";
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static void InsertProduct(string barcode, string name, decimal unitPrice)
        {
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO Products(Barcode, Name, UnitPrice) VALUES(@b,@n,@p)";
                cmd.Parameters.AddWithValue("@b", barcode);
                cmd.Parameters.AddWithValue("@n", name);
                cmd.Parameters.AddWithValue("@p", unitPrice);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static bool ProductExists(string barcode)
        {
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(1) FROM Products WHERE Barcode = @b";
                cmd.Parameters.AddWithValue("@b", barcode);
                conn.Open();
                var count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        public static DataTable GetMovements()
        {
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT Date, Barcode, Type, Quantity FROM StockMovements ORDER BY Date DESC";
                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public static void InsertMovement(string barcode, int quantity, string type, DateTime date)
        {
            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "INSERT INTO StockMovements(Barcode, Quantity, Type, Date) VALUES(@b,@q,@t,@d)";
                cmd.Parameters.AddWithValue("@b", barcode);
                cmd.Parameters.AddWithValue("@q", quantity);
                cmd.Parameters.AddWithValue("@t", type);
                cmd.Parameters.AddWithValue("@d", date);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}

