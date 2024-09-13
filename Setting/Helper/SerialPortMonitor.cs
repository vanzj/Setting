using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
namespace Setting.Helper
{


    class SerialPortMonitor
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess,
            uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
            uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("Kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        private const uint GENERIC_READ = 0x80000000;
        private const uint OPEN_EXISTING = 3;
        private const int INVALID_HANDLE_VALUE = -1;

        public static bool IsSerialPortPresent(string portName)
        {
            string serialPortName = "\\\\.\\" + portName;
            IntPtr hSerial = CreateFile(serialPortName, GENERIC_READ, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);
            bool isPresent = (hSerial.ToInt32() != INVALID_HANDLE_VALUE);
            if (isPresent)
            {
                CloseHandle(hSerial);
            }
            return isPresent;
        }
    }
}
