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
    public class SerialPortScanHelper
    {

        private SerialDevice MYSerialDevice { get; set; }
        private Dictionary<DeviceWatcher, String> mapDeviceWatchersToDeviceSelector;
        public List<COMInfo> COMInfoList;//

        private COMInfo CurrentCom { get; set; }
        static readonly object _object = new object();

        Timer ScanTimer;
        Thread t;
        bool isInit = true;




        private SerialPortScanHelper()
        {            // 设置WMI查询来监听串口设备的添加和移除事件
            mapDeviceWatchersToDeviceSelector = new Dictionary<DeviceWatcher, String>();
            COMInfoList = new List<COMInfo>();
            t = new Thread(TimerRead_Tick);
            t.Start();

            ScanTimer = new System.Timers.Timer();
            ScanTimer.Elapsed += Timer_Tick; ; // 订阅Tick事件
            ScanTimer.Interval = 1000; // 设置计时器的时间间隔为1秒
            ScanTimer.AutoReset = true;
            ScanTimer.Enabled = true;
     
            InitializeDeviceWatchers();
            StartDeviceWatchers();
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            CurrentCom = COMInfoList.FirstOrDefault(c => c.IsScreen == null);//找到一个没有询问过的串口
            MYSerialDevice?.Dispose();
            MYSerialDevice = null;
            if (CurrentCom != null)
            {
                InitCOM(CurrentCom.Id);
                CurrentCom.IsScreen = false;
            }
            else
            {//所有
                if (isInit)
                {
                    isInit = false;
                    Messenger.Default.Send(new FindScreenEvent { isLocal = true, DeviceInfos = new List<DeviceInfo>() {} }); ;

                }
                MYSerialDevice?.Dispose();
                MYSerialDevice = null;
                ScanTimer.Stop();
            }
        }
        private void StartDeviceWatchers()
        {
            // Start all device watchers
            foreach (DeviceWatcher deviceWatcher in mapDeviceWatchersToDeviceSelector.Keys)
            {
                if ((deviceWatcher.Status != DeviceWatcherStatus.Started)
                    && (deviceWatcher.Status != DeviceWatcherStatus.EnumerationCompleted))
                {
                    deviceWatcher.Start();
                }
            }
        }

        private void InitializeDeviceWatchers()
        {

            // Target all Serial Devices present on the system
            var deviceSelector = SerialDevice.GetDeviceSelector();
            var deviceWatcher = DeviceInformation.CreateWatcher(deviceSelector);
            AddDeviceWatcher(deviceWatcher, deviceSelector);
        }
        private void AddDeviceWatcher(DeviceWatcher deviceWatcher, String deviceSelector)
        {
            deviceWatcher.Added += new TypedEventHandler<DeviceWatcher, DeviceInformation>(this.OnDeviceAdded);
            deviceWatcher.Removed += new TypedEventHandler<DeviceWatcher, DeviceInformationUpdate>(this.OnDeviceRemoved);
            mapDeviceWatchersToDeviceSelector.Add(deviceWatcher, deviceSelector);
        }

        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            Messenger.Default.Send(new DebugInfoEvent($"OnDeviceRemoved{args.Id}"));
            var tempCominfo = COMInfoList.First(c => c.Id == args.Id);

            if (tempCominfo != null)
            {
                tempCominfo.Connected = false;
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 连接断开"));

                SerialPortHelper.Instance.Lost(args.Id);
            }
        }

        private void OnDeviceAdded(DeviceWatcher sender, DeviceInformation args)
        {
            Messenger.Default.Send(new DebugInfoEvent($"OnDeviceA   dded{args.Id}"));
            if (COMInfoList.All(c => c.Id != args.Id))
            {
                COMInfoList.Add(new COMInfo() { Id = args.Id });
                if (!ScanTimer.Enabled)
                {
                    ScanTimer.Start();
                }
            }
            var tempCominfo = COMInfoList.First(c => c.Id == args.Id);

            if (tempCominfo.IsScreen == true)
            {   // 连接过的串口
               //todo  重新连接。
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 恢复连接"));
                SerialPortHelper.Instance.ReConnect(args.Id);
            }
        }


        private string ReadMsg { get; set; }
        private void TimerRead_Tick()
        {
            do
            {
                try
                {

                    if (MYSerialDevice != null && MYSerialDevice != null)
                    {
                        var count = MYSerialDevice.BytesReceived;
                        var rBuffer = MYSerialDevice.InputStream.ReadAsync(new Windows.Storage.Streams.Buffer(50), 50, InputStreamOptions.None).GetAwaiter().GetResult(); ;
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


        private static SerialPortScanHelper instance = null;
        public static SerialPortScanHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
                    {
                        if (instance == null)
                        {

                            instance = new SerialPortScanHelper();
                        }
                    }
                }
                return instance;
            }
        }
        public void Init()
        {
            ScanTimer.Start();
            //启动串口监听。
        }
        private bool InitCOM(string COMID)
        {
            try
            {

                try
                {
                    MYSerialDevice = SerialDevice.FromIdAsync(COMID).GetAwaiter().GetResult(); ;
                    if (MYSerialDevice != null)
                    {
                        MYSerialDevice.BaudRate = 115200;
                        MYSerialDevice.Handshake = SerialHandshake.None;
                        MYSerialDevice.Parity = SerialParity.None;
                        MYSerialDevice.DataBits = 8;
                        MYSerialDevice.StopBits = SerialStopBitCount.One;
                        MYSerialDevice.ReadTimeout = TimeSpan.FromSeconds(0.1);
                        MYSerialDevice.WriteTimeout = TimeSpan.FromSeconds(10);
                        MYSerialDevice.IsRequestToSendEnabled = false;
                        MYSerialDevice.IsDataTerminalReadyEnabled = true;

                    }
                    SendgetMacSendMessage();

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 打开{COMID}成功"));
                return true;
            }
            catch (Exception ex)
            {
                Messenger.Default.Send(new SendEndEvent());
                Messenger.Default.Send(new DebugInfoEvent($"串口扫描==> 打开{COMID}失败，失败原因：{ex.Message}"));
                return false;
            }
        }

        private void SrialPort_DataReceived(string msgreturn)
        {

            try
            {
                Messenger.Default.Send(new DebugInfoEvent("接收消息<==  " + MYSerialDevice.PortName + "  " + msgreturn));
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
                                Messenger.Default.Send(new FindScreenEvent { isLocal = true, DeviceInfos = new List<DeviceInfo>() { new DeviceInfo() { DevNo = getMacmsg.data.Replace(":", ""), Name = "新屏幕", BlueNo = CurrentCom.Id } } }); ;
                                Messenger.Default.Send(new DebugInfoEvent("串口扫描=>  " + MYSerialDevice?.PortName + "连接成功"));
                                if (CurrentCom !=null)
                                {
                                    CurrentCom.IsScreen = true;
                                    CurrentCom.Mac = getMacmsg.data.Replace(":","");
                                }

                            }
                            else
                            {
                                Messenger.Default.Send(new DebugInfoEvent("串口扫描+==  " + MYSerialDevice.PortName + "未返回mac地址未能自动连接"));
                            }


                        }

                        break;
                    default:
                        Messenger.Default.Send(new DebugInfoEvent("串口未连接成功"));
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

                if (MYSerialDevice != null)
                {
                    MYSerialDevice?.Dispose();
                }
                t.Abort();

            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }


        public void SendgetMacSendMessage()
        {

            var cmd = new getMacSend();
            string msg = JsonConvert.SerializeObject(cmd);
            if (MYSerialDevice != null)
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
                    if (MYSerialDevice != null)
                    {

                        string str = Sendmsg + "**";



                        // send request cmd 
                        var sendDatas = Encoding.UTF8.GetBytes(str);
                        Messenger.Default.Send(new DebugInfoEvent($"发送消息==>  长度{sendDatas.Length}"));
                        var wBuffer = CryptographicBuffer.CreateFromByteArray(sendDatas);

                        var sw = MYSerialDevice.OutputStream.WriteAsync(wBuffer).GetAwaiter().GetResult();


                        Messenger.Default.Send(new DebugInfoEvent("发送消息==>  " + MYSerialDevice.PortName + Sendmsg + "**"));


                    }
                    else
                    {
                        Messenger.Default.Send(new DebugInfoEvent("发送消息==>  失败串口关闭" + MYSerialDevice?.PortName + Sendmsg + "**"));
                    }
                }
                else
                {
                    Messenger.Default.Send(new DebugInfoEvent("发送消息==>  失败串口忙" + MYSerialDevice.PortName + Sendmsg + "**"));
                }

            }
            catch (Exception ex)
            {

                Messenger.Default.Send(new DebugInfoEvent($"发送消息失败 ：{ex.ToString()}"));
            }
            finally
            {
                isSending = false;
            }

        }

    }

    public class COMInfo
    {
        /// <summary>
        ///  屏幕mac地址
        /// </summary>
        public string Mac { get; set; }
        /// <summary>
        /// 是否是屏幕
        /// </summary>
        public bool? IsScreen { get; set; }

        /// <summary>
        /// 设备Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        ///  串口连接状态。
        /// </summary>
        public bool Connected { get; set; } = true;

    }
}
