using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Setting.ViewModel
{
    public class PointViewModel : ViewModelBase

    {
        /// <summary>
        /// 左平移
        /// </summary>
        public ICommand Left
        {
            get
            {
                return new RelayCommand(() =>
                {
                   
                });
            }

        }

    }

    public class PonitItem
    {
        /// <summary>
        /// 唯一Id 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 颜色
        /// </summary>
        public SolidColorBrush Fill { get; set; }
        /// <summary>
        /// 是否选中，用于选中颜色显示。
        /// </summary>
        public bool Select { get; set; }

        /// <summary>
        /// 横坐标
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 纵坐标
        /// </summary>
        public int Y { get; set; }

    }
}
