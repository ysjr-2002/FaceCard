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
using System.Windows.Shapes;
using Common;

namespace DYW.ImageReview
{
    /// <summary>
    /// PassHistoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PassHistoryWindow : Window
    {
        public PassHistoryWindow()
        {
            InitializeComponent();

            dtpStart.Value = DateTime.Now.GetDayStart();
            dtpEnd.Value = DateTime.Now.GetDayEnd();
            this.AddHandler(PagerControl.GoPageEvent, new CustomPageRoutedEventHandler(SkipPage));
        }

        private void Row_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var row = e.Source as DataGridRow;
            var data = row.DataContext as PassHistory;
            grid.DataContext = data;

            var p = dgPassHisotry.PointToScreen(new Point(0, 0));
            var x = p.X + dgPassHisotry.ActualWidth;
            var y = p.Y + dgPassHisotry.ActualHeight;

            popup.HorizontalOffset = x - popup.Width;
            popup.VerticalOffset = y - popup.Height;
            popup.IsOpen = true;
        }

        private void SkipPage(object sender, PageIndexChangedEventArgs e)
        {
            PageInfo page = new PageInfo();
            page.PageIndex = e.NewPageIndex;
            QueryRecord(page);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PageInfo page = new PageInfo();
            pager.DataContext = page;
        }

        private void btnSearch_click(object sender, RoutedEventArgs e)
        {
            PageInfo page = new PageInfo();
            QueryRecord(page);
        }

        private void QueryRecord(PageInfo page)
        {
            var key = txtKey.Text.Process();
            var inout = ((ComboBoxItem)cmbInOut.SelectedValue).Content.ToString();
            var alarm = ((ComboBoxItem)cmbAlaram.SelectedValue).Content.ToString();
            var start = dtpStart.Value;
            var end = dtpEnd.Value;

            using (var db = new DoorDBContext())
            {
                var query = from n in db.PassHistorys select n;
                if (!key.IsEmpty())
                    query = query.Where(s => s.Card.StartsWith(key) || s.Name.StartsWith(key));

                if (inout != "全部")
                    query = query.Where(s => s.PassType == inout);

                if (alarm != "全部")
                {
                    var b = alarm == "报警";
                    query = query.Where(s => s.Alaram == b);
                }
                query = query.Where(s => s.PassTime >= start && s.PassTime <= end);
                query = query.OrderByDescending(s => s.PassTime);

                page.RecordTotal = query.Count();
                var list = query.Skip((page.PageIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                dgPassHisotry.ItemsSource = list;

                pager.DataContext = page;
            }
        }

        private void DataGridRow_MouseDoubleclick(object sender, MouseButtonEventArgs e)
        {
            var item = dgPassHisotry.SelectedItem;
            if (item == null)
                return;

            var history = item as PassHistory;
            grid.DataContext = history;
            var p = dgPassHisotry.PointToScreen(new Point(0, 0));
            var x = p.X + dgPassHisotry.ActualWidth;
            var y = p.Y + dgPassHisotry.ActualHeight;
            popup.HorizontalOffset = x - popup.Width;
            popup.VerticalOffset = y - popup.Height;
            popup.IsOpen = true;

        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void dgPassHisotry_MouseLeave(object sender, MouseEventArgs e)
        {
            popup.IsOpen = false;
        }
    }
}
