using Common;
using Common.Dialog;
using DYW.ImageReview.Core;
using DYW.ImageReview.Entitys;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DYW.ImageReview
{
    /// <summary>
    /// 人像管理
    /// </summary>
    public partial class PersonWindow : Window
    {
        public PersonWindow()
        {
            InitializeComponent();
            this.Loaded += PersonWindow_Loaded;
            this.AddHandler(PagerControl.GoPageEvent, new CustomPageRoutedEventHandler(SkipPage));
        }

        private void SkipPage(object sender, PageIndexChangedEventArgs e)
        {
            PageInfo page = new PageInfo();
            page.PageIndex = e.NewPageIndex;
            QueryPerson(page);
        }

        private void PersonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            QueryPerson(new PageInfo());
        }

        private void QueryPerson(PageInfo page)
        {
            var key = txtKey.Text.Process();
            using (var db = new DoorDBContext())
            {
                var query = from n in db.Employees select n;
                if (!key.IsEmpty())
                {
                    query = query.Where(s => s.EmpName.Contains(key) || s.EmpCard.Contains(key));
                }
                query = query.OrderByDescending(s => s.CreateTime);
                page.RecordTotal = query.Count();
                var list = query.Skip((page.PageIndex - 1) * page.PageSize).Take(page.PageSize).ToList();
                dgPersons.ItemsSource = list;

                this.pager.DataContext = page;
            }
        }

        private void btnSearch_click(object sender, RoutedEventArgs e)
        {
            QueryPerson(new PageInfo());
        }

        private void btnAdd_click(object sender, RoutedEventArgs e)
        {
            var window = new EmployeeNew(null);
            window.ShowDialog();
            QueryPerson(new PageInfo());
        }

        private void btnUpdate_click(object sender, RoutedEventArgs e)
        {
        }

        private void Row_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var row = e.Source as DataGridRow;
            var data = row.DataContext as Employee;
            grid.DataContext = data;

            var p = dgPersons.PointToScreen(new Point(0, 0));
            var x = p.X + dgPersons.ActualWidth;
            var y = p.Y + dgPersons.ActualHeight;

            popup.HorizontalOffset = x - popup.Width;
            popup.VerticalOffset = y - popup.Height;
            popup.IsOpen = true;
        }

        private void btnDelete_click(object sender, RoutedEventArgs e)
        {
            if (dgPersons.SelectedItem == null)
                return;

            if (CustomDialog.Confirm("确认删除选中的记录？", "提示") == MessageBoxResult.No)
                return;

            var emp = dgPersons.SelectedItem as Employee;
            using (var db = new DoorDBContext())
            {
                var find = db.Employees.First(s => s.EmpID == emp.EmpID);
                db.Employees.Remove(find);
                var effect = db.SaveChanges();
                if (effect > 0)
                    CustomDialog.Show("记录删除成功！");
            }

            QueryPerson(new PageInfo());
        }

        private void DataGridRow_MouseDoubleclick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = sender as DataGridRow;
            var emp = row.DataContext as Employee;
            if (emp == null)
                return;

            EmployeeNew photo = new EmployeeNew(emp);
            photo.ShowDialog();
        }

        private void btnImport_click(object sender, RoutedEventArgs e)
        {
            gridImport.Visibility = Visibility.Visible;
        }

        private async void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (empFiles == null || empFiles.Length == 0)
                return;

            btnStart.IsEnabled = false;
            progress.Maximum = empFiles.Length;
            await DoImport(empFiles);

            gridImport.Visibility = Visibility.Collapsed;
            CustomDialog.Show("员工导入成功！");
            ResetImport();
            QueryPerson(new PageInfo());
        }

        private void ResetImport()
        {
            lblTotal.Content = "";
            lblImport.Content = "";
            btnStart.IsEnabled = true;
            txtFolder.Clear();
            progress.Value = 0;
        }

        private Task DoImport(string[] files)
        {
            return Task.Factory.StartNew(() =>
             {
                 using (var db = new DoorDBContext())
                 {
                     var count = 0;
                     foreach (var f in files)
                     {
                         var card = System.IO.Path.GetFileNameWithoutExtension(f);
                         Employee emp = new Employee
                         {
                             EmpID = Utility.GUID(),
                             EmpCard = card,
                             EmpPhoto = f,
                             CreateTime = DateTime.Now
                         };
                         db.Employees.Add(emp);
                         count++;
                         System.Windows.Application.Current.Dispatcher.Invoke(() =>
                         {
                             progress.Value = count;
                             lblImport.Content = "已导入" + count + "个";
                         });
                         Thread.Sleep(0);
                     }
                     count = db.SaveChanges();
                 }
             });
        }

        private string[] empFiles = null;

        private void btnFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFolder.Text = fbd.SelectedPath;

                var jpgFiles = Directory.GetFiles(txtFolder.Text, "*.jpg", SearchOption.AllDirectories);
                var pngFiles = Directory.GetFiles(txtFolder.Text, "*.png", SearchOption.AllDirectories);
                List<string> totals = new List<string>();
                totals.AddRange(jpgFiles);
                totals.AddRange(pngFiles);
                empFiles = totals.ToArray();

                lblTotal.Content = string.Format("共{0}个文件", empFiles.Length);
            }
        }

        private void Image_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gridImport.Visibility = Visibility.Collapsed;
        }

        private void PopupClose_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            popup.IsOpen = false;
        }

        private void dgPersons_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            popup.IsOpen = false;
        }
    }
}
