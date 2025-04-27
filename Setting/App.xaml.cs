using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;
using NLog;
using Setting.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZZJDMD.Properties;

namespace Setting
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // 注册 UI 线程未捕获异常事件
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
            // 注册非 UI 线程未捕获异常事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
         // 注册异步任务未观察异常事件
        TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
}

    private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
            var exception = e.Exception as Exception;
            logger.Error($" {JsonConvert.SerializeObject(exception)}");
        }

    private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = e.ExceptionObject as Exception;

            logger.Error($" {JsonConvert.SerializeObject(exception)}");
            // 如果需要，可以在这里保存日志或退出应用程序
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // 记录异常信息
            logger.Error($" {JsonConvert.SerializeObject(e.Exception)}");
            // 标记异常已处理，防止应用程序崩溃
            e.Handled = true;
        }
    }
}
