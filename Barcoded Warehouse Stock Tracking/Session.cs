namespace Barcoded_Warehouse_Stock_Tracking
{
    public static class Session
    {
        public static long? UserId { get; set; }
        public static string Username { get; set; }
        public static string Role { get; set; } // "Admin" | "Cashier"

        public static bool IsLoggedIn => UserId.HasValue;

        public static bool IsAdmin => string.Equals(Role, "Admin");
    }
}

