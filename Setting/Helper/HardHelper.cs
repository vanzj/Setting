using GalaSoft.MvvmLight.Messaging;
using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Setting.Helper
{
    public class HardHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DispatcherTimer timer;

        int i = 0;
        private static List<float> cpuList = new List<float>();
        private static List<float> gpuList = new List<float>();
        static readonly object _object = new object();
        public static bool hasCPUUse { get; set; }
        public static bool hasGPUUse { get; set; }
        public static bool hasCPUTemp { get; set; }
        public static bool hasGPUTemp { get; set; }
        public static bool hasNetwork { get; set; }
        const int _1MB = 1048576;
        private static string NetworkName { get; set; }
        private static float NetworkMaxDown { get; set; }

        private static int index = 0;


        private static HardHelper instance = null;
        public static HardHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
                    {
                        if (instance == null)
                        {

                            instance = new HardHelper();
                        }
                    }
                }
                return instance;
            }
        }

        public void Start()
        {
            if (!timer.IsEnabled)
            {
                myComputer.Open();
                timer.Start(); // 启动计时器
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                HardInfoEvent hardInfoEvent = new HardInfoEvent();
                // 更新所有传感器
                foreach (var item in myComputer.Hardware)
                {

                    // 检查是否是Intel核显
                    if (item.HardwareType == HardwareType.GpuNvidia || item.HardwareType == HardwareType.GpuAmd)
                    {
                        item.Update();
                        // 遍历GPU的所有传感器
                        foreach (var sensor in item.Sensors)
                        {
                            // 查找GPU使用率传感器
                            if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                            {
                                gpuList.Add(sensor.Value.Value);

                                if (gpuList.Count() > 0 && index == 0)
                                {

                                    hardInfoEvent.GPUUse = (int)(gpuList.Sum() / gpuList.Count());

                                }
                                if (gpuList.Count() > 10)
                                {
                                    gpuList.RemoveAt(0);
                                }

                            }
                            if (sensor.SensorType == SensorType.Temperature && index == 0 && sensor.Name == "GPU Core")
                            {
                                hardInfoEvent.GPUTemp = (int)(sensor.Value);
                            }
                        }
                    }
                    if (item.HardwareType == HardwareType.Cpu)
                    {
                        item.Update();
                        // 遍历GPU的所有传感器
                        foreach (var sensor in item.Sensors)
                        {
                            // 查找GPU使用率传感器
                            if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                            {
                                cpuList.Add(sensor.Value.Value);

                                if (cpuList.Count() > 0 && index == 0)
                                {
                                    hardInfoEvent.CPUUse = (int)(cpuList.Sum() / cpuList.Count());
                                }
                                if (cpuList.Count() > 10)
                                {
                                    cpuList.RemoveAt(0);
                                }

                            }
                            if (sensor.SensorType == SensorType.Temperature && sensor.Name == "Core Average" && index == 0)
                            {
                                hardInfoEvent.CPUTemp = (int)(sensor.Value);
                            }
                        }
                    }

                    if (item.HardwareType == HardwareType.Network && item.Name == NetworkName)
                    {
                        item.Update();
                        // 遍历GPU的所有传感器
                        foreach (var sensor in item.Sensors)
                        {
                            // 查找GPU使用率传感器
                            if (sensor.SensorType == SensorType.Throughput && sensor.Name == "Upload Speed" && index == 0)
                            {


                                if (sensor.Value < _1MB)
                                {
                                    hardInfoEvent.UpLoad = (int)(sensor.Value / 1024);
                                    hardInfoEvent.UpLoadflag = ABCEnum.KB;
                                }
                                else
                                {
                                    hardInfoEvent.UpLoad = (int)(sensor.Value / _1MB);
                                    hardInfoEvent.UpLoadflag = ABCEnum.MB;
                                }

                            }
                            if (sensor.SensorType == SensorType.Throughput && sensor.Name == "Download Speed" && index == 0)
                            {

                                if (sensor.Value < _1MB)
                                {
                                    hardInfoEvent.DownLoad = (int)(sensor.Value / 1024);
                                    hardInfoEvent.DownLoadflag = ABCEnum.KB;
                                }
                                else
                                {
                                    hardInfoEvent.DownLoad = (int)(sensor.Value / _1MB);
                                    hardInfoEvent.DownLoadflag = ABCEnum.MB;
                                }

                            }
                        }
                    }

                }

                if (index == 0)
                {
                    Messenger.Default.Send(hardInfoEvent);
                }
                index++;
                if (index >= 10)
                {
                    index = 0;

                }
            }
            catch (Exception ex)
            {

                logger.Error(JsonConvert.SerializeObject(ex));
            }
            
           
        }

        public void End()
        {
            myComputer.Close();
            timer?.Stop();
        }
        public void Check()
        { // 创建一个新的计算机对象

            // 开始更新计算机对象以获取硬件信息
            myComputer.Open();
            foreach (var item in myComputer.Hardware)
            {
                if (item.HardwareType == HardwareType.Cpu)
                {
                    item.Update();

                    foreach (var sensor in item.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name == "CPU Total")
                        {
                            hasCPUUse = true;
                        }
                        else if (sensor.SensorType == SensorType.Temperature && sensor.Name == "Core Average")
                        {
                            hasCPUTemp = true;
                        }

                    }
                }
                else if (item.HardwareType == HardwareType.GpuNvidia || item.HardwareType == HardwareType.GpuAmd)
                {
                    item.Update();

                    foreach (var sensor in item.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name == "GPU Core")
                        {
                            hasGPUUse = true;
                        }
                        else if (sensor.SensorType == SensorType.Temperature && sensor.Name == "GPU Core")
                        {
                            hasGPUTemp = true;
                        }
                    }
                }
                else if (item.HardwareType == HardwareType.Network)
                {
                    item.Update();

                    foreach (var sensor in item.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Data && sensor.Name == "Data Downloaded")
                        {
                            if (sensor.Value > NetworkMaxDown)
                            {
                                NetworkMaxDown = sensor.Value.Value;
                                NetworkName = item.Name;
                            }
                        }

                    }
                    hasNetwork = true;
                }
            }
            myComputer.Close();
        }



        Computer myComputer = new Computer();
        private HardHelper()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // 设置计时器的时间间隔为1秒
            timer.Tick += Timer_Tick; ; // 订阅Tick事件
        
            //启动CPU监测
            myComputer.IsGpuEnabled = true;
            myComputer.IsCpuEnabled = true;
            myComputer.IsNetworkEnabled = true;

        }

     
    }

   

}
