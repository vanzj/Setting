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
    public class SerialPortSendMsgHelper
    {

        List<SerialPortSendMsg> serialPortSendMsgs = new List<SerialPortSendMsg>();

        static readonly object _object = new object();
        static readonly object _objectInitCOM = new object();
        private SerialPortSendMsgHelper()
        {

       

        }

    
   


        private static SerialPortSendMsgHelper instance = null;
        public static SerialPortSendMsgHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
                    {
                        if (instance == null)
                        {

                            instance = new SerialPortSendMsgHelper();
                        }
                    }
                }
                return instance;
            }
        }

        public bool InitCOM(string DevNo)
        {
            var temp = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            if (temp == null)
            {
                lock (_objectInitCOM)
                {
                    temp = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
                    if (temp == null)
                    {
                       
                        if (!string.IsNullOrEmpty(DevNo))
                        {
                           var COMID = SerialPortScanHelper.Instance.SerialPortList.FirstOrDefault(c => c.DevNo == DevNo)?.Id;
                            if (string.IsNullOrEmpty(COMID))
                            {
                                return false;
                            }
                            SerialPortSendMsg serialPortSendMsg = new SerialPortSendMsg();
                            serialPortSendMsg.InitCOM(DevNo);
                            serialPortSendMsgs.Add(serialPortSendMsg);
                        }

                    }
                }
            }
            return true;
        }


        public void Lost(string COMID)
        {
            if (string.IsNullOrEmpty(COMID))
            {
                return ;
            }
            var  serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentComId == COMID);
            serialPortSendMsg?.Lost(COMID);
        }


        public bool ClosePort(string COMID)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentComId == COMID);
            if( serialPortSendMsg != null)
            {
                serialPortSendMsg?.ClosePort(COMID);
                TcpDefaultHelper.Instance.DisConnect(serialPortSendMsg.CurrentDevNo);
                serialPortSendMsgs.Remove(serialPortSendMsg);
            }
        
      
            return true;
        }
        public bool CloseAllPort()
        {
            try
            {
                foreach (var item in serialPortSendMsgs)
                {
                    item.ClosePort();
                }

            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }


        bool isSending = false;

        public void SendLuminanceSendMessage(int arge,string DevNo)
        {
           var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendLuminanceSendMessage(arge);
        }

        public void SendGetInfoSendMessage(string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendGetInfoSendMessage();
        }
        public void SendNetWork(string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendNetWork();
        }
        public void SendEnableRotateMessage(string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendEnableRotateMessage();
        }
        public void SendDisenableRotateMessage(string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendDisenableRotateMessage();
        }

        public void SendOpenSendMessage(string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendOpenSendMessage();
        }
        public void SendCloseSendMessage(string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendCloseSendMessage();
        }
        public void SendThemeCirculateSendMessage(List<string> cmdlist,string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendThemeCirculateSendMessage(cmdlist);
        }

        public void SendThemeDynamicSendMessage(List<string> cmdlist, string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendThemeDynamicSendMessage(cmdlist);
        }
        public void SendThemeSegmentSendMessage(List<string> cmdlist, string DevNo)
        {
            var serialPortSendMsg = serialPortSendMsgs.FirstOrDefault(c => c.CurrentDevNo == DevNo);
            serialPortSendMsg?.SendThemeSegmentSendMessage(cmdlist);
        }   
    }

   
}
