using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Setting.Helper
{
   public class LiveShowHelper
    {

    

        static readonly object _object = new object();


        private static LiveShowHelper instance = null;
        public static LiveShowHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (_object)
                    {
                        if (instance == null)
                        {

                            instance = new LiveShowHelper();
                        }
                    }
                }
                return instance;
            }
        }
        private LiveShowHelper() { }



        private DispatcherTimer timer;
        public void Start(int FramRate)
        {
            if (timer==null)
            {
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1000 / FramRate); // 设置计时器的时间间隔为1秒
                timer.Tick += Timer_Tick; ; // 订阅Tick事件
            }
            timer.Start(); // 启动计时器
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
           
                Messenger.Default.Send(new NextFrameEvent {  });
              
        }

        public void End()
        {
            timer?.Stop();
        }
    }
}
