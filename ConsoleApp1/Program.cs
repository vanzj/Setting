
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;


namespace LibdotnetTest
{
    class Program
    {
  
  
        static void Main(string[] args)
        {
            var templist = ScanDeviceComportsAsync().GetAwaiter().GetResult();

            
                ConnectToDeviceAsync("COM5").GetAwaiter().GetResult();
            var pid =  deviceSerial.UsbProductId;
            var vid = deviceSerial.UsbVendorId;

            byte[] byteArray = System.Text.Encoding.Default.GetBytes("{\"cmd\":\"getMac\",\"data\":\"\"}**");
            var wBuffer = CryptographicBuffer.CreateFromByteArray(byteArray);
            var sw =  deviceSerial.OutputStream.WriteAsync(wBuffer).GetAwaiter().GetResult(); ;
            var rBuffer =  deviceSerial.InputStream.ReadAsync(
        new Windows.Storage.Streams.Buffer(12), 12, InputStreamOptions.None).GetAwaiter().GetResult();


            Console.ReadLine();
          // deviceSerial.Dispose();
        }

   
        


     
            public static async Task<List<string>> ScanDeviceComportsAsync()
            {
                string aqs = SerialDevice.GetDeviceSelector();
                var deviceCollection = await DeviceInformation.FindAllAsync(aqs);
                List<string> portNamesList = new List<string>();
                foreach (var item in deviceCollection)
                {
                    var serialDevice = await SerialDevice.FromIdAsync(item.Id);
                    var portName = serialDevice.PortName;
                    portNamesList.Add(portName);
                    serialDevice.Dispose();
                }
                return portNamesList;
            }

  
        private static SerialDevice deviceSerial;
        public static async Task ConnectToDeviceAsync(string comport)
        {
            DeviceInformationCollection serialDeviceInfos =
                await DeviceInformation.FindAllAsync(SerialDevice.GetDeviceSelector(comport));

            try
            {
                deviceSerial = await SerialDevice.FromIdAsync(serialDeviceInfos[0].Id);

                if (deviceSerial != null)
                {
                    deviceSerial.BaudRate = 115200;
                    deviceSerial.Handshake = SerialHandshake.None;
                    deviceSerial.Parity = SerialParity.None;
                    deviceSerial.DataBits = 8;
                    deviceSerial.StopBits = SerialStopBitCount.One;
                    deviceSerial.ReadTimeout = TimeSpan.FromSeconds(0.1);
                    deviceSerial.WriteTimeout = TimeSpan.FromSeconds(0.1);
                    deviceSerial.IsRequestToSendEnabled = false;
                    deviceSerial.IsDataTerminalReadyEnabled = true;

                    deviceSerial.ErrorReceived += DeviceSerial_ErrorReceived;
                    Connected = true;
                }
                else
                    Connected = false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static void DeviceSerial_ErrorReceived(SerialDevice sender, ErrorReceivedEventArgs args)
        {
            ; 
        }
    }

}
 