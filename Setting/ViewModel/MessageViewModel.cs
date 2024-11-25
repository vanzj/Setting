using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Setting.Event;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Setting.ViewModel
{
  public  class MessageViewModel : ViewModelBase
    {
        DispatcherTimer timer;
        private ObservableCollection<MsgInfo> msgList;
        public ObservableCollection<MsgInfo> MsgList { get => msgList; set => Set(ref msgList, value); }

 

        private Visibility visibility;
        public Visibility Visibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                RaisePropertyChanged();
            }
        }

        public MessageViewModel()
        {
            Visibility = Visibility.Collapsed;
            MsgList = new ObservableCollection<MsgInfo>();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
            Messenger.Default.Register<MsgEvent>(this, HandlMsgEvent);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {

            List<int> RemoveIndexList = new List<int>();
            foreach (var item in MsgList)
            {
                item.Count--;
                if (item.Count<0)
                {
                    RemoveIndexList.Insert (0,msgList.IndexOf(item));
                }
            }
            foreach (var item in RemoveIndexList)
            {
                MsgList.RemoveAt(item);
            }
            if (MsgList.Count==0)
            {
                Visibility = Visibility.Collapsed;
            }
            else
            {
                Visibility = Visibility.Visible;
            }
        }

        private void HandlMsgEvent(MsgEvent obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (MsgList.Count>=3)
                {
                    MsgList.RemoveAt(0);
                }
                MsgList.Add(new MsgInfo(obj.Msg));
            });
        }
    }

    public class MsgInfo
    {
       public string Msg { get; set; }
        public int Count { get; set; }
    
    public  MsgInfo(string msg)
        {
            Msg = msg;
            Count = 3;
        }
    }

}
