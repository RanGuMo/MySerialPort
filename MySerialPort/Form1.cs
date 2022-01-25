using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MySerialPort
{
    public partial class Form1 : Form
    {

        byte[] data1 = new byte[] { };
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;//设置该属性 为false
        }
        int i = 0;
        public FileStream MyFs;

        private void Form1_Load(object sender, EventArgs e)
        {


            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                cmbPort.Items.Clear();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    cmbPort.Items.Add(sValue);
                }
                if (cmbPort.Items.Count > 0)
                    cmbPort.SelectedIndex = 0;
            }

            cmbBaud.Text = "115200";

        }

        bool isOpened = false;//串口状态标志
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isOpened)
            {
                serialPort.PortName = cmbPort.Text;
                serialPort.BaudRate = Convert.ToInt32(cmbBaud.Text, 10);
                try
                {
                    serialPort.Open();     //打开串口
                    button1.Text = "关闭串口";
                    cmbPort.Enabled = false;//关闭使能
                    cmbBaud.Enabled = false;
                    isOpened = true;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(post_DataReceived);//串口接收处理函数
                }
                catch
                {
                    MessageBox.Show("串口打开失败！");
                }
            }
            else
            {
                try
                {
                    serialPort.Close();     //关闭串口
                    button1.Text = "打开串口";
                    cmbPort.Enabled = true;//打开使能
                    cmbBaud.Enabled = true;
                    isOpened = false;
                }
                catch
                {
                    MessageBox.Show("串口关闭失败！");
                }
            }

        }

      


        byte[] xuhao = new byte[] { 0x41, 0x54, 0x2B, 0x53, 0x45, 0x4C, 0x3D, 0x2D, 0x69, 0x31, 0x0D, 0x0A };
        byte[] uuid1 = new byte[] { 0x41, 0x54, 0x2B, 0x53, 0x55, 0x55, 0x49, 0x44, 0x3D, 0x2D, 0x62, 0x66, 0x66, 0x66, 0x30, 0x0D, 0x0A };
        byte[] uuid2 = new byte[] { 0x41, 0x54, 0x2B, 0x53, 0x55, 0x55, 0x49, 0x44, 0x3D, 0x2D, 0x61, 0x31, 0x38, 0x30, 0x61, 0x0D, 0x0A };

        byte[] bluetoothName = new byte[] { 0x41, 0x54, 0x2B, 0x53, 0x43, 0x41, 0x4E, 0x3D, 0x2D, 0x73, 0x31, 0x0D, 0x0A };
        byte[] bluetoothStop = new byte[] { 0x41, 0x54, 0x2B, 0x53, 0x43, 0x41, 0x4E, 0x3D, 0x2D, 0x73, 0x30, 0x0D, 0x0A };
        byte[] startConnection = new byte[] { 0x41, 0x54, 0x2B, 0x43, 0x4E, 0x4E, 0x3D, 0x2D, 0x6D, 0x30, 0x30, 0x33, 0x35, 0x46, 0x46, 0x32, 0x32, 0x38, 0x33, 0x43, 0x38, 0x20, 0x20, 0x2D, 0x69, 0x30, 0x20, 0x2D, 0x74, 0x30, 0x20, 0x2D, 0x78, 0x31, 0x0D, 0x0A };
        byte[] operationUuid = new byte[] { 0x41, 0x54, 0x2B, 0x53, 0x45, 0x4C, 0x3D, 0x2D, 0x6D, 0x30, 0x30, 0x33, 0x35, 0x46, 0x46, 0x32, 0x32, 0x38, 0x33, 0x43, 0x38, 0x20, 0x2D, 0x73, 0x66, 0x66, 0x66, 0x30, 0x20, 0x2D, 0x75, 0x66, 0x66, 0x66, 0x31, 0x0D, 0x0A };
        byte[] oneSend = new byte[] { 0x41, 0x54, 0x2B, 0x57, 0x4E, 0x50, 0x3D, 0x2D, 0x64, 0x34, 0x33, 0x36, 0x46, 0x36, 0x45, 0x36, 0x45, 0x36, 0x35, 0x36, 0x33, 0x37, 0x34, 0x36, 0x35, 0x36, 0x34, 0x30, 0x64, 0x30, 0x61, 0x0D, 0x0A };
        byte[] halfSend = new byte[] { 0x41, 0x54, 0x2B, 0x57, 0x4E, 0x50, 0x3D, 0x2D, 0x64, 0x37, 0x62, 0x33, 0x30, 0x33, 0x31, 0x33, 0x32, 0x33, 0x31, 0x33, 0x31, 0x33, 0x31, 0x33, 0x33, 0x33, 0x30, 0x33, 0x31, 0x33, 0x33, 0x33, 0x34, 0x33, 0x37, 0x33, 0x32, 0x33, 0x30, 0x32, 0x33, 0x33, 0x34, 0x33, 0x35, 0x37, 0x64, 0x0D, 0x0A };
        byte[] stopSend = new byte[] { 0x41, 0x54, 0x2B, 0x57, 0x4E, 0x50, 0x3D, 0x2D, 0x64, 0x37, 0x62, 0x33, 0x30, 0x33, 0x35, 0x32, 0x33, 0x33, 0x39, 0x34, 0x31, 0x37, 0x64, 0x0D, 0x0A };
        //传入系统当前的时间
        //AT+WNP=-d7b
        byte[] halfSendStart = new byte[] { 0x41, 0x54, 0x2B, 0x57, 0x4E, 0x50, 0x3D, 0x2D, 0x64, 0x37, 0x62 };

        //byte[] content = data1;
        //7d\r\n
        byte[] halfSendEnd = new byte[] { 0x37, 0x64, 0x0D, 0x0A };
        //温度计uuid
        byte[] thermometer = new byte[] { 0x41, 0x54, 0x2B, 0x43, 0x4E, 0x4E, 0x3D, 0x2D, 0x6D, 0x38, 0x30, 0x30, 0x35, 0x44, 0x46, 0x43, 0x34, 0x30, 0x46, 0x34, 0x44, 0x20, 0x2D, 0x69, 0x30, 0x20, 0x2D, 0x74, 0x30, 0x20, 0x2D, 0x78, 0x31, 0x0D, 0x0A };
        //血压
        byte[] bloodPressure = new byte[] { 0x41, 0x54, 0x2B, 0x43, 0x4E, 0x4E, 0x3D, 0x2D, 0x6D, 0x38, 0x30, 0x30, 0x35, 0x44, 0x46, 0x43, 0x34, 0x30, 0x46, 0x35, 0x36, 0x2D, 0x69, 0x30, 0x20, 0x2D, 0x74, 0x30, 0x20, 0x2D, 0x78, 0x31, 0x0D, 0x0A };


        private void post_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Delay(500);
            // string str = serialPort.ReadLine();//字符串方式读   
            string str = serialPort.ReadExisting();//字符串方式读  

            // ReceiveTbox.Text = "";//先清除上一次的数据
            ReceiveTbox.AppendText(str);
            ////////////////////////////////数据转换处理/////////////////////////////////////////////////////////////
            if (str.Contains("F04FFF7FAFFFF00"))    //接收到温度计的数据
            {
                textBox1.Text = du(MidStrEx_New(str, "F04FFF7FAFFFF00", "A6\r\n"));
            }
            else if (str.Contains("SUCCESS\r\nDISCONNECT\r\n-lF74A3C013B32\r\n-m8005DFC40F4D\r\n-r8"))
            {
                textBox1.Text = "温度计断开连接！！！";
            }
            else if (str.Contains("SUCCESS\r\nMTU_UPDATE\r\n-lF74A3C013B32\r\n-m8005DFC40F4D\r\n-u251"))
            {
                textBox1.Text = "温度计已连接！！！";
            }

            if (str.Contains("dFFFE0D"))      //接收到血压的数据
            {

                textBox2.Text = xueya(MidStrEx_New(str, "dFFFE0D", "\r\n"));
            }
            else if (str.Contains("SUCCESS\r\nDISCONNECT\r\n-lF74A3C013B32\r\n-m8005DFC40F56\r\n-r8"))
            {
                textBox2.Text = "血压断开连接！！！";
            }
            else if (str.Contains("SUCCESS\r\nMTU_UPDATE\r\n-lF74A3C013B32\r\n-m8005DFC40F56\r\n-u251"))
            {
                textBox2.Text = "血压连接成功！！！";
            }
            else if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m8005DFC40F56\r\n-uFFF1\r\n-dFFFE045E5601"))
            {//error P
                textBox2.Text = "7S内打气不上30mmHg(气袋没绑好)";
            }
            else if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m8005DFC40F56\r\n-uFFF1\r\n-dFFFE045E5602"))
            {//
                textBox2.Text = "气袋压力超过295mmHg超压保护";
            }
            else if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m8005DFC40F56\r\n-uFFF1\r\n-dFFFE045E5603"))
            {//error 1
                textBox2.Text = "测量不到有效的脉搏";
            }
            else if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m8005DFC40F56\r\n-uFFF1\r\n-dFFFE045E5604"))
            {//error 2
                textBox2.Text = "血压测量过程中干预过多（测量中移动、说话等）！！！";
            }
            else if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m8005DFC40F56\r\n-uFFF1\r\n-dFFFE045F5605"))
            {//error 3
                textBox2.Text = "血压测量结果数值有误";
            }
            else if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m8005DFC40F56\r\n-uFFF1\r\n-dFFFE045F5606"))
            {//error 3
                textBox2.Text = "电池低电压";
            }

            if (str.Contains("+V=")&&str.Contains("OK"))
            {
                textBox5.Text= MidStrEx_New(str, "V=", "\r\nOK\r\n")+"V(原值：12.565V)";//+V=12.562\r\nOK\r\n
            }



            if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m0035FF2283C8\r\n-uFFF1\r\n-d7B"))      //接收到血糖仪的数据
            {

                //double i = double.Parse(xuetangyi("303347303037312E233546")); //转double类型
                //double d = i / 18.0;    //将mg/dL（毫克每分升）  转换为 mmol/L（毫摩尔每升）  需要除 18

                //string ss = string.Format("{0:F1}", d);//默认为保留两位  //F1 保留一位小数，F2保留两位小数
                //textBox3.Text = "血糖：" + ss + "mmol/L";
                string mid = MidStrEx_New(str, "SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m0035FF2283C8\r\n-uFFF1\r\n-d7B30322339447D\r\nSUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m0035FF2283C8\r\n-uFFF1\r\n-d7B", "7D\r\n");
                if (mid.Length > 20)
                {
                    string xuet;
                    string xuetang = xuetangyi(mid);
                    if (xuetang.Substring(2, 1) == "G")//葡萄糖
                    {
                        xuet = xuetang.Substring(4, 3);//diaplay

                        int i = (int)(double.Parse(xuet) / 18.0 * 10); //保留一位小数（不做四舍五入）

                        textBox3.Text = "血糖：" + ((i * 1.0) / 10).ToString() + "mmol/L"; //转double类型,并除18 。string.Format("{0:F1}", double.Parse(xuet) / 18.0) 保留一位小数（四舍五入）

                    }
                    else if (xuetang.Substring(2, 1) == "C")//胆固醇
                    {
                        xuet = xuetang.Substring(4, 3);//diaplay

                        int i = (int)(double.Parse(xuet) / 38.67 * 10); //保留一位小数（不做四舍五入）

                        textBox4.Text = "胆固醇：" + ((i * 1.0) / 10).ToString() + "mmol/L"; //转double类型,并除38.67 。string.Format("{0:F1}", double.Parse(xuet) / 38.67)保留一位小数（四舍五入）
                    }
                    else if (xuetang.Substring(2, 1) == "U")//尿酸
                    {
                        xuet = xuetang.Substring(4, 3);//diaplay
                    }
                    else if (xuetang.Substring(2, 1) == "H")//血红蛋白
                    {
                        xuet = xuetang.Substring(4, 3);//diaplay
                    }


                }



            }
            else if (str.Contains("SUCCESS\r\nMTU_UPDATE\r\n-lF74A3C013B32\r\n-m0035FF2283C8\r\n-u27"))
            {
                textBox3.Text = "血糖仪连接成功！";
            }
            else if (str.Contains("SER_FOUND_TIMEOUT\r\nCONNECT\r\n-lF74A3C013B32\r\n-m0035FF2283C8"))
            {
                textBox3.Text = "血糖仪连接超时，请重新连接！";
            }
            else if (str.Contains("DEVICE_NOT_FOUND\r\nCONNECT\r\n-lF74A3C013B32\r\n-m0035FF2283C8"))
            {
                textBox3.Text = "未找到设备！";
            }
            else if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m0035FF2283C8\r\n-uFFF1\r\n-d00"))//1
            {
                textBox3.Text = "已校时成功，正在等待数据回传！";
            }
            else if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m0035FF2283C8\r\n-uFFF1\r\n-d0000"))//2
            {
                textBox3.Text = "已校时成功，正在等待数据回传！";
            }




            ///////////////////////////////数据转换处理/////////////////////////////////////////////////////////////




        }

        private void aaa()
        {

            while (true)
            {
                string str = serialPort.ReadLine();//字符串方式读
                if (str.Contains("nETB04"))
                {
                    Delay(1000);
                    serialPort.Write(bluetoothStop, 0, bluetoothStop.Length);
                    Delay(1000);
                    serialPort.Write(startConnection, 0, startConnection.Length);
                    Delay(1000);
                    serialPort.Write(operationUuid, 0, operationUuid.Length);
                    Delay(1000);
                    serialPort.Write(oneSend, 0, oneSend.Length);
                    for (int i = 0; i < 15; i++)
                    {
                        Delay(500);
                        serialPort.Write(halfSend, 0, halfSend.Length);
                    }
                }
                if (str.Contains("DISCONNECT"))
                {
                    Delay(1000);
                    serialPort.Write(bluetoothName, 0, bluetoothName.Length);
                }
            }






        }

        private void button2_Click(object sender, EventArgs e)
        {
            //发送数据
            if (serialPort.IsOpen)
            {
                //string values = this.SendTbox.Text;
                //string[] varr = values.TrimEnd().Split(' ');
                //byte[] data1 = new byte[varr.Length];
                //if (varr.Length > 1)
                //{
                //    for (int i = 0; i < varr.Length; i++)
                //    {
                //        data1[i] = Convert.ToByte(varr[i].ToString(), 16);
                //    }
                //    serialPort.Write(data1, 0, data1.Length);
                //}

                serialPort.Write(xuhao, 0, xuhao.Length);
                Delay(500);
                serialPort.Write(uuid1, 0, uuid1.Length);
                Delay(500);
                serialPort.Write(uuid2, 0, uuid2.Length);
                Delay(500);

                serialPort.Write(bluetoothName, 0, bluetoothName.Length);
                //Delay(3000);
                // serialPort.Write(bluetoothStop, 0, bluetoothStop.Length);
                //Delay(1000);
                //serialPort.Write(startConnection, 0, startConnection.Length);
                //Delay(1000);
                //serialPort.Write(operationUuid, 0, operationUuid.Length);
                //Delay(1000);
                //serialPort.Write(oneSend, 0, oneSend.Length);
                //for (int i = 0; i < 15; i++)
                //{
                //    Delay(500);
                //    serialPort.Write(halfSend, 0, halfSend.Length);
                //}

                //serialPort1.Write(stopSend, 0, stopSend.Length);





            }
        }

        //延时函数，等待时可做其他操作
        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)//毫秒
            {
                Application.DoEvents();//可执行某无聊的操作
            }
        }



        //停止搜索
        private void button3_Click(object sender, EventArgs e)
        {
            serialPort.Write(bluetoothStop, 0, bluetoothStop.Length);
            timer2.Stop(); 
        }
        //连接血糖仪
        private void button4_Click(object sender, EventArgs e)
        {



        }


        private void button6_Click(object sender, EventArgs e)
        {
            serialPort.Write(xuhao, 0, xuhao.Length);
            Delay(500);
            serialPort.Write(uuid1, 0, uuid1.Length);
            Delay(500);
            serialPort.Write(uuid2, 0, uuid2.Length);
            Delay(500);
            serialPort.Write(bluetoothName, 0, bluetoothName.Length);
            timer2.Start();
        }


        public static string MidStrEx_New(string sourse, string startstr, string endstr)
        {
            try
            {
                Regex rg = new Regex("(?<=(" + startstr + "))[.\\s\\S]*?(?=(" + endstr + "))", RegexOptions.Multiline | RegexOptions.Singleline);
                return rg.Match(sourse).Value;
            }
            catch (Exception)
            {

                throw;
            }
       
        }

        //温度计连接按钮
        private void button5_Click(object sender, EventArgs e)
        {
            serialPort.Write(xuhao, 0, xuhao.Length);
            Delay(500);
            serialPort.Write(uuid1, 0, uuid1.Length);
            Delay(500);
            serialPort.Write(uuid2, 0, uuid2.Length);
            Delay(500);
            serialPort.Write(thermometer, 0, thermometer.Length);




        }

        //数据接收文本框
        private void ReceiveTbox_TextChanged(object sender, EventArgs e)
        {
            this.ReceiveTbox.SelectionStart = this.ReceiveTbox.Text.Length;
            this.ReceiveTbox.SelectionLength = 0;
            this.ReceiveTbox.ScrollToCaret();
        }

        /// <summary>
        /// 温度计数据处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string du(string str)
        {
            var a = str;
            a = a.Substring(0, a.Length - 2);
            string[] b = new string[2];
            b[0] = a.Substring(0, 2);
            b[1] = a.Substring(2, 2);
            if (Convert.ToInt32(b[0], 16) + Convert.ToInt32(b[1], 16) * 0.01 == 224)
            {
                return "温度计温度过低！";
            }
            else if (Convert.ToInt32(b[0], 16) + Convert.ToInt32(b[1], 16) * 0.01 == 240) //-dFFFF04FFF7FAFFFF00F00086A6
            {
                return "温度计温度过高！";
            }

            return (Convert.ToInt32(b[0], 16) + Convert.ToInt32(b[1], 16) * 0.01).ToString() + "度";
        }

        /// <summary>
        /// 血压的数据处理
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>

        public string xueya(string str)
        {
            var a = str;
            a = a.Substring(0, a.Length);
            //115500 6F 82 43 53 1401010C06  //（高）6F：111   82：150  （低）43：67  （脉搏）53：83
            string[] b = new string[a.Length];
            b[0] = a.Substring(6, 2);//（高压）
            b[1] = a.Substring(10, 2);//（低压）
            b[2] = a.Substring(12, 2);//（脉搏）
            return "高压:" + Convert.ToInt32(b[0], 16) + " 低压:" + Convert.ToInt32(b[1], 16) + " 脉搏:" + Convert.ToInt32(b[2], 16);

        }




        public string xuetangyi(string str)  //303347303037312E233546
        {


            byte[] data = new byte[str.Length / 2];
            int i;
            try                                                                                 //如果此时用户输入字符串中含有非法字符（字母，汉字，符号等等，try，catch块可以捕捉并提示）
            {
                string buffer = str;
                buffer = buffer.Replace("0x", "");                                    //为了保证汉字转编码输出结果（0xXX）可以通用，所以程序允许输入0xXX（可以带空格），程序会将0x和空格自动去除
                buffer = buffer.Replace(" ", string.Empty);
                for (i = 0; i < buffer.Length / 2; i++)                                         //转换偶数个
                {
                    data[i] = Convert.ToByte(buffer.Substring(i * 2, 2), 16);                   //转换
                }
                string str2 = BytesToString(data);
                return str2;



            }
            catch
            {
                MessageBox.Show("数据转换错误，请输入数字。", "错误");
                return "";
            }


        }






        private string BytesToString(byte[] Bytes)                                              //过程同上
        {
            string Mystring;
            Encoding FromEcoding = Encoding.GetEncoding("gb2312");
            Encoding ToEcoding = Encoding.GetEncoding("UTF-8");
            byte[] Tobytes = Encoding.Convert(FromEcoding, ToEcoding, Bytes);
            Mystring = ToEcoding.GetString(Tobytes);                                            //得到的是UTF8字节码序列，需要转换为UTF8字符串
            return Mystring;                                                                    //转换
        }



        //连接血压
        private void button7_Click(object sender, EventArgs e)
        {
            serialPort.Write(xuhao, 0, xuhao.Length);
            Delay(500);
            serialPort.Write(uuid1, 0, uuid1.Length);
            Delay(500);
            serialPort.Write(uuid2, 0, uuid2.Length);
            Delay(500);
            serialPort.Write(bloodPressure, 0, bloodPressure.Length);
        }


        //连接血糖仪的方法
        public void Connectionxuetangyi(string str)
        {


            serialPort.Write(xuhao, 0, xuhao.Length);
            Delay(500);
            serialPort.Write(uuid1, 0, uuid1.Length);
            Delay(500);
            serialPort.Write(uuid2, 0, uuid2.Length);
            serialPort.Write(startConnection, 0, startConnection.Length);//开始连接血糖仪
                                                                         //Delay(500);
            serialPort.Write(operationUuid, 0, operationUuid.Length);  //操作血糖仪的uuid
            Delay(500);

            serialPort.Write(oneSend, 0, oneSend.Length);          //发送一次
            for (int i = 0; i < 15; i++)
            {
                Delay(500);
                serialPort.Write(halfSend, 0, halfSend.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了
                if (str.Contains("NOTIFY"))
                {
                    break;
                }
            }

        }

        //连接温度计的方法
        public void Connectionwenduji()
        {

            serialPort.Write(xuhao, 0, xuhao.Length);
            Delay(500);
            serialPort.Write(uuid1, 0, uuid1.Length);
            Delay(500);
            serialPort.Write(uuid2, 0, uuid2.Length);
            Delay(500);
            serialPort.Write(thermometer, 0, thermometer.Length);

        }

        //连接血压的方法
        public void Connectionxueya()
        {

            serialPort.Write(xuhao, 0, xuhao.Length);
            Delay(500);
            serialPort.Write(uuid1, 0, uuid1.Length);
            Delay(500);
            serialPort.Write(uuid2, 0, uuid2.Length);
            Delay(500);
            serialPort.Write(bloodPressure, 0, bloodPressure.Length);

        }






        //定时器
        private void timer1_Tick(object sender, EventArgs e)
        {

        }
        int count;

        private void button8_Click(object sender, EventArgs e)
        {
            //double i = double.Parse(xuetangyi("303347303037312E233546")); //转double类型
            //double d =i/ 18.0;    //将mg/L（毫克每升）  转换为 mmol/L（毫摩尔每升）  需要除 18

            //string ss=string.Format("{0:F1}", d);//默认为保留两位  //F1 保留一位小数，F2保留两位小数
            //textBox3.Text = "血糖："+ ss + "mmol/L";
            string link = "01" + DateTime.Now.ToString("yyMMddHHmmss");
            textBox4.Text = ShuzicheckSum(link);   //得到校验和（一个16进制数）

            string srcStr = ShuzicheckSum(link);
            string srcStr1 = "#";
            string srcStr2 = link;

            StringBuilder sbHex = new StringBuilder();
            int tempI;
            char[] values = srcStr.ToCharArray();//转换为字符数组
            foreach (char value in values)
            {
                tempI = Convert.ToInt32(value);//先转10进制

                sbHex.Append(Convert.ToString(tempI, 16).PadLeft(2, '0') + " ");//得到16进制的ASCII码
            }

            //string si = sbHex.ToString();
            //MessageBox.Show(si);
            //byte[] StringsToByte = StringToBytes(sbHex.ToString());                             //得到字符串的GB2132字节编码
            //string stringByte = "";
            //foreach (byte MyByte in StringsToByte)                                              //遍历
            //{
            //    string Str = MyByte.ToString("x").ToUpper();                                    //转换为16进制大写字符串
            //    stringByte += "0x" + (Str.Length == 1 ? "0" + Str : Str + ",");              //填写
            //}

            //byte v1 = Convert.ToByte(srcStr1, 16);
            byte[] v2 = System.Text.Encoding.Default.GetBytes(srcStr1);
            //byte v3= Convert.ToByte(srcStr2, 16);
            byte[] v4 = System.Text.Encoding.Default.GetBytes(srcStr2);
            string sbHex1 = sbHex.ToString().Substring(0, sbHex.Length - 1);
            string[] varr = sbHex1.Split(' ');
            byte[] data1 = new byte[varr.Length];
            for (int i = 0; i < varr.Length; i++)
            {
                data1[i] = Convert.ToByte(varr[i].ToString(), 16);
            }
            serialPort.Write(xuhao, 0, xuhao.Length);
            Delay(500);
            serialPort.Write(uuid1, 0, uuid1.Length);
            Delay(500);
            serialPort.Write(uuid2, 0, uuid2.Length);

            ///////////无需搜索蓝牙
            Delay(500);
            serialPort.Write(startConnection, 0, startConnection.Length);//开始连接血糖仪
            Delay(200);
            serialPort.Write(operationUuid, 0, operationUuid.Length);  //操作血糖仪的uuid
            Delay(200);
            serialPort.Write(oneSend, 0, oneSend.Length);          //发送一次
                                                                   //////////////////////////////////////////////////////////////
            do
            {
                count++;
                Delay(500);
                serialPort.Write(halfSendStart, 0, halfSendStart.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了

                serialPort.Write(v4, 0, v4.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了
                serialPort.Write(v2, 0, v2.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了
                serialPort.Write(data1, 0, data1.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了

                serialPort.Write(halfSendEnd, 0, halfSendEnd.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了
            } while (serialPort.ReadExisting().Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m0035FF2283C8\r\n-uFFF1\r\n-d00"));

            textBox1.Text = count.ToString();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            string str = serialPort.ReadExisting();


            if (str.Contains("m0035FF2283C8"))  //血糖仪的uuid
            {
                //Delay(500);
                //serialPort.Write(bluetoothStop, 0, bluetoothStop.Length);
                //Delay(500);
                //serialPort.Write(xuhao, 0, xuhao.Length);
                //Delay(500);
                //serialPort.Write(uuid1, 0, uuid1.Length);
                //Delay(500);
                //serialPort.Write(uuid2, 0, uuid2.Length);
                serialPort.Write(startConnection, 0, startConnection.Length);//开始连接血糖仪
                Delay(200);
                serialPort.Write(operationUuid, 0, operationUuid.Length);  //操作血糖仪的uuid
                Delay(200);
                serialPort.Write(oneSend, 0, oneSend.Length);          //发送一次
                for (int i = 0; i < 15; i++)
                {
                    Delay(500);
                    serialPort.Write(halfSend, 0, halfSend.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了
                    if (str.Contains("SUCCESS\r\nNOTIFY\r\n-lF74A3C013B32\r\n-m0035FF2283C8\r\n-uFFF1\r\n-d000"))
                    {
                        break;
                    }
                }



            }
            else if (str.Contains("m8005DFC40F4D"))//温度计的uuid
            {

                Delay(500);
                serialPort.Write(bluetoothStop, 0, bluetoothStop.Length);
                serialPort.Write(thermometer, 0, thermometer.Length);//连接温度计蓝牙




            }
            else if (str.Contains("m8005DFC40F56"))  //血压uuid
            {
                Delay(500);
                serialPort.Write(bloodPressure, 0, bloodPressure.Length);//连接血压蓝牙



            }


            if (str.Contains("DISCONNECT\r\n-lF74A3C013B32\r\n-m0035FF2283C8"))//血糖仪的断开
            {
                //Delay(1000);
                // serialPort.Write(bluetoothName, 0, bluetoothName.Length);//搜索蓝牙

                //发起连接
                Connectionxuetangyi(str);


            }
            else if (str.Contains("DISCONNECT\r\n-lF74A3C013B32\r\n-m8005DFC40F4D"))//温度计的断开
            {

                //serialPort.Write(bluetoothName, 0, bluetoothName.Length);//搜索蓝牙

                //发起连接
                Connectionwenduji();
            }
            else if (str.Contains("DISCONNECT\r\n-lF74A3C013B32\r\n-m8005DFC40F56"))//血压的断开
            {
                //发起连接
                Connectionxueya();
                //serialPort.Write(bluetoothName, 0, bluetoothName.Length);//搜索蓝牙
            }
        }







        string dateTime = "";





        private void button4_Click_1(object sender, EventArgs e)
        {
            if (dateTime == "")
            {
                dateTime = DateTime.Now.ToString("yyMMddHHmmss");
            }
            else
            {


            }






            string link = "01" + dateTime;
            // string link = "01170331145030";

            textBox4.Text = ShuzicheckSum(link);   //得到校验和（一个16进制数）


            string srcStr = link + "#" + ShuzicheckSum(link);

            StringBuilder sbHex = new StringBuilder();
            int tempI;
            char[] values = srcStr.ToCharArray();//转换为字符数组
            foreach (char value in values)
            {
                tempI = Convert.ToInt32(value);//先转10进制

                sbHex.Append(Convert.ToString(tempI, 16).PadLeft(2, '0'));//得到16进制的ASCII码
            }

            string s22222 = sbHex.ToString();
            ///////////////////////////////////////////////


            string s7 = "3031323131323130313134373032233438";
            //        "3031323131323130313133363230233461"
            byte[] s8 = System.Text.Encoding.Default.GetBytes(s22222);
            serialPort.Write(halfSendStart, 0, halfSendStart.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了
                                                                       //serialPort.WriteLine(s5);  //半秒一次，直到滴的一声响，就表示是连接成功了


            serialPort.Write(s8, 0, s8.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了
                                                 //serialPort.Write(data1, 0, data1.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了

            serialPort.Write(halfSendEnd, 0, halfSendEnd.Length);  //半秒一次，直到滴的一声响，就表示是连接成功了

            //MessageBox.Show("aaa");

            /////////////////////没有四舍五入取小数//////////////////////
            //float f = 0.9999f;
            //int i = (int)(f * 10);
            //f = (float)(i * 1.0) / 10;
            //Console.WriteLine(f);
            /////////////////////////////////////////////////////////
        }


        //求  {01211208172645#37}  的方法
        /// <summary>
        /// 传入一组字符串数字，通过其ASCII码值进行相加（16进制相加），得到的结果取后面2位（2ba取ba），求（ba）的补数
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public string ShuzicheckSum(string link)
        {

            // string link = "01" + DateTime.Now.ToString("yyMMddHHmmss");

            // string link = "01211208172645";   //01211130134720#45

            //string a = "abcdef";
            //var b = Enumerable.Range(0, a.Length / 2).Select(i => a.Substring(i * 2, 2)).ToArray();

            //MessageBox.Show(b[0]);   ////将abcdef变成数组，每两个为一个值，b[0]=ab,b[1]=cd,b[2]=ef

            StringBuilder stringBuilder = new StringBuilder(link.Length * 2);
            for (int i = 0; i < link.Length; i++)
            {
                stringBuilder.Append(((int)link[i]).ToString("x2"));//x2是小写，将字符串转换为16进制的字符串
            }
            //MessageBox.Show(stringBuilder.ToString());

            //将字符串  转换  字符串数组，例子：将abcdef变成数组，每两个为一个值，b[0]=ab,b[1]=cd,b[2]=ef
            string[] hex = Enumerable.Range(0, stringBuilder.ToString().Length / 2).Select(i => stringBuilder.ToString().Substring(i * 2, 2)).ToArray();
            int a = 0;
            for (int x = 0; x < hex.Length; x++)
            {
                a += Convert.ToInt32(hex[x], 16);
            }
            string a1 = a.ToString("x");//求出的 十进制和 转换为  16进制的和

            //MessageBox.Show(a1.Substring(a1.Length - 2));
            int ii = Convert.ToInt32(a1.Substring(a1.Length - 2), 16);//取求得的16进制结果，取后面两位，例如；16进制和为：2BA，取BA
            string result = (~ii).ToString("x");//求 BA 的补数
                                                //MessageBox.Show(result);
                                                //Console.WriteLine(xuetangyi(link));

            return result;
        }
        //求一个数字的补数///
        public int FindComplement(int num)
        {
            int highbit = 0;
            for (int i = 1; i <= 30; ++i)
            {
                if (num >= 1 << i)
                {
                    highbit = i;
                }
                else
                {
                    break;
                }
            }
            int mask = highbit == 30 ? 0x7fffffff : (1 << (highbit + 1)) - 1;
            return num ^ mask;
        }




        private byte[] StringToBytes(string TheString)                                          //utf8编码转GB2132编码
        {
            Encoding FromEcoding = Encoding.GetEncoding("UTF-8");                               //UTF8编码
            Encoding ToEcoding = Encoding.GetEncoding("gb2312");                                //GB2312编码
            byte[] FromBytes = FromEcoding.GetBytes(TheString);                                 //获取汉字UTF8字节序列
            byte[] Tobytes = Encoding.Convert(FromEcoding, ToEcoding, FromBytes);               //转换为GB2132字节码
            return Tobytes;                                                                     //返回
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //string str = "+V=12.306\r\n";
            string str = "+V=11.7\r\n";

            if (str.Contains("+V="))      //接收到血压的数据
            {

                textBox2.Text = "电压："+MidStrEx_New(str, "V=", "\r\n");
                textBox1.Text ="电量：" +Voltage(MidStrEx_New(str, "V=", "\r\n"));
            }
        }

        public string Voltage(string str)
        {//12.306
            double b = ((3.5 - (12.5 - Convert.ToDouble(str))) * 100 / 3.5);

            
           return (int)Convert.ToDouble(b.ToString()) + "%";

        }

        private void button10_Click(object sender, EventArgs e)
        {
            byte[] dy
               = new byte[] { 0x41, 0x54, 0x2B, 0x56, 0x0D, 0x0A };
            //发送数据
            if (serialPort.IsOpen)
            {

          serialPort.Write(dy,0,dy.Length);


            }
        }
    }
}
