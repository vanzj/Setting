using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using HidSharp.Reports;
using Setting.Event;
using Setting.Event.MsgSendEvent;
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
using System.Windows.Interop;
using Windows.System;


namespace Setting.ViewModel
{
    public class ScreenInfoListViewModel : ViewModelBase
    {
 

        private void ReloadThisPageScreenList(ScreenDeviceInfoViewModel CurrentPage)
        {
            DeviceInfoShowList = new ObservableCollection<ScreenDeviceInfoViewModel>();
            if (CurrentPage == null)
            {

                var LastPge = ((AllScreen.Count - 1) / size) + 1;
                if (page > LastPge)
                {//最后一页没数据刷新。页面
                    page = LastPge;
                }
                for (int i = 0; i < size && i < AllScreen.Count - ((page - 1) * size); i++)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DeviceInfoShowList.Add(AllScreen[(page - 1) * size + i]);
                    });
                }
                buttonShow();
            }
            else
            {// 定位页面;
                var index = AllScreen.FindIndex(c => c.DeviceInfo.DevNo == CurrentPage.DeviceInfo.DevNo);

                page = (index / 5) + 1;
                for (int i = 0; i < size && i < AllScreen.Count - ((page - 1) * size); i++)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        DeviceInfoShowList.Add(AllScreen[(page - 1) * size + i]);
                    });

                }
                buttonShow();
            }
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
            Messenger.Default.Register<ConnectScreenEvent>(this, HandleConnectScreenEvent);

            #region 发送消息事件
            Messenger.Default.Register<SendThemeSegmentSendMessageEvent>(this, HandleSendThemeSegmentSendMessageEvent);
            Messenger.Default.Register<SendLuminanceSendMessageEvent>(this, HandleSendLuminanceSendMessageEvent);
            Messenger.Default.Register<SendDisenableRotateMessageEvent>(this, HandleSendDisenableRotateMessageEvent);
            Messenger.Default.Register<SendEnableRotateMessageEvent>(this, HandleSendEnableRotateMessageEvent);
            Messenger.Default.Register<SendCloseSendMessageEvent>(this, HandleSendCloseSendMessageEvent);
            Messenger.Default.Register<SendOpenSendMessageEvent>(this, HandleSendOpenSendMessageEvent);
            Messenger.Default.Register<SendThemeCirculateSendMessageEvent>(this, HandleSendThemeCirculateSendMessageEvent);
            Messenger.Default.Register<SendNetWorkEvent>(this, HandleSendNetWorkEvent);
            Messenger.Default.Register<SendThemeDynamicSendMessageEvent>(this, HandleSendThemeDynamicSendMessageEvent);


            #endregion
            Messenger.Default.Register<SendStartEvent>(this, HandleSendStartEvent);
            Messenger.Default.Register<SendEndEvent>(this, HandleSendEndEvent);
        }

        private void HandleSendEndEvent(SendEndEvent @event)
        {
            if (@event.DevNo == CurrentDevInfo.DeviceInfo.DevNo)
            {
                Messenger.Default.Send(new SendEndStoryEvent());
            }
        }

        private void HandleSendStartEvent(SendStartEvent @event)
        {
            if (@event.DevNo == CurrentDevInfo.DeviceInfo.DevNo)
            {
                Messenger.Default.Send(new SendStartStoryEvent());
            }
        }

        private void HandleSendThemeDynamicSendMessageEvent(SendThemeDynamicSendMessageEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendThemeDynamicSendMessage(@event.data, CurrentDevInfo.DeviceInfo.DevNo);
        }

        private void HandleSendNetWorkEvent(SendNetWorkEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendNetWork(CurrentDevInfo.DeviceInfo.DevNo);
        }

        private void HandleSendThemeCirculateSendMessageEvent(SendThemeCirculateSendMessageEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendThemeCirculateSendMessage(@event.data, CurrentDevInfo.DeviceInfo.DevNo);
        }

        private void HandleSendOpenSendMessageEvent(SendOpenSendMessageEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendOpenSendMessage(CurrentDevInfo.DeviceInfo.DevNo);
        }

        private void HandleSendCloseSendMessageEvent(SendCloseSendMessageEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendCloseSendMessage(CurrentDevInfo.DeviceInfo.DevNo);
        }

        private void HandleSendEnableRotateMessageEvent(SendEnableRotateMessageEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendEnableRotateMessage(CurrentDevInfo.DeviceInfo.DevNo);
        }
        #region 发送消息事件
        private void HandleSendDisenableRotateMessageEvent(SendDisenableRotateMessageEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendDisenableRotateMessage( CurrentDevInfo.DeviceInfo.DevNo);
        }

        private void HandleSendLuminanceSendMessageEvent(SendLuminanceSendMessageEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendLuminanceSendMessage(@event.Luminance, CurrentDevInfo.DeviceInfo.DevNo);
        }

        private void HandleSendThemeSegmentSendMessageEvent(SendThemeSegmentSendMessageEvent @event)
        {
            SerialPortSendMsgHelper.Instance.SendThemeSegmentSendMessage(@event.Msg,CurrentDevInfo.DeviceInfo.DevNo);

        }
        #endregion
        private void HandleConnectScreenEvent(ConnectScreenEvent obj)
        {
            foreach (var item in AllScreen)
            {
                var temp = SerialPortScanHelper.Instance.SerialPortList.FirstOrDefault(c => c.DevNo == item.DeviceInfo.DevNo&& c.Connected);
                    if (temp!=null )
                    {
                        item.LinkUSB = Visibility.Visible;
                        item.LinkBreak = Visibility.Collapsed;
                    }
        
                }



            if (obj!=null && obj.DeviceInfos!=null && obj.DeviceInfos.Count>0)
            {
                var DeviceInfo = obj.DeviceInfos.First();
               var tempScreen = AllScreen.FirstOrDefault(c => c.DeviceInfo.DevNo == DeviceInfo.DevNo);
                if (tempScreen!=null)
                {

                    ChangeScreen(tempScreen);
       
                }
            }
        }

        private void HandleLostScreenEvent(LostScreenEvent obj)
        {
            foreach (var item in AllScreen)
            {
                var temp = SerialPortScanHelper.Instance.SerialPortList.FirstOrDefault(c => c.DevNo == item.DeviceInfo.DevNo && c.Connected);
                if (temp == null)
                {
                    item.LinkUSB = Visibility. Collapsed;
                    item.LinkBreak = Visibility.Visible;
                }

            }
        }

        private void HandleCanChangeScreenEvent(CanChangeScreenEvent obj)
        {

            ChangeScreen(null);
            Messenger.Default.Send(new EndChangeScreenEvent());//没有屏幕退出

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

            ChangeScreen(obj.CurrentDevInfo);





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
                        AllScreen.Add(new ScreenDeviceInfoViewModel(dev));
                    }
                }

                ChangeScreen(CurrentDevInfo);
                if (AllScreen.Count == 1)
                {
                    AllScreen.ForEach(c => c.Delete = false);
                }

            }

        }

        private void HandleKeyDownEvent(KeyDownEvent obj)
        {
            if (CurrentDevInfo != null)
            {
                Messenger.Default.Send(new KeyDownEventScreen() { Key = obj.Key, DevNo = CurrentDevInfo.DeviceInfo.DevNo });

            }

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
            if (!obj.isLocal)
            {                //获取网络的优先
                foreach (var item in obj.DeviceInfos)
                {
                    if (AllScreen.All(c => c.DeviceInfo.DevNo != item.DevNo))
                    {
                        AllScreen.Add(new ScreenDeviceInfoViewModel(item) );
                    }
                }
            }
            else
            {

                var NeedUpload = new List<DeviceInfo>();
                foreach (var device in obj.DeviceInfos)
                {
                    var deviceScreen = AllScreen.FirstOrDefault(c => c.DeviceInfo.DevNo == device.DevNo);
                    SerialPortSendMsgHelper.Instance.InitCOM(device.DevNo);
                    if (deviceScreen == null)
                    {
                        var temp = client.AddUsingGETAsync(device.DevNo, device.Name, "9").GetAwaiter().GetResult();

                    }
                    else
                    {
                        deviceScreen.LinkUSB = Visibility.Visible;
                        deviceScreen.LinkBreak = Visibility.Collapsed;
                    }
                }

                var devList = client.MacListUsingGETAsync().GetAwaiter().GetResult();
                if (devList.Code == 0)
                {
                    foreach (var item in devList.Data)
                    {
                        if (AllScreen.All(c => c.DeviceInfo.DevNo != item.DevNo))
                        {
                      
                            AllScreen.Add(new ScreenDeviceInfoViewModel(item));
                        }
                    }

                    ReloadThisPageScreenList(CurrentDevInfo);
                }
                else
                {
                    foreach (var device in obj.DeviceInfos)
                    {
                        if (AllScreen.All(c => c.DeviceInfo.DevNo != device.DevNo))
                        {
                            var deviceScreen = new ScreenDeviceInfoViewModel(device);
                            deviceScreen.LinkUSB = Visibility.Visible;
                            deviceScreen.LinkBreak = Visibility.Collapsed;
                            AllScreen.Add(deviceScreen);
                        }
                    }
                }
                if (CurrentDevInfo == null)
                {
                    ChangeScreen(null);
                }
                if (SerialPortScanHelper.Instance.SerialPortList.FirstOrDefault(c=>c.Connected)!=null)
                {
                    var firstSerialPort = SerialPortScanHelper.Instance.SerialPortList.FirstOrDefault(c => c.Connected);
                    var firstScreen =      AllScreen.FirstOrDefault(c => c.DeviceInfo.DevNo == firstSerialPort.DevNo);
                    if (firstScreen !=null)
                    {
                        ChangeScreen(firstScreen);
                        ReloadThisPageScreenList(firstScreen);
                    }
                  
                }
                foreach (var device in obj.DeviceInfos)
                {
                    var deviceScreen = AllScreen.FirstOrDefault(c => c.DeviceInfo.DevNo == device.DevNo);
                    if (deviceScreen != null)
                    {
                        deviceScreen.LinkUSB = Visibility.Visible;
                        deviceScreen.LinkBreak = Visibility.Collapsed;
                    }
                }
            }
        }


        private void ChangeScreen(ScreenDeviceInfoViewModel device)
        {

         
            string JsonDir = Environment.CurrentDirectory + "\\Json\\Theme\\";
            if (device == null)
            {//未指定屏幕
                var firstCom = SerialPortScanHelper.Instance.SerialPortList.FirstOrDefault(c => c.Connected);
                if (firstCom == null)
                {
                    CurrentDevInfo = AllScreen.FirstOrDefault();
                }
                else
                {

                    CurrentDevInfo = AllScreen.FirstOrDefault(c => c.DeviceInfo.DevNo == firstCom.DevNo );
                }

            }
            else
            {//指定屏幕
           
                CurrentDevInfo = device;
            }
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());
            ReloadThisPageScreenList(CurrentDevInfo);
            if (CurrentDevInfo != null)
            {


                List<JsonFileInfo> jsonFileInfos = new List<JsonFileInfo>();
                jsonFileInfos.AddRange(GetDefualt());
                var ThemeList = client.MacListUsingGET2Async(CurrentDevInfo.DeviceInfo.Id.ToString(), 9).GetAwaiter().GetResult();
                if (ThemeList.Code == 0)
                {
                    foreach (var item in ThemeList.Data)
                    {
                        if (!File.Exists(JsonDir + item.FileName) && item.FileName != null)
                        {  //下载。
                            FileHelper.DownloadJsonFileAsync(item.Url, JsonDir + item.FileName).GetAwaiter().GetResult();
                        }
                    }
                    jsonFileInfos.AddRange(ThemeList.Data.Where(c => c.FileName != null).Select(c => new JsonFileInfo() { Name = c.ResName, FileName = c.FileName, Id = c.Id }));
                }

                // 添加获取的。
                Messenger.Default.Send(new GetThemeListEvent()
                {
                    device = CurrentDevInfo,
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
            SerialPortSendMsgHelper.Instance.InitCOM(CurrentDevInfo?.DeviceInfo.DevNo);
            foreach (var item in AllScreen)
            {
                if (item.DeviceInfo.DevNo != CurrentDevInfo?.DeviceInfo.DevNo)
                {
                    item.Thickness = 0;
                    item.PopupOpen = false;
                }
                else
                {
                    item.Thickness = 2;
                }
            }
            Messenger.Default.Send(new ScreenNameAndDevNoEvent { Name = CurrentDevInfo?.DeviceInfoName  ,DevNo = CurrentDevInfo?.DeviceInfo.DevNo});
        }

        private List<JsonFileInfo> GetDefualt()
        {
            return FileHelper.GetOrInitThemeList();
        }
    }


    public class ScreenDeviceInfoViewModel : ViewModelBase
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
        private Visibility linkUSB;

        public Visibility LinkUSB
        {
            get { return linkUSB; }
            set
            {
                linkUSB = value;
                RaisePropertyChanged();
            }
        }
        private Visibility linkBreak;

        public Visibility LinkBreak
        {
            get { return linkBreak; }
            set
            {
                linkBreak = value;
                RaisePropertyChanged();
            }
        }
        private int thickness;


        public int Thickness
        {
            get { return thickness; }
            set
            {
                thickness = value;
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
        private string deviceInfoName { get; set; }
        private string deviceInfoOldName { get; set; }
        public string DeviceInfoName
        {
            get { return deviceInfoName; }
            set
            {
                deviceInfoName = value;
                RaisePropertyChanged();
            }
        }
        public string DeviceInfoOldName
        {
            get { return deviceInfoOldName; }
            set
            {
                deviceInfoOldName = value;
                RaisePropertyChanged();
            }
        }

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

        public RelayCommand<ScreenDeviceInfoViewModel> EndRenameCommand
        {
            get
            {
                return new RelayCommand<ScreenDeviceInfoViewModel>(item =>
                {

                    IsReName = false;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
                    JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                    var temp = jdClient.ModifyNameUsingGETAsync(this.DeviceInfo.Id, this.DeviceInfoName).GetAwaiter().GetResult();
                    if (temp.Code == 0)
                    {
                        DeviceInfo.Name = DeviceInfoName;
                        DeviceInfoOldName = DeviceInfoName;
                        Messenger.Default.Send(new ScreenNameAndDevNoEvent { Name = this.DeviceInfoName, DevNo = this.DeviceInfo.DevNo });
                    }
                    // FileHelper.SaveThemeName(this.JsonFileInfo, deviceInfo?.DeviceInfo?.Id?.ToString());
                });
            }
        }


        public ScreenDeviceInfoViewModel(DeviceInfo deviceInfo)
        {
     
            DeviceInfo = deviceInfo;
            DeviceInfoOldName= deviceInfo.Name;
            deviceInfoName = deviceInfo.Name;
            Messenger.Default.Register<KeyDownEventScreen>(this, HandleKeyDownEventScreen);
            Delete = true;
            IsReName = false;
            ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
            Read = IsReName ? Visibility.Collapsed : Visibility.Visible;

            LinkBreak = Visibility.Visible;
            LinkUSB = Visibility.Collapsed;

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
                    JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                    var temp = jdClient.ModifyNameUsingGETAsync(this.DeviceInfo.Id, this.DeviceInfoName).GetAwaiter().GetResult();
                    if (temp.Code == 0)
                    {
                        DeviceInfo.Name = DeviceInfoName;
                        DeviceInfoOldName = DeviceInfoName;
                        Messenger.Default.Send(new ScreenNameAndDevNoEvent { Name = this.DeviceInfoName, DevNo = this.DeviceInfo.DevNo });
                    }
                    

                }
                if (obj.Key == Key.Escape)
                {
                    DeviceInfoName = DeviceInfoOldName;
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
                    Messenger.Default.Send(new ScreenClickedEvent { CurrentDevInfo = this });


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
