using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Setting.ViewModel
{
    public class PointListViewModel : ViewModelBase
    {
        public int xIndex = 85;
        public int yIndex = 5;
        private ObservableCollection<PonitItem> showPonitList;
        private ObservableCollection<PonitItem> allPonitList;

        /// <summary>
        /// 所有点，
        /// </summary>
        public ObservableCollection<PonitItem> AllPonitList { get => allPonitList; set => Set(ref allPonitList, value); }
        /// <summary>
        /// 显示点
        /// </summary>
        public ObservableCollection<PonitItem> ShowPonitList { get => showPonitList; set => Set(ref showPonitList, value); }
        /// </summary>




        public PointListViewModel()
        {
            AllPonitList = new ObservableCollection<PonitItem>();
            ShowPonitList = new ObservableCollection<PonitItem>();
            for (int i = 0; i < xIndex; i++)
            {
                for (int j = 0; j < yIndex; j++)
                {
                    ShowPonitList.Add(new PonitItem(i, j));
                    AllPonitList.Add(new PonitItem(i, j));
                }
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
        /// 横坐标
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// 纵坐标
        /// </summary>
        public int Y { get; set; }

        public PonitItem(int x,int y)
        {
            Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff0000"));
            X = x;
            Y = y;
            Id = new Guid().ToString("D");
        }

    }
}
