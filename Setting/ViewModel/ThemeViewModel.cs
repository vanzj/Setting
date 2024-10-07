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
using System.Windows.Input;
using Webapi;

namespace Setting.ViewModel
{
    public class ThemeListViewModel : ViewModelBase
    {



        private ScreenDeviceInfoViewModel CurrentDevInfo = null;
        private List<ThemeItem> AllThemeList = new List<ThemeItem>();
        private int page { set; get; } = 1;
        private int size { set; get; } = 7;
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
        }









        public JsonFileInfo CurrJsonFileInfo { get; set; }

        private ObservableCollection<ThemeItem> themeItemShowList;
        /// <summary>
        /// 主题列表
        /// </summary>
        public ObservableCollection<ThemeItem> ThemeItemShowList { get => themeItemShowList; set => Set(ref themeItemShowList, value); }


        public ThemeListViewModel()
        {


            Messenger.Default.Register<ThemeItemClickedEvent>(this, HandleThemeItemClickedEvent);
            Messenger.Default.Register<InputNewThemeEvent>(this, HandleAddNewThemeEvent);
            Messenger.Default.Register<CopyThemeEvent>(this, HandleCopyThemeEvent);
            Messenger.Default.Register<RemoveThemeEvent>(this, HandleRemoveThemeEvent);
            Messenger.Default.Register<ChangeThemeNameEvent>(this, HandleChangeThemeNameEvent);
            Messenger.Default.Register<ThemeLotFocusEvent>(this, HandleThemeLotFocusEvent);
            Messenger.Default.Register<CursorModelChangeEvent>(this, HandleCursorModelChangeEvent);
            Messenger.Default.Register<LoadedEvent>(this, HandleCursorModelChangeEvent);

            Messenger.Default.Register<KeyDownEvent>(this, HandleKeyDownEvent);
            Messenger.Default.Register<GetThemeListEvent>(this, HandleGetThemeListEvent);




        }

