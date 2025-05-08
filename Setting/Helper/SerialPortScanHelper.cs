using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Event;
using Setting.Model;
using Setting.Model.CMDModel;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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

        private Dictionary<DeviceWatcher, String> mapDeviceWatchersToDeviceSelector;
        public List<ScanSerialPort> SerialPortList;//
        static readonly object _object = new object();

        private SerialPortScanHelper()
        {   // 设置WMI查询来监听串口设备的添加和移除事件
            mapDeviceWatchersToDeviceSelector = new Dictionary<DeviceWatcher, String>();
            Messenger.Default.Register<RemoveScreenInfoEvent>(this, HandleRemoveScreenInfoEvent);
            SerialPortList = new List<ScanSerialPort>();
            InitializeDeviceWatchers();
            StartDeviceWatchers();
        }

        private void HandleRemoveScreenInfoEvent(RemoveScreenInfoEvent @event)
        {
            
            var tempCominfo = SerialPortList.FirstOrDefault(c => c.DevNo == @event.CurrentDevInfo.DeviceInfo.DevNo);
            if (tempCominfo != null)
            {
                 SerialPortList.Remove(tempCominfo);
            }
        }

        private void StartDeviceWatchers()
        {
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
            Messenger.Default.Send(new  DebugInfoEvent($"扫描：OnDeviceRemoved{args.Id}"));
            if (!args.Id.Contains("VID_303A&PID_1001"))
            {
                return;
            }
            var tempCominfo = SerialPortList.FirstOrDefault(c => c.Id == args.Id);
            if (tempCominfo != null)
            {

                tempCominfo.DeviceRemoved();
              
                Messenger.Default.Send(new LostScreenEvent() { DeviceInfos = new List<DeviceInfo>() { new DeviceInfo() { DevNo = tempCominfo.DevNo } } });
                if (string.IsNullOrEmpty(tempCominfo.DevNo))
                {
                    SerialPortList.Remove(tempCominfo);
                }
            }
        }

        private void OnDeviceAdded(DeviceWatcher sender, DeviceInformation args)
        {
            Messenger.Default.Send(new  DebugInfoEvent($"扫描：OnDeviceA   dded{args.Id}"));
            if (!args.Id.Contains("VID_303A&PID_1001"))
            {
                return;
            }
            if (SerialPortList.All(c => c.Id != args.Id))
            {
                SerialPortList.Add(new ScanSerialPort(args.Id));
            }
            else
            {
                //var tempCominfo = SerialPortList.First(c => c.Id == args.Id);
                //tempCominfo.DeviceReConnect();
                var SerialPort = SerialPortList.FirstOrDefault(c => c.Id == args.Id);
                SerialPort.Connected = true;
                Messenger.Default.Send(new ConnectScreenEvent() { DeviceInfos = new List<DeviceInfo>() { new DeviceInfo() { DevNo = SerialPort.DevNo } } });
            }
           
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

      

 
        public bool ClosePort()
        {
            try
            {
                foreach (var item in SerialPortList)
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



    }

    
}
