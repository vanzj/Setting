using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Setting.Event;
using Setting.Helper;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Webapi;

namespace Setting.ViewModel
{
    public class ScreenInfoViewModel : ViewModelBase
    {

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


        public bool PopupOpen
        {
            get { return popupOpen; }
            set
            {
                popupOpen = value;
                RaisePropertyChanged();
            }
        }

        private void ReloadThisPageThemeList()
        {
            DeviceInfoShowList = new ObservableCollection<DeviceInfo>();
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
        public RelayCommand<DeviceInfo> ReMoveCommand
        {
            get
            {
                return new RelayCommand<DeviceInfo>(item =>
                {
                    PopupOpen = false;
                    if (AllScreen.Count>1&& AllScreen!=null)
                    {
                        AllScreen.Remove(AllScreen.First(c => c.DevNo == item.DevNo));
                      
                        JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                        jdClient.UnbindUsingGETAsync(item.Id);
                        if (CurrentDevInfo.DevNo == item.DevNo)
                        {
                            CurrentDevInfo = null;
                        }
                        var devList = jdClient.MacListUsingGETAsync().GetAwaiter().GetResult();
                        if (devList.Code == 0)
                        {
                            foreach (var dev in devList.Data)
                            {
                                if (AllScreen.All(c => c.DevNo != dev.DevNo))
                                {
                                    AllScreen.Add(dev);
                                }
                            }
                            if (CurrentDevInfo == null)
                            {
                                ChangeScreen(AllScreen.FirstOrDefault());
                            }
                            ReloadThisPageThemeList();
                        }
                    }
                });
            }
        }
        private ObservableCollection<DeviceInfo> deviceInfoShowList;
        /// <summary>
        /// 主题列表
        /// </summary>
        public ObservableCollection<DeviceInfo> DeviceInfoShowList { get => deviceInfoShowList; set => Set(ref deviceInfoShowList, value); }
        private int page { set; get; } = 1;
        private int size { set; get; } = 5;

        private void InitScreenList()
        {
            DeviceInfoShowList = new ObservableCollection<DeviceInfo>();
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
                    PrePageThemeList();
                });
            }
        }
        public RelayCommand RightCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    NextPageThemeList();
                });
            }
        }  /// <summary>
           /// 
           /// </summary>
        private void NextPageThemeList()
        {
            DeviceInfoShowList = new ObservableCollection<DeviceInfo>();
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
        private void PrePageThemeList()
        {
            if (page <= 1)
            {
                return;
            }
            page--;
            DeviceInfoShowList = new ObservableCollection<DeviceInfo>();

            for (int i = (page - 1) * size; i < size && i < AllScreen.Count - ((page - 1) * size); i++)
            {
                DeviceInfoShowList.Add(AllScreen[i]);
            }
            buttonShow();
        }
        public RelayCommand<DeviceInfo> ChangeDeviceInfo
        {
            get
            {
                return new RelayCommand<DeviceInfo>(item =>
                {
                    PopupOpen = true;
                   // Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = item.JsonFileInfo, CurrentDevInfo = deviceInfo });
                });
            }
        }
        private DeviceInfo CurrentDevInfo = null;

        List<DeviceInfo> AllScreen = new List<DeviceInfo>();

        public ScreenInfoViewModel()
        {
            Messenger.Default.Register<CursorModelChangeEvent>(this, HandleCursorModelChangeEvent);
            Messenger.Default.Register<FindScreenEvent>(this, HanderFindScreenEvent);
        }

        private void HandleCursorModelChangeEvent(CursorModelChangeEvent obj)
        {
          
        }

        private void HanderFindScreenEvent(FindScreenEvent obj)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            if (obj.DeviceInfos.Any(c => c.Id == null))
            {//绑定设备
                foreach (var item in obj.DeviceInfos)
                {
                    if (item.Id == null&& AllScreen.All(c => c.DevNo != item.DevNo))
                    {
                        var temp = client.AddUsingGETAsync(item.DevNo, item.Name, "9").GetAwaiter().GetResult();

                       

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
                    if (AllScreen.All(c => c.DevNo != item.DevNo))
                    {
                        AllScreen.Add(item);
                    }
                }
                ReloadThisPageThemeList();
                if (CurrentDevInfo == null)
                {
                    ChangeScreen(AllScreen.FirstOrDefault());
                    InitScreenList();
                }
                if (obj.DeviceInfos.Count<=10)
                {
                    for (int i = obj.DeviceInfos.Count; i <=10; i++)
                    {
                        var tempdev = Guid.NewGuid().ToString("N").Substring(0,12);

                        var temp = client.AddUsingGETAsync(tempdev, "测试删除功能", "9").GetAwaiter().GetResult();
                    }
                }
            }
        }


        private void ChangeScreen(DeviceInfo device)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            CurrentDevInfo = device;
            if (CurrentDevInfo != null)
            {
                var ThemeList = client.MacListUsingGET2Async(CurrentDevInfo.Id.ToString(), 9).GetAwaiter().GetResult();



                foreach (var item in ThemeList.Data)
                {
                    //下载。
                }

                List<JsonFileInfo> jsonFileInfos = new List<JsonFileInfo>();
                jsonFileInfos.AddRange(GetDefualt());
                // 添加获取的。
                Messenger.Default.Send(new GetThemeListEvent()
                {device= CurrentDevInfo,
                    jsonFileInfos = jsonFileInfos
                });
                Messenger.Default.Send(new InitEndEvent() { endName = "init" });
            }

         
        }

        private List<JsonFileInfo> GetDefualt()
        {
            return FileHelper.GetOrInitThemeList();
        }
    }
}
