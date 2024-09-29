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
using Setting.Enum;
using System.Drawing;
using System.Drawing.Imaging;
using GIfTool;
using ColorConverter = System.Windows.Media.ColorConverter;
using NLog;
using Setting.Event;
using Setting.Model.CMDModel;
using Webapi;

namespace Setting.ViewModel
{
    public class PointListViewModel : ViewModelBase
    {

        private DeviceInfo CurrentDevInfo { get; set; }

        #region 页面属性
        /// <summary>
        /// 文件名
        /// </summary>
        private JsonFileInfo jsonFileInfo;
        /// <summary>
        /// 文件名
        /// </summary>
        public JsonFileInfo JsonFileInfo
        {
            get { return jsonFileInfo; }
            set { jsonFileInfo = value; RaisePropertyChanged(); }
        }
        private string debugInfo;
        /// <summary>
        /// 文件名
        /// </summary>
        public string DebugInfo
        {
            get { return debugInfo; }
            set { debugInfo = value; RaisePropertyChanged(); }
        }


        private CursorEnum CursorEnum { get; set; }

        /// <summary>
        /// 亮度
        /// </summary>
        private int luminance;
        /// <summary>
        /// 亮度
        /// </summary>
        public int Luminance
        {
            get { return luminance; }
            set { luminance = value; RaisePropertyChanged(); }
        }
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
        private string comName;

        public string ComName
        {
            get { return comName; }
            set { comName = value; RaisePropertyChanged(); }
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

        private bool removeFrame;

        public bool RemoveFrame
        {
            get { return removeFrame; }
            set { removeFrame = value; RaisePropertyChanged(); }
        }

        private bool addFrame;

        public bool AddFrame
        {
            get { return addFrame; }
            set { addFrame = value; RaisePropertyChanged(); }
        }


        private bool outgif;

        public bool Outgif
        {
            get { return outgif; }
            set { outgif = value; RaisePropertyChanged(); }
        }

        private bool startLive;

        public bool StartLive
        {
            get { return startLive; }
            set { startLive = value; RaisePropertyChanged(); }
        }
        private bool endLive;

        public bool EndLive
        {
            get { return endLive; }
            set { endLive = value; RaisePropertyChanged(); }
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


        #endregion


        private CPUHelper CPUHelper { get; set; }
        private GPUHelper GPUHelper { get; set; }
   
        public int xIndex = 85;
        public int yIndex = 5;
        public bool IsSend = false;
        private Dictionary<int, List<PointItem>> AllPonitList { get; set; }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private RelayCommand _openFileCommand;

        public RelayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new RelayCommand(ExecuteOpenFileCommand));
            }
        }

