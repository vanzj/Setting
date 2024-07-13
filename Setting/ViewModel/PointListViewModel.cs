using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Setting.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Setting.ViewModel
{
    public class PointListViewModel : ViewModelBase
    {
        public int xIndex = 85;
        public int yIndex = 5;
        public Dictionary<int, List<PonitItem>> AllPonitList { get; set; }

        /// <summary>
        /// 所有帧数
        /// </summary>
        private int framesCount;
        /// <summary>
        /// 
        /// </summary>
        public int FramesCount
        {
            get { return framesCount; }
            set { framesCount = value; RaisePropertyChanged(); }
        }

        private int currFrame;

        public int CurrentFrame
        {
            get { return currFrame; }
            set { currFrame = value; RaisePropertyChanged(); }
        }


        private ObservableCollection<PonitItem> showPonitList;
        /// <summary>
        /// 显示点
        /// </summary>
        public ObservableCollection<PonitItem> ShowPonitList { get => showPonitList; set => Set(ref showPonitList, value); }

       

        private RelayCommand _openFileCommand;

        public RelayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new RelayCommand(ExecuteOpenFileCommand));
            }
        }


        public RelayCommand LeftCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                XMoveCommand(AllPonitList[CurrentFrame], -1);
                    BuildShow(CurrentFrame);
                });
            }
        }
        public RelayCommand RightCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    BuildShow(CurrentFrame);
                });
            }
        }


        public RelayCommand LeftFrameCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    CurrentFrame--;
                    if (CurrentFrame < 0)
                    {
                        CurrentFrame = framesCount-1;
                    }

                    BuildShow(CurrentFrame);
                });
            }
        }
        public RelayCommand RightframeCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    CurrentFrame++;
                    if (CurrentFrame >= framesCount)
                    {
                        CurrentFrame = 0;
                    }

                    BuildShow(CurrentFrame);
                });
            }
        }

        public RelayCommand TopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    YMoveCommand(AllPonitList[CurrentFrame], 1);
                    BuildShow(CurrentFrame);
                });
            }
        }
        public RelayCommand BottomCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    YMoveCommand(AllPonitList[CurrentFrame], 1);
                    BuildShow(CurrentFrame);
                });
            }
        }



        private void ExecuteOpenFileCommand()
        {

            // 创建一个OpenFileDialog实例
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                // 设置一些基本属性，如过滤器
                Filter = "Text files | *.gif",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            // 显示对话框
            // 注意：ShowDialog方法将返回一个可空的bool值，当用户选择文件并点击“打开”时为true
            if (openFileDialog.ShowDialog() == true)
            {
                // 获取用户选择的文件路径
                string file = openFileDialog.FileName;

                var stream = new FileStream(file, FileMode.Open);
                var decoder = new GifBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                CurrentFrame = 0;
                FramesCount = decoder.Frames.Count;
                AllPonitList = new Dictionary<int, List<PonitItem>>();
                for (int frame = 0; frame < FramesCount; frame++)
                {

                    BitmapFrame t = decoder.Frames[frame];

                    var pw = t.PixelWidth;
                    var ph = t.PixelHeight;
                    var pwdouble = (double)pw;
                    var phdouble = (double)ph;
                    var xindexdouble = (double)yIndex;
                    var CurrentImgxIndex = (int)(pw / (ph / xindexdouble));
                    var OneStep = phdouble / yIndex;
                   var OnFrameAllPonitList = new List<PonitItem>();

                    for (int y = 0; y < yIndex; y++)
                    {
                        for (int x = 0; x < CurrentImgxIndex; x++)
                        {

                            OnFrameAllPonitList.Add(new PonitItem(x, y, BitmapHelper.GetPixelColor(t, x, y, OneStep)));
                        }
                    }
                    var MaxX = OnFrameAllPonitList.Max(c => c.X);
                    var addCount = xIndex - MaxX;
                    var xMove = addCount / 2;
                    XMoveCommand(OnFrameAllPonitList , xMove);
                    AllPonitList.Add(frame, OnFrameAllPonitList);
                }
           
                BuildShow(CurrentFrame);
                stream.Close();
            }
        }

      
        private void XMoveCommand(List<PonitItem> OnFrameAllPonitList, int xMove)
        {

            OnFrameAllPonitList.ForEach(c => c.X = c.X + xMove);
            var minx = OnFrameAllPonitList.Min(c => c.X);
            var maxX = OnFrameAllPonitList.Max(c => c.X);
            if (minx>0)
            {
                //左边 加
                for (int x = 0; x < minx; x++)
                {
                    for (int y = 0; y < yIndex; y++)
                    {
                        OnFrameAllPonitList.Add(new PonitItem(x, y));
                    }
                }
            }
              
            
            
            if (maxX+1 < xIndex)
            {//you边加
                for (int x = maxX + 1; x < xIndex; x++)
                {
                    for (int y = 0; y < yIndex; y++)
                    {
                        OnFrameAllPonitList.Add(new PonitItem(x, y));
                    }
                }
            }
        }
        private void YMoveCommand(List<PonitItem> OnFrameAllPonitList, int yMove)
        {

            OnFrameAllPonitList.ForEach(c => c.Y = c.Y + yMove);
            var minY = OnFrameAllPonitList.Min(c => c.Y);
            var maxY = OnFrameAllPonitList.Max(c => c.Y);

            if (minY > 0)
            {
                //上 加
                for (int y = 0; y < minY; y++)
                {
                    for (int x = 0; x < xIndex; x++)
                    {
                        OnFrameAllPonitList.Add(new PonitItem(x, y));
                    }
                }
            }

            if (maxY + 1 < yIndex)
            {//you边加
                for (int y = maxY + 1; y < yIndex; y++)
                {
                    for (int x = 0; x < yIndex; x++)
                    {
                        OnFrameAllPonitList.Add(new PonitItem(x, y));
                    }
                }
            }
        }
        public void BuildShow(int  frameIndex)
        {
            ShowPonitList = new ObservableCollection<PonitItem>();
           var  OnFrameAllPonitList = AllPonitList[frameIndex];
            OnFrameAllPonitList.Sort();
            foreach (var c in OnFrameAllPonitList)
            {

                if (c.X >= 0 && c.X < xIndex && c.Y>=0&&c.Y<yIndex)
                {
                    ShowPonitList.Add(c);
                }
            }

          
        }

        public PointListViewModel()
        {
            AllPonitList = new Dictionary<int, List<PonitItem>>();
            var OnFrameAllPonitList = new List<PonitItem>();
            ShowPonitList = new ObservableCollection<PonitItem>();
            for (int i = 0; i < xIndex; i++)
            {
                for (int j = 0; j < yIndex; j++)
                {
                    ShowPonitList.Add(new PonitItem(i, j));
                    OnFrameAllPonitList.Add(new PonitItem(i, j));
                }
            }
        }
    }

    public class PonitItem : IComparable<PonitItem>
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

        public PonitItem(int x, int y)
        {
         
            Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff0000"));
            X = x;
            Y = y;
            Id = new Guid().ToString("D");
        }
        public PonitItem(int x, int y, Color color )
        {
          
            Fill = new SolidColorBrush(color);
            X = x;
            Y = y;
            Id = new Guid().ToString("D");
        }
     
        public int CompareTo(PonitItem other)
        {  // 返回值小于0表示this实例小于other，0表示两者相等，大于0表示this实例大于other。

            var tempx = X - other.X;
            var tempy = Y - other.Y;
            if (tempy != 0)
            {
                return tempy;
            }
            else
            {
                return tempx;
            }

        }
    }



}
