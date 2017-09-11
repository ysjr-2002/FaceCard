using DYW.ImageReview.Entitys;
using DYW.ImageReview.Model;
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
using System.Windows.Shapes;
using System.ComponentModel;
using Common.Dialog;
using Common;
using DYW.ImageReview.Core;

namespace DYW.ImageReview
{
    /// <summary>
    /// MainWindowNew.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindowNew : Window
    {
        private MainViewModelNew core;
        public MainWindowNew()
        {
            InitializeComponent();
            core = new MainViewModelNew();
            this.DataContext = core;
            this.Loaded += MainWindowNew_Loaded;
        }

        private void MainWindowNew_Loaded(object sender, RoutedEventArgs e)
        {
            AutoRun();
            core.Init();
        }

        private void AutoRun()
        {
            var appname = "ImageReivew";
            var apppath = System.Windows.Forms.Application.ExecutablePath;
            Utility.runWhenStart(ConfigProfile.Current.AutoRun, appname, apppath);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new PersonWindow();
            window.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var window = new PassHistoryWindow();
            window.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            core.Server_OnMessageInComming(null, new Server.MessageEventArgs { Ip = "192.168.1.156", CardNo = 1 });
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var dialog = CustomDialog.Confirm("确认关闭软件吗？");
            if (dialog == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            core.Dispose();
            base.OnClosing(e);
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            core.doComapre(1, ConfigProfile.Current.VideoInName, "进入");
        }
    }
}
