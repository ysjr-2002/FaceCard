using DYW.ImageReview.Core;
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

namespace DYW.ImageReview
{
    /// <summary>
    /// PhotoWindow.xaml 的交互逻辑
    /// </summary>
    public partial class PhotoWindow : Window
    {
        public PhotoWindow(Employee emp)
        {
            InitializeComponent();
            this.DataContext = emp;
        }
    }
}
