using GalaSoft.MvvmLight.Messaging;
using System;
using System.Timers;
using System.Windows.Threading;

namespace Setting.Helper
{
   public class LiveShowHelper
    {
        private Timer timer;
        public void Start(int FramRate)
        {
            if (timer==null)
            {
                timer = new Timer((1000 / FramRate));
                timer.Elapsed += Timer_Elapsed; ; // 订阅Tick事件
            }
            timer.Start(); // 启动计时器
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Messenger.Default.Send(new NextFrameEvent { });
        }


        public void End()
        {
            timer?.Stop();
        }
    }
}
