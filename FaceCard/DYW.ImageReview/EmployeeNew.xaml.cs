using Common;
using Common.Dialog;
using DYW.ImageReview.Core;
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
using System.ComponentModel;

namespace DYW.ImageReview
{
    /// <summary>
    /// EmployeeNew.xaml 的交互逻辑
    /// </summary>
    public partial class EmployeeNew : Window
    {
        private Employee emp = null;
        private bool isedit = false;
        public EmployeeNew(Employee employee)
        {
            InitializeComponent();
            if (employee == null)
            {
                emp = new Employee();
            }
            else
            {
                this.emp = employee;
                isedit = true;
            }
            this.DataContext = emp;
            this.Loaded += EmployeeNew_Loaded;
        }

        private void EmployeeNew_Loaded(object sender, RoutedEventArgs e)
        {
            camera.Connect();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Check())
                return;

            if (isedit == false)
            {
                using (var db = new DoorDBContext())
                {
                    emp.EmpID = Utility.GUID();
                    emp.CreateTime = DateTime.Now;
                    db.Employees.Add(emp);
                    var count = db.SaveChanges();
                    if (count > 0)
                    {
                        CustomDialog.Show("人员信息保存成功！");
                        NextEmployee();
                    }
                    else
                    {
                        CustomDialog.Show("人员信息保存失败！");
                    }
                }
            }
            else
            {
                using (var db = new DoorDBContext())
                {
                    var find = db.Employees.FirstOrDefault(s => s.EmpID == emp.EmpID);
                    find.EmpName = emp.EmpName;
                    find.EmpDuty = emp.EmpDuty;
                    find.EmpCard = emp.EmpCard;
                    find.EmpPhone = emp.EmpPhone;
                    find.EmpAddress = emp.EmpAddress;
                    find.EmpRemark = emp.EmpRemark;
                    find.EmpPhoto = emp.EmpPhoto;
                    var count = db.SaveChanges();
                    if (count > 0)
                    {
                        CustomDialog.Show("人员信息编辑成功！");
                    }
                    else
                    {
                        CustomDialog.Show("人员信息保存失败！");
                    }
                    this.Close();
                }
            }
        }

        private bool Check()
        {
            if (emp.EmpCard.IsEmpty())
            {
                MessageBox.Show("请填写卡号！");
                return false;
            }

            if (emp.EmpPhoto.IsEmpty())
            {
                MessageBox.Show("请选择人像！");
                return false;
            }
            return true;
        }

        private void NextEmployee()
        {
            emp = new Employee();
            this.DataContext = emp;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSnap_Click(object sender, RoutedEventArgs e)
        {
            if (camera.IsConnected == false)
                return;

            var filepath = camera.Snap();
            if (filepath.IsEmpty())
                return;
            emp.EmpPhoto = filepath;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            camera.StopCamera();
            base.OnClosing(e);
        }

        private void btnChoice_Click(object sender, RoutedEventArgs e)
        {
            var path = Utility.OpenFileDialog();
            emp.EmpPhoto = path;
        }
    }
}
