using GalaSoft.MvvmLight.Messaging;
using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Setting.Helper
{
    public class HardwareHelper
    {
        private DispatcherTimer timer;


        public void Start()
        {
            if (!timer.IsEnabled)
            {
                timer.Start(); // 启动计时器

            }
        }
        private string networkHardName = "";
        private void Timer_Tick(object sender, EventArgs e)
        {

            HardInfoEvent hardInfoEvent = new HardInfoEvent();
            myComputer.Reset();
            foreach (var hardware in myComputer.Hardware)
            {

                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    // only fire the update when found
                    hardware.Update();

                    // loop through the data

             
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("Core Average"))
                        {
                            hardInfoEvent.CPUTemp = (int)sensor.Value.GetValueOrDefault();
                        }
          
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("CPU Total"))
                        {
                            hardInfoEvent.CPUUse = (int)sensor.Value.GetValueOrDefault();
                      
                        }
                    }
     
            
                }
                if (hardware.HardwareType == HardwareType.GpuAmd || hardware.HardwareType == HardwareType.GpuNvidia)
                {
                    // only fire the update when found
                    hardware.Update();
      
                    // loop through the data
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU Core"))
                        {
                            hardInfoEvent.GPUTemp = (int)sensor.Value.GetValueOrDefault();
                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Core Max"))
                        {
                            hardInfoEvent.GPUUse += (int)sensor.Value.GetValueOrDefault();
                        }

                    }

                }

                // Targets AMD & Nvidia GPUS
                if (hardware.HardwareType == HardwareType.GpuIntel)
                {
                    // only fire the update when found
                    hardware.Update();

                    // loop through the data
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU"))
                        {
                            // store
                            Console.WriteLine($"{hardware.Name}-{sensor.Name}-{sensor.SensorType}-{sensor.Value.GetValueOrDefault()}");
                            hardInfoEvent.GPUCoreUse += (int)sensor.Value.GetValueOrDefault();
                        }

                    }

                }
                if (hardware.HardwareType == HardwareType.Network )
                {
                    hardware.Update();
                    // loop through the data
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (networkHardName == "")
                        {
                            if (sensor.SensorType == SensorType.Data && sensor.Name.Contains("Data Downloaded"))
                            {
                                if ((int)sensor.Value.GetValueOrDefault() > 0)
                                {
                                    networkHardName = hardware.Name;
                                }
                            }
                        }

                        else
                        {
                            if (sensor.SensorType == SensorType.Throughput && sensor.Name.Contains("Upload Speed"))
                            {

                                hardInfoEvent.UpLoad = ((int)sensor.Value.GetValueOrDefault() / (1024));
                            }
                            if (sensor.SensorType == SensorType.Throughput && sensor.Name.Contains("Download Speed"))
                            {
                                hardInfoEvent.DownLoad = ((int)sensor.Value.GetValueOrDefault() / (1024));
                            }
                        }

                    }

                }
                if (hardInfoEvent.GPUCoreUse>hardInfoEvent.GPUUse)
                {
                    hardInfoEvent.GPUUse = hardInfoEvent.GPUCoreUse;

                }

                if(hardInfoEvent.GPUTemp == 0)
                {
                    hardInfoEvent.GPUTemp = hardInfoEvent.CPUTemp;
                }

                Messenger.Default.Send(hardInfoEvent);
            }


        }


     

        public void End()
        {
            timer?.Stop();
        }
        GPPUUpdateVisitor updateVisitor = new GPPUUpdateVisitor();
        Computer myComputer = new Computer();
        public HardwareHelper()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000); // 设置计时器的时间间隔为1秒
            timer.Tick += Timer_Tick; ; // 订阅Tick事件
            myComputer.Open();
            //启动GPU监测
            myComputer.IsGpuEnabled = true;
            myComputer.IsCpuEnabled = true;
            myComputer.IsNetworkEnabled = true;
            myComputer.Accept(updateVisitor);

        }


    }

    public class GPPUUpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware)
                subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }

    }

}