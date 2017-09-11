using Common;
using Common.Dialog;
using DYW.ImageReview.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DYW.ImageReview
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainWindow : Window
    {
        private FaceCore core = null;

        public MainWindow()
        {
            InitializeComponent();

            core = new FaceCore();
            this.DataContext = core;
            this.Loaded += MainWindow_Loaded;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            if (e.Key == Key.F1)
            {
                PersonWindow window = new PersonWindow();
                window.ShowDialog();
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
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

        protected override void OnClosing(CancelEventArgs e)
        {
            core.Dispose();
            base.OnClosing(e);
        }

        private void Close_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var dialog = CustomDialog.Confirm("确认关闭软件吗？");
            if (dialog == MessageBoxResult.No)
            {
                return;
            }
            this.Close();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            core.OnReadCard(new byte[] { 1 }, 351899);
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            core.OnReadCard(new byte[] { 1 }, 351805);
        }
    }
}
