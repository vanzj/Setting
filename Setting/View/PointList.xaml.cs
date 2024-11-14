using Setting.ViewModel;
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
    /// PointList.xaml 的交互逻辑
    /// </summary>
    public partial class PointList : UserControl
    {
    
        public PointList()
        {
            InitializeComponent();
        }



        private void OnMouseEnter(object sender, MouseEventArgs e)
        {

            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                var item = sender as ListViewItem;
                if (item != null && item.Content is PointItem model)
                {
                    model.ChangeColorCommand.Execute(model);
                }
            }

        }
    }
}
