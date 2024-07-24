using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;
using Newtonsoft.Json;
using Setting.Helper;
using Setting.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;


namespace Setting.ViewModel
{
    public class PointListViewModel : ViewModelBase
    {
        public CPUHelper CPUHelper { get; set; }

        /// <summary>
        /// 所有帧数
        /// </summary>
        private Visibility changeColorModel;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Visibility ChangeColorModel
        {
            get { return changeColorModel; }
            set { changeColorModel = value; RaisePropertyChanged(); }
        }

        public int xIndex = 85;
        public int yIndex = 5;
        public Dictionary<int, List<PointItem>> AllPonitList { get; set; }

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
        private int currCpu;

        public int CurrCpu
        {
            get { return currCpu; }
            set { currCpu = value; RaisePropertyChanged(); }
        }

        private SolidColorBrush changeColor;

        public SolidColorBrush ChangeColor
        {
            get { return changeColor; }
            set { changeColor = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<PointItem> showPonitList;
        /// <summary>
        /// 显示点
        /// </summary>
        public ObservableCollection<PointItem> ShowPonitList { get => showPonitList; set => Set(ref showPonitList, value); }

       

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
                    YMoveCommand(AllPonitList[CurrentFrame], -1);
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

        public RelayCommand ChangeColorModelCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (ChangeColorModel==Visibility.Hidden)
                    {
                        ChangeColorModel = Visibility.Visible;
                    }
                    else
                    {
                        ChangeColorModel = Visibility.Hidden;
                    }
                    
                });
            }
        }


        public RelayCommand OutputComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var json = JsonConvert.SerializeObject(ShowPonitList.Where(c => c.X <= 2).Select(c => new PointInit(c.Fill.Color) { X = c.X, Y = c.Y}));

                });
            }
        }

        public RelayCommand addOneComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.one, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.two, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.three, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.four, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.five, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.six, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.seven, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.eight, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.nine, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.zero, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X +=5);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.percent, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.celsius, Const.AbcColor));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.lianjiexian, Const.AbcColor));
                    BuildShow(CurrentFrame);
                });
            }
        }
        public RelayCommand CpuInfoStartComand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    CPUHelper = new CPUHelper();
                    CPUHelper.Start();


                });
            }
        }
        public RelayCommand CpuInfoEndComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    CPUHelper = new CPUHelper();
                    CPUHelper.End();


                });
            }
        }
        public RelayCommand CleanComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AllPonitList[currFrame].ForEach(c => c.Fill = new SolidColorBrush(Const.BackGroupColor));
           
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
                AllPonitList = new Dictionary<int, List<PointItem>>();
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
                   var OnFrameAllPonitList = new List<PointItem>();

                    for (int y = 0; y < yIndex; y++)
                    {
                        for (int x = 0; x < CurrentImgxIndex; x++)
                        {

                            OnFrameAllPonitList.Add(new PointItem(x, y, BitmapHelper.GetPixelColor(t, x, y, OneStep)));
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

      
        private void XMoveCommand(List<PointItem> OnFrameAllPonitList, int xMove)
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
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }
              
            
            
            if (maxX+1 < xIndex)
            {//you边加
                for (int x = maxX + 1; x < xIndex; x++)
                {
                    for (int y = 0; y < yIndex; y++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }
        }
        private void YMoveCommand(List<PointItem> OnFrameAllPonitList, int yMove)
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
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }

            if (maxY + 1 < yIndex)
            {//
                for (int y = maxY + 1; y < yIndex; y++)
                {
                    for (int x = 0; x < xIndex; x++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }
        }
        public void BuildShow(int  frameIndex)
        {
            ShowPonitList = new ObservableCollection<PointItem>();
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
            CurrentFrame = 0;
            FramesCount = 1;
            Messenger.Default.Register<PonitClickedEvent>(this, HandlePonitClickedEvent);
            Messenger.Default.Register<CpuInfoEvent>(this, HandleCpuInfoEvent);
            ChangeColorModel = Visibility.Hidden;
            ChangeColor = new SolidColorBrush(Const.AbcColor);
            AllPonitList = new Dictionary<int, List<PointItem>>();
            var OnFrameAllPonitList = new List<PointItem>();
            ShowPonitList = new ObservableCollection<PointItem>();
            for (int i = 0; i < xIndex; i++)
            {
                for (int j = 0; j < yIndex; j++)
                {
                    OnFrameAllPonitList.Add(new PointItem(i, j));
                }
            }
            AllPonitList.Add(CurrentFrame, OnFrameAllPonitList);
            BuildShow(CurrentFrame);
        }

        private void HandleCpuInfoEvent(CpuInfoEvent obj)
        {
            AllPonitList[CurrentFrame] = AllPonitList[currFrame].Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
            AllPonitList[CurrentFrame].ForEach(c => c.Fill = new SolidColorBrush(Const.BackGroupColor));
            XMoveCommand(AllPonitList[CurrentFrame], 1);
            AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
            AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.celsius, Const.AbcColor));
            var cpucelsius = (int)obj.CpuTemp;

            do
            {
                int remainder = cpucelsius % 10;
                XMoveCommand(AllPonitList[CurrentFrame], 1);
                AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, Const.AbcColor));
                cpucelsius = cpucelsius / 10;
            } while (cpucelsius >= 1);

            XMoveCommand(AllPonitList[CurrentFrame], 1);
            AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
            AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.lianjiexian, Const.AbcColor));
            XMoveCommand(AllPonitList[CurrentFrame], 1);
            AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
            AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.percent, Const.AbcColor));
            var cpu = (int)obj.CpuUse;

            do
            {
                int remainder = cpu % 10;
                XMoveCommand(AllPonitList[CurrentFrame], 1);
                AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, Const.AbcColor));
                cpu = cpu / 10;
            } while (cpu >= 1);
            BuildShow(CurrentFrame);
        }

        private void HandlePonitClickedEvent(PonitClickedEvent item)
        {
            if (ChangeColorModel == Visibility.Visible)
            {
                AllPonitList[CurrentFrame].Find(c => c.Y == item.Y && c.X == item.X).Fill = ChangeColor;
                BuildShow(CurrentFrame);
            }
        }
    }

    public class PointItem :ViewModelBase, IComparable<PointItem>
    {
        public RelayCommand<PointItem> ChangeColorCommand
        {
            get
            {
                return new RelayCommand<PointItem>(item =>
                {
                    Messenger.Default.Send(new PonitClickedEvent { X = item.X,Y= item.Y});
                });
            }
        }
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

        public PointItem(int x, int y)
        {
         
            Fill = new SolidColorBrush(Const.BackGroupColor);
            X = x;
            Y = y;
        }
        public PointItem(int x, int y, Color color )
        {
          
            Fill = new SolidColorBrush(color);
            X = x;
            Y = y;
        }
     
        public int CompareTo(PointItem other)
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
