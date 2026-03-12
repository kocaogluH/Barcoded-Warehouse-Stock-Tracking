using System;
using System.Collections.Generic;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public static class StockData
    {
        public static List<Product> Products { get; } = new List<Product>();
        public static List<StockMovement> Movements { get; } = new List<StockMovement>();
    }

    public class Product
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class StockMovement
    {
        public string Barcode { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; } // "Giriş" veya "Çıkış"
        public DateTime Date { get; set; }
    }
}

