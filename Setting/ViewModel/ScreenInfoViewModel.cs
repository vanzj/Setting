using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Setting.Event;
using Setting.Helper;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Webapi;

namespace Setting.ViewModel
{
    public class ScreenInfoListViewModel : ViewModelBase
    {
        /// <summary>
        /// 连接着的屏幕
        /// </summary>
        private Dictionary<string, string> devNoComNo = new Dictionary<string, string>();

        private void ReloadThisPageScreenList()
        {
            DeviceInfoShowList = new ObservableCollection<ScreenDeviceInfoViewModel>();
            var LastPge = ((AllScreen.Count - 1) / size) + 1;
            if (page > LastPge)
            {//最后一页没数据刷新。页面
                page = LastPge;
            }
            for (int i = 0; i < size && i < AllScreen.Count - ((page - 1) * size); i++)
            {
                DeviceInfoShowList.Add(AllScreen[(page - 1) * size + i]);
            }
            buttonShow();
        }

        private ObservableCollection<ScreenDeviceInfoViewModel> deviceInfoShowList;
        /// <summary>
        /// 主题列表
        /// </summary>
        public ObservableCollection<ScreenDeviceInfoViewModel> DeviceInfoShowList { get => deviceInfoShowList; set => Set(ref deviceInfoShowList, value); }
        private int page { set; get; } = 1;
        private int size { set; get; } = 5;

