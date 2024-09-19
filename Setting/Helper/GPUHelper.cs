using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Setting.Helper
{
    public class GPUHelper
    {
        private DispatcherTimer timer;

        int i = 0;
        List<double> GPUList = new List<double>();
        List<float> TempList = new List<float>();

        public void Start()
        {
            if (!timer.IsEnabled)
            {
                timer.Start(); // 启动计时器

            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            myComputer.Reset();
 
            GPUList.Add(GetGPUUsage());
            TempList.Add(getGPU());
            if (i == 5)
            {
                i = 1;
                Messenger.Default.Send(new HardInfoEvent { Temp = (int)(TempList.Sum(c => c) / TempList.Count) ,Use = (int)(GPUList.Sum(c=>c)/ GPUList.Count)}) ;
                GPUList = new List<double>();
                TempList = new List<float>();
            }
            i++;
        }

        public void End()
        {
            timer?.Stop();
        }

        // 用于获得GPU信息
        PerformanceCounter[] counters;
        GPPUUpdateVisitor updateVisitor = new GPPUUpdateVisitor();
        Computer myComputer = new Computer();
        public GPUHelper()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100); // 设置计时器的时间间隔为1秒
            timer.Tick += Timer_Tick; ; // 订阅Tick事件
            myComputer.Open();
            //启动GPU监测
            myComputer.GPUEnabled = true;
            myComputer.Accept(updateVisitor);

        }

        // 返回所有核心的GPU的占用率的值
        public  double GetGPUUsage()
        {


            var templist = myComputer.Hardware[0].Sensors.Where(c => c.SensorType == SensorType.Load && c.Value.HasValue).ToList();




            return ((templist.Sum(c => c.Value.Value)) / templist.Count);
        }

        public float getGPU()
        {
            


            var templist = myComputer.Hardware[0].Sensors.Where(c => c.SensorType == SensorType.Temperature&& c.Value.HasValue).ToList();




            return ((templist.Sum(c => c.Value.Value) )/ templist.Count);

        }
    }

    public  class GPPUUpdateVisitor: IVisitor
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
