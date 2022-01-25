using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;          //使用串口
using System.Threading.Tasks;   //线程
using System.Runtime.InteropServices;

namespace MySerialPort
{
    public partial class Form2 : Form
    {


        /*泛型集合用于图表显示*/
        private List<int> List_ia = new List<int>();    //电流ia
        private List<int> List_ic = new List<int>();    //电流ic
        private List<int> List_udc = new List<int>();    //母线电压dc
        private List<int> List_vel = new List<int>();    //速度

        Random random = new Random();

        public Form2()
        {
            InitializeComponent();
        }



      


        private void DrawChart(List<int> List_ia, List<int> List_ic, List<int> List_udc, List<int> List_vel)
        {
            chart2.Series[0].Points.Clear();    //清除所有点
           


            for (int i = 0; i < List_ia.Count; i++)
            {
                chart2.Series[0].Points.AddXY(i + 1, List_ia[i]);   //添加点
            }
            
          
        }

        //清除所有的列表
        private void ClearAllList()
        {
            List_ia.Clear();
            List_ic.Clear();
            List_udc.Clear();
            List_vel.Clear();
        }



        //当大于80时移除list头部
        private void ListRemove(List<int> list)
        {
            if (list.Count >= 80)
            {
                list.RemoveAt(0);
            }
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            if (button_start.Text == "开始")
            {
                timer2.Start();
                button_start.Text = "暂停";
            }
            else
            {
                timer2.Stop();
                button_start.Text = "开始";
            }
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            int num1 = random.Next(0, 99);   //随机数
            int num2 = random.Next(0, 99);
            int num3 = random.Next(0, 99);
            int num4 = random.Next(0, 99);

            List_ia.Add(num1); ListRemove(List_ia);
            List_ic.Add(num2); ListRemove(List_ic);
            List_udc.Add(num3); ListRemove(List_udc);
            List_vel.Add(num4); ListRemove(List_vel);

            DrawChart(List_ia, List_ic, List_udc, List_vel);
        }
    }
}