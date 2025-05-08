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

namespace Setting.Helper
{
    public class SerialPortSendMsg
    {

        public   SerialDevice MYSerialDevice { get; set; }
        public string CurrentDevNo { get; set; }

        private string currentCmd { get; set; }
        public string CurrentComId { get; set; }
      
        private int index { get; set; }
        private bool isFrist { get; set; }
        private List<string> msgList { get; set; }


        Thread t;
        Thread SendGetInfoSendMessageT;



        private string ReadMsg { get; set; }
   
        private void TimerRead_Tick()
        {
            do
            {
                try
                {

                    if (MYSerialDevice != null   )
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

     
       

        public bool InitCOM(string DevNo)
        {
            CurrentDevNo = DevNo;
            string COMID = "";
            MYSerialDevice?.Dispose();
            MYSerialDevice = null;
            t?.Abort();
            SendGetInfoSendMessageT?.Abort();
            try
            {

                COMID = SerialPortScanHelper.Instance.SerialPortList.FirstOrDefault(c => c.DevNo == DevNo)?.Id;

                if (string.IsNullOrEmpty(COMID))
                {
                    if (!string.IsNullOrEmpty(CurrentComId))
                    {
                        Messenger.Default.Send(new LostCurrentScreenEvent() { DevNo = DevNo });
                    }
                    return false;
                }

                if (CurrentComId == COMID&& MYSerialDevice!=null)
                {
                    return false;
                }
               
                try
                {

                    MYSerialDevice = SerialDevice.FromIdAsync(COMID).GetAwaiter().GetResult(); ;
                    if (MYSerialDevice != null)
                    {
                        CurrentComId = COMID;
                        MYSerialDevice.BaudRate = 115200;
                        MYSerialDevice.Handshake = SerialHandshake.None;
                        MYSerialDevice.Parity = SerialParity.None;
                        MYSerialDevice.DataBits = 8;
                        MYSerialDevice.StopBits = SerialStopBitCount.One;
                        MYSerialDevice.ReadTimeout = TimeSpan.FromSeconds(0.1);
                        MYSerialDevice.WriteTimeout = TimeSpan.FromSeconds(10);
                        MYSerialDevice.IsRequestToSendEnabled = false;
                        MYSerialDevice.IsDataTerminalReadyEnabled = true;
                        Messenger.Default.Send(new  DebugInfoEvent($"通信：==> 打开成功:{COMID}"));
                        t = new Thread(TimerRead_Tick);
                        SendGetInfoSendMessageT = new Thread(() =>
                        {
                            do
                            {
                                
                                    SendGetInfoSendMessage();
                                Thread.Sleep(5000);

                            } while (true);
                         
                        });
                        t.Start();
                        SendGetInfoSendMessageT.Start();

                        if (!string.IsNullOrEmpty(this.CurrentDevNo))
                        {
                            Messenger.Default.Send(new ConnectCurrentScreenEvent(this.CurrentDevNo));
                            TcpDefaultHelper.Instance.Connect(this.CurrentDevNo);
                        }
                    }
                    else
                    {
                        Messenger.Default.Send(new  DebugInfoEvent($"通信：==> 打开失败:{COMID}"));
                        CurrentComId = "";
                    }
                }
                catch (Exception ex)
                {
                    CurrentDevNo = "";
                    MYSerialDevice?.Dispose();
                    MYSerialDevice = null;
                    t?.Abort();
                    SendGetInfoSendMessageT?.Abort();
                    CurrentComId = "";
                    throw new Exception(ex.Message);
                }
        
                return true;
            }
            catch (Exception ex)
            {
                CurrentDevNo = "";
                Messenger.Default.Send(new SendEndEvent(this.CurrentDevNo));
                Messenger.Default.Send(new  DebugInfoEvent($"通信：==> 打开{COMID}失败，失败原因：{ex.Message}"));
                MYSerialDevice?.Dispose();
                MYSerialDevice = null;
                t?.Abort();
                SendGetInfoSendMessageT?.Abort();
                return false;
            }
        }

        private void SrialPort_DataReceived(string msgreturn)
        {

            try
            {
                Messenger.Default.Send(new DebugInfoEvent("通信：接收消息<==  " + MYSerialDevice?.PortName + "  " + msgreturn));
                var msgobject = JsonConvert.DeserializeObject<ReturnBase<object>>(msgreturn);
                switch (msgobject.cmd)
                {
                    case "GetMac":
                    case "getMac":
                        if (msgobject.cmd == "getMac" || msgobject.cmd == "GetMac")
                        {

                            var getMacmsg = JsonConvert.DeserializeObject<getMacRetrun>(msgreturn);
                            if (!string.IsNullOrEmpty(getMacmsg.data))
                            {
                                Messenger.Default.Send(new DebugInfoEvent("通信：串口扫描=>  " + MYSerialDevice?.PortName + "连接成功"));
    
                            }
                            else
                            {
                                Messenger.Default.Send(new DebugInfoEvent("通信：串口扫描+==  " + MYSerialDevice?.PortName + "未返回mac地址未能自动连接"));
                            }


                        }

                        break;
                    case "getInfo":
                        if (msgobject.cmd == "getInfo")
                        {

                            var GetInfoRetrun = JsonConvert.DeserializeObject<GetInfoRetrun>(msgreturn);
                            TcpDefaultHelper.Instance.Changebrightness ( int.Parse(GetInfoRetrun.data.luminance),GetInfoRetrun.data.mac.Replace(":", ""));
                            if (string.IsNullOrEmpty(this.CurrentDevNo))
                            {
                                this.CurrentDevNo = GetInfoRetrun.data.mac.Replace(":", "");
                                Messenger.Default.Send(new ConnectCurrentScreenEvent(this.CurrentDevNo));

                                TcpDefaultHelper.Instance.Connect(this.CurrentDevNo);
                            }

                            Messenger.Default.Send(new ScreenInfoEvent(GetInfoRetrun.data.status == "open", int.Parse(GetInfoRetrun.data.luminance), GetInfoRetrun.data.rotate == "1",GetInfoRetrun.data.mac.Replace(":", "")) );
                        }
                        break;
                    case "open":
                        Messenger.Default.Send(new MsgEvent("屏幕开启成功")); ;
                        break;
                    case "close":
                        Messenger.Default.Send(new MsgEvent("屏幕关闭成功")); 
                        break;
                    case "luminance":
                        Messenger.Default.Send(new MsgEvent("亮度调节成功")); 
                        break;
                    case "theme":
                        if (msgobject.cmd == currentCmd)
                        {
                            index++;
                            if (index < msgList.Count)
                            {
                                currentCmd = "themeSendStart";

                                Write(msgList[index]);
                            }

                        }
                        break;
                    case "themeSendStart":
                        if (msgobject.cmd == currentCmd)
                        {
                            var themeSendStartreturnmsg = JsonConvert.DeserializeObject<ThemeSendStartRetrun>(msgreturn);
                            if (!themeSendStartreturnmsg.HasDetail())
                            {
                                index++;
                                if (index < msgList.Count)
                                {
                                    currentCmd = "themeSegment";

                                    Write(msgList[index]);
                                }
                            }

                        }
                        break;
                    case "themeSegment":
                        if (msgobject.cmd == currentCmd)
                        {
                            index++;
                            if (index < msgList.Count)
                            {

                                Write(msgList[index]);
                            }
                            if (index >= msgList.Count)
                            {
                                if (index>1||isFrist)
                                {
                                    isFrist = false;
                                    currentCmd = "";
                                    Messenger.Default.Send(new MsgEvent("主题应用成功"));
                                }
                                Messenger.Default.Send(new SendEndEvent(this.CurrentDevNo));
                            }
                        }
                        break;
                    default:
                        Messenger.Default.Send(new DebugInfoEvent("通信：串口未连接成功"));
                        break;
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }


        public void Lost(string COMID)
        {
            if (CurrentComId == COMID)
            {
                MYSerialDevice?.Dispose();
                MYSerialDevice = null;
                t.Abort();
                SendGetInfoSendMessageT?.Abort();
                Messenger.Default.Send(new LostCurrentScreenEvent() { DevNo = this.CurrentDevNo });
                return ;
            }

        }


        public bool ClosePort(string COMID="")
        {
            try
            {
            
                if (CurrentComId == COMID)
                {
                    Messenger.Default.Send(new LostCurrentScreenEvent() { DevNo = this.CurrentDevNo });
                }
                CurrentComId = "";
                if (MYSerialDevice != null)
                {
                    MYSerialDevice?.Dispose();
                }
                t?.Abort();
                SendGetInfoSendMessageT?.Abort();
            }
            catch (Exception ex)
            {

                return false;
            }

            return true;
        }



        bool isSending = false;
        public void Write(string Sendmsg)
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
                        Messenger.Default.Send(new  DebugInfoEvent($"通信：通信：发送消息==>  长度{sendDatas.Length}"));
                        var wBuffer = CryptographicBuffer.CreateFromByteArray(sendDatas);

                        var sw = MYSerialDevice.OutputStream.WriteAsync(wBuffer).GetAwaiter().GetResult();


                        Messenger.Default.Send(new DebugInfoEvent("通信：发送消息==>  " + MYSerialDevice?.PortName + Sendmsg + "**"));


                    }
                    else
                    {
                        Messenger.Default.Send(new DebugInfoEvent("通信：发送消息==>  失败串口关闭" + MYSerialDevice?.PortName + Sendmsg + "**"));
                    }
                }
                else
                {
                    Messenger.Default.Send(new DebugInfoEvent("通信：发送消息==>  失败串口忙" + MYSerialDevice?.PortName + Sendmsg + "**"));
                }

            }
            catch (Exception ex)
            {

                Messenger.Default.Send(new  DebugInfoEvent($"通信：通信：发送消息失败 ：{ex.ToString()}"));
            }
            finally
            {
                isSending = false;
            }

        }
        public void SendLuminanceSendMessage(int arge)
        {
            var cmd = new LuminanceSend(arge.ToString());
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }

        public void SendGetInfoSendMessage( )
        {
            if (string.IsNullOrEmpty(currentCmd))
            {
                var cmd = new GetInfoSend();
                string msg = JsonConvert.SerializeObject(cmd);
                Write(msg);
            }
       
        }
        public void SendNetWork()
        {
            var cmd = new getMacSend();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public void SendEnableRotateMessage()
        {
            var cmd = new RotateSend("0");
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public void SendDisenableRotateMessage()
        {
            var cmd = new RotateSend("1");
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }

        public void SendOpenSendMessage()
        {
            var cmd = new OpenSend();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public void SendCloseSendMessage()
        {
            var cmd = new CloseSend();
            string msg = JsonConvert.SerializeObject(cmd);
            Write(msg);
        }
        public void SendThemeCirculateSendMessage(List<string> cmdlist)
        {
            var cmd = new ThemeSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }

        public void SendThemeDynamicSendMessage(List<string> cmdlist)
        {
            var cmd = new ThemeSend();
            isFrist = true;
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }
        public void SendThemeSegmentSendMessage(List<string> cmdlist)
        {

            var cmd = new ThemeSegmentSend();
            currentCmd = cmd.cmd;
            index = 0;
            msgList = cmdlist;
            Write(cmdlist[index]);
        }
    }

   
}
