using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
   public class SerialPortHelper
    {
        public SerialPort SerialPort { get; set; }

        public bool InitCOM(string PortName)
        {
            SerialPort = new SerialPort(PortName, 115200, Parity.None, 8, StopBits.One);
            SerialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            SerialPort.ReceivedBytesThreshold = 1;
            SerialPort.RtsEnable = true;
            return OpenPort();
        }
        public bool OpenPort()
        {
            try
            {
                SerialPort.Open();
            }
            catch (Exception ex)
            {
         
                return false;
            }

            return SerialPort.IsOpen;
        }
        private void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(List<string> msg)
        {

        }
    }
}
