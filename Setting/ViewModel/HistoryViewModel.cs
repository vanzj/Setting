using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Event;
using Setting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Setting.ViewModel
{
    public class HistoryViewModel : ViewModelBase
    {

       private bool reBackIsEnabled;

        public bool ReBackIsEnabled
        {
            get { return reBackIsEnabled; }
            set
            {
                reBackIsEnabled = value;
                RaisePropertyChanged();
            }
        }


        private bool cancelIsEnabled;

        public bool CancelIsEnabled
        {
            get { return cancelIsEnabled; }
            set
            {
                cancelIsEnabled = value;
                RaisePropertyChanged();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public HistoryItem InitItem { get; set; }

        /// <summary>
        /// 撤回列表
        /// </summary>
        public Stack<HistoryItem> Cancel { get; set; }

        /// <summary>
        /// 重做列表
        /// </summary>
        public Stack<HistoryItem> ReBack { get; set; }

        public HistoryViewModel()
        {
            Cancel = new Stack<HistoryItem>();
            ReBack = new Stack<HistoryItem>();

            Messenger.Default.Register<HistroyAddEvent>(this, HanderHistroyAddEvent);
            Messenger.Default.Register<HistroyInitEvent>(this, HanderHistroyInitEvent);
            Messenger.Default.Register<LiveStartEvent>(this, HanderLiveStartEvent);
    
        }

       
        private void HanderLiveStartEvent(LiveStartEvent obj)
        {
            CancelIsEnabled = false;
            ReBackIsEnabled = false;
        }

        private void HanderHistroyInitEvent(HistroyInitEvent obj)
        {
            Cancel.Clear();
            ReBack.Clear();
            InitItem = obj.HistoryItem;
            Cancel.Push(InitItem);
            CancelIsEnabled = false;
            ReBackIsEnabled = false;
        }

        private void HanderHistroyAddEvent(HistroyAddEvent obj)
        {
            ReBack.Push( new HistoryItem()
            {
               
                ShowPointItems =   obj.HistoryItem.ShowPointItems,
            });
            Cancel.Clear();
            CancelIsEnabled = false;
            ReBackIsEnabled =  true;
        }

       

        public RelayCommand ReBackHistoryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var pop = ReBack.Pop();
                    Cancel.Push(pop);
                    CancelIsEnabled = true;
                    if (ReBack.Count==0)
                    {
                        ReBackIsEnabled = false;
                    }
                    
                    Messenger.Default.Send<InitFromHistroyEvent>(new InitFromHistroyEvent() { HistoryItem = Cancel.Peek(),HistoryEnum= HistoryEnum.ReBack });
                });
            }
        }
        public RelayCommand CancelHistoryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var pop = Cancel.Pop();
                    ReBack.Push(pop);
                    ReBackIsEnabled = true;
                    if (Cancel.Count == 0)
                    {
                        CancelIsEnabled = false;
                    }
                    Messenger.Default.Send<InitFromHistroyEvent>(new InitFromHistroyEvent() { HistoryItem = ReBack.Peek()  , HistoryEnum = HistoryEnum.Cancel});
                });
            }
        }
        public RelayCommand RedoHistoryCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Cancel.Clear();
                    ReBack.Clear();
                    Cancel.Push( InitItem);
                    CancelIsEnabled = false;
                    ReBackIsEnabled = false;
                    Messenger.Default.Send<InitFromHistroyEvent>(new InitFromHistroyEvent() { HistoryItem =InitItem,HistoryEnum = HistoryEnum.Redo });
                    Messenger.Default.Send<RedoSaveEvent>(new RedoSaveEvent() { });

                    
                });
            }
        }


    }
    public class HistoryItem
    {


        public List<HistoryShowItemPoint> ShowPointItems { get; set; }

        public bool IsAdd { get; set; }
        /// <summary>
        ///  上下左右 1234;
        /// </summary>
        public addenum add { get; set; }
    }

   public  enum addenum
    {
        top,
        bottom,
        left,
        right,
    }

    public class HistoryShowItemPoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        

        public  SolidColorBrush OldFill { get; set; }
        public SolidColorBrush NewFill { get; set; }
    }
}