        private void HandleGetThemeListEvent(GetThemeListEvent obj)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                CurrentDevInfo = obj.device;
                AllThemeList.Clear();
                foreach (var item in obj.jsonFileInfos)
                {
                    AllThemeList.Add(new ThemeItem(item, CurrentDevInfo));
                }
                InitThemeList();
            }
           );

          Messenger.Default.Send(new ThemeItemClickedEvent() {  CurrentDevInfo = obj.device, JsonFileInfo = ThemeItemShowList.First().JsonFileInfo });

        }

        private void HandleKeyDownEvent(KeyDownEvent obj)
        {
            Messenger.Default.Send(new KeyDownEventTheme() { Key = obj.Key, FileName = CurrJsonFileInfo.FileName });

        }

        private void HandleCursorModelChangeEvent(LoadedEvent obj)
        {
            if (AllThemeList.Count > 0)
            {
                Messenger.Default.Send(new ThemeItemClickedEvent
                {
                    JsonFileInfo = AllThemeList.First().JsonFileInfo,
                    CurrentDevInfo = CurrentDevInfo,
                });
            }
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
            var LastPge = ((AllThemeList.Count - 1) / size) + 1;
            if (page == LastPge || LastPge == 1)
            {
                VisibilityRight = Visibility.Hidden;
            }
            else
            {
                VisibilityRight = Visibility.Visible;
            }

        }

        private void InitThemeList()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ThemeItemShowList = new ObservableCollection<ThemeItem>();
                page = 1;
                for (int i = 0; i < size && i < AllThemeList.Count - ((page - 1) * size); i++)
                {
                    ThemeItemShowList.Add(AllThemeList[(page - 1) * size + i]);
                }
            });

            //Messenger.Default.Send(new ThemeItemClickedEvent
            //{
            //    JsonFileInfo = ThemeItemShowList.First().JsonFileInfo,
            //    CurrentDevInfo = CurrentDevInfo,
            //});
            buttonShow();
        }
        /// <summary>
        /// 
        /// </summary>
        private void LastPageThemeList()
        {
            ThemeItemShowList = new ObservableCollection<ThemeItem>();
            if (AllThemeList.Count == 1)
            {
                page = 1;
            }
            else
            {
                var LastPge = ((AllThemeList.Count - 1) / size) + 1;
                page = LastPge;
            }

            for (int i = 0; i < size && i < AllThemeList.Count - ((page - 1) * size); i++)
            {
                ThemeItemShowList.Add(AllThemeList[(page - 1) * size + i]);
            }
            buttonShow();
        }
        private void ReloadThisPageThemeList()
        {
            ThemeItemShowList = new ObservableCollection<ThemeItem>();
            var LastPge = ((AllThemeList.Count - 1) / size) + 1;
            if (page > LastPge)
            {//最后一页没数据刷新。页面
                page = LastPge;
            }
            for (int i = 0; i < size && i < AllThemeList.Count - ((page - 1) * size); i++)
            {
                ThemeItemShowList.Add(AllThemeList[(page - 1) * size + i]);
            }
            buttonShow();
        }
        /// <summary>
        /// 
        /// </summary>
        private void NextPageThemeList()
        {
            ThemeItemShowList = new ObservableCollection<ThemeItem>();
            var LastPge = ((AllThemeList.Count - 1) / size) + 1;
            if (page >= LastPge)
            {
                return;
            }
            page++;
            for (int i = 0; i < size && i < AllThemeList.Count - ((page - 1) * size); i++)
            {
                ThemeItemShowList.Add(AllThemeList[(page - 1) * size + i]);
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
            ThemeItemShowList = new ObservableCollection<ThemeItem>();

            for (int i = (page - 1) * size; i < size && i < AllThemeList.Count - ((page - 1) * size); i++)
            {
                ThemeItemShowList.Add(AllThemeList[i]);
            }
            buttonShow();
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
            foreach (var item in ThemeItemShowList)
            {
                if (cursor != null)
                {
                    item.Cursor = cursor;
                }
            }
        }

        private void HandleThemeLotFocusEvent(ThemeLotFocusEvent obj)
        {
            if (ThemeItemShowList != null)
            {
                foreach (var item in ThemeItemShowList)
                {
                    item.PopupOpen = false;

                }
            }

        }

        private void HandleRemoveThemeEvent(RemoveThemeEvent obj)
        {
            AllThemeList.Remove(AllThemeList.First(c => c.JsonFileInfo.FileName == obj.JsonFileInfo.FileName));

            ReloadThisPageThemeList();
            FileHelper.Remove(obj.JsonFileInfo, obj.CurrentDevInfo?.DeviceInfo?.Id?.ToString());
            if (CurrJsonFileInfo.FileName == obj.JsonFileInfo.FileName)
            {
                Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = ThemeItemShowList.First().JsonFileInfo, CurrentDevInfo = obj.CurrentDevInfo, });
            }
        }

        private void ReloadThemeList(ThemeItem theme)
        {
            JdClient client = new JdClient(HttpClientHelper.Instance.GetHttpClient());


            AllThemeList.Add(theme);
            var themeList = AllThemeList.Where(c => c.JsonFileInfo.Id == null && !c.JsonFileInfo.Default);

            if (themeList.Count() > 0)
            {
                var res = client.MacListUsingGET2Async(CurrentDevInfo?.DeviceInfo?.Id?.ToString(), 9).GetAwaiter().GetResult();

                foreach (var item in themeList)
                {
                    var resdata = res.Data.FirstOrDefault(c => c.FileName == item.JsonFileInfo.FileName);
                    if (resdata != null)
                    {
                        item.JsonFileInfo.Id = resdata.Id;
                    }
                }
            }

            LastPageThemeList();
        }

        private void HandleCopyThemeEvent(CopyThemeEvent obj)
        {
            var temp = new ThemeItem(new JsonFileInfo()
            {
                Name = obj.JsonFileInfo.Name + "副本",
                FileName = Guid.NewGuid().ToString("N")+".json"
            }, obj.CurrentDevInfo);
            ReloadThemeList(temp);
            FileHelper.Copy(obj.JsonFileInfo, temp.JsonFileInfo, obj.CurrentDevInfo?.DeviceInfo?.Id?.ToString());

            Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = temp.JsonFileInfo, CurrentDevInfo = CurrentDevInfo, });
        }

        private void HandleThemeItemClickedEvent(ThemeItemClickedEvent obj)
        {
            CurrentDevInfo = obj.CurrentDevInfo;

            CurrJsonFileInfo = obj.JsonFileInfo;
            foreach (var item in ThemeItemShowList)
            {
                if (item.JsonFileInfo.FileName != obj.JsonFileInfo.FileName)
                {

                    item.PopupOpen = false;
                    item.ChangeRead();

                }

            }
        }

        private void HandleChangeThemeNameEvent(ChangeThemeNameEvent obj)
        {
            foreach (var item in ThemeItemShowList)
            {
                if (item.JsonFileInfo.FileName == obj.JsonFileInfo.FileName)
                {
                    item.JsonFileInfo.Name = obj.JsonFileInfo.Name;
                    FileHelper.SaveThemeName(obj.JsonFileInfo, obj.CurrentDevInfo?.DeviceInfo?.Id?.ToString());
                }
            }
        }

        private void HandleAddNewThemeEvent(InputNewThemeEvent obj)
        {
            CurrentDevInfo = obj.CurrentDevInfo;
            var temp = new ThemeItem(obj.JsonFileInfo, CurrentDevInfo);
            ReloadThemeList(temp);
        }
        public RelayCommand CreateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var JsonFileInfo = new JsonFileInfo()
                    {
                        FileName = Guid.NewGuid().ToString("N") + ".json",
                        Name = "新建模板" + DateTime.Now.ToString("MMddHHmmss")
                    };
                    var temp = new ThemeItem(JsonFileInfo, CurrentDevInfo);
                    ReloadThemeList(temp);
                    CurrJsonFileInfo = JsonFileInfo;
                    foreach (var themeItem in ThemeItemShowList)
                    {
                        if (themeItem.JsonFileInfo.FileName != JsonFileInfo.FileName)
                        {
                            themeItem.ChangeRead();
                        }
                    }
                    Messenger.Default.Send(new NewThemeEvent { JsonFileInfo = JsonFileInfo, CurrentDevInfo = CurrentDevInfo });

                });
            }
        }

    }


    public class ThemeItem : ViewModelBase
    {
        private ScreenDeviceInfoViewModel deviceInfo;
        private JsonFileInfo jsonFileInfo;

        public JsonFileInfo JsonFileInfo
        {
            get { return jsonFileInfo; }
            set
            {
                jsonFileInfo = value;
                RaisePropertyChanged();
            }
        }

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
        /// <summary>
        /// 编辑模式
        /// </summary>
        public bool IsReName { get; set; }




        private bool canRename;

        public bool CanRename
        {
            get { return canRename; }
            set
            {
                canRename = value;
                RaisePropertyChanged();
            }
        }

        private bool copy;

        public bool Copy
        {
            get { return copy; }
            set
            {
                copy = value;
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



        private void HandleKeyDownEventTheme(KeyDownEventTheme obj)
        {
            if (JsonFileInfo.FileName == obj.FileName)
            {
                PopupOpen = false;
                if (obj.Key == Key.Enter)
                {
                    IsReName = false;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
                    FileHelper.SaveThemeName(this.JsonFileInfo, deviceInfo?.DeviceInfo?.Id?.ToString());

                }
                if (obj.Key == Key.Escape)
                {
                    IsReName = false;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
                    JsonFileInfo.Name = JsonFileInfo.BakName;

                }
            }
        }

        public ThemeItem(JsonFileInfo jsonFileInfo, ScreenDeviceInfoViewModel deviceInfo)
        {
            this.deviceInfo = deviceInfo;
            Messenger.Default.Register<KeyDownEventTheme>(this, HandleKeyDownEventTheme);

            JsonFileInfo = jsonFileInfo;
            IsReName = false;
            ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
            Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
            CanRename = true;
            if (jsonFileInfo.Default)
            {

                Delete = false;
            }
            else
            {
                Delete = true;

            }
            if (JsonFileInfo.IsDynamic)
            {
                Copy = false;
            }
            else
            {
                Copy = true;
            }
        }

        public void ChangeRead()
        {
            IsReName = false;
            ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
            Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
        }

        public RelayCommand<ThemeItem> ChangeThemeCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {
                    PopupOpen = true;
                    Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = item.JsonFileInfo, CurrentDevInfo = deviceInfo });
                });
            }
        }


        public RelayCommand<ThemeItem> BeginRenameCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {
                    JsonFileInfo.BakName = JsonFileInfo.Name;

                    IsReName = true;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
                });
            }
        }


        public RelayCommand<ThemeItem> EndRenameCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {

                    IsReName = false;
                    ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
                    Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
                    FileHelper.SaveThemeName(this.JsonFileInfo, deviceInfo?.DeviceInfo?.Id?.ToString());
                });
            }
        }

        public RelayCommand<ThemeItem> CopyCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {
                    PopupOpen = false;
                    Messenger.Default.Send(new CopyThemeEvent { JsonFileInfo = item.JsonFileInfo, CurrentDevInfo = deviceInfo });
                });
            }
        }

        public RelayCommand<ThemeItem> ReMoveCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {
                    PopupOpen = false;
                    Messenger.Default.Send(new RemoveThemeEvent { JsonFileInfo = item.JsonFileInfo, CurrentDevInfo = deviceInfo });
                });
            }
        }
    }
}