        public RelayCommand SendCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (jsonFileInfo == null)
                    {
                        return;
                    }
                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;
                    if (JsonFileInfo.IsDynamic)
                    {
                        if (jsonFileInfo.FileName =="cpu")
                        {
                            if (CPUHelper == null)
                            {
                                CPUHelper = new CPUHelper();
                            }
                            CPUHelper.Start();

                            //Task.Run(() =>
                            //{
                            SerialPortHelper.Instance.SendThemeDynamicSendMessage();
                            //}
                            //);
                            IsSend = true;
                        }
                        if (jsonFileInfo.FileName == "gpu")
                        {
                            if (GPUHelper == null)
                            {
                                GPUHelper = new GPUHelper();
                            }
                            GPUHelper.Start();

                            //Task.Run(() =>
                            //{
                            SerialPortHelper.Instance.SendThemeDynamicSendMessage();
                            //}
                            //);
                            IsSend = true;
                        }
                        if (jsonFileInfo.FileName == "wifi")
                        {

                            //Task.Run(() =>
                            //{
                            SerialPortHelper.Instance.SendThemeDynamicSendMessage();
                            //}
                            //);
                            IsSend = true;
                        }

                    }
                    else
                    {

                        var msg = MessageHelper.Build(AllPonitList, xIndex, yIndex, fileName);

                        //Task.Run(() =>
                        //{
                        SerialPortHelper.Instance.SendThemeCirculateSendMessage(msg);
                        //}
                        //);

                    }

                })
                {

                };
            }
        }

        
               public RelayCommand SendOnepackageCommandTwo
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new SendStartEvent());

                    if (jsonFileInfo == null)
                    {
                        return;
                    }
                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;
                    if (JsonFileInfo.IsDynamic)
                    {
                        var themeSend = new ThemeSend();

                        themeSend.data = new ThemeData()
                        {
                            model = "dynamic",
                            name = ""
                        };
                        ThemeSendStartSend themeSendStartSend = new ThemeSendStartSend();
                        themeSendStartSend.data = new ThemeSendStartData()
                        {
                            name = jsonFileInfo.FileName,
                            count = AllPonitList.Count.ToString(),
                            frameCount = AllPonitList.Count.ToString(),
                            frameRate = "30",
                            brightness = 255
                        };
                        var templist = new List<string>();
                        templist.Add(JsonConvert.SerializeObject(themeSend));
                        templist.Add(JsonConvert.SerializeObject(themeSendStartSend));
                        SerialPortHelper.Instance.SendThemeDynamicSendMessage(templist);
                        if (jsonFileInfo.FileName == "cpu")
                        {
                            if (CPUHelper == null)
                            {
                                CPUHelper = new CPUHelper();
                            }
                            CPUHelper.Start();
                        }
                        if (jsonFileInfo.FileName == "gpu")
                        {
                            if (GPUHelper == null)
                            {
                                GPUHelper = new GPUHelper();
                            }
                            GPUHelper.Start();
                        }
                        if (jsonFileInfo.FileName == "wifi")
                        {


                        }
                        
                        IsSend = true;
                    }
                    else
                    {

                        var msg = MessageHelper.BuildOnePackageModeTwo(AllPonitList, xIndex, yIndex, fileName);

                        //Task.Run(() =>
                        //{
                        SerialPortHelper.Instance.SendThemeCirculateSendMessage(msg);
                        //}
                        //);

                    }

                })
                {

                };
            }
        }
        public RelayCommand SendOnepackageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (jsonFileInfo == null)
                    {
                        return;
                    }
                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;
                    if (JsonFileInfo.IsDynamic)
                    {
                        if (CPUHelper == null)
                        {
                            CPUHelper = new CPUHelper();
                        }
                        CPUHelper.Start();

                        //Task.Run(() =>
                        //{
                        SerialPortHelper.Instance.SendThemeDynamicSendMessage();
                        //}
                        //);
                        IsSend = true;
                    }
                    else
                    {

                        var msg = MessageHelper.BuildOnePackage(AllPonitList, xIndex, yIndex, fileName);

                        //Task.Run(() =>
                        //{
                        SerialPortHelper.Instance.SendThemeCirculateSendMessage(msg);
                        //}
                        //);

                    }

                })
                {

                };
            }
        }
        public RelayCommand OutPutGIFCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (jsonFileInfo == null || JsonFileInfo.IsDynamic)
                    {
                        return;
                    }
                    var GIFName = JsonFileInfo.Name;

                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;

                    var templist = MessageHelper.Buildgif(AllPonitList, xIndex, yIndex, fileName);

                    OutPutGIF outPutGIF = new OutPutGIF();

                    if (!Directory.Exists(Environment.CurrentDirectory + "//OutPut"))
                    {
                        Directory.CreateDirectory(
                                     Environment.CurrentDirectory + "//OutPut"
                           );


                    }
                    outPutGIF.Output(templist, xIndex, yIndex, 20, Environment.CurrentDirectory + "//OutPut//" + GIFName + ".gif");

                })
                {

                };
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
                var ThemeName = "新建模板" + DateTime.Now.ToString("YYMMddHHmmssms");
                var TempJsonFileInfo = new JsonFileInfo()
                {
                    Name = ThemeName,
                    FileName = Guid.NewGuid().ToString("N")
                };
                InputGIF inputGIF = new InputGIF();
                var ImgInfo = inputGIF.OPENGIF(file, xIndex, yIndex, TempJsonFileInfo.FileName);


                FramesCount = int.Parse(ImgInfo[0].frameCount)>110?110: int.Parse(ImgInfo[0].frameCount);

                AllPonitList = new Dictionary<int, List<PointItem>>();
                int i = 0;
                foreach (var Oneseg in ImgInfo)
                {
                    if (i>=110)
                    {
                      
                        continue;
                    }
                    var oneframePointList = new List<PointItem>();
                    int x = 0;
                    int y = 0;
                    foreach (var item in Oneseg.pointList[0].frameRGB)
                    {

                        var temp = new PointItem(x, y, (System.Windows.Media.Color)ColorConverter.ConvertFromString("#FF" + item));
                        oneframePointList.Add(temp);
                        x++;
                        if (x >= xIndex)
                        {
                            y++;
                            x = 0;
                        }

                    }
                   
                    AllPonitList.Add(int.Parse(Oneseg.pointList[0].frameIndex), oneframePointList);
                    i++;
                }
               
                CurrentFrame = 0;
                BuildShowInit(CurrentFrame);
                AddFrame = true;
                RemoveFrame = true;

                if (FramesCount == 1)
                {
                    RemoveFrame = false;
                }
                if (FramesCount>=110)
                {
                    AddFrame = false;
                }
                FileHelper.SaveThemeName(TempJsonFileInfo,CurrentDevInfo?.Id.ToString());
                FileHelper.Save(JsonConvert.SerializeObject(AllPonitList), TempJsonFileInfo, CurrentDevInfo?.Id.ToString());
                JsonFileInfo = TempJsonFileInfo;
                Messenger.Default.Send(new InputNewThemeEvent { JsonFileInfo = TempJsonFileInfo ,CurrentDevInfo = CurrentDevInfo });
            }
        }
        private void BuildShow(int frameIndex)
        {

            if (string.IsNullOrEmpty(JsonFileInfo.NewFileName))
            {
                JsonFileInfo.NewFileName = Guid.NewGuid().ToString("N");

            }
   
            if (AllPonitList.Count > 0)
            {
                var OnFrameAllPonitList = AllPonitList[frameIndex];

                List<HistoryShowItemPoint> historyShowItemPoints = new List<HistoryShowItemPoint>();
                var NewshowPonit = OnFrameAllPonitList.Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                for (int i = 0; i < xIndex * yIndex; i++)
                {
                    if (NewshowPonit[i].Fill != ShowPonitList[i].Fill)
                    {
                        historyShowItemPoints.Add(new HistoryShowItemPoint()
                        {
                            X = NewshowPonit[i].X,
                            Y = NewshowPonit[i].Y,
                            OldFill = ShowPonitList[i].Fill,
                            NewFill = NewshowPonit[i].Fill
                        });

                        ShowPonitList[i].Fill = NewshowPonit[i].Fill;
                    }
                }
                Messenger.Default.Send(new HistroyAddEvent
                {
                    HistoryItem = new HistoryItem()
                    {
                        IsAdd = false,
                        ShowPointItems = historyShowItemPoints,
                    }
                });
            }
          

        }

        private void BuildShowAdd(int frameIndex,addenum addenum)
        {

            if (string.IsNullOrEmpty(JsonFileInfo.NewFileName))
            {
                JsonFileInfo.NewFileName = Guid.NewGuid().ToString("N");

            }

            if (AllPonitList.Count > 0)
            {
                var OnFrameAllPonitList = AllPonitList[frameIndex];

                List<HistoryShowItemPoint> historyShowItemPoints = new List<HistoryShowItemPoint>();
                var NewshowPonit = OnFrameAllPonitList.Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                for (int i = 0; i < xIndex * yIndex; i++)
                {
                    if (NewshowPonit[i].Fill != ShowPonitList[i].Fill)
                    {
                        historyShowItemPoints.Add(new HistoryShowItemPoint()
                        {
                            X = NewshowPonit[i].X,
                            Y = NewshowPonit[i].Y,
                            OldFill = ShowPonitList[i].Fill,
                             NewFill = NewshowPonit[i].Fill
                        });

                        ShowPonitList[i].Fill = NewshowPonit[i].Fill;
                    }
                }
                Messenger.Default.Send(new HistroyAddEvent
                {
                    HistoryItem = new HistoryItem()
                    {
                        IsAdd = true,
                        add  = addenum,
                        ShowPointItems = historyShowItemPoints,
                    }
                });
            }


        }

        private void BuildShowInit(int frameIndex)
        {
           
            if (AllPonitList.Count > 0)
            {
                var OnFrameAllPonitList = AllPonitList[frameIndex];
      
                if (ShowPonitList == null)
                {
                    ShowPonitList = new ObservableCollection<PointItem>();
                    foreach (var c in OnFrameAllPonitList)
                    {

                        if (c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex)
                        {
                            ShowPonitList.Add(new PointItem() { 
                            X = c.X,
                            Y= c.Y,
                            Fill = c.Fill

                            }
                                );
                        }
                    }
                }
                else
                {
                    var showPonit = OnFrameAllPonitList.Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                    for (int i = 0; i < xIndex * yIndex; i++)
                    {
                        if (showPonit[i].Fill != ShowPonitList[i].Fill)
                        {
                            ShowPonitList[i].Fill = showPonit[i].Fill;
                        }
                    }

                }


                Messenger.Default.Send(new HistroyInitEvent
                {
                    HistoryItem = new HistoryItem()
                    {
                        IsAdd = false,
                        ShowPointItems = this.AllPonitList[CurrentFrame].Select(c => new HistoryShowItemPoint()
                        {
                            X = c.X,
                            Y = c.Y,
                            OldFill = null,
                            NewFill = c.Fill
                        }).ToList(),
                    }
                });
            }


        }

        private void BuildShowPreView(int frameIndex)
        {

            if (AllPonitList.Count > 0)
            {
                var OnFrameAllPonitList = AllPonitList[frameIndex];
        
                var showPonit = OnFrameAllPonitList.Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                for (int i = 0; i < xIndex * yIndex; i++)
                {
                    if (showPonit[i].Fill != ShowPonitList[i].Fill)
                    {
                        ShowPonitList[i].Fill = showPonit[i].Fill;
                    }
                }


            }


        }

        #region 上下左右移动
        public RelayCommand LeftCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    XMoveCommand(AllPonitList[CurrentFrame], -1);
                    BuildShowAdd(CurrentFrame,addenum.left);
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
                    BuildShowAdd(CurrentFrame,addenum.right);
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
                    BuildShowAdd(CurrentFrame,addenum.top);
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
                    BuildShowAdd(CurrentFrame,addenum.bottom);
                });
            }
        }
        private void XMoveCommand(List<PointItem> OnFrameAllPonitList, int xMove)
        {

            OnFrameAllPonitList.ForEach(c => c.X = c.X + xMove);
            var minx = OnFrameAllPonitList.Min(c => c.X);
            var maxX = OnFrameAllPonitList.Max(c => c.X);
            if (minx > 0)
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



            if (maxX + 1 < xIndex)
            {//you边加
                for (int x = maxX + 1; x < xIndex; x++)
                {
                    for (int y = 0; y < yIndex; y++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }
            OnFrameAllPonitList.Sort();
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
            OnFrameAllPonitList.Sort();
        }
        #endregion

        #region 上下帧数

        public RelayCommand LeftFrameCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    CurrentFrame--;
                    if (CurrentFrame < 0)
                    {
                        CurrentFrame = framesCount - 1;
                    }

                    BuildShowInit(CurrentFrame);
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

                    BuildShowInit(CurrentFrame);
                });
            }
        }
        public RelayCommand AddRightFrameCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    // 0 1 2 3 4 添加3
                    // 0 1 2 *3  4 5
                    AddRightFrameMethod();
                });
            }
        }

        private void AddRightFrameMethod()
        {
     
            for (int i = FramesCount; i >= 0; i--)
            {

                if (i > CurrentFrame + 1)
                {
                    var OldList = AllPonitList[i - 1];
                    var temp = new List<PointItem>();
                    if (AllPonitList.TryGetValue(i, out temp))
                    {
                        AllPonitList[i] = OldList;
                    }
                    else
                    {
                        AllPonitList.Add(i, OldList);
                    }
                }
                else if (i == CurrentFrame + 1)
                {
                    var OnFrameAllPonitList = new List<PointItem>();
                    
                        for (int y = 0; y < yIndex; y++)
                    { 

                        for (int x = 0; x < xIndex; x++)
                        {
                            OnFrameAllPonitList.Add(new PointItem(x, y));
                        }
                    }
                    OnFrameAllPonitList.Sort();
                    AllPonitList[i] = OnFrameAllPonitList;
                }
            }
            CurrentFrame++;
            FramesCount++;
            if (FramesCount >=110)
            {
                AddFrame = false;
            }
            RemoveFrame = true;
            BuildShowInit(CurrentFrame);
        }

        public RelayCommand DeleteFrameCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    // 0 1   2  3  4 =>删除2
                    // 0 1  *2 *3
                    for (int i = 0; i < framesCount - 1; i++)
                    {

                        if (i >= CurrentFrame)
                        {
                            var OldList = AllPonitList[i + 1];
                            AllPonitList[i] = OldList;
                        }

                    }

                    AllPonitList.Remove(framesCount - 1);
                    FramesCount--;
                    if (CurrentFrame >= framesCount)
                    {
                        CurrentFrame--;
                    }
                    AddFrame = true;
                    if (FramesCount == 1)
                    {
                        RemoveFrame = false;
                    }
                    BuildShowInit(CurrentFrame);
                });
            }
        }

        #endregion

        #region 编辑
        public RelayCommand<CursorEnum> ChangeCursorCommand
        {
            get
            {
                return new RelayCommand<CursorEnum>(a =>
                {
                    CursorEnum = a;
                    Messenger.Default.Send(new CursorModelChangeEvent { model = a });
                });
            }
        }
        #endregion


        #region 测试
        public RelayCommand OutputComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var json = JsonConvert.SerializeObject(ShowPonitList.Where(c => c.X <= 2).Select(c => new PointInit(c.Fill.Color) { X = c.X, Y = c.Y }));

                });
            }
        }

        public RelayCommand addOneComand
        {
            get
            {

                return new RelayCommand(() =>
                {
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.one, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.two, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.three, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.four, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.five, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.six, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.seven, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.eight, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.nine, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.zero, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.percent, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.celsius, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.lianjiexian, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    BuildShow(CurrentFrame);
                });
            }
        }

        public RelayCommand SaveCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    if (JsonFileInfo == null)
                    {
                        return;
                    }
                    Dictionary<int, List<PointItem>> saveInfo = new Dictionary<int, List<PointItem>>();
                    foreach (var item in AllPonitList)
                    {
                        var templist = item.Value.Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                        templist.Sort();
                        saveInfo.Add(item.Key, templist);
                    }
                    FileHelper.Save(JsonConvert.SerializeObject(saveInfo), JsonFileInfo,CurrentDevInfo?.Id?.ToString());

                }
                );
            }
        }



        public RelayCommand CleanComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    AllPonitList[currFrame].ForEach(c => c.Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor)));
                   
                    BuildShow(CurrentFrame);
                });
            }
        }
        #endregion


        #region Cpu动态
        public RelayCommand LiveShowStartComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LiveShowStart();
                });
            }
        }

        private void LiveShowStart()
        {
            if (jsonFileInfo == null)
            {
                return;
            }
            Messenger.Default.Send(new LiveStartEvent());
            StartLive = false;
            EndLive = true;
            AddFrame = false;
            RemoveFrame = false;
            if (JsonFileInfo.IsDynamic)
            {
                if (jsonFileInfo.FileName == "cpu")
                {
                    if (CPUHelper == null)
                    {

                        CPUHelper = new CPUHelper();
                    }
                    CPUHelper.Start();
                }
                if (jsonFileInfo.FileName == "gpu")
                {
                    if (GPUHelper == null)
                    {

                        GPUHelper = new GPUHelper();
                    }
                    GPUHelper.Start();
                }

            }
            else
            {
        
                LiveShowHelper.Instance .Start(30);
            }
        }

        public RelayCommand LiveShowEndComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LiveShowEnd();

                });
            }
        }

        private void LiveShowEnd()
        {
            if (JsonFileInfo == null)
            {
                return;
            }
            Messenger.Default.Send(new HistroyInitEvent
            {
                HistoryItem = new HistoryItem()
                {
                    IsAdd = false,
                    ShowPointItems = this.AllPonitList[CurrentFrame].Select(c => new HistoryShowItemPoint()
                    {
                        X = c.X,
                        Y = c.Y,
                        OldFill = null,
                        NewFill = c.Fill
                    }).ToList(),
                }
            });
            StartLive = true;
            EndLive = false;
            RemoveFrame = true;
            AddFrame = true;    
            if (FramesCount == 1)
            {
                RemoveFrame = false;
            }
            if (framesCount >= 110)
            {
                AddFrame = false;

            }
            if (JsonFileInfo.IsDynamic)
            {
                GPUHelper?.End();
                CPUHelper?.End();
            }
            else
            {
                LiveShowHelper.Instance.End();
            }
        }
        #endregion

        #region

        private Visibility open;
        public Visibility Open
        {
            get { return open; }
            set
            {
                open = value;
                RaisePropertyChanged();
            }
        }
        private Visibility close;

        public Visibility Close
        {
            get { return close; }
            set
            {
                close = value;
                RaisePropertyChanged();
            }
        }


        public RelayCommand SendOpenSendMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                   SerialPortHelper.Instance.SendOpenSendMessage();
                    Close = Visibility.Visible;
                    Open = Visibility.Collapsed;
                }
                );
            }
        }


        public RelayCommand SendCloseSendMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                   SerialPortHelper.Instance.SendCloseSendMessage();
                    Open  = Visibility.Visible;
                    Close = Visibility.Collapsed;
                }
                );
            }
        }
        public RelayCommand SendLuminanceSendMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SerialPortHelper.Instance.SendLuminanceSendMessage(Luminance);
                }
                );
            }
        }

        #endregion






        public PointListViewModel()
        {
            Messenger.Default.Register<PonitClickedEvent>(this, HandlePonitClickedEvent);
            Messenger.Default.Register<HardInfoEvent>(this, HandleCpuInfoEvent);

            Messenger.Default.Register<InitFromHistroyEvent>(this, HanderInitFromHistroyEvent);
            Messenger.Default.Register<ThemeItemClickedEvent>(this, HandleChangeTheMeEvent);
            Messenger.Default.Register<NewThemeEvent>(this, HandleNewThemeEvent);
            Messenger.Default.Register<DebugInfoEvent>(this, HandleDebugInfoEvent);

            Messenger.Default.Register<NextFrameEvent>(this, HandleNextFrameEvent);
            Messenger.Default.Register<LumianceChangeEvent>(this, HandleLumianceChangeEventt);
            Open = Visibility.Visible;
            Close = Visibility.Collapsed;
            Luminance = 255;
            ComName = "COM5";
            CursorEnum = CursorEnum.MOVE;
            ChangeColor = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor));
            DebugInfo = "*********************************************调试信息******************" + "\r\n";
        }

        private void HandleLumianceChangeEventt(LumianceChangeEvent obj)
        {
            SerialPortHelper.Instance.SendLuminanceSendMessage(Luminance);
        }

        public RelayCommand CleanDebugInfoCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    DebugInfo = "*********************************************调试信息******************" + "\r\n";
                }
                );
            }
        }

        private void HandleNextFrameEvent(NextFrameEvent obj)
        {
            if (!JsonFileInfo.IsDynamic)
            {
                CurrentFrame++;
                if (CurrentFrame >= framesCount)
                {
                    CurrentFrame = 0;
                }
                BuildShowPreView(CurrentFrame);
            }
        }

        private void HandleDebugInfoEvent(DebugInfoEvent obj)
        {
            logger.Info(obj.Msg);
            if (obj.Msg.Contains("themeSegment"))
            {
                if (obj.Msg.Contains("发送消息"))
                {

                    DebugInfo += "发送消息 ==> themeSegment " + "\r\n";
                }
                else if (obj.Msg.Contains("接收消息"))
                {

                    DebugInfo += "接收消息<==  themeSegment " + "\r\n";
                }

            }
            else
            {
                if (DebugInfo.Length > 5000)
                {
                    DebugInfo = DebugInfo.Substring(3000);
                }
                DebugInfo += obj.Msg;
            }

        }

        private void HandleNewThemeEvent(NewThemeEvent obj)
        {
            LiveShowHelper.Instance.End();
            CPUHelper?.End();
            GPUHelper?.End();
            CurrentFrame = 0;
            FramesCount = 1;
            AllPonitList = new Dictionary<int, List<PointItem>>();
            var OnFrameAllPonitList = new List<PointItem>();
            for (int x = 0; x < xIndex; x++)
            {
                for (int y = 0; y < yIndex; y++)
                {
                    OnFrameAllPonitList.Add(new PointItem(x, y));
                }
            }
            OnFrameAllPonitList.Sort();
            AllPonitList[0] = OnFrameAllPonitList;
            BuildShowInit(CurrentFrame);
            FileHelper.SaveThemeName(obj.JsonFileInfo,obj.CurrentDevInfo?.Id?.ToString());
            FileHelper.Save(JsonConvert.SerializeObject(AllPonitList), obj.JsonFileInfo,obj.CurrentDevInfo?.Id?.ToString());
        }
        #region EventHander
        private void HanderInitFromHistroyEvent(InitFromHistroyEvent obj)
        {
            switch (obj.HistoryEnum)
            {
                case HistoryEnum.ReBack:
                    if (obj.HistoryItem.IsAdd)
                    {
                        switch (obj.HistoryItem.add)
                        {
                            case addenum.top:
                                AllPonitList[CurrentFrame].ForEach(c => c.Y -= 1);
                                break;
                            case addenum.bottom:
                                AllPonitList[CurrentFrame].ForEach(c => c.Y += 1);
                                break;
                            case addenum.left:
                                AllPonitList[CurrentFrame].ForEach(c => c.X += 1);
                                break;
                            case addenum.right:
                                AllPonitList[CurrentFrame].ForEach(c => c.X -= 1);
                                break;
                            default:
                                break;
                        }
                    }
                    foreach (var item in obj.HistoryItem.ShowPointItems)
                    {
                        if (!obj.HistoryItem.IsAdd)
                        {
                            var temp = AllPonitList[CurrentFrame].FirstOrDefault(c => c.X == item.X && c.Y == item.Y);
                            temp.Fill = item.OldFill;

                          
                        }
                        var tempShowpoint = ShowPonitList.FirstOrDefault(c => c.X == item.X && c.Y == item.Y);
                        tempShowpoint.Fill = item.OldFill;
                    }


                    break;
                case HistoryEnum.Cancel:
                    if (obj.HistoryItem.IsAdd)
                    {

                        switch (obj.HistoryItem.add)
                        {
                            case addenum.top:
                                AllPonitList[CurrentFrame].ForEach(c => c.Y += 1);
                                break;
                            case addenum.bottom:
                                AllPonitList[CurrentFrame].ForEach(c => c.Y -= 1);
                                break;
                            case addenum.left:
                                AllPonitList[CurrentFrame].ForEach(c => c.X -= 1);
                                break;
                            case addenum.right:
                                AllPonitList[CurrentFrame].ForEach(c => c.X += 1);
                                break;
                            default:
                                break;
                        }

                    }
                    foreach (var item in obj.HistoryItem.ShowPointItems)
                    {
                        if (!obj.HistoryItem.IsAdd)
                        {
                            var temp = AllPonitList[CurrentFrame].FirstOrDefault(c => c.X == item.X && c.Y == item.Y);
                            temp.Fill = item.NewFill;


                        }
                        var tempShowpoint = ShowPonitList.FirstOrDefault(c => c.X == item.X && c.Y == item.Y);
                        tempShowpoint.Fill = item.NewFill;
                    }
                    break;
                case HistoryEnum.Redo:
                    AllPonitList[CurrentFrame] = obj.HistoryItem.ShowPointItems.Select(c =>
                     new PointItem
                     {X =c.X,
                     Y=c.Y,
                     Fill = c.NewFill
                     }).ToList();
                    var showPonit = AllPonitList[CurrentFrame].Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                    for (int i = 0; i < xIndex * yIndex; i++)
                    {
                        if (showPonit[i].Fill != ShowPonitList[i].Fill)
                        {
                            ShowPonitList[i].Fill = showPonit[i].Fill;
                        }
                    }

                    break;
                default:
                    break;
            }
        

        }
        private void HandleChangeTheMeEvent(ThemeItemClickedEvent obj)
        {
            CurrentDevInfo = obj.CurrentDevInfo;
            LiveShowEnd();
            CurrentFrame = 0;
            IsSend = false;
            CursorEnum = CursorEnum.MOVE;
            //ChangeColor = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor));
            JsonFileInfo = obj.JsonFileInfo;
            if (!JsonFileInfo.IsDynamic)
            {
                Outgif = true;
                var json = FileHelper.Open(obj.JsonFileInfo.FileName);
                AllPonitList = JsonConvert.DeserializeObject<Dictionary<int, List<PointItem>>>(json);
                FramesCount = AllPonitList.Count;
                BuildShowInit(CurrentFrame);
            }
            else
            {
                Outgif = false;
                AllPonitList = new Dictionary<int, List<PointItem>>();
                var OnFrameAllPonitList = new List<PointItem>();
                for (int x = 0; x < xIndex; x++)
                {
                    for (int y = 0; y < yIndex; y++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
                AllPonitList[0] = OnFrameAllPonitList;
                FramesCount = AllPonitList.Count;
                BuildShowInit(CurrentFrame);
            }
            RemoveFrame = true;
            AddFrame = true;
            if (FramesCount == 1)
            {
                RemoveFrame = false;
            }
            if (framesCount>=110)
            {
                AddFrame = false;

            }
            LiveShowStart();
        }
        private void HandleCpuInfoEvent(HardInfoEvent obj)
        {
            if (JsonFileInfo.IsDynamic)
            {
                AllPonitList[CurrentFrame] = AllPonitList[currFrame].Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                AllPonitList[CurrentFrame].ForEach(c => c.Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor)));
                XMoveCommand(AllPonitList[CurrentFrame], 1);
                AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.celsius, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                var cpucelsius = (int)obj.Temp;

                do
                {
                    int remainder = cpucelsius % 10;
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    cpucelsius = cpucelsius / 10;
                } while (cpucelsius >= 1);
                if (obj.Temp<100&&obj.Temp>=10)
                {
                    XMoveCommand(AllPonitList[CurrentFrame], 4);
                }
                else if (obj.Temp < 10 && obj.Temp > 0)
                {
                    XMoveCommand(AllPonitList[CurrentFrame], 8);
                }
                else if (obj.Temp == 0)
                {
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)0, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 8);
                }
                XMoveCommand(AllPonitList[CurrentFrame], 1);
                AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.lianjiexian, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                XMoveCommand(AllPonitList[CurrentFrame], 1);
                AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.percent, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                var cpu = (int)obj.Use;

                do
                {
                    int remainder = cpu % 10;
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    cpu = cpu / 10;
                } while (cpu >= 1);

                if (obj.Use < 100 && obj.Use >= 10)
                {
                    XMoveCommand(AllPonitList[CurrentFrame], 4);
                }
                else if (obj.Use < 10 && obj.Use > 0)
                {
                    XMoveCommand(AllPonitList[CurrentFrame], 8);
                }
                else if (obj.Use == 0)
                {
                    XMoveCommand(AllPonitList[CurrentFrame], 1);
                    AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)0, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(AllPonitList[CurrentFrame], 8);
                }
                XMoveCommand(AllPonitList[CurrentFrame], 22);

                AllPonitList[currFrame].Sort();
                BuildShowPreView(CurrentFrame);

                if (IsSend)
                {
                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;
                    var msg = MessageHelper.BuildDynamic(AllPonitList, xIndex, yIndex, fileName);

                    SerialPortHelper.Instance.SendThemeSegmentSendMessage(msg);
                }
            }

        }
        private void HandlePonitClickedEvent(PonitClickedEvent item)
        {
            if (CursorEnum == CursorEnum.Magic)
            {
                AllPonitList[CurrentFrame].Find(c => c.Y == item.Y && c.X == item.X).Fill = ChangeColor;
                BuildShow(CurrentFrame);
            }
            else if (CursorEnum == CursorEnum.ERASE)
            {
                AllPonitList[CurrentFrame].Find(c => c.Y == item.Y && c.X == item.X).Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor));
                BuildShow(CurrentFrame);
            }
        }
        #endregion

    }

    public class PointItem : ViewModelBase, IComparable<PointItem>, IPontBase
    {
        [JsonIgnore]
        public RelayCommand<PointItem> ChangeColorCommand
        {
            get
            {
                return new RelayCommand<PointItem>(item =>
                {
                    Messenger.Default.Send(new PonitClickedEvent { X = item.X, Y = item.Y });
                });
            }
        }
        private SolidColorBrush fill { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public SolidColorBrush Fill
        {
            get { return fill; }
            set { fill = value; RaisePropertyChanged(); }
        }

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

            Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor));
            X = x;
            Y = y;
        }
        public PointItem(int x, int y, System.Windows.Media.Color color)
        {

            Fill = new SolidColorBrush(color);
            X = x;
            Y = y;
        }
        public PointItem()
        {
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

    public interface IPontBase
    {
        /// <summary>
        /// 颜色
        /// </summary>
        SolidColorBrush Fill { get; set; }

        /// <summary>
        /// 横坐标
        /// </summary>
        int X { get; set; }
        /// <summary>
        /// 纵坐标
        /// </summary>
        int Y { get; set; }
    }

}
