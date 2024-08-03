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
            foreach (var item in FileHelper.GetThemeList())
            {
                    ThemeItemList.Add(new ThemeItem(item));
            }
            Messenger.Default.Register<ThemeItemClickedEvent>(this, HandleThemeItemClickedEvent);
            Messenger.Default.Register<AddNewThemeEvent>(this, HandleAddNewThemeEvent);
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
                    FileHelper.SaveName(obj.JsonFileInfo);
                }
            }
        }

        private void HandleAddNewThemeEvent(AddNewThemeEvent obj)
        {

            var temp = new ThemeItem(obj.JsonFileInfo);
            ThemeItemList.Add(temp) ;

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

        }
        public ThemeItem(JsonFileInfo jsonFileInfo)
        {
            JsonFileInfo =jsonFileInfo;
            IsReName = false;
            ReName = IsReName ? Visibility.Visible : Visibility.Collapsed;
            Read = IsReName ? Visibility.Collapsed : Visibility.Visible;

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

                    FileHelper.SaveName(this.JsonFileInfo);
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
