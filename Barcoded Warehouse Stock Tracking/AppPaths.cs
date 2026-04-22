using System;
using System.IO;

namespace Barcoded_Warehouse_Stock_Tracking
{
    public static class AppPaths
    {
        public static string DataDirectory
        {
            get
            {
                var dir = AppDomain.CurrentDomain.GetData("DataDirectory") as string;
                if (string.IsNullOrWhiteSpace(dir))
                {
                    dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                }
                return dir;
            }
        }

        public static string DatabaseFilePath => Path.Combine(DataDirectory, "warehouse.db");
    }
}

