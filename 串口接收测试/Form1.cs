using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 串口接收测试
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }



        #region 接收方法
        //     private List<byte> buffer = new List<byte>(4096);
        //     private void sp_DataReceived(objectsender, EventArgs e) //sp是串口控件
        //     {
        //         int n = sp.BytesToRead;
        //         byte[] buf = new byte[n];
        //         sp.Read(buf, 0, n);

        //         //1.缓存数据
        //         buffer.AddRange(buf);
        //         //2.完整性判断
        //         while (buffer.Count >= 4) //至少包含帧头（2字节）、长度（1字节）、校验位（1字节）；根据设计不同而不同
        //         {
        //             //2.1 查找数据头
        //             if (buffer[0] == 0x01) //传输数据有帧头，用于判断
        //             {
        //                 int len = buffer[2];
        //                 if (buffer.Count < len + 4) //数据区尚未接收完整
        //                 {
        //                     break;
        //                 }
        //                 //得到完整的数据，复制到ReceiveBytes中进行校验
        //                 buffer.CopyTo(0, ReceiveBytes, 0, len + 4);
        //                 byte valid; //开始校验
        //                 valid = this.JY(ReceiveBytes);
        //                 if (jiaoyan != ReceiveBytes[len + 3]) //校验失败，最后一个字节是校验位
        //                 {
        //                     buffer.RemoveRange(0, len + 4);
        //                     MessageBox.Show("数据包不正确！");
        //                     continue;
        //                 }
        //                 buffer.RemoveRange(0, len + 4);
        //                  // 执行其他代码，对数据进行处理。
        //}
        //             else //帧头不正确时，记得清除
        //             {
        //                 buffer.RemoveAt(0);
        //             }
        //         }
        //     }


        #endregion

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
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenClosePort(cmbPort.Text, Convert.ToInt32(cmbBaud.Text, 10));//打开串口
            button1.Text = "关闭串口";
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string values = this.SendTbox.Text;
            string[] varr = values.TrimEnd().Split(' ');
            byte[] data1 = new byte[varr.Length];
            if (varr.Length > 1)
            {
                for (int i = 0; i < varr.Length; i++)
                {
                    data1[i] = Convert.ToByte(varr[i].ToString(), 16);
                }
                //serialPort1.Write(data1, 0, data1.Length);
              SendData(data1);//发送数据
            }


        }



        public  string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }


        public  SerialPort SerialPort = null;
        public  SerialPort OpenClosePort(string comName, int baud)
        {
            //串口未打开
            if (SerialPort == null || !SerialPort.IsOpen)
            {
                SerialPort = new SerialPort();
                //串口名称
                SerialPort.PortName = comName;
                //波特率
                SerialPort.BaudRate = baud;
                //数据位
                SerialPort.DataBits = 8;
                //停止位
                SerialPort.StopBits = StopBits.One;
                //校验位
                SerialPort.Parity = Parity.None;
                //打开串口
                SerialPort.Open();
                //串口数据接收事件实现
                SerialPort.DataReceived += new SerialDataReceivedEventHandler(ReceiveData);

                return SerialPort;
            }
            //串口已经打开
            else
            {
                SerialPort.Close();
                return SerialPort;
            }
        }

        public  void ReceiveData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort _SerialPort = (SerialPort)sender;

            int _bytesToRead = _SerialPort.BytesToRead;
            byte[] recvData = new byte[_bytesToRead];
            int SDataTemp = _SerialPort.Read(recvData, 0, _bytesToRead);
            //this.ReceiveTbox.AppendText(unicode.GetString(recvData));
            UTF8Encoding unicode = new UTF8Encoding();
            
            this.ReceiveTbox.Invoke
            (
                new MethodInvoker
                (
                    delegate
                    {
                        string returnStr = "";
                        for (int i = 0; i < SDataTemp; i++)
                        {
                            returnStr += recvData[i].ToString(format: "X2") + " ";
                        }
                        

                            this.ReceiveTbox.AppendText(returnStr);
                       
                       
                    }
                )
            );

            //this.ReceiveTbox.AppendText(Encoding.GetEncoding("gb2312").GetString(recvData));


            //向控制台打印数据
            Debug.WriteLine("收到数据：" + recvData);


        }

        public  bool SendData(byte[] data)
        {
            if (SerialPort != null && SerialPort.IsOpen)
            {
                SerialPort.Write(data, 0, data.Length);
                Debug.WriteLine("发送数据：" + data);
                return true;
            }
            else
            {
                return false;
            }
        }




    }
}
