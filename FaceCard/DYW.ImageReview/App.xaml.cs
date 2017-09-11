using Common.Dialog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DYW.ImageReview.Core;
using Common.Log;
using DYW.ImageReview.Server;

namespace DYW.ImageReview
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var bnew = false;
            var appname = System.Windows.Forms.Application.ProductName;
            var mutex = new Mutex(true, appname, out bnew);
            if (bnew)
            {
                base.OnStartup(e);

                ConfigProfile.Current.ReadConfig();
                Channel.Init();
                CardPhoto.GetCardPhotos();

                Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
                Application.Current.MainWindow = new MainWindowNew();
                Application.Current.MainWindow.Show();
                mutex.WaitOne();
            }
            else
            {
                CustomDialog.Show("系统已运行！");
                Application.Current.Shutdown();
            }
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            LogHelper.Info("捕获未处理的异常信息->" + e.Exception.StackTrace);
        }
    }
}
