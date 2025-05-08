
using GalaSoft.MvvmLight.Messaging;
using GIfTool;
using Setting.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Timer = System.Timers.Timer;


namespace Setting.Helper
{
    public class TcpDefaultHelper
    {

        public List<TcpDefault> tcpDefaults = new List<TcpDefault>();





        static readonly object _object = new object();

        static readonly object _addobject = new object();

        private TcpDefaultHelper()
        {


        }
        public void Changebrightness(int brightness,string DevNo)
        {
            var tcp = tcpDefaults.FirstOrDefault(c => c.DevNo == DevNo);
            if (tcp != null) {
                lock (_object)
                {
                    
                    tcp.brightness = brightness;
                
                }

            }

        }

        public void Connect(string DevNo)
        {
            var tcp = tcpDefaults.FirstOrDefault(c => c.DevNo == DevNo);
            if (tcp == null)
            {
                lock (_object)
                {
                     tcp = tcpDefaults.FirstOrDefault(c => c.DevNo == DevNo);
                    if (tcp == null)
                    { 
                        tcp = new TcpDefault(DevNo);
                        tcpDefaults.Add(tcp);
                    }
                }
            }
        }
        public void DisConnect(string DevNo)
        {
            var tcp = tcpDefaults.FirstOrDefault(c => c.DevNo == DevNo);
            if (tcp != null)
            {
                lock (_object)
                {
                    tcp.DisConnect();
                    tcpDefaults.Remove(tcp);
                    
                }
            }
        }
        private static TcpDefaultHelper instance = null;
        public static TcpDefaultHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
                    {
                        if (instance == null)
                        {

                            instance = new TcpDefaultHelper();
                        }
                    }
                }
                return instance;
            }
        }

    }



}
