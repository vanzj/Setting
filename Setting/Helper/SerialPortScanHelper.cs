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

        private Dictionary<DeviceWatcher, String> mapDeviceWatchersToDeviceSelector;
        public List<MYSerialPort> SerialPortList;//
        static readonly object _object = new object();

        private SerialPortScanHelper()
        {   // 设置WMI查询来监听串口设备的添加和移除事件
            mapDeviceWatchersToDeviceSelector = new Dictionary<DeviceWatcher, String>();
            SerialPortList = new List<MYSerialPort>();
            InitializeDeviceWatchers();
            StartDeviceWatchers();
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
            var tempCominfo = SerialPortList.FirstOrDefault(c => c.Id == args.Id);
            if (tempCominfo != null)
            {
                Messenger.Default.Send(new LostScreenEvent() { DeviceInfos = new List<DeviceInfo>() { new DeviceInfo() { BlueNo = args.Id } } });
                tempCominfo.DeviceRemoved();
            }
        }

        private void OnDeviceAdded(DeviceWatcher sender, DeviceInformation args)
        {
            if (!args.Name.Contains("ESP32-S3"))
            {
                return;
            }

            Messenger.Default.Send(new  DebugInfoEvent($"扫描：OnDeviceA   dded{args.Id}"));
            Messenger.Default.Send(new ConnectScreenEvent() { DeviceInfos = new List<DeviceInfo>() { new DeviceInfo() { BlueNo = args.Id } } });
            if (SerialPortList.All(c => c.Id != args.Id))
            {
                SerialPortList.Add(new MYSerialPort(args.Id));
            }
            else
            {
                var tempCominfo = SerialPortList.First(c => c.Id == args.Id);
                tempCominfo.DeviceReConnect();
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
