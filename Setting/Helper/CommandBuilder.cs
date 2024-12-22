using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management; // 需要添加对 System.Management 的引用
namespace Setting.Helper
{
    public static class CommandBuilder
    {
        private static string mac = "";

        public static byte[] BuildHeartCmd()
        {
            if (string.IsNullOrEmpty(mac))
            {
                mac = GetBiosUUID12();
            }

            return System.Text.Encoding.ASCII.GetBytes($"?HEART#{mac}**");



        }


        public static byte[] BuildGIFSuccessCmd()
        {
            if (string.IsNullOrEmpty(mac))
            {
                mac = GetBiosUUID12();
            }

            return System.Text.Encoding.ASCII.GetBytes($"!TRANSPARENTDATA#{mac}#00**");



        }

        public static string GetBiosUUID12()
        {
            var uuid = GetBiosUUID();
            if (uuid == null || uuid == "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF" || uuid.Length<12)
            {
                Guid guid = new Guid();
                // Step 2: Convert to string without hyphens (optional)
                string guidString = guid.ToString("N"); // "N" format specifier removes hyphens

                // Step 3: Extract the last 12 characters
                return  guidString.Substring(guidString.Length - 12);
            }
            return uuid.Substring(uuid.Length - 12);
        }
        public static string GetBiosUUID()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT UUID FROM Win32_BIOS"))
                {
                    foreach (ManagementObject bios in searcher.Get())
                    {
                        return bios["UUID"]?.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while getting BIOS UUID: {ex.Message}");
            }

            return null; // 如果没有找到或发生错误，则返回 null
        }


    }
}
