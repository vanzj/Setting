using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using Setting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

           // Messenger.Default.Register<HistroyInitEvent>(this, HanderHistroyInitEvent);
            Messenger.Default.Register<HistroyAddEvent>(this, HanderHistroyAddEvent);

        }



        private void HanderHistroyAddEvent(HistroyAddEvent obj)
        {
            Cancel.Push( new HistoryItem()
            {
                Title = obj.HistoryItem.Title,
                CurrentFrame = obj.HistoryItem.CurrentFrame,
                FrameCount = obj.HistoryItem.FrameCount,
                PointItems =  JsonConvert.DeserializeObject<Dictionary<int, List<PointItem>>>(JsonConvert.SerializeObject( obj.HistoryItem.PointItems)),
            });
            ReBack.Clear();
            CancelIsEnabled = true;
            ReBackIsEnabled = false;
        }

        //private void HanderHistroyInitEvent(HistroyInitEvent obj)
        //{

           

        //    InitItem = new HistoryItem()
        //    {
        //        Title = obj.HistoryItem.Title,
        //        CurrentFrame = obj.HistoryItem.CurrentFrame,
        //        FrameCount = obj.HistoryItem.FrameCount,
        //        PointItems = JsonConvert.DeserializeObject<Dictionary<int, List<PointItem>>>(JsonConvert.SerializeObject(obj.HistoryItem.PointItems)),
        //    };
        //    Cancel.Push(new HistoryItem()
        //    {
        //        Title = obj.HistoryItem.Title,
        //        CurrentFrame = obj.HistoryItem.CurrentFrame,
        //        FrameCount = obj.HistoryItem.FrameCount,
        //        PointItems = JsonConvert.DeserializeObject<Dictionary<int, List<PointItem>>>(JsonConvert.SerializeObject(obj.HistoryItem.PointItems)),
        //    });
        //    ReBack.Clear();
        //    CancelIsEnabled = false;
        //    ReBackIsEnabled = false;
        //}

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
                    
                    Messenger.Default.Send<InitFromHistroyEvent>(new InitFromHistroyEvent() { HistoryItem = Cancel.Peek() });
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
                    if (Cancel.Count == 1)
                    {
                        CancelIsEnabled = false;
                    }
                    Messenger.Default.Send<InitFromHistroyEvent>(new InitFromHistroyEvent() { HistoryItem = Cancel.Peek() });
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
                    Cancel.Push(new HistoryItem()
                    {
                        Title = InitItem.Title,
                        CurrentFrame = InitItem.CurrentFrame,
                        FrameCount = InitItem.FrameCount,
                        PointItems = JsonConvert.DeserializeObject<Dictionary<int, List<PointItem>>>(JsonConvert.SerializeObject(InitItem.PointItems)),
                    });
                    CancelIsEnabled = false;
                    ReBackIsEnabled = false;
                    Messenger.Default.Send<InitFromHistroyEvent>(new InitFromHistroyEvent() { HistoryItem = Cancel.Peek() });
                });
            }
        }


    }
    public class HistoryItem
    {
        public int FrameCount { get; set; }
        public int CurrentFrame { get; set; }
        public string Title { get; set; }

        public Dictionary<int, List<PointItem>> PointItems { get; set; }
    }

}
