/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Setting"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace Setting.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
            ///
            SimpleIoc.Default.Register<HistoryViewModel>();
            SimpleIoc.Default.Register<PointListViewModel>();//TODO  
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<ThemeListViewModel>();
            SimpleIoc.Default.Register<ScreenInfoListViewModel>();
            SimpleIoc.Default.Register<MessageViewModel>();

        }


        public MessageViewModel MessageList

        {
            get
            {
                var a = ServiceLocator.Current.GetInstance<MessageViewModel>();
                return a;
            }
        }
        public ScreenInfoListViewModel ScreenInfoList

        {
            get
            {
                var a = ServiceLocator.Current.GetInstance<ScreenInfoListViewModel>();
                return a;
            }
        }

        public HistoryViewModel HistoryList

        {
            get
            {
                var a = ServiceLocator.Current.GetInstance<HistoryViewModel>();
                return a;
            }
        }

        public PointListViewModel PointList
        {
            get
            {
                var a = ServiceLocator.Current.GetInstance<PointListViewModel>();
                return a;
            }
        }
        public ThemeListViewModel ThemeList
        {
            get
            {
                var a = ServiceLocator.Current.GetInstance<ThemeListViewModel>();
                return a;
            }
        }
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}