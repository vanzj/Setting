using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using LibreHardwareMonitor.Hardware.Cpu;
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

namespace Setting.ViewModel
{
    public class ThemeListViewModel : ViewModelBase
    {
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
            HardHelper.Instance.Check();

            foreach (var item in FileHelper.GetOrInitThemeList())
            {
                if (item.FileName == "cpu" )
                {
                    if (HardHelper.hasCPUUse && HardHelper.hasCPUTemp)
                    {
                        AllThemeList.Add(new ThemeItem(item));
                    }
                }
                else if (item.FileName == "gpu")
                {
                    if (HardHelper.hasGPUUse && HardHelper.hasGPUTemp)
                    {
                        AllThemeList.Add(new ThemeItem(item));
                    }
                }
                 else if (item.FileName == "wifi" )
                {
                    if (HardHelper.hasNetwork )
                    {
                        AllThemeList.Add(new ThemeItem(item));
                    }
                }
                else
                {
                    AllThemeList.Add(new ThemeItem(item));
                }

            }
            InitThemeList();
            Messenger.Default.Register<ThemeItemClickedEvent>(this, HandleThemeItemClickedEvent);
            Messenger.Default.Register<InputNewThemeEvent>(this, HandleAddNewThemeEvent);
            Messenger.Default.Register<CopyThemeEvent>(this, HandleCopyThemeEvent);
            Messenger.Default.Register<RemoveThemeEvent>(this, HandleRemoveThemeEvent);
            Messenger.Default.Register<ChangeThemeNameEvent>(this, HandleChangeThemeNameEvent);
            Messenger.Default.Register<ThemeLotFocusEvent>(this, HandleThemeLotFocusEvent);
            Messenger.Default.Register<CursorModelChangeEvent>(this, HandleCursorModelChangeEvent);
            Messenger.Default.Register<LoadedEvent>(this, HandleCursorModelChangeEvent);

            Messenger.Default.Register<KeyDownEvent>(this, HandleKeyDownEvent);

       
            

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
                    JsonFileInfo = AllThemeList.First().JsonFileInfo
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
            ThemeItemShowList = new ObservableCollection<ThemeItem>();
            page = 1;
            for (int i = 0; i < size && i < AllThemeList.Count - ((page - 1) * size); i++)
            {
                ThemeItemShowList.Add(AllThemeList[(page - 1) * size + i]);
            }
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

            for (int i = (page - 1) * size;  i < ((page - 1) * size)+size; i++)
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
            foreach (var item in ThemeItemShowList)
            {
                item.PopupOpen = false;
                

            }
        }

