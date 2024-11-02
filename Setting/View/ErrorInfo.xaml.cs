using GalaSoft.MvvmLight.Messaging;
using Setting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Setting.View
{
    /// <summary>
    /// ErrorInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorInfo : UserControl
    {
        public ErrorInfo()
        {
            InitializeComponent();
            Messenger.Default.Register<LostScreenEvent>(this, HandleLostScreenEvent); 
            Messenger.Default.Register<ReConnectScreenEvent>(this, HandleReConnectScreenEvent);
        }

        private void HandleReConnectScreenEvent(ReConnectScreenEvent obj)
        {
            this.Dispatcher.Invoke(()=>{
                this.Visibility = Visibility.Collapsed;
            });
        }

        private void HandleLostScreenEvent(LostScreenEvent obj)
        {
            this.Dispatcher.Invoke(() => {
                this.Visibility = Visibility.Visible;
            });
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Visibility = Visibility.Collapsed;
        }
    }
}
