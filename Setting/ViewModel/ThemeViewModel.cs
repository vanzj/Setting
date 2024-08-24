using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Setting.Helper;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Setting.ViewModel
{
  public  class ThemeListViewModel : ViewModelBase
    {
      public JsonFileInfo CurrJsonFileInfo { get; set; }

        private ObservableCollection<ThemeItem> themeItemList;
        /// <summary>
        /// 主题列表
        /// </summary>
        public ObservableCollection<ThemeItem> ThemeItemList { get => themeItemList; set => Set(ref themeItemList, value); }


        public ThemeListViewModel()
        {
            ThemeItemList = new ObservableCollection<ThemeItem>();
            foreach (var item in FileHelper.GetOrInitThemeList())
            {
                    ThemeItemList.Add(new ThemeItem(item));
            }
            Messenger.Default.Register<ThemeItemClickedEvent>(this, HandleThemeItemClickedEvent);
            Messenger.Default.Register<InputNewThemeEvent>(this, HandleAddNewThemeEvent);
            Messenger.Default.Register<CopyThemeEvent>(this, HandleCopyThemeEvent);
            Messenger.Default.Register<RemoveThemeEvent>(this, HandleRemoveThemeEvent);
            Messenger.Default.Register<ChangeThemeNameEvent>(this, HandleChangeThemeNameEvent);

        }

        private void HandleRemoveThemeEvent(RemoveThemeEvent obj)
        {
            ThemeItemList.Remove(themeItemList.First(c => c.JsonFileInfo.FileName == obj.JsonFileInfo.FileName));
            FileHelper.Remove(obj.JsonFileInfo);
            if (CurrJsonFileInfo.FileName == obj.JsonFileInfo.FileName)
            {
                Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = ThemeItemList.First().JsonFileInfo});
            }
        }

        private void HandleCopyThemeEvent(CopyThemeEvent obj)
        {
            var temp = new ThemeItem(new JsonFileInfo()
            {
                Name = obj.JsonFileInfo.Name + "副本",
                FileName = Guid.NewGuid().ToString()
            });
       
            ThemeItemList.Add(temp);
            FileHelper.Copy(obj.JsonFileInfo,temp.JsonFileInfo);

            Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = temp.JsonFileInfo });
        }

        private void HandleThemeItemClickedEvent(ThemeItemClickedEvent obj)
        {
            CurrJsonFileInfo = obj.JsonFileInfo;
            foreach (var item in ThemeItemList)
            {
                if (item.JsonFileInfo.FileName != obj.JsonFileInfo.FileName)
                {
                    item.ChangeRead();
                    
                }
                
            }
        }

        private void HandleChangeThemeNameEvent(ChangeThemeNameEvent obj)
        {
            foreach (var item in ThemeItemList)
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

            var temp = new ThemeItem(obj.JsonFileInfo);
            ThemeItemList.Add(temp) ;

        }
        public RelayCommand CreateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var JsonFileInfo = new JsonFileInfo()
                    {
                        FileName = Guid.NewGuid().ToString(),
                        Name = "新建模板" + DateTime.Now.ToString("YYMMddHHmmssms")
                    };
                    var temp = new ThemeItem(JsonFileInfo);
                    ThemeItemList.Add(temp);

                    CurrJsonFileInfo = JsonFileInfo;
                    foreach (var themeItem in ThemeItemList)
                    {
                        if (themeItem.JsonFileInfo.FileName != JsonFileInfo.FileName)
                        {
                            themeItem.ChangeRead();
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
        /// <summary>
        /// 编辑模式
        /// </summary>
        public bool IsReName { get; set; }
        private Visibility actionVisibility;

        public Visibility ActionVisibility
        {
            get { return actionVisibility; }
            set
            {
                actionVisibility = value;
                RaisePropertyChanged();
            }
        }
        private Visibility delete;

        public Visibility Delete
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
            IsReName = false;
            ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
            Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
            ActionVisibility = JsonFileInfo.IsDynamic? Visibility.Collapsed : Visibility.Visible;
            Delete = JsonFileInfo.Default ? Visibility.Collapsed : Visibility.Visible;
        }
        public ThemeItem(JsonFileInfo jsonFileInfo)
        {
            JsonFileInfo =jsonFileInfo;
            IsReName = false;
            ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
            Read = IsReName ? Visibility.Collapsed : Visibility.Visible;
            ActionVisibility = JsonFileInfo.IsDynamic ? Visibility.Collapsed : Visibility.Visible;
            Delete = JsonFileInfo.Default ? Visibility.Collapsed : Visibility.Visible;
        }

      public void  ChangeRead()
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
                    Messenger.Default.Send(new ThemeItemClickedEvent { JsonFileInfo = item.JsonFileInfo });
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

                    Messenger.Default.Send(new CopyThemeEvent { JsonFileInfo = item.JsonFileInfo });
                });
            }
        }
     
        public RelayCommand<ThemeItem> ReMoveCommand
        {
            get
            {
                return new RelayCommand<ThemeItem>(item =>
                {

                    Messenger.Default.Send(new RemoveThemeEvent { JsonFileInfo = item.JsonFileInfo });
                });
            }
        }
    }
}
