using DotNetty.Common.Utilities;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Event;
using Setting.Model;
using Setting.Model.CMDModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using Timer = System.Timers.Timer;
namespace Setting.Helper
{
    public class ScanSerialPort
    {

        /// <summary>
        ///  屏幕mac地址
        /// </summary>
        public string DevNo { get; set; }
        /// <summary>
        /// 是否是屏幕
        /// </summary>
        public bool IsScreen { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///  串口连接状态。
        /// </summary>
        public bool Connected { get; set; } = true;

        private SerialDevice SerialDevice { get; set; }
        Thread t;
        Thread timeOut;
        public ScanSerialPort(string id)
        {
            this.Id = id;
            InitCOM();

        }
        int failcount = 0;

        private string ReadMsg { get; set; }
        private void timeOutThread()
        {
            Thread.Sleep(60*1000);
            DeviceRemoved();
        }
        private void TimerRead()
        {
            do
            {
                try
                {

                    if (SerialDevice != null && SerialDevice != null)
                    {
                        var count = SerialDevice.BytesReceived;
                        var rBuffer = SerialDevice.InputStream.ReadAsync(new Windows.Storage.Streams.Buffer(50), 50, InputStreamOptions.None).GetAwaiter().GetResult(); ;
                        string hexString = CryptographicBuffer.EncodeToHexString(rBuffer);

                        byte[] bytes = HexStringToByteArray(hexString);
                        string normalString = Encoding.UTF8.GetString(bytes);

                        ReadMsg += normalString;

                        ReadMsg = ReadMsg.Replace("\r", ""); ;

                        if (ReadMsg.Contains("\n"))
                        {

                            var templist = ReadMsg.Split('\n');
                            for (int i = 0; i < templist.Length - 1; i++)
                            {
                                SrialPort_DataReceived(templist[i]);
                            }
                            ReadMsg = templist[templist.Length - 1];
                        }
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {


                }

            } while (true);

        }


        static byte[] HexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
                throw new ArgumentException(hexString);

            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length; i += 2)
                bytes[i / 2] = System.Convert.ToByte(hexString.Substring(i, 2), 16);
            return bytes;
        }


       public void DeviceRemoved()
        {
            SerialDevice?.Dispose();
            SerialDevice = null;
            Connected = false;
            t?.Abort();
            timeOut?.Abort();
            SerialPortSendMsgHelper.Instance.ClosePort(Id);
        }

        private bool InitCOM()
        {
            try
            {
                try
                {
                    SerialDevice = SerialDevice.FromIdAsync(Id).GetAwaiter().GetResult(); ;
                    if (SerialDevice != null)
                    {
                        SerialDevice.BaudRate = 115200;
                        SerialDevice.Handshake = SerialHandshake.None;
                        SerialDevice.Parity = SerialParity.None;
                        SerialDevice.DataBits = 8;
                        SerialDevice.StopBits = SerialStopBitCount.One;
                        SerialDevice.ReadTimeout = TimeSpan.FromSeconds(0.1);
                        SerialDevice.WriteTimeout = TimeSpan.FromSeconds(10);
                        SerialDevice.IsRequestToSendEnabled = false;
                        SerialDevice.IsDataTerminalReadyEnabled = true;
                    }
                    Messenger.Default.Send(new DebugInfoEvent($"扫描：串口扫描==> 打开{Id}成功"));
                    t = new Thread(TimerRead);
                    timeOut = new Thread(timeOutThread);
                    t.Start();
                    timeOut.Start();
                    SendgetMacSendMessage();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                return true;
            }
            catch (Exception ex)
            {
               // Messenger.Default.Send(new SendEndEvent());
                Messenger.Default.Send(new DebugInfoEvent($"扫描：串口扫描==> 打开{Id}失败，失败原因：{ex.Message}"));
                return false;
            }
        }

        private void SrialPort_DataReceived(string msgreturn)
        {

            try
            {
                Messenger.Default.Send(new DebugInfoEvent("扫描：接收消息<==  " + SerialDevice.PortName + "  " + msgreturn));
                var msgobject = JsonConvert.DeserializeObject<ReturnBase<string>>(msgreturn);
                switch (msgobject.cmd)
                {
                    case "GetMac":
                    case "getMac":
                        if (msgobject.cmd == "getMac" || msgobject.cmd == "GetMac")
                        {

                            var getMacmsg = JsonConvert.DeserializeObject<getMacRetrun>(msgreturn);

                            if (!string.IsNullOrEmpty(getMacmsg.data))
                            {
                                Messenger.Default.Send(new DebugInfoEvent("扫描：串口扫描=>  " + SerialDevice?.PortName + "连接成功"));
                                SerialDevice?.Dispose();
                                SerialDevice = null;
                                IsScreen = true;
                                DevNo = getMacmsg.data.Replace(":", "");

                                Messenger.Default.Send(new FindScreenEvent { isLocal = true, DeviceInfos = new List<DeviceInfo>() { new DeviceInfo() { DevNo = getMacmsg.data.Replace(":", ""), Name = "新屏幕" + DevNo.Substring(0, 4) } } }); ;
                 
                            }
                            else
                            {
                                Messenger.Default.Send(new DebugInfoEvent("扫描：串口扫描+==  " + SerialDevice.PortName + "未返回mac地址未能自动连接"));
                            }


                        }

                        break;
                    default:
                        Messenger.Default.Send(new DebugInfoEvent("扫描：串口未连接成功"));
                        break;
                }
            }
            catch (Exception)
            {

            }
        }








        public bool ClosePort()
        {
            try
            {
                if (SerialDevice != null)
                {
                    SerialDevice?.Dispose();
                    SerialDevice = null;
                }
                t.Abort();
                timeOut.Abort();

            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }


        private void SendgetMacSendMessage()
        {

            var cmd = new getMacSend();
            string msg = JsonConvert.SerializeObject(cmd);

            if (SerialDevice != null)
            {
                Write(msg);
            }


        }

        bool isSending = false;
        private void Write(string Sendmsg)
        {
            try
            {
                if (isSending == false)
                {
                    isSending = true;
                    if (SerialDevice != null)
                    {

                        string str = Sendmsg + "**";
                        // send request cmd 
                        var sendDatas = Encoding.UTF8.GetBytes(str);
                        Messenger.Default.Send(new DebugInfoEvent($"扫描：发送消息==>  长度{sendDatas.Length}"));
                        var wBuffer = CryptographicBuffer.CreateFromByteArray(sendDatas);

                        var sw = SerialDevice.OutputStream.WriteAsync(wBuffer).GetAwaiter().GetResult();


                        Messenger.Default.Send(new DebugInfoEvent("扫描：发送消息==>  " + SerialDevice.PortName + Sendmsg + "**"));


                    }
                    else
                    {
                        Messenger.Default.Send(new DebugInfoEvent("扫描：发送消息==>  失败串口关闭" + SerialDevice?.PortName + Sendmsg + "**"));
                    }
                }
                else
                {
                    Messenger.Default.Send(new DebugInfoEvent("扫描：发送消息==>  失败串口忙" + SerialDevice.PortName + Sendmsg + "**"));
                }

            }
            catch (Exception ex)
            {

                Messenger.Default.Send(new DebugInfoEvent($"扫描：发送消息失败 ：{ex.ToString()}"));
            }
            finally
            {
                isSending = false;
            }

        }

    }


}
