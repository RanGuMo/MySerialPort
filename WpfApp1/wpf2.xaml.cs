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

namespace WpfApp1
{
    /// <summary>
    /// wpf2.xaml 的交互逻辑
    /// </summary>
    public partial class wpf2 : Window
    {
        public wpf2()
        {
            InitializeComponent();
            DataContext = new TempModel();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string a = TxtA.Text;
            string p = TxtP.Text;

            if (a == "root" && p == "123")
            {
                MessageBox.Show("登录成功！");
            }
            else
            {
                MessageBox.Show("登录失败！");
            }
        }
    }
}
