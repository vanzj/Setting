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
using AnimatedGif;
using System.Windows.Interop;
using Setting.Event.MsgSendEvent;
using System.Windows.Markup;

namespace Setting.ViewModel
{
    public class PointListViewModel : ViewModelBase
    {

        #region 页面属性


        private bool seedEnabled;

        public bool SeedEnabled
        {
            get { return seedEnabled; }
            set { seedEnabled = value; RaisePropertyChanged(); }
        }

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
        private string msgText;
        /// <summary>
        /// 文件名
        /// </summary>
        public string MsgText
        {
            get { return msgText; }
            set
            {
                msgText = value;
                RaisePropertyChanged();
                MsgTextLength = msgText.Length;
            }
        }


        private int msgTextLength;
        /// <summary>
        /// 文件名
        /// </summary>
        public int MsgTextLength

        {
            get { return msgTextLength; }
            set { msgTextLength = value; RaisePropertyChanged(); }
        }
        private Visibility msgTextVisibility;
        /// <summary>
        /// 文件名
        /// </summary>
        public Visibility MsgTextVisibility
        {
            get { return msgTextVisibility; }
            set { msgTextVisibility = value; RaisePropertyChanged(); }
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



        private LiveShowHelper LiveShowHelper { get; set; }
        public int xIndex = 85;
        public int yIndex = 5;
        public bool IsSend = false;
        private AllPonitListExInfo allPonitListExInfo { get; set; } = new AllPonitListExInfo();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private RelayCommand _openFileCommand;

        public RelayCommand OpenFileCommand
        {
            get
            {
                return _openFileCommand ?? (_openFileCommand = new RelayCommand(ExecuteOpenFileCommand));
            }
        }


        public RelayCommand SendOnepackageCommandTwo
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new SendStartEvent(this.DevNo));

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
                            count = allPonitListExInfo.AllPonitList.Count.ToString(),
                            frameCount = allPonitListExInfo.AllPonitList.Count.ToString(),
                            frameRate = "10",
                            brightness = 255
                        };
                        var templist = new List<string>();
                        templist.Add(JsonConvert.SerializeObject(themeSend));
                        templist.Add(JsonConvert.SerializeObject(themeSendStartSend));
                        Messenger.Default.Send(new SendThemeDynamicSendMessageEvent() { data = templist });
                        HardHelper.Instance.Start();
                        IsSend = true;
                    }
                    else
                    {

                        var msg = MessageHelper.BuildOnePackage(allPonitListExInfo.AllPonitList, xIndex, yIndex, fileName, Luminance, allPonitListExInfo.FrameRate);

                        //Task.Run(() =>
                        //{
                        Messenger.Default.Send(new SendThemeCirculateSendMessageEvent() { data = msg });
                        //}
                        //);

                    }

                })
                {

                };
            }
        }
        public RelayCommand StartSendTextCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MsgText = "";
                    MsgTextVisibility = Visibility.Visible;

                });
            }
        }
        public RelayCommand SendTextCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    var text = MsgText;

                    JdClient jdClient = new JdClient(HttpClientHelper.Instance.GetHttpClient());
                    var temp = jdClient.TransferGifUsingGETAsync(16, 14, text, 16).GetAwaiter().GetResult();
                    if (temp.Code == 0)
                    {
                        int brightness = 255;
                        int defaultFramerate = 6;
                        var url = temp.Data.ToString();
                        InputGIF inputGIF = new InputGIF();
                        var ImgInfo = inputGIF.OPENGIFURL(url, 85, 5, "gifword", brightness, defaultFramerate);
                        Messenger.Default.Send(new SendStartEvent(this.DevNo));
                        var data = MessageHelper.BuildOnePackageGIFURL(ImgInfo, 85, 5, "gifword", defaultFramerate);
                        Messenger.Default.Send(new SendThemeCirculateSendMessageEvent() { data = data });

                        MsgText = "";
                        MsgTextVisibility = Visibility.Collapsed;
                    }
                });
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


                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;

                    // 创建一个OpenFileDialog实例
                    var SaveFileDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        // 设置一些基本属性，如过滤器
                        Filter = "Text files | *.gif",
                        FileName = fileName,
                        InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                    };


                    // 显示对话框
                    // 注意：ShowDialog方法将返回一个可空的bool值，当用户选择文件并点击“打开”时为true
                    if (SaveFileDialog.ShowDialog() == true)
                    {
                        // 获取用户选择的文件路径
                        string file = SaveFileDialog.FileName;

                        var GIFName = JsonFileInfo.Name;

                        var templist = MessageHelper.Buildgif(allPonitListExInfo.AllPonitList, xIndex, yIndex, fileName, allPonitListExInfo.FrameRate);

                        OutPutGIF outPutGIF = new OutPutGIF();


                        outPutGIF.Output(templist, xIndex, yIndex, 20, file);

                    }

                })
                {

                };
            }
        }


        private void ExecuteOpenFileCommand()
        {
            ExecuteOpenFileMethod();

        }

        private void ExecuteOpenFileMethod(JsonFileInfo newJsonFileInfo = null)
        {
            bool isadd = false;
            if (newJsonFileInfo == null)
            {
                isadd = true;
            }

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

                if (newJsonFileInfo == null)
                {
                    var ThemeName = "新建模板" + "-" + ThemeCountHelper.GetNextCount();
                    var guid = Guid.NewGuid().ToString("N");
                    newJsonFileInfo = new JsonFileInfo()
                    {
                        Name = ThemeName,
                        FileName = guid.Substring(guid.Length - 6)
                    };
                }

                InputGIF inputGIF = new InputGIF();
                var ImgInfo = inputGIF.OPENGIF(file, xIndex, yIndex, newJsonFileInfo.FileName);


                FramesCount = int.Parse(ImgInfo[0].frameCount) > 110 ? 110 : int.Parse(ImgInfo[0].frameCount);

                allPonitListExInfo = new AllPonitListExInfo();
                allPonitListExInfo.FrameRate = ImgInfo[0].frameRate;
                int i = 0;
                foreach (var Oneseg in ImgInfo)
                {
                    if (i >= 110)
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

                    allPonitListExInfo.AllPonitList.Add(int.Parse(Oneseg.pointList[0].frameIndex), oneframePointList);
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
                if (FramesCount >= 110)
                {
                    AddFrame = false;
                }
                FileHelper.SaveThemeName(newJsonFileInfo);
                FileHelper.Save(JsonConvert.SerializeObject(allPonitListExInfo), newJsonFileInfo);
                JsonFileInfo = newJsonFileInfo;

                if (isadd)
                {
                    Messenger.Default.Send(new InputNewThemeEvent { JsonFileInfo = newJsonFileInfo });
                }

            }
        }
        private void BuildShow(int frameIndex)
        {
            var guid = Guid.NewGuid().ToString("N");
            if (string.IsNullOrEmpty(JsonFileInfo.NewFileName))
            {
                JsonFileInfo.NewFileName = guid.Substring(guid.Length - 6);

            }

            if (allPonitListExInfo.AllPonitList.Count > 0)
            {
                var OnFrameAllPonitList = allPonitListExInfo.AllPonitList[frameIndex];

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

        private void BuildShowAdd(int frameIndex, addenum addenum)
        {
            var guid = Guid.NewGuid().ToString("N");
            if (string.IsNullOrEmpty(JsonFileInfo.NewFileName))
            {
                JsonFileInfo.NewFileName = guid.Substring(guid.Length - 6);

            }

            if (allPonitListExInfo.AllPonitList.Count > 0)
            {
                var OnFrameAllPonitList = allPonitListExInfo.AllPonitList[frameIndex];

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
                        add = addenum,
                        ShowPointItems = historyShowItemPoints,
                    }
                });
            }


        }

        private void BuildShowInit(int frameIndex)
        {

            if (allPonitListExInfo.AllPonitList.Count > 0)
            {
                var OnFrameAllPonitList = allPonitListExInfo.AllPonitList[frameIndex];

                if (ShowPonitList == null)
                {
                    ShowPonitList = new ObservableCollection<PointItem>();
                    foreach (var c in OnFrameAllPonitList)
                    {

                        if (c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex)
                        {
                            ShowPonitList.Add(new PointItem()
                            {
                                X = c.X,
                                Y = c.Y,
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
                        ShowPointItems = this.allPonitListExInfo.AllPonitList[CurrentFrame].Select(c => new HistoryShowItemPoint()
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

            if (allPonitListExInfo.AllPonitList.Count > 0)
            {
                var OnFrameAllPonitList = allPonitListExInfo.AllPonitList[frameIndex];

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
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], -1);
                    BuildShowAdd(CurrentFrame, addenum.left);
                });
            }
        }
        public RelayCommand RightCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    BuildShowAdd(CurrentFrame, addenum.right);
                });
            }
        }

        public RelayCommand TopCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    YMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], -1);
                    BuildShowAdd(CurrentFrame, addenum.top);
                });
            }
        }
        public RelayCommand BottomCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    YMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    BuildShowAdd(CurrentFrame, addenum.bottom);
                });
            }
        }
        private void XMoveCommand(List<PointItem> OnFrameAllPonitList, int xMove)
        {
            var maxy = OnFrameAllPonitList.Max(c => c.Y);
            var miny = OnFrameAllPonitList.Min(c => c.Y);
            var ylenght = maxy - miny + 1;
            OnFrameAllPonitList.ForEach(c => c.X = c.X + xMove);
            var minx = OnFrameAllPonitList.Min(c => c.X);
            var maxX = OnFrameAllPonitList.Max(c => c.X);




            if (minx > 0)
            {
                //左边 加
                for (int x = 0; x < minx; x++)
                {
                    for (int y = miny; y < ylenght; y++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }



            if (maxX + 1 < xIndex)
            {//you边加
                for (int x = maxX + 1; x < xIndex; x++)
                {
                    for (int y = miny; y < ylenght; y++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }
            minx = OnFrameAllPonitList.Min(c => c.X);
            maxX = OnFrameAllPonitList.Max(c => c.X);

            // 补缺
            for (int x = minx; x < maxX + 1; x++)
            {
                for (int y = miny; y < maxy + 1; y++)
                {
                    if (!OnFrameAllPonitList.Any(c => c.X == x && c.Y == y))
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }







            OnFrameAllPonitList.Sort();

        }
        private void YMoveCommand(List<PointItem> OnFrameAllPonitList, int yMove)
        {
            var minx = OnFrameAllPonitList.Min(c => c.X);
            var maxX = OnFrameAllPonitList.Max(c => c.X);
            var xlenght = maxX - minx + 1;
            OnFrameAllPonitList.ForEach(c => c.Y = c.Y + yMove);
            var miny = OnFrameAllPonitList.Min(c => c.Y);
            var maxy = OnFrameAllPonitList.Max(c => c.Y);


            if (miny > 0)
            {
                //上 加
                for (int y = 0; y < miny; y++)
                {
                    for (int x = minx; x < xlenght; x++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }

            if (maxy + 1 < yIndex)
            {//
                for (int y = maxy + 1; y < yIndex; y++)
                {
                    for (int x = minx; x < xlenght; x++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
            }
            miny = OnFrameAllPonitList.Min(c => c.Y);
            maxy = OnFrameAllPonitList.Max(c => c.Y);
            // 补缺
            for (int x = minx; x < maxX + 1; x++)
            {
                for (int y = miny; y < maxy + 1; y++)
                {
                    if (!OnFrameAllPonitList.Any(c => c.X == x && c.Y == y))
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
                    var OldList = allPonitListExInfo.AllPonitList[i - 1];
                    var temp = new List<PointItem>();
                    if (allPonitListExInfo.AllPonitList.TryGetValue(i, out temp))
                    {
                        allPonitListExInfo.AllPonitList[i] = OldList;
                    }
                    else
                    {
                        allPonitListExInfo.AllPonitList.Add(i, OldList);
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
                    allPonitListExInfo.AllPonitList[i] = OnFrameAllPonitList;
                }
            }
            CurrentFrame++;
            FramesCount++;
            if (FramesCount >= 110)
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
                            var OldList = allPonitListExInfo.AllPonitList[i + 1];
                            allPonitListExInfo.AllPonitList[i] = OldList;
                        }

                    }

                    allPonitListExInfo.AllPonitList.Remove(framesCount - 1);
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
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.one, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.two, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.three, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.four, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.five, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.six, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.seven, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.eight, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.nine, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.zero, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.percent, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.celsius, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.lianjiexian, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
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
                    Save();
                }
                );
            }
        }


        private void Save()
        {
            if (JsonFileInfo == null)
            {
                return;
            }
            AllPonitListExInfo saveInfo = new AllPonitListExInfo();
            foreach (var item in allPonitListExInfo.AllPonitList)
            {
                var templist = item.Value.Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                templist.Sort();
                saveInfo.AllPonitList.Add(item.Key, templist);
            }
            FileHelper.Save(JsonConvert.SerializeObject(saveInfo), JsonFileInfo);

        }


        public RelayCommand CleanComand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    allPonitListExInfo.AllPonitList[currFrame].ForEach(c => c.Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor)));

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

                HardHelper.Instance.Start();
            }
            else
            {
                if (LiveShowHelper == null)
                {
                    LiveShowHelper = new LiveShowHelper();
                }

                LiveShowHelper.Start(allPonitListExInfo.FrameRate);
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
            if (JsonFileInfo.IsDynamic)
            {
                HardHelper.Instance.End();
            }
            else
            {
                LiveShowHelper.End();
            }
            Messenger.Default.Send(new HistroyInitEvent
            {
                HistoryItem = new HistoryItem()
                {
                    IsAdd = false,
                    ShowPointItems = this.allPonitListExInfo.AllPonitList[CurrentFrame].Select(c => new HistoryShowItemPoint()
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

        private Visibility disenableRotate;
        public Visibility DisenableRotate
        {
            get { return disenableRotate; }
            set
            {
                disenableRotate = value;
                RaisePropertyChanged();
            }
        }
        private Visibility enableRotate;

        public Visibility EnableRotate
        {
            get { return enableRotate; }
            set
            {
                enableRotate = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SendNetWorkCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new SendNetWorkEvent() { });
                }
                );
            }
        }
        public RelayCommand SendOpenSendMessageCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new SendOpenSendMessageEvent() { });
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

                    Messenger.Default.Send(new SendCloseSendMessageEvent() { });
                    Open = Visibility.Visible;
                    Close = Visibility.Collapsed;
                }
                );
            }
        }
        public RelayCommand SendEnableRotateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    Messenger.Default.Send(new SendEnableRotateMessageEvent() { });
                    DisenableRotate = Visibility.Visible;
                    EnableRotate = Visibility.Collapsed;
                }
                );
            }
        }
        public RelayCommand SendDisableRotateCommand
        {
            get
            {
                return new RelayCommand(() =>
                {

                    Messenger.Default.Send(new SendDisenableRotateMessageEvent() { });
                    EnableRotate = Visibility.Visible;
                    DisenableRotate = Visibility.Collapsed;
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
                    Messenger.Default.Send(new SendLuminanceSendMessageEvent() { Luminance = Luminance });
                }
                );
            }
        }

        #endregion



        private string accountName;
        public string AccountName
        {
            get { return accountName; }
            set
            {
                accountName = value;
                RaisePropertyChanged();
            }
        }



        private string screenName;
        public string ScreenName
        {
            get { return screenName; }
            set
            {
                screenName = value;
                RaisePropertyChanged();
            }
        }
        public string DevNo { get; set; }


        public PointListViewModel()
        {
            Messenger.Default.Register<InputThemeEvent>(this, HandlInputThemeEvent);
            Messenger.Default.Register<PonitClickedEvent>(this, HandlePonitClickedEvent);
            Messenger.Default.Register<HardInfoEvent>(this, HandleCpuInfoEvent);
            Messenger.Default.Register<ScreenNameAndDevNoEvent>(this, HandleScreenNameAndDevNoEvent);
            Messenger.Default.Register<AccountEvent>(this, HandleAccountEvent);
            Messenger.Default.Register<InitFromHistroyEvent>(this, HanderInitFromHistroyEvent);
            Messenger.Default.Register<ThemeItemClickedEvent>(this, HandleChangeTheMeEvent);
            Messenger.Default.Register<NewThemeEvent>(this, HandleNewThemeEvent);
            Messenger.Default.Register<DebugInfoEvent>(this, HandleDebugInfoEvent);
            Messenger.Default.Register<ScreenInfoEvent>(this, HandleScreenInfoEvent);
            Messenger.Default.Register<MsgSendCloseEvent>(this, HandleMsgSendCloseEvent);



            Messenger.Default.Register<NextFrameEvent>(this, HandleNextFrameEvent);
            Messenger.Default.Register<LumianceChangeEvent>(this, HandleLumianceChangeEventt);

            Messenger.Default.Register<LostCurrentScreenEvent>(this, HandleLostScreenEvent);
            Messenger.Default.Register<ConnectCurrentScreenEvent>(this, HandleReConnectScreenEvent);
            Messenger.Default.Register<RedoSaveEvent>(this, HandleRedoSaveEvent);
            Messenger.Default.Register<FrameChangeEvent>(this, HandleFrameChangeEvent);
            SeedEnabled = false;
            Open = Visibility.Visible;
            Close = Visibility.Collapsed;
            EnableRotate = Visibility.Visible;
            DisenableRotate = Visibility.Collapsed;
            MsgTextVisibility = Visibility.Collapsed;
            Luminance = 255;
            CursorEnum = CursorEnum.MOVE;
            ChangeColor = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor));
            DebugInfo = "*********************************************调试信息******************" + "\r\n";
        }

        private void HandleMsgSendCloseEvent(MsgSendCloseEvent @event)
        {
            MsgText = "";
            MsgTextVisibility = Visibility.Collapsed;
        }

        private void HandleFrameChangeEvent(FrameChangeEvent @event)
        {
            CurrentFrame = @event.CurrentFrame;
            BuildShowInit(CurrentFrame);
        }

        private void HandleRedoSaveEvent(RedoSaveEvent @event)
        {
            Save();
        }

        private void HandleScreenInfoEvent(ScreenInfoEvent obj)
        {
            if (obj.DevNo == this.DevNo)
            {
                Luminance = obj.lum;
                if (obj.IsOpen)
                {
                    Close = Visibility.Visible;
                    Open = Visibility.Collapsed;
                }
                else
                {
                    Open = Visibility.Visible;
                    Close = Visibility.Collapsed;
                }
                if (obj.IsRotate)
                {
                    EnableRotate = Visibility.Visible;
                    DisenableRotate = Visibility.Collapsed;
                }
                else
                {
                    DisenableRotate = Visibility.Visible;
                    EnableRotate = Visibility.Collapsed;
                }
            }
          
        }

        private void HandleReConnectScreenEvent(ConnectCurrentScreenEvent obj)
        {
            if (DevNo == obj.DevNo)
            {
                Messenger.Default.Send(new MsgEvent("屏幕连接成功"));
                SeedEnabled = true;
            }
        }

        private void HandleLostScreenEvent(LostCurrentScreenEvent obj)
        {
            if (DevNo == obj.DevNo)
            {
                Messenger.Default.Send(new MsgEvent("屏幕断开连接"));
                SeedEnabled = false;
            }

        }

        private void HandleAccountEvent(AccountEvent obj)
        {
            AccountName = obj.Name;
        }

        private void HandleScreenNameAndDevNoEvent(ScreenNameAndDevNoEvent obj)
        {
            DevNo = obj.DevNo;
           var temp = SerialPortScanHelper.Instance.SerialPortList.Where(c=>c.DevNo==obj.DevNo).FirstOrDefault();
            if (temp is null)
            {
                SeedEnabled = false;
            }
            else
            {
                if (!temp.Connected)
                {
                    SeedEnabled = false;
                }
                else
                {
                    SeedEnabled = true;
                }
            } 
            ScreenName = obj.Name;
        }

        private void HandlInputThemeEvent(InputThemeEvent obj)
        {
            ExecuteOpenFileMethod(obj.JsonFileInfo);
        }

        private void HandleLumianceChangeEventt(LumianceChangeEvent obj)
        {
            Messenger.Default.Send(new SendLuminanceSendMessageEvent() { Luminance = Luminance });
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

                    DebugInfo += $"{DateTime.Now.ToString("O")}" + "发送消息 ==> themeSegment " + "\r\n";
                }
                else if (obj.Msg.Contains("接收消息"))
                {

                    DebugInfo += $"{DateTime.Now.ToString("O")}" + "接收消息<==  themeSegment " + "\r\n";
                }

            }
            else
            {
                if (DebugInfo.Length > 5000)
                {
                    DebugInfo = DebugInfo.Substring(3000);
                }
                DebugInfo += $"{DateTime.Now.ToString("O")}" + obj.Msg;
            }

        }

        private void HandleNewThemeEvent(NewThemeEvent obj)
        {
            LiveShowHelper?.End();
            HardHelper.Instance.End();

            CurrentFrame = 0;
            FramesCount = 1;
            allPonitListExInfo = new AllPonitListExInfo();
            var OnFrameAllPonitList = new List<PointItem>();
            for (int x = 0; x < xIndex; x++)
            {
                for (int y = 0; y < yIndex; y++)
                {
                    OnFrameAllPonitList.Add(new PointItem(x, y));
                }
            }
            OnFrameAllPonitList.Sort();
            allPonitListExInfo.AllPonitList[0] = OnFrameAllPonitList;
            BuildShowInit(CurrentFrame);
            FileHelper.SaveThemeName(obj.JsonFileInfo);
            FileHelper.Save(JsonConvert.SerializeObject(allPonitListExInfo), obj.JsonFileInfo);
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
                                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.Y -= 1);
                                break;
                            case addenum.bottom:
                                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.Y += 1);
                                break;
                            case addenum.left:
                                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 1);
                                break;
                            case addenum.right:
                                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X -= 1);
                                break;
                            default:
                                break;
                        }
                    }
                    foreach (var item in obj.HistoryItem.ShowPointItems)
                    {
                        if (!obj.HistoryItem.IsAdd)
                        {
                            var temp = allPonitListExInfo.AllPonitList[CurrentFrame].FirstOrDefault(c => c.X == item.X && c.Y == item.Y);
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
                                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.Y += 1);
                                break;
                            case addenum.bottom:
                                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.Y -= 1);
                                break;
                            case addenum.left:
                                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X -= 1);
                                break;
                            case addenum.right:
                                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 1);
                                break;
                            default:
                                break;
                        }

                    }
                    foreach (var item in obj.HistoryItem.ShowPointItems)
                    {
                        if (!obj.HistoryItem.IsAdd)
                        {
                            var temp = allPonitListExInfo.AllPonitList[CurrentFrame].FirstOrDefault(c => c.X == item.X && c.Y == item.Y);
                            temp.Fill = item.NewFill;


                        }
                        var tempShowpoint = ShowPonitList.FirstOrDefault(c => c.X == item.X && c.Y == item.Y);
                        tempShowpoint.Fill = item.NewFill;
                    }
                    break;
                case HistoryEnum.Redo:
                    allPonitListExInfo.AllPonitList[CurrentFrame] = obj.HistoryItem.ShowPointItems.Select(c =>
                     new PointItem
                     {
                         X = c.X,
                         Y = c.Y,
                         Fill = c.NewFill
                     }).ToList();
                    var showPonit = allPonitListExInfo.AllPonitList[CurrentFrame].Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
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
                allPonitListExInfo = JsonConvert.DeserializeObject<AllPonitListExInfo>(json);
                FramesCount = allPonitListExInfo.AllPonitList.Count;
                BuildShowInit(CurrentFrame);
            }
            else
            {
                Outgif = false;
                allPonitListExInfo = new AllPonitListExInfo();
                var OnFrameAllPonitList = new List<PointItem>();
                for (int x = 0; x < xIndex; x++)
                {
                    for (int y = 0; y < yIndex; y++)
                    {
                        OnFrameAllPonitList.Add(new PointItem(x, y));
                    }
                }
                allPonitListExInfo.AllPonitList[0] = OnFrameAllPonitList;
                FramesCount = allPonitListExInfo.AllPonitList.Count;
                BuildShowInit(CurrentFrame);
            }
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
            LiveShowStart();
        }
        private void HandleCpuInfoEvent(HardInfoEvent obj)
        {
            if (JsonFileInfo.IsDynamic && jsonFileInfo.FileName == "cpu")
            {
                allPonitListExInfo.AllPonitList[CurrentFrame] = allPonitListExInfo.AllPonitList[currFrame].Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor)));
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.celsius, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                var cpucelsius = (int)obj.CPUTemp;

                do
                {
                    int remainder = cpucelsius % 10;
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    cpucelsius = cpucelsius / 10;
                } while (cpucelsius >= 1);
                if (obj.CPUTemp < 100 && obj.CPUTemp >= 10)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                else if (obj.CPUTemp < 10 && obj.CPUTemp > 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                else if (obj.CPUTemp == 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)0, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 8);
                }
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.lianjiexian, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.percent, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                var cpu = (int)obj.CPUUse;

                do
                {
                    int remainder = cpu % 10;
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    cpu = cpu / 10;
                } while (cpu >= 1);

                if (obj.CPUUse < 100 && obj.CPUUse >= 10)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                else if (obj.CPUUse < 10 && obj.CPUUse > 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 8);
                }
                else if (obj.CPUUse == 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)0, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 22);

                allPonitListExInfo.AllPonitList[currFrame].Sort();
                BuildShowPreView(CurrentFrame);

                if (IsSend)
                {
                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;
                    var msg = MessageHelper.BuildDynamic(allPonitListExInfo.AllPonitList, xIndex, yIndex, fileName, Luminance, allPonitListExInfo.FrameRate);

                    Messenger.Default.Send(new SendThemeSegmentSendMessageEvent() { Msg = msg });
                }
            }
            if (JsonFileInfo.IsDynamic && jsonFileInfo.FileName == "gpu")
            {
                allPonitListExInfo.AllPonitList[CurrentFrame] = allPonitListExInfo.AllPonitList[currFrame].Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor)));
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.celsius, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                var gpucelsius = (int)obj.GPUTemp;

                do
                {
                    int remainder = gpucelsius % 10;
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    gpucelsius = gpucelsius / 10;
                } while (gpucelsius >= 1);
                if (obj.GPUTemp < 100 && obj.GPUTemp >= 10)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                else if (obj.GPUTemp < 10 && obj.GPUTemp > 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 8);
                }
                else if (obj.GPUTemp == 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)0, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.lianjiexian, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 5);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.percent, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                var gpu = (int)obj.GPUUse;

                do
                {
                    int remainder = gpu % 10;
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    gpu = gpu / 10;
                } while (gpu >= 1);

                if (obj.GPUUse < 100 && obj.GPUUse >= 10)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                else if (obj.GPUUse < 10 && obj.GPUUse > 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 8);
                }
                else if (obj.GPUUse == 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 8);
                }
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 22);

                allPonitListExInfo.AllPonitList[currFrame].Sort();
                BuildShowPreView(CurrentFrame);

                if (IsSend)
                {
                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;
                    var msg = MessageHelper.BuildDynamic(allPonitListExInfo.AllPonitList, xIndex, yIndex, fileName, Luminance);

                    Messenger.Default.Send(new SendThemeSegmentSendMessageEvent() { Msg = msg });
                }
            }
            if (JsonFileInfo.IsDynamic && jsonFileInfo.FileName == "wifi")
            {
                allPonitListExInfo.AllPonitList[CurrentFrame] = allPonitListExInfo.AllPonitList[currFrame].Where(c => c.X >= 0 && c.X < xIndex && c.Y >= 0 && c.Y < yIndex).ToList();
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor)));

                var wificelsius = (int)obj.DownLoad;
                Console.WriteLine($"下行{obj.DownLoad},上行{obj.UpLoad}");
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.Down, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));


                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 19);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, obj.DownLoadflag, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));

                var tempwificelsius = wificelsius;
                do
                {
                    int remainder = tempwificelsius % 10;
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    tempwificelsius = tempwificelsius / 10;
                } while (tempwificelsius >= 1);


                if (wificelsius < 1000 && wificelsius >= 100)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                else if (wificelsius < 100 && wificelsius >= 10)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 8);
                }
                else if (wificelsius < 10 && wificelsius >= 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 12);
                }
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 1);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.fengexian, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                var upload = (int)obj.UpLoad;

                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 19);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, obj.UpLoadflag, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));

                var tempupload = upload;
                do
                {
                    int remainder = tempupload % 10;
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                    allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                    allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, (ABCEnum)remainder, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                    tempupload = tempupload / 10;
                } while (tempupload >= 1);


                if (upload < 1000 && upload >= 100)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 4);
                }
                else if (upload < 100 && upload >= 10)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 8);
                }
                else if (upload < 10 && upload >= 0)
                {
                    XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 12);
                }
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 1);
                allPonitListExInfo.AllPonitList[CurrentFrame].ForEach(c => c.X += 3);
                allPonitListExInfo.AllPonitList[currFrame].AddRange(ABCHelper.GetPonitItems(0, 0, 0, ABCEnum.Up, (System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.AbcColor)));
                XMoveCommand(allPonitListExInfo.AllPonitList[CurrentFrame], 2);
                allPonitListExInfo.AllPonitList[currFrame].Sort();
                BuildShowPreView(CurrentFrame);

                if (IsSend)
                {
                    var fileName = string.IsNullOrEmpty(JsonFileInfo.NewFileName) ? JsonFileInfo.FileName : JsonFileInfo.NewFileName;
                    var msg = MessageHelper.BuildDynamic(allPonitListExInfo.AllPonitList, xIndex, yIndex, fileName.Replace(".json", ""), Luminance);

                    Messenger.Default.Send(new SendThemeSegmentSendMessageEvent() { Msg = msg });


                }
            }
        }
        private void HandlePonitClickedEvent(PonitClickedEvent item)
        {
            if (CursorEnum == CursorEnum.Magic)
            {
                var currentpoint = allPonitListExInfo.AllPonitList[CurrentFrame].Find(c => c.Y == item.Y && c.X == item.X);
                if (currentpoint.Fill != changeColor)
                {
                    currentpoint.Fill = ChangeColor;
                    BuildShow(CurrentFrame);
                }

            }
            else if (CursorEnum == CursorEnum.ERASE)
            {
                var currentpoint = allPonitListExInfo.AllPonitList[CurrentFrame].Find(c => c.Y == item.Y && c.X == item.X);
                if (currentpoint.Fill != new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor)))
                {
                    currentpoint.Fill = new SolidColorBrush((System.Windows.Media.Color)ColorConverter.ConvertFromString(ColorConst.BackGroupColor));
                    BuildShow(CurrentFrame);
                }
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

    public class AllPonitListExInfo
    {
        public Dictionary<int, List<PointItem>> AllPonitList { get; set; } = new Dictionary<int, List<PointItem>>();
        public int FrameRate { get; set; } = 20;
    }
}
