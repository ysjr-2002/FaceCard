using DYW.ImageReview.Entitys;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DYW.ImageReview
{
    /// <summary>
    /// PagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PagerControl : UserControl
    {
        public PagerControl()
        {
            InitializeComponent();
        }

        public event CustomPageRoutedEventHandler GoPage
        {
            add { this.AddHandler(GoPageEvent, value); }
            remove { this.RemoveHandler(GoPageEvent, value); }
        }

        public static readonly RoutedEvent GoPageEvent =
             EventManager.RegisterRoutedEvent("GoPage", RoutingStrategy.Bubble, typeof(CustomPageRoutedEventHandler), typeof(PagerControl));

        protected virtual void OnGoPage(int page)
        {
            PageIndexChangedEventArgs newEvent = new PageIndexChangedEventArgs(GoPageEvent, this);
            newEvent.NewPageIndex = page;
            this.RaiseEvent(newEvent);
        }

        private void nextPage_Click(object sender, MouseButtonEventArgs e)
        {
            var page = this.DataContext as PageInfo;
            if (page.PageIndex >= page.PageTotal)
                return;

            var newPage = page.PageIndex + 1;
            OnGoPage(newPage);
        }

        private void prePage_Click(object sender, MouseButtonEventArgs e)
        {
            var page = this.DataContext as PageInfo;
            if (page.PageIndex == 1)
                return;

            var newPage = page.PageIndex - 1;
            OnGoPage(newPage);
        }
    }

    public delegate void CustomPageRoutedEventHandler(object sender, PageIndexChangedEventArgs e);

    public class PageIndexChangedEventArgs : RoutedEventArgs
    {
        public PageIndexChangedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
        {
        }
        public int NewPageIndex { get; set; }
    }
}