        private void HandleRemoveThemeEvent(RemoveThemeEvent obj)
        {
            AllThemeList.Remove(AllThemeList.First(c => c.JsonFileInfo.FileName == obj.JsonFileInfo.FileName));
            ReloadThisPageThemeList();
            FileHelper.Remove(obj.JsonFileInfo);
            if (CurrJsonFileInfo.FileName == obj.JsonFileInfo.FileName)
            {
                Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = AllThemeList.First().JsonFileInfo });
            }
        }

        private void HandleCopyThemeEvent(CopyThemeEvent obj)
        {
            var guid = Guid.NewGuid().ToString("N");
            var temp = new ThemeItem(new JsonFileInfo()
            {
                Name = obj.JsonFileInfo.Name + "副本",
                FileName = guid.Substring(guid.Length - 6)
            }, true);

            AllThemeList.Add(temp);
            LastPageThemeList();
            FileHelper.Copy(obj.JsonFileInfo, temp.JsonFileInfo);

            Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = temp.JsonFileInfo });
        }

        private void HandleThemeItemClickedEvent(ThemeItemClickedEvent obj)
        {
            CurrJsonFileInfo = obj.JsonFileInfo;
            CurrJsonFileInfo.BakName  = obj.JsonFileInfo.Name;
            foreach (var item in ThemeItemShowList)
            {
                if (item.JsonFileInfo.FileName != obj.JsonFileInfo.FileName)
                {
                    item.Thickness = 0;
                    item.PopupOpen = false;
                    item.ChangeRead();
                }
                else
                {
                    item.Thickness = 2;
                }

            }
            foreach (var themeItem in AllThemeList)
            {
                if (themeItem.JsonFileInfo.FileName == obj.JsonFileInfo.FileName)
                {
                    themeItem.Thickness = 2;
                }
                else
                {
                    themeItem.Thickness = 0;
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
                    FileHelper.SaveThemeName(obj.JsonFileInfo);
                }
            }
        }

        private void HandleAddNewThemeEvent(InputNewThemeEvent obj)
        {

            var temp = new ThemeItem(obj.JsonFileInfo, true);
            AllThemeList.Add(temp);
            foreach (var themeItem in ThemeItemShowList)
            {
                if (themeItem.JsonFileInfo.FileName != obj.JsonFileInfo.FileName)
                {
                    themeItem.Thickness = 0;
                    themeItem.PopupOpen = false;
                    themeItem.ChangeRead();
                }
                else
                {
                    themeItem.Thickness = 2;
                }
            }
            foreach (var themeItem in AllThemeList)
            {
                if (themeItem.JsonFileInfo.FileName == obj.JsonFileInfo.FileName)
                {
                    themeItem.Thickness = 2;
                }
                else
                {
                    themeItem.Thickness = 0;
                }
            }
            LastPageThemeList();
        }
        public RelayCommand CreateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var guid = Guid.NewGuid().ToString("N");
                    var JsonFileInfo = new JsonFileInfo()
                    {
                         FileName = guid.Substring(guid.Length - 6),
                        Name = "新建模板" + "-" + ThemeCountHelper.GetNextCount()
                    };
                    var temp = new ThemeItem(JsonFileInfo,true);
                    AllThemeList.Add(temp);
                    LastPageThemeList();
                    CurrJsonFileInfo = JsonFileInfo;
                    foreach (var themeItem in ThemeItemShowList)
                    {
                        if (themeItem.JsonFileInfo.FileName != temp.JsonFileInfo.FileName)
                        {
                            themeItem.Thickness = 0;
                            themeItem.PopupOpen = false;
                            themeItem.ChangeRead();
                        }
                        else
                        {
                            themeItem.Thickness = 2;
                        }
                    }
                    foreach (var themeItem in AllThemeList)
                    {
                        if (themeItem.JsonFileInfo.FileName == temp.JsonFileInfo.FileName)
                        {
                            themeItem.Thickness = 2;
                        }
                        else
                        {
                            themeItem.Thickness = 0;
                        }
                    }
                    Messenger.Default.Send(new NewThemeEvent { JsonFileInfo = JsonFileInfo });

                });
            }
        }

    }


    public class ThemeItem : ViewModelBase
    {
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

        public ThemeItem()
        {
            Messenger.Default.Register<KeyDownEventTheme>(this, HandleKeyDownEventTheme);
            Thickness = 2;
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
                   
                        FileHelper.SaveThemeName(this.JsonFileInfo);
                    
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

        public ThemeItem(JsonFileInfo jsonFileInfo,bool isCreat=false)
        {


            Messenger.Default.Register<KeyDownEventTheme>(this, HandleKeyDownEventTheme);
            if (isCreat)
            {
                Thickness = 2;
            }
           
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
                    Thickness = 2;
                    Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = item.JsonFileInfo });
                    Messenger.Default.Send(new CursorModelChangeEvent { model = Enum.CursorEnum.MOVE });
                    
                });
            }
        }


        public RelayCommand<ThemeItem> BeginRenameCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {
           
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
                     FileHelper.SaveThemeName(this.JsonFileInfo);
                });
            }
        }

        public RelayCommand<ThemeItem> CopyCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {
                    Thickness = 0;
                    PopupOpen = false;
                    Messenger.Default.Send(new CopyThemeEvent { JsonFileInfo = item.JsonFileInfo });
                });
            }
        }

        public RelayCommand<ThemeItem> InputCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {
                    PopupOpen = false;
                    Messenger.Default.Send(new InputThemeEvent { JsonFileInfo = item.JsonFileInfo });
                });
            }
        }

        public RelayCommand<ThemeItem> ReMoveCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {
                    Thickness = 0;
                    PopupOpen = false;
                    Messenger.Default.Send(new RemoveThemeEvent { JsonFileInfo = item.JsonFileInfo });
                });
            }
        }
    }
}
