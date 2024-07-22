using Newtonsoft.Json;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setting.Helper
{
    public class CPUHelper
    {
        // 用于获得CPU信息
        PerformanceCounter[] counters;
        UpdateVisitor updateVisitor = new UpdateVisitor();
        Computer myComputer = new Computer();
        public CPUHelper()
        {
            
            myComputer.Open();

            //启动CPU监测
            myComputer.CPUEnabled = true;


            myComputer.Accept(updateVisitor);
        }

        // 返回所有核心的CPU的占用率的值
        public  double GetCPUUsage()
        {


            var templist = myComputer.Hardware[0].Sensors.Where(c => c.SensorType == SensorType.Load && c.Value.HasValue).ToList();




            return ((templist.Sum(c => c.Value.Value)) / templist.Count);
        }

        public float getCPU()
        {
            


            var templist = myComputer.Hardware[0].Sensors.Where(c => c.SensorType == SensorType.Temperature&& c.Value.HasValue).ToList();




            return ((templist.Sum(c => c.Value.Value) )/ templist.Count);

        }
    }

    public  class UpdateVisitor: IVisitor
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
