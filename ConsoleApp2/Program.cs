using System;
using System.Threading;
using LibreHardwareMonitor.Hardware;

namespace PPSU_hwmonitor_c
{
    class Program
    {
        /**
         *  Define vars to hold stats
         **/

        // CPU Temp
        static float cpuTemp;
        // CPU Usage
        static float cpuUsage;
        // CPU Power Draw (Package)
        static float cpuPowerDrawPackage;
        // CPU Frequency
        static float cpuFrequency;
        // GPU Temperature
        static float gpuTemp;
        // GPU Usage
        static float gpuUsage;
        // GPU Core Frequency
        static float gpuCoreFrequency;
        // GPU Memory Frequency
        static float gpuMemoryFrequency;

        /**
         * Init OpenHardwareMonitor.dll Computer Object
         **/

        static Computer c = new Computer()
        {
            IsGpuEnabled = true,
            IsCpuEnabled = true,
            IsNetworkEnabled = true,
            //RAMEnabled = true, // uncomment for RAM reports
            //MainboardEnabled = true, // uncomment for Motherboard reports
            //FanControllerEnabled = true, // uncomment for FAN Reports
            //HDDEnabled = true, // uncomment for HDD Report
        };

        /**
         * Pulls data from OHM
         **/

        static void ReportSystemInfo()
        {
            foreach (var hardware in c.Hardware)
            {

                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    // only fire the update when found
                    hardware.Update();

                    // loop through the data
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("CPU Package"))
                        {
                            Console.WriteLine($"{hardware.Name}-{sensor.Name}-{sensor.SensorType}-{sensor.Value.GetValueOrDefault()}");

                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("CPU Total"))
                        {

                            Console.WriteLine($"{hardware.Name}-{sensor.Name}-{sensor.SensorType}-{sensor.Value.GetValueOrDefault()}");

                        }
                
                       
                    }



                  
                }


                // Targets AMD & Nvidia GPUS
                if (hardware.HardwareType == HardwareType.GpuIntel || hardware.HardwareType == HardwareType.GpuAmd||hardware.HardwareType == HardwareType.GpuNvidia)
                {
                    // only fire the update when found
                    hardware.Update();
                   
                    // loop through the data
                    foreach (var sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Name.Contains("GPU Core"))
                        {
                            Console.WriteLine($"{hardware.Name}-{sensor.Name}-{sensor.SensorType}-{sensor.Value.GetValueOrDefault()}");
                        }
                        else if (sensor.SensorType == SensorType.Load && sensor.Name.Contains("GPU Core"))
                        {
                            // store
                            Console.WriteLine($"{hardware.Name}-{sensor.Name}-{sensor.SensorType}-{sensor.Value.GetValueOrDefault()}");
                        }
                      
                    }

                }
                if (hardware.HardwareType == HardwareType.Network )
                {
                    // only fire the update when found
                    hardware.Update();

                    // loop through the data
                    foreach (var sensor in hardware.Sensors)
                    {
                        Console.WriteLine($"{hardware.Name}-{sensor.Name}-{sensor.SensorType}-{sensor.Value.GetValueOrDefault()}");
                    }

                }
                // ... you can access any other system information you want here

            }
        }

        static void Main(string[] args)
        {
            c.Open();

            ReportSystemInfo();
            Console.ReadLine();
        }
    }
}