        private void InitScreenList()
        {
            DeviceInfoShowList = new ObservableCollection<ScreenDeviceInfoViewModel>();
            page = 1;
            for (int i = 0; i < size && i < AllScreen.Count - ((page - 1) * size); i++)
            {
                DeviceInfoShowList.Add(AllScreen[(page - 1) * size + i]);
            }
            buttonShow();
        }
        private void buttonShow()
        {
            if (page == 1)
            {
                VisibilityLeft = Visibility.Hidden;
            }
            else
            {
                VisibilityLeft = Visibility.Visible;
            }
            var LastPge = ((AllScreen.Count - 1) / size) + 1;
            if (page == LastPge || LastPge == 1)
            {
                VisibilityRight = Visibility.Hidden;
            }
            else
            {
                VisibilityRight = Visibility.Visible;
            }

        }
        private Visibility visibilityLeft;
        private Visibility visibilityRight;
        public Visibility VisibilityLeft
        {
            get { return visibilityLeft; }
            set
            {
                visibilityLeft = value;
                RaisePropertyChanged();
            }
        }
        public Visibility VisibilityRight
        {
            get { return visibilityRight; }
            set
            {
                visibilityRight = value;
                RaisePropertyChanged();
            }
        }
        public RelayCommand LeftCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    PrePageScreenList();
                });
            }
        }
        public RelayCommand RightCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NextPageScreenList();
                });
            }
        }  /// <summary>
           /// 
           /// </summary>
        private void NextPageScreenList()
        {
            DeviceInfoShowList = new ObservableCollection<ScreenDeviceInfoViewModel>();
            var LastPge = ((AllScreen.Count - 1) / size) + 1;
            if (page >= LastPge)
            {
                return;
            }
            page++;
            for (int i = 0; i < size && i < AllScreen.Count - ((page - 1) * size); i++)
            {
                DeviceInfoShowList.Add(AllScreen[(page - 1) * size + i]);
            }
            buttonShow();
        } /// <summary>
          /// 
          /// </summary>
        private void PrePageScreenList()
        {
            if (page <= 1)
            {
                return;
            }
            page--;
            DeviceInfoShowList = new ObservableCollection<ScreenDeviceInfoViewModel>();

            for (int i = (page - 1) * size; i < size && i < AllScreen.Count - ((page - 1) * size); i++)
            {
                DeviceInfoShowList.Add(AllScreen[i]);
            }
            buttonShow();
        }
     
        private ScreenDeviceInfoViewModel CurrentDevInfo = null;

        List<ScreenDeviceInfoViewModel> AllScreen = new List<ScreenDeviceInfoViewModel>();

        public ScreenInfoListViewModel()
        {
            Messenger.Default.Register<CursorModelChangeEvent>(this, HandleCursorModelChangeEvent);
            Messenger.Default.Register<FindScreenEvent>(this, HanderFindScreenEvent);
            Messenger.Default.Register<KeyDownEvent>(this, HandleKeyDownEvent);
            Messenger.Default.Register<RemoveScreenInfoEvent>(this, HandleRemoveScreenInfoEvent);
            Messenger.Default.Register<ScreenClickedEvent>(this, HandleScreenClickedEvent);
            Messenger.Default.Register<ScreenLotFocusEvent>(this, HandleScreenLotFocusEvent);
            Messenger.Default.Register<CanChangeScreenEvent>(this, HandleCanChangeScreenEvent);
            
            Messenger.Default.Register<LostScreenEvent>(this, HandleLostScreenEvent);
        }

        private void HandleCanChangeScreenEvent(CanChangeScreenEvent obj)
        {
            if (devNoComNo.Count>0)
            {
                var devno = devNoComNo.First().Key;

                var dev = AllScreen.FirstOrDefault(c => c.DeviceInfo.DevNo == devno);
                    if (dev!=null)
                {
                    ChangeScreen(dev);
                }

            }
            else if(DeviceInfoShowList.Count>0)
            {
                ChangeScreen(DeviceInfoShowList.FirstOrDefault());
            }
            Messenger.Default.Send(new EndChangeScreenEvent());//没有屏幕退出
        }

        private void HandleLostScreenEvent(LostScreenEvent obj)
        {
            foreach (var item in obj.DeviceInfos)
            {
                if (devNoComNo.ContainsKey(item.DevNo))
                {
                    devNoComNo.Remove(item.DevNo);
                }
            }

          
        }

        private void HandleScreenLotFocusEvent(ScreenLotFocusEvent obj)
        {
            foreach (var item in AllScreen)
            {
              item.PopupOpen = false;
                
            }
        }

        private void HandleScreenClickedEvent(ScreenClickedEvent obj)
        {
            if (obj.fromScreen)
            {
                ChangeScreen(obj.CurrentDevInfo);
                foreach (var item in AllScreen)
                {
                    if (item.DeviceInfo.DevNo != obj.CurrentDevInfo.DeviceInfo.DevNo)
                    {
                        item.PopupOpen = false;
                    }
                }
                string comid = "";
                if (devNoComNo.ContainsKey(obj.CurrentDevInfo.DeviceInfo.DevNo))
                {
                    comid = devNoComNo[obj.CurrentDevInfo.DeviceInfo.DevNo];
                }

                Messenger.Default.Send(new ScreenClickedEvent() { ComId = comid, CurrentDevInfo = obj.CurrentDevInfo });
            }
     
        }

        private void HandleRemoveScreenInfoEvent(RemoveScreenInfoEvent obj)
        {
           
                AllScreen.Remove(AllScreen.First(c => c.DeviceInfo.DevNo == obj.CurrentDevInfo.DeviceInfo.DevNo));
                JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                 var temp = jdClient.UnbindUsingGETAsync(obj.CurrentDevInfo.DeviceInfo.Id).GetAwaiter().GetResult();
                if (CurrentDevInfo.DeviceInfo.DevNo == obj.CurrentDevInfo.DeviceInfo.DevNo)
                {
                    CurrentDevInfo = null;
                }
                var devList = jdClient.MacListUsingGETAsync().GetAwaiter().GetResult();
                if (devList.Code == 0)
                {
                    foreach (var dev in devList.Data)
                    {
                        if (AllScreen.All(c => c.DeviceInfo.DevNo != dev.DevNo))
                        {
                            string comid = "";
                            if (devNoComNo.ContainsKey(dev.DevNo))
                            {
                                comid = devNoComNo[dev.DevNo];
                            }
                            AllScreen.Add(new ScreenDeviceInfoViewModel(dev, comid));
                        }
                    }
                    ReloadThisPageScreenList();
                    if (CurrentDevInfo == null)
                    {
                        ChangeScreen(DeviceInfoShowList.FirstOrDefault());
                    }
                    else
                    {
                        if (DeviceInfoShowList.All(c=>c.DeviceInfo.DevNo !=CurrentDevInfo.DeviceInfo.DevNo))
                        {
                            ChangeScreen(DeviceInfoShowList.FirstOrDefault());
                        }
                    }
                    if (AllScreen.Count ==1 )
                    {
                        AllScreen.ForEach(c => c.Delete = false);
                    }

                }
            
        }

        private void HandleKeyDownEvent(KeyDownEvent obj)
        {
            Messenger.Default.Send(new KeyDownEventScreen() { Key = obj.Key, DevNo = CurrentDevInfo.DeviceInfo.DevNo });

        }

        private void HandleCursorModelChangeEvent(CursorModelChangeEvent obj)
        {
          
                Cursor cursor = null;
                switch (obj.model)
                {
                    case Enum.CursorEnum.MOVE:
                        cursor = CursorHelper.MOVE();
                        break;
                    case Enum.CursorEnum.ERASE:
                        cursor = CursorHelper.ERASE();
                        break;
                    case Enum.CursorEnum.Magic:
                        cursor = CursorHelper.MAGIC();
                        break;
                    default:
                        break;
                }
                foreach (var item in DeviceInfoShowList)
                {
                    if (cursor != null)
                    {
                        item.Cursor = cursor;
                    }
                }
            
        }

        private void HanderFindScreenEvent(FindScreenEvent obj)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            if (obj.DeviceInfos.Any(c => c.Id == null) && obj.isLocal)
            {//绑定设备
                foreach (var item in obj.DeviceInfos)
                {
                    if (devNoComNo.ContainsKey(item.DevNo))
                    {
                        devNoComNo[item.DevNo] = item.BlueNo;
                    }
                    else
                    {
                        devNoComNo.Add(item.DevNo, item.BlueNo);
                    }
                    if (item.Id == null && AllScreen.All(c => c.DeviceInfo. DevNo != item.DevNo))
                    {
                        var temp = client.AddUsingGETAsync(item.DevNo, item.Name, "9").GetAwaiter().GetResult();
                    }
                    else
                    {
                        var screen = AllScreen.First(c => c.DeviceInfo.DevNo == item.DevNo);
                        screen.ComId = item.BlueNo;
                             }
                    
                }
                var devList = client.MacListUsingGETAsync().GetAwaiter().GetResult();
                if (devList.Code == 0)
                {
                    Messenger.Default.Send(new FindScreenEvent { DeviceInfos = devList.Data.ToList() });
                }
            }
            else
            {
                foreach (var item in obj.DeviceInfos)
                {
                    if (AllScreen.All(c => c.DeviceInfo. DevNo != item.DevNo))
                    {
                        string comid = "";
                        if (devNoComNo.ContainsKey(item.DevNo))
                        {
                            comid = devNoComNo[item.DevNo];
                        }
                        AllScreen.Add(new ScreenDeviceInfoViewModel(item, comid));
                
                    }
                }
                ReloadThisPageScreenList();
                if (CurrentDevInfo == null)
                {
                                   InitScreenList();
                }
                
            }




        }


        private void ChangeScreen(ScreenDeviceInfoViewModel device)
        {
            string JsonDir = Environment.CurrentDirectory + "\\Json\\Theme\\";

            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            CurrentDevInfo =  device;


            if (CurrentDevInfo != null)
            {
                if (devNoComNo.ContainsKey(CurrentDevInfo.DeviceInfo.DevNo))
                {
                    CurrentDevInfo.ComId = devNoComNo[CurrentDevInfo.DeviceInfo.DevNo];

                    Messenger.Default.Send(new ScreenClickedEvent() { ComId = device.ComId, CurrentDevInfo = device });
                }

                List<JsonFileInfo> jsonFileInfos = new List<JsonFileInfo>();
                jsonFileInfos.AddRange(GetDefualt());
                var ThemeList = client.MacListUsingGET2Async(CurrentDevInfo.DeviceInfo. Id.ToString(), 9).GetAwaiter().GetResult();
                if (ThemeList.Code == 0)
                {
                    foreach (var item in ThemeList.Data)
                    {
                        if (!File.Exists(JsonDir+item.FileName)&& item.FileName!=null)
                        {  //下载。
                            FileHelper.DownloadJsonFileAsync(item.Url, JsonDir + item.FileName).GetAwaiter().GetResult();
                        }
                    }
                    jsonFileInfos.AddRange(ThemeList.Data.Where(c=>c.FileName!=null).Select(c => new JsonFileInfo() { Name = c.ResName, FileName = c.FileName ,Id = c.Id}));
                }
                Messenger.Default.Send(new EndChangeScreenEvent());//没有屏幕退出
                // 添加获取的。
                Messenger.Default.Send(new GetThemeListEvent()
                { device = CurrentDevInfo,
                    jsonFileInfos = jsonFileInfos
                });

            }
            else
            {
                List<JsonFileInfo> jsonFileInfos = new List<JsonFileInfo>();
                jsonFileInfos.AddRange(GetDefualt());
                // 添加获取的。
                Messenger.Default.Send(new GetThemeListEvent()
                {
                    device = CurrentDevInfo,
                    jsonFileInfos = jsonFileInfos
                });
         
            }


        }

        private List<JsonFileInfo> GetDefualt()
        {
            return FileHelper.GetOrInitThemeList();
        }
    }


    public  class  ScreenDeviceInfoViewModel : ViewModelBase
    {
        private Cursor cursor;
        public Cursor Cursor
        {
            get { return cursor; }
            set
            {
                cursor = value;
                RaisePropertyChanged();
            }
        }
        private bool delete;

        public bool Delete
        {
            get { return delete; }
            set
            {
                delete = value;
                RaisePropertyChanged();
            }
        }
        private Visibility reName;

        public Visibility ReName
        {
            get { return reName; }
            set
            {
                reName = value;
                RaisePropertyChanged();
            }
        }
        private Visibility read;

        public Visibility Read
        {
            get { return read; }
            set
            {
                read = value;
                RaisePropertyChanged();
            }
        }

        private bool popupOpen;
        /// <summary>
        /// 编辑模式
        /// </summary>
        public bool IsReName { get; set; }

        public bool PopupOpen
        {
            get { return popupOpen; }
            set
            {
                popupOpen = value;
                RaisePropertyChanged();
            }
        }
        public DeviceInfo DeviceInfo { get; set; }


        public RelayCommand<ScreenDeviceInfoViewModel> BeginRenameCommand
        {
            get
            {
                return new RelayCommand<ScreenDeviceInfoViewModel>(item =>
                {
                    IsReName = true;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
                });
            }
        }

        public string ComId { get; set; }
        public RelayCommand<ScreenDeviceInfoViewModel> EndRenameCommand
        {
            get
            {
                return new RelayCommand<ScreenDeviceInfoViewModel>(item =>
                {

                    IsReName = false;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
                   // FileHelper.SaveThemeName(this.JsonFileInfo, deviceInfo?.DeviceInfo?.Id?.ToString());
                });
            }
        }


        public ScreenDeviceInfoViewModel(DeviceInfo deviceInfo,string comId)
        {
            ComId = comId;
            DeviceInfo = deviceInfo;
            Messenger.Default.Register<KeyDownEventScreen>(this, HandleKeyDownEventScreen);
            Delete = true;
            IsReName = false;
            ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
            Read = IsReName ? Visibility.Collapsed : Visibility.Visible;

        }
        private void HandleKeyDownEventScreen(KeyDownEventScreen obj)
        {
            if (DeviceInfo.DevNo == obj.DevNo)
            {
                PopupOpen = false;
                if (obj.Key == Key.Enter)
                {
                    IsReName = false;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;

                   
                }
                if (obj.Key == Key.Escape)
                {
                    IsReName = false;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }
        public RelayCommand<ScreenDeviceInfoViewModel> ChangeDeviceInfo
        {
            get
            {
                return new RelayCommand<ScreenDeviceInfoViewModel>(item =>
                {
                    PopupOpen = true;
                    Messenger.Default.Send(new ScreenClickedEvent { fromScreen=true ,CurrentDevInfo = this });


                });
            }
        }
        public RelayCommand<ScreenDeviceInfoViewModel> ReMoveCommand
        {
            get
            {
                return new RelayCommand<ScreenDeviceInfoViewModel>(item =>
                {
                    PopupOpen = false;
                    Messenger.Default.Send(new RemoveScreenInfoEvent { CurrentDevInfo = this });

              
                });
            }
        }
    }
}
