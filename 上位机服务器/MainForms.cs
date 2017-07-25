using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;

namespace 上位机服务器
{
    public partial class MainForms : Form
    {
        #region 全局变量定义

        public static bool ServerChange = false;                           //服务器状态是否发生改变
        //定义主页的复选框（电压页、离子页、恒温页、接收机页、隔离器页、历史查看页）的Name字符串数组
        private string[] Voltage = { "CH_V1", "CH_V2", "CH_V3", "CH_V4", "CH_CFV", "CH_OSCF", "CH_OSCI" };
        private string[] Ion = { "CH_IONV", "CH_IONI", "CH_FLUX", "CH_FLR" };
        private string[] Contem = { "CH_THB", "CH_THC", "CH_THD", "CH_NECK", "CH_OVN1", "CH_OVN2", "CH_ISOL" };
        private string[] Receive = { "CH_IFL", "CH_TUNE", "CH_DIO" };
        private string[] Isolation = { "CH_CH1", "CH_CH2", "CH_CH3", "CH_CH4", "CH_CH5", "CH_CH6", "CH_CH7", "CH_CH8" };
        private string[] history = { "checkBox25", "checkBox26", "checkBox27", "checkBox28", "checkBox29", "checkBox30","checkBox31",
                                "checkBox17", "checkBox16", "checkBox15", "checkBox14", 
                                "checkBox24", "checkBox23", "checkBox22", "checkBox21", "checkBox20", "checkBox19", "checkBox18", 
                                "checkBox9", "checkBox8", "checkBox7", "checkBox35", "checkBox34", "checkBox33", "checkBox32",
                                "checkBox13", "checkBox12", "checkBox11", "checkBox10"};
        private bool sysFlag = false;
        private int timeRecover=0;
        private int countTime = 0;
        private string freBack = "";
        #endregion

        #region 事件和委托
        delegate void addDelegate();
        delegate void addData(int a, int length);
        #endregion

        #region 窗体初始化
        public MainForms()
        {
            InitializeComponent();
            ComboBox.Text = "".Equals(ComboBox.Text) ? "电压页" : ComboBox.Text;
            ComboBox_SelectedCong(true, false, false, false, false);                    //按钮可操作/不可操作初始化
            TextBox.CheckForIllegalCrossThreadCalls = false;
            txtStatus3.Text = "点击网络配置，然后启动服务器即可.";
           // txtStatus2.Text = "服务器已建立，可启动服务器.";
            timer2.Interval = 1000;
            timer2.Start();
            setControlsAvailability(false);                     //设置系统配置页面组件可用性状态
            PortName_ComboBox.Text = "us";
            comboBox1.Text = "us";
            dataGridView1.Rows.Clear();
            if (radioButton4.Checked)
            {
                textBox5.MaxLength = 7;

                textBox5.Text = float.Parse(textBox5.Text).ToString("0.0000").PadRight(4);
                sys_zong_value.Text = textBox5.Text;
            }
            if (radioButton3.Checked)
            {
                textBox5.MaxLength = 9;
                textBox5.Text = float.Parse(textBox5.Text).ToString("0.000000").PadRight(4);
                sys_zong_value.Text = textBox5.Text;
              
            }


            OleDbConnection mycon = null;
            OleDbDataReader myReader = null;
            
            try
            {
                mycon = new OleDbConnection(Program.ConStr);
                mycon.Open();
                string sql = "select * from sensor";
                OleDbCommand mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                int count = 0;
                while (myReader.Read())
                {
                    count++;
                }
                myReader.Close();
                myReader = mycom.ExecuteReader();
                if (count > 1)
                    dataGridView1.Rows.Add(count - 1);

                int i = 0;
                string[] content = { "date", "V1", "V2", "V3", "V4", "CFV", "OSCV", "OSCI", "IONV", "IONI", "FLUX", "FLR", "THB", "THC", "THD",
                                     "NECK","OVN1","OVN2","ISOL","IFL","TUNE","DIO","CH1","CH2","CH3","CH4","CH5","CH6","CH7","CH8" };
                while (myReader.Read())
                {
                    for (int temp = 0; temp < content.Length; temp++)
                    {   if(temp==0)
                        dataGridView1.Rows[i].Cells[temp].Value = myReader[content[temp]].ToString().Trim();
                        else{                       
                          double setDate=    double.Parse(myReader[content[temp]].ToString());
                          dataGridView1.Rows[i].Cells[temp].Value = setDate.ToString("0.0").PadRight(4);
                     
                    }
                    }
                    i++;
                }
                myReader.Close();
            }
            finally
            {
                mycon.Close();
            }
        }
        #endregion

        #region 按键事件
        // 历史查看：查询按钮点击事件
        private void button11_Click(object sender, EventArgs e)
        {
            if (checkBox25.Checked) plot5.Channels[0].Visible = true; else plot5.Channels[0].Visible = false;
            if (checkBox26.Checked) plot5.Channels[1].Visible = true; else plot5.Channels[1].Visible = false;
            if (checkBox27.Checked) plot5.Channels[2].Visible = true; else plot5.Channels[2].Visible = false;
            if (checkBox28.Checked) plot5.Channels[3].Visible = true; else plot5.Channels[3].Visible = false;
            if (checkBox29.Checked) plot5.Channels[4].Visible = true; else plot5.Channels[4].Visible = false;
            if (checkBox30.Checked) plot5.Channels[5].Visible = true; else plot5.Channels[5].Visible = false;
            if (checkBox31.Checked) plot5.Channels[6].Visible = true; else plot5.Channels[6].Visible = false;

            if (checkBox17.Checked) plot5.Channels[7].Visible = true; else plot5.Channels[7].Visible = false;
            if (checkBox16.Checked) plot5.Channels[8].Visible = true; else plot5.Channels[8].Visible = false;
            if (checkBox15.Checked) plot5.Channels[9].Visible = true; else plot5.Channels[9].Visible = false;
            if (checkBox14.Checked) plot5.Channels[10].Visible = true; else plot5.Channels[10].Visible = false;

            if (checkBox24.Checked) plot5.Channels[11].Visible = true; else plot5.Channels[11].Visible = false;
            if (checkBox23.Checked) plot5.Channels[12].Visible = true; else plot5.Channels[12].Visible = false;
            if (checkBox22.Checked) plot5.Channels[13].Visible = true; else plot5.Channels[13].Visible = false;
            if (checkBox21.Checked) plot5.Channels[14].Visible = true; else plot5.Channels[14].Visible = false;
            if (checkBox20.Checked) plot5.Channels[15].Visible = true; else plot5.Channels[15].Visible = false;
            if (checkBox19.Checked) plot5.Channels[16].Visible = true; else plot5.Channels[16].Visible = false;
            if (checkBox18.Checked) plot5.Channels[17].Visible = true; else plot5.Channels[17].Visible = false;


            if (checkBox9.Checked) plot5.Channels[18].Visible = true; else plot5.Channels[18].Visible = false;
            if (checkBox8.Checked) plot5.Channels[19].Visible = true; else plot5.Channels[19].Visible = false;
            if (checkBox7.Checked) plot5.Channels[20].Visible = true; else plot5.Channels[20].Visible = false;

            if (checkBox35.Checked) plot5.Channels[21].Visible = true; else plot5.Channels[21].Visible = false;
            if (checkBox34.Checked) plot5.Channels[22].Visible = true; else plot5.Channels[22].Visible = false;
            if (checkBox33.Checked) plot5.Channels[23].Visible = true; else plot5.Channels[23].Visible = false;
            if (checkBox32.Checked) plot5.Channels[24].Visible = true; else plot5.Channels[24].Visible = false;
            if (checkBox13.Checked) plot5.Channels[25].Visible = true; else plot5.Channels[25].Visible = false;
            if (checkBox12.Checked) plot5.Channels[26].Visible = true; else plot5.Channels[26].Visible = false;
            if (checkBox11.Checked) plot5.Channels[27].Visible = true; else plot5.Channels[27].Visible = false;
            if (checkBox10.Checked) plot5.Channels[28].Visible = true; else plot5.Channels[28].Visible = false;

            for (int temp = 0; temp < 29; temp++)
                plot5.Channels[temp].Clear();
            string startdat = dateTimePicker1.Value.ToString();
            string enddat = dateTimePicker2.Value.ToString();
            DateTime starTime = Convert.ToDateTime(startdat);//2010-7-1 00:00:00
            DateTime endTime = Convert.ToDateTime(enddat);//2010-7-1 00:00:00
            dataGridView1.Rows.Clear();
            OleDbConnection mycon = null;
            OleDbDataReader myReader = null;
            try
            {
                mycon = new OleDbConnection(Program.ConStr);
                mycon.Open();
                string sql = "select * from sensor where date between #" + starTime + "# and #" + endTime + "# order by date asc";
                OleDbCommand mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                int count = 0;
                while (myReader.Read())
                {
                    count++;
                }
                myReader.Close();
                myReader = mycom.ExecuteReader();
                if (count > 1)
                {
                    dataGridView1.Rows.Add(count - 1);
                }
                int i = 0;
                string[] content = { "date", "V1", "V2", "V3", "V4", "CFV", "OSCV", "OSCI", "IONV", "IONI", "FLUX", "FLR", "THB", "THC", "THD",
                                     "NECK","OVN1","OVN2","ISOL","IFL","TUNE","DIO","CH1","CH2","CH3","CH4","CH5","CH6","CH7","CH8" };
                while (myReader.Read())
                {
                    for (int temp = 0; temp < content.Length; temp++)
                    {
                        dataGridView1.Rows[i].Cells[temp].Value = myReader[content[temp]].ToString().Trim();
                    }
                    DateTime plotTimer = DateTime.Parse(myReader["date"].ToString());
                    for (int temp = 0; temp < content.Length - 1; temp++)
                    {
                        plot5.Channels[temp].AddXY(plotTimer, double.Parse(myReader[content[temp + 1]].ToString()));
                    }
                    i++;
                }
                myReader.Close();
            }
            finally
            {
                mycon.Close();
            }
        }

        // 系统设置：离线数据导入—打开数据源文件
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "txt文件|*.txt";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = false;
            openFileDialog1.Title = "打开txt";
            openFileDialog1.FileName = "";
            openFileDialog1.Multiselect = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName.ToString();
                textBox1.Text = filePath;
            }
        }

        // 系统设置：离线数据导入—导入数据库
        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先选择txt数据文件！");
                return;
            }
            button2.Enabled = false;
            button3.Enabled = false;
            StreamReader objReader = new StreamReader(textBox1.Text, UnicodeEncoding.GetEncoding("GB2312"));
            string sLine = "";
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
               
                if (sLine != null && !sLine.Equals("") && sLine.Length == 252 && sLine[10] == ' ' && sLine[19] == ',')
                {
                    string s1 = sLine.Substring(0, 19);
                    DateTime saveTime = Convert.ToDateTime(s1);
                    string s2 = sLine.Substring(20, 232);
                    byte[] sendbytes = new byte[s2.Length / 2];
                    for (int i = 0; i < s2.Length; i += 2)
                    {
                        sendbytes[i / 2] = Convert.ToByte(s2.Substring(i, 2), 16);
                    }
                    // 电压页
                    float V1 = BitConverter.ToSingle(sendbytes, 0);
                    float V2 = BitConverter.ToSingle(sendbytes, 4);
                    float V3 = BitConverter.ToSingle(sendbytes, 8);
                    float V4 = BitConverter.ToSingle(sendbytes, 12);
                    float CFV = BitConverter.ToSingle(sendbytes, 16);
                    float OSCV = BitConverter.ToSingle(sendbytes, 20);
                    float OSCI = BitConverter.ToSingle(sendbytes, 24);
                    // 离子泵页
                    float IONV = BitConverter.ToSingle(sendbytes, 28);
                    float IONI = BitConverter.ToSingle(sendbytes, 32);
                    float FLUX = BitConverter.ToSingle(sendbytes, 36);
                    float FLR = BitConverter.ToSingle(sendbytes, 40);
                    // 恒温页
                    float THB = BitConverter.ToSingle(sendbytes, 44);
                    float THC = BitConverter.ToSingle(sendbytes, 48);
                    float THD = BitConverter.ToSingle(sendbytes, 52);
                    float NECK = BitConverter.ToSingle(sendbytes, 56);
                    float OVN1 = BitConverter.ToSingle(sendbytes, 60);
                    float OVN2 = BitConverter.ToSingle(sendbytes, 64);
                    float ISOL = BitConverter.ToSingle(sendbytes, 68);
                    // 接收机页
                    float IFL = BitConverter.ToSingle(sendbytes, 72);
                    float TUNE = BitConverter.ToSingle(sendbytes, 76);
                    float DIO = BitConverter.ToSingle(sendbytes, 80);
                    // 隔离器页
                    float CH1 = BitConverter.ToSingle(sendbytes, 84);
                    float CH2 = BitConverter.ToSingle(sendbytes, 88);
                    float CH3 = BitConverter.ToSingle(sendbytes, 92);
                    float CH4 = BitConverter.ToSingle(sendbytes, 96);
                    float CH5 = BitConverter.ToSingle(sendbytes, 100);
                    float CH6 = BitConverter.ToSingle(sendbytes, 104);
                    float CH7 = BitConverter.ToSingle(sendbytes, 108);
                    float CH8 = BitConverter.ToSingle(sendbytes, 112);
                    OleDbConnection con = new OleDbConnection(Program.ConStr);
                    try
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            string tx = "insert into sensor values('"
                                + s1 + "','"
                                + V1 + "','" + V2 + "','" + V3 + "','" + V4 + "','" + CFV + "','" + OSCV + "','" + OSCI
                                + "','" + IONV + "','" + IONI + "','" + FLUX + "','" + FLR
                                + "','" + THB + "','" + THC + "','" + THD + "','" + NECK + "','" + OVN1 + "','" + OVN2 + "','" + ISOL
                                + "','" + IFL + "','" + TUNE + "','" + DIO
                                + "','" + CH1 + "','" + CH2 + "','" + CH3 + "','" + CH4 + "','" + CH5 + "','" + CH6 + "','" + CH7 + "','" + CH8 + "')";
                            OleDbCommand cmd = new OleDbCommand(tx, con);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
            MessageBox.Show("数据添加成功！");
            button2.Enabled = true;
            button3.Enabled = true;
        }

        // 系统设置：两路秒信号配置—移相设置按钮点击事件
        private void button6_Click(object sender, EventArgs e)
        {
            string message = textBox3.Text.Trim();
            //MessageBox.Show("" + NetDebugForm.hasOpen);
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("请输入发送的数据.");
                return;
			}
            else if (NetDebugForm.hasOpen&&MessageBox.Show("确定进行配置吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes) //服务器已经开启且移相数值输入正确
            {
                int data;
                if (message.StartsWith("-"))
                {
                    message = message.Substring(1);
                    double tempMessage = Double.Parse(message);

                    if (PortName_ComboBox.Text == "ms")
                        tempMessage *= 10000;
                    else
                        tempMessage *= 10;
                    data = -(int)tempMessage;
                }
                else
                {
                    double tempMessage = Double.Parse(message);
                    if (PortName_ComboBox.Text == "ms")
                        tempMessage *= 10000;
                    else
                        tempMessage *= 10;

                    data = (int)tempMessage;
                }
                data %= 10000000;
               
				Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
             

                byte[] sendbytes = new byte[11];
                byte[] temp = BitConverter.GetBytes(data);
                sendbytes[0] = 0x2e;
                sendbytes[1] = 0x2e;
                sendbytes[2] = 0x00;
                sendbytes[3] = 0x0b;
                sendbytes[4] = 0x01;
                sendbytes[5] = temp[0];
                sendbytes[6] = temp[1];
                sendbytes[7] = temp[2];
                sendbytes[8] = temp[3];
                sendbytes[9] = 0x2f;
                sendbytes[10] = 0x2f;
                foreach (int conn in dictsocket.Keys)
                    try
                    {
                        //检测客户端Socket的状态
                        if (dictsocket[conn].Connected)
                            dictsocket[conn].Send(sendbytes);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
            }
        }

        // 系统设置：两路秒信号配置—脉宽设置按钮点击事件
        private void button7_Click(object sender, EventArgs e)
        {
            string message = textBox4.Text.Trim();
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("请输入发送的数据.");
                return;
            }
            else if (isNumberic(message) && NetDebugForm.hasOpen && NetDebugForm.hasOpen && MessageBox.Show("确定进行配置吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes) //服务器已经开启且脉宽数值输入正确
            {
                int data = int.Parse(message);
                Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                if (comboBox1.Text == "ms")
                    data *= 10000;
                else
                    data *= 10;
                byte[] sendbytes = new byte[11];
                byte[] temp = BitConverter.GetBytes(data);
                #region 存入数据库
                OleDbConnection con = new OleDbConnection(Program.ConStr);
                    try
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                        {
                            string tx = "insert into sspw values('"
                                + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToLongTimeString().ToString() + "','"
                                + data + "')";
                            OleDbCommand cmd = new OleDbCommand(tx, con);
                            cmd.ExecuteNonQuery();
                            cmd.Dispose();
                        }
                    }
                    catch (SystemException ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        con.Close();
                    }              
                #endregion
                sendbytes[0] = 0x2e;
                sendbytes[1] = 0x2e;
                sendbytes[2] = 0x00;
                sendbytes[3] = 0x0b;
                sendbytes[4] = 0x02;
                sendbytes[5] = temp[0];
                sendbytes[6] = temp[1];
                sendbytes[7] = temp[2];
                sendbytes[8] = temp[3];
                sendbytes[9] = 0x2f;
                sendbytes[10] = 0x2f;
                foreach (int conn in dictsocket.Keys)
                    try
                    {
                        //检测客户端Socket的状态
                        if (dictsocket[conn].Connected)
                            dictsocket[conn].Send(sendbytes);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
            }
        }

        // 系统设置：两路秒信号配置—信号同步事件
        private void checkBox36_CheckedChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox3.Text) && checkBox36.Checked)
            {
                if (MessageBox.Show("确定信号同步吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                    byte[] sendbytes = new byte[11];
                    int data = 5000000;
                    byte[] temp = BitConverter.GetBytes(data);
                    sendbytes[0] = 0x2e;
                    sendbytes[1] = 0x2e;
                    sendbytes[2] = 0x00;
                    sendbytes[3] = 0x0b;
                    sendbytes[4] = 0x0b;
                    sendbytes[5] = temp[0];
                    sendbytes[6] = temp[1];
                    sendbytes[7] = temp[2];
                    sendbytes[8] = temp[3];
                    sendbytes[9] = 0x2f;
                    sendbytes[10] = 0x2f;
                    foreach (int conn in dictsocket.Keys)
                        try
                        {
                            //检测客户端Socket的状态
                            if (dictsocket[conn].Connected)
                                dictsocket[conn].Send(sendbytes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                }
            }
        }

        //系统设置：六路继电器配置
        /*private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;
            if (box.Checked && MessageBox.Show("确定是要关闭该继电器吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (relayStateSend())
                    box.Text = "已关机";
            }
            if ((!box.Checked) && relayStateSend())
            {
                box.Text = "已开机";
            }
        }*/

        //系统设置：六路继电器配置共用方法_手动开/关：3路继电器
        private bool relayStateSend(byte RelayID, bool OnOff)
        {
            bool result = false;
            if (NetDebugForm.hasOpen)
            {
                Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                byte[] sendbytes = new byte[9];
                byte temp1 = RelayID;
                byte temp2 = 0;
                if (OnOff)  //打开
                    temp2 = 0x01;
                else if (!OnOff)    //关闭
                    temp2 = 0x02;
                
                sendbytes[0] = 0x2e;
                sendbytes[1] = 0x2e;
                sendbytes[2] = 0x00;
                sendbytes[3] = 0x09;
                sendbytes[4] = 0x03;
                sendbytes[5] = temp1;
                sendbytes[6] = temp2;
                sendbytes[7] = 0x2f;
                sendbytes[8] = 0x2f;
                foreach (int conn in dictsocket.Keys)
                    try
                    {
                        //检测客户端Socket的状态
                        if (dictsocket[conn].Connected)
                        {
                            dictsocket[conn].Send(sendbytes);
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
            }
            return result;
        }
        //六路继电器自动关闭  关机 false 开机 true
        private bool relayStateAutoSend(bool stat)
        {
            bool result = false;
            if (NetDebugForm.hasOpen)
            {
                Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                byte[] sendbytes = new byte[8];
                byte temp = 0x00;
                if (!stat)  //关机 false
                    temp += 0x02;
                else        //开机    true
                    temp += 0x01;
                              
                sendbytes[0] = 0x2e;
                sendbytes[1] = 0x2e;
                sendbytes[2] = 0x00;
                sendbytes[3] = 0x08;
                sendbytes[4] = 0x09;
                sendbytes[5] = temp;
                sendbytes[6] = 0x2f;
                sendbytes[7] = 0x2f;
                foreach (int conn in dictsocket.Keys)
                    try
                    {
                        //检测客户端Socket的状态
                        if (dictsocket[conn].Connected)
                        {
                            dictsocket[conn].Send(sendbytes);
                            result = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
            }
            return result;
        }

        // 系统设置：综合器设置—频率设置
        private void button8_Click(object sender, EventArgs e)
        {
            if (NetDebugForm.hasOpen && MessageBox.Show("确定进行配置吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (!string.IsNullOrEmpty(textBox5.Text) && NetDebugForm.hasOpen)
                {
                     freBack = sys_zong_value.Text;
                     sysFlag = false;
                    Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                    byte[] sendbytes = new byte[13];
                    double myValue = double.Parse(textBox5.Text);

                    if (myValue < 10||myValue>50) {
                        MessageBox.Show("频率值需在10到50之间");
                        return;
                    }
                    int sendValue;
                    if (radioButton3.Checked)
                        sendValue = (int)(myValue * 1000000);
                    else
                        sendValue = (int)(myValue * 10000);

                    double myValue2 = double.Parse(sys_zong_value.Text);
                    sys_zong_value.Text = (myValue + myValue2).ToString();

                    #region  存入数据库
                    OleDbConnection con = new OleDbConnection(Program.ConStr);                 
                        //综合器频率
                        try
                        {
                            con.Open();
                            if (con.State == ConnectionState.Open)
                            {
                                string tx = "insert into syszong values('"
                                    + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToLongTimeString().ToString() + "','"
                                    + sendValue + "')";
                                OleDbCommand cmd = new OleDbCommand(tx, con);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                        catch (SystemException ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        finally
                        {
                            con.Close();
                        }
                    #endregion



                    byte[] temp = BitConverter.GetBytes(sendValue);
                    sendbytes[0] = 0x2e;
                    sendbytes[1] = 0x2e;
                    sendbytes[2] = 0x00;
                    sendbytes[3] = 0x0d;
                    sendbytes[4] = 0x04;
                    sendbytes[5] = 0x01;// 符号位
                    if (radioButton3.Checked)//六位精度
                        sendbytes[6] = 0x02;
                    else
                        sendbytes[6] = 0x01;
                    sendbytes[7] = temp[0];// (byte)((sendValue >> 24) & 0xFF);
                    sendbytes[8] = temp[1];//(byte)((sendValue >> 16) & 0xFF);
                    sendbytes[9] = temp[2];//(byte)((sendValue >> 8) & 0xFF);
                    sendbytes[10] = temp[3];//(byte)(sendValue & 0xFF);
                    sendbytes[11] = 0x2f;
                    sendbytes[12] = 0x2f;
                    foreach (int conn in dictsocket.Keys)
                        try
                        {
                            //检测客户端Socket的状态
                            if (dictsocket[conn].Connected)
                                dictsocket[conn].Send(sendbytes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }


                    sys_zong_value.Text = textBox5.Text;                  
                  //  System.Threading.Thread.Sleep(2000);
                  //  sys_zong_value.Text = freBack;
                
                }
                else
                {
                    MessageBox.Show("请输入发送的数据.");
                }
            }
        }

        // 系统设置：综合器设置—相位设置
        private void button9_Click(object sender, EventArgs e)
        {
            if (NetDebugForm.hasOpen && MessageBox.Show("确定进行配置吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int data = int.Parse(textBox6.Text);
            
                int jueduizhi = 0;
                if (data >= 0)
                {
                    jueduizhi = 1;
                
                }
                else
                {
                    jueduizhi = -1;
                    data = 0 - data;
                }
                int bai = data / 100;
                int shi = (data % 100) / 10;
                int ge = (data % 10) / 1;
                int synth;
				int time;
                if (ge == 0)
                {
                    synth = 0;
					time = 0;
                }
                else
                {
                   
					time=(int)(Math.Pow(10, ge));
                    double tempSys =-1*jueduizhi * (bai * 10 + shi) * 10000 / 7 / time;         
                    synth = (int)tempSys;

                }
                if ((time==0)||(synth==0))
                {
                    return;
                }
                byte[] synthtemp = BitConverter.GetBytes(synth);
                byte[] timetemp = BitConverter.GetBytes(time);
                if (!string.IsNullOrEmpty(textBox6.Text) && NetDebugForm.hasOpen)
                {
                    Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                    byte[] sendbytes = new byte[17];
                    sendbytes[0] = 0x2e;
                    sendbytes[1] = 0x2e;
                    sendbytes[2] = 0x00;
                    sendbytes[3] = 0x11;
                    sendbytes[4] = 0x06;
                    sendbytes[5] = 0x01;
                    if (radioButton3.Checked)
                        sendbytes[6] = 0x02;
                    else
                        sendbytes[6] = 0x01;

                    sendbytes[7] = synthtemp[0];// (byte)((synth >> 24) & 0xFF);
                    sendbytes[8] = synthtemp[1];//(byte)((synth >> 16) & 0xFF);
                    sendbytes[9] = synthtemp[2];//(byte)((synth >> 8) & 0xFF);
                    sendbytes[10] = synthtemp[3];//(byte)(synth & 0xFF);

                    sendbytes[11] = timetemp[0];// (byte)((ge >> 24) & 0xFF);
                    sendbytes[12] = timetemp[1];//(byte)((ge >> 16) & 0xFF);
                    sendbytes[13] = timetemp[2];//(byte)((ge >> 8) & 0xFF);
                    sendbytes[14] = timetemp[3];//(byte)(ge & 0xFF);

                    sendbytes[15] = 0x2f;
                    sendbytes[16] = 0x2f;
                    foreach (int conn in dictsocket.Keys)
                        try
                        {
                            //检测客户端Socket的状态
                            if (dictsocket[conn].Connected)
                                dictsocket[conn].Send(sendbytes);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                  //  freBack = sys_zong_value.Text;
                    double synthD = synth;

                    if (radioButton3.Checked)
                        synthD = synthD / 1000000;
                    else
                        synthD = synthD / 10000;

                    
                    double synthChange = double.Parse(sys_zong_value.Text) +synthD ;
                    String tempvalue = synthChange.ToString("0.000000").PadRight(4);
                    freBack = sys_zong_value.Text;
                    sys_zong_value.Text = tempvalue + "";                  
                    sysFlag = true;
                    timeRecover = time*10;
                }
                else
                {
                    MessageBox.Show("请输入发送的数据.");
                }
            }
        }

        // 系统设置：系统时间校正
        private void button5_Click(object sender, EventArgs e)
        {
            if (NetDebugForm.hasOpen && MessageBox.Show("确定进行配置吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                DateTime DT = System.DateTime.Now;
                string dt = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                DateTime starTime;

                if (false == DateTime.TryParse(textBox2.Text, out starTime))
                {
                    MessageBox.Show("时间格式不正确！");
                    return;
                }
                if (NetDebugForm.hasOpen)
                {
                    byte[] sendbytes = new byte[21];
                    int year = DT.Year;
                    int month = DT.Month;
                    int day = DT.Day;
                    int hour = DT.Hour;
                    int minute = DT.Minute;
                    int second = DT.Second;
                    sendbytes[0] = 0x2e;
                    sendbytes[1] = 0x2e;
                    sendbytes[2] = 0x00;
                    sendbytes[3] = 0x15;
                    sendbytes[4] = 0x07;
                    sendbytes[5] = (byte)(year / 1000);
                    sendbytes[6] = (byte)(year % 1000 / 100);
                    sendbytes[7] = (byte)(year % 100 / 10);
                    sendbytes[8] = (byte)(year % 10 / 1);
                    sendbytes[9] = (byte)(month % 100 / 10);
                    sendbytes[10] = (byte)(month % 10 / 1);
                    sendbytes[11] = (byte)(day % 100 / 10);
                    sendbytes[12] = (byte)(day % 10 / 1);
                    sendbytes[13] = (byte)(hour % 100 / 10);
                    sendbytes[14] = (byte)(hour % 10 / 1);
                    sendbytes[15] = (byte)(minute % 100 / 10);
                    sendbytes[16] = (byte)(minute % 10 / 1);
                    sendbytes[17] = (byte)(second % 100 / 10);
                    sendbytes[18] = (byte)(second % 10 / 1);
                    sendbytes[19] = 0x2f;
                    sendbytes[20] = 0x2f;
                    foreach (int conn in dictsocket.Keys)
                        try
                        {
                            //检测客户端Socket的状态
                            if (dictsocket[conn].Connected)
                            {
                                dictsocket[conn].Send(sendbytes);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                }
            }
        }

        // 历史查看：曲线和图表切换
        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "曲线")
            {
                button4.Text = "列表";
                dataGridView1.BringToFront();
            }
            else
            {
                button4.Text = "曲线";
                plot5.BringToFront();
                label2.BringToFront();
                ComboBox.BringToFront();
                checkBox25.BringToFront();
                checkBox26.BringToFront();
                checkBox27.BringToFront();
                checkBox28.BringToFront();
                checkBox29.BringToFront();
                checkBox30.BringToFront();
                checkBox31.BringToFront();

                checkBox17.BringToFront();
                checkBox16.BringToFront();
                checkBox15.BringToFront();
                checkBox14.BringToFront();

                checkBox24.BringToFront();
                checkBox23.BringToFront();
                checkBox22.BringToFront();
                checkBox21.BringToFront();
                checkBox20.BringToFront();
                checkBox19.BringToFront();
                checkBox18.BringToFront();

                checkBox9.BringToFront();
                checkBox8.BringToFront();
                checkBox7.BringToFront();

                checkBox35.BringToFront();
                checkBox34.BringToFront();
                checkBox33.BringToFront();
                checkBox32.BringToFront();
                checkBox13.BringToFront();
                checkBox12.BringToFront();
                checkBox11.BringToFront();
                checkBox10.BringToFront();

            }
        }

        // 历史查看：删除数据
        private void button13_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定进行删除操作吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                string startdat = dateTimePicker1.Value.ToString();
                string enddat = dateTimePicker2.Value.ToString();
                deleteData(false, startdat, enddat);
            }
        }

        // 历史查看：删除所有数据
        private void button14_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定进行删除全部操作吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                deleteData(true, null, null);
            }
        }

        #endregion

        #region 定时器事件
        //定时器显示当前时间 + 根据服务器是否发生变化去激活相应控件
        private void timer1_Tick(object sender, EventArgs e)
        {

            if (ServerChange)
            {

                if (NetDebugForm.hasOpen)                   //将系统设置页面设置按钮激活
                {
                   
                     txtStatus2.Text = "服务器已建立，可启动服务器.";
                     txtStatus3.Text = "";
                    setControlsAvailability(true);
                }
                else                                        //将系统设置页面设置按钮禁止
                {

                    txtStatus2.Text = "";
                    txtStatus3.Text = "点击网络配置，然后启动服务器即可.";
                    setControlsAvailability(false);

                }
                ServerChange = false;
            }

            txtNowTime.Text = "当前时间:" + System.DateTime.Now.ToString(" HH:mm:ss  yyyy-MM-dd");
            textBox2.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if(sysFlag){
                if (countTime == timeRecover)
                {
                   
                    sys_zong_value.Text = freBack;
                    sysFlag = false;
                    timeRecover = 0;
                    countTime = 0;
                }
                else {

                  //  MessageBox.Show(timeRecover.ToString());
                    countTime++;
                }
                    
            
            }
        

        }
        // 定时更新数据
        private void timer2_Tick(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection(Program.ConStr);
            OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    string tx1 = "Select top 1 * from [sensor] order by date desc";
                    OleDbCommand sqlCmd = new OleDbCommand(tx1, con);
                    oleAdapter.SelectCommand = sqlCmd;
                    DataSet ds = new DataSet();
                    oleAdapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                    
                        string V1 = ds.Tables[0].Rows[0]["V1"].ToString();
                        string V2 = ds.Tables[0].Rows[0]["V2"].ToString();
                        string V3 = ds.Tables[0].Rows[0]["V3"].ToString();
                        string V4 = ds.Tables[0].Rows[0]["V4"].ToString();
                        string CFV = ds.Tables[0].Rows[0]["CFV"].ToString();
                        string OSCV = ds.Tables[0].Rows[0]["OSCV"].ToString();
                        string OSCI = ds.Tables[0].Rows[0]["OSCI"].ToString();
                      

                      
                        textBox7.Text = Convert.ToDouble(V1).ToString("f1");
                        textBox8.Text = Convert.ToDouble(V2).ToString("f1");
                        textBox9.Text = Convert.ToDouble(V3).ToString("f1");
                        textBox10.Text = Convert.ToDouble(V4).ToString("f1");
                        textBox11.Text = Convert.ToDouble(CFV).ToString("f1");
                        textBox34.Text = Convert.ToDouble(OSCV).ToString("f1");
                        textBox35.Text = Convert.ToDouble(OSCI).ToString("f1");

                        textBox42.Text = Convert.ToDouble(V1).ToString("f1");
                        textBox41.Text = Convert.ToDouble(V2).ToString("f1");
                        textBox40.Text ="-"+ Convert.ToDouble(V3).ToString("f1");
                        textBox39.Text = Convert.ToDouble(V4).ToString("f1");
                        textBox38.Text = Convert.ToDouble(CFV).ToString("f1");
                        textBox37.Text = Convert.ToDouble(OSCV).ToString("f1");
                        textBox36.Text = Convert.ToDouble(OSCI).ToString("f1");

                        plotgraph.Channels[0].AddXY(DateTime.Now, double.Parse(V1));
                        plotgraph.Channels[1].AddXY(DateTime.Now, double.Parse(V2));
                        plotgraph.Channels[2].AddXY(DateTime.Now, double.Parse(V3));
                        plotgraph.Channels[3].AddXY(DateTime.Now, double.Parse(V4));
                        plotgraph.Channels[4].AddXY(DateTime.Now, double.Parse(CFV));
                        plotgraph.Channels[5].AddXY(DateTime.Now, double.Parse(OSCV));
                        plotgraph.Channels[6].AddXY(DateTime.Now, double.Parse(OSCI));
                        string IONV = ds.Tables[0].Rows[0]["IONV"].ToString();
                        string IONI = ds.Tables[0].Rows[0]["IONI"].ToString();
                        string FLUX = ds.Tables[0].Rows[0]["FLUX"].ToString();
                        string FLR = ds.Tables[0].Rows[0]["FLR"].ToString();
                        textBox16.Text = Convert.ToDouble(IONV).ToString("f1");
                        textBox14.Text = Convert.ToDouble(IONI).ToString("f1");
                        textBox13.Text = Convert.ToDouble(FLUX).ToString("f1");
                        textBox12.Text = Convert.ToDouble(FLR).ToString("f1");

                        textBox46.Text = Convert.ToDouble(IONV).ToString("f1");
                        textBox45.Text = Convert.ToDouble(IONI).ToString("f1");
                        textBox44.Text = Convert.ToDouble(FLUX).ToString("f1");
                        textBox43.Text = Convert.ToDouble(FLR).ToString("f1");

                        plot1.Channels[0].AddXY(DateTime.Now, double.Parse(IONV));
                        plot1.Channels[1].AddXY(DateTime.Now, double.Parse(IONI));
                        plot1.Channels[2].AddXY(DateTime.Now, double.Parse(FLUX));
                        plot1.Channels[3].AddXY(DateTime.Now, double.Parse(FLR));
                        string THB = ds.Tables[0].Rows[0]["THB"].ToString();
                        string THC = ds.Tables[0].Rows[0]["THC"].ToString();
                        string THD = ds.Tables[0].Rows[0]["THD"].ToString();
                        string NECK = ds.Tables[0].Rows[0]["NECK"].ToString();
                        string OVN1 = ds.Tables[0].Rows[0]["OVN1"].ToString();
                        string OVN2 = ds.Tables[0].Rows[0]["OVN2"].ToString();
                        string ISOL = ds.Tables[0].Rows[0]["OSCI"].ToString();
                        textBox21.Text = Convert.ToDouble(THB).ToString("f1");
                        textBox20.Text = Convert.ToDouble(THC).ToString("f1");
                        textBox19.Text = Convert.ToDouble(THD).ToString("f1");
                        textBox18.Text = Convert.ToDouble(NECK).ToString("f1");
                        textBox17.Text = Convert.ToDouble(OVN1).ToString("f1");
                        textBox32.Text = Convert.ToDouble(OVN2).ToString("f1");
                        textBox15.Text = Convert.ToDouble(ISOL).ToString("f1");

                        textBox53.Text = Convert.ToDouble(THB).ToString("f1");
                        textBox52.Text = Convert.ToDouble(THC).ToString("f1");
                        textBox51.Text = Convert.ToDouble(THD).ToString("f1");
                        textBox50.Text = Convert.ToDouble(NECK).ToString("f1");
                        textBox49.Text = Convert.ToDouble(OVN1).ToString("f1");
                        textBox48.Text = Convert.ToDouble(OVN2).ToString("f1");
                        textBox47.Text = Convert.ToDouble(ISOL).ToString("f1");

                        plot2.Channels[0].AddXY(DateTime.Now, double.Parse(THB));
                        plot2.Channels[1].AddXY(DateTime.Now, double.Parse(THC));
                        plot2.Channels[2].AddXY(DateTime.Now, double.Parse(THD));
                        plot2.Channels[3].AddXY(DateTime.Now, double.Parse(NECK));
                        plot2.Channels[4].AddXY(DateTime.Now, double.Parse(OVN1));
                        plot2.Channels[5].AddXY(DateTime.Now, double.Parse(OVN2));
                        plot2.Channels[6].AddXY(DateTime.Now, double.Parse(ISOL));
                        string IFL = ds.Tables[0].Rows[0]["IFL"].ToString();
                        string TUNE = ds.Tables[0].Rows[0]["TUNE"].ToString();
                        string DIO = ds.Tables[0].Rows[0]["DIO"].ToString();
                        textBox26.Text = Convert.ToDouble(IFL).ToString("f1");
                        textBox25.Text = Convert.ToDouble(TUNE).ToString("f1");
                        textBox24.Text = Convert.ToDouble(DIO).ToString("f1");

                        textBox56.Text = Convert.ToDouble(IFL).ToString("f1");
                        textBox55.Text = Convert.ToDouble(TUNE).ToString("f1");
                        textBox54.Text = Convert.ToDouble(DIO).ToString("f1");

                        plot3.Channels[0].AddXY(DateTime.Now, double.Parse(IFL));
                        plot3.Channels[1].AddXY(DateTime.Now, double.Parse(TUNE));
                        plot3.Channels[2].AddXY(DateTime.Now, double.Parse(DIO));
                        string CH1 = ds.Tables[0].Rows[0]["CH1"].ToString();
                        string CH2 = ds.Tables[0].Rows[0]["CH2"].ToString();
                        string CH3 = ds.Tables[0].Rows[0]["CH3"].ToString();
                        string CH4 = ds.Tables[0].Rows[0]["CH4"].ToString();
                        string CH5 = ds.Tables[0].Rows[0]["CH5"].ToString();
                        string CH6 = ds.Tables[0].Rows[0]["CH6"].ToString();
                        string CH7 = ds.Tables[0].Rows[0]["CH7"].ToString();
                        string CH8 = ds.Tables[0].Rows[0]["CH8"].ToString();
                        textBox31.Text = Convert.ToDouble(CH1).ToString("f1");
                        textBox30.Text = Convert.ToDouble(CH2).ToString("f1");
                        textBox29.Text = Convert.ToDouble(CH3).ToString("f1");
                        textBox28.Text = Convert.ToDouble(CH4).ToString("f1");
                        textBox27.Text = Convert.ToDouble(CH5).ToString("f1");
                        textBox33.Text = Convert.ToDouble(CH6).ToString("f1");
                        textBox23.Text = Convert.ToDouble(CH7).ToString("f1");
                        textBox22.Text = Convert.ToDouble(CH8).ToString("f1");

                        textBox64.Text = Convert.ToDouble(CH1).ToString("f1");
                        textBox63.Text = Convert.ToDouble(CH2).ToString("f1");
                        textBox62.Text = Convert.ToDouble(CH3).ToString("f1");
                        textBox61.Text = Convert.ToDouble(CH4).ToString("f1");
                        textBox60.Text = Convert.ToDouble(CH5).ToString("f1");
                        textBox59.Text = Convert.ToDouble(CH6).ToString("f1");
                        textBox58.Text = Convert.ToDouble(CH7).ToString("f1");
                        textBox57.Text = Convert.ToDouble(CH8).ToString("f1");

                        plot4.Channels[0].AddXY(DateTime.Now, double.Parse(CH1));
                        plot4.Channels[1].AddXY(DateTime.Now, double.Parse(CH2));
                        plot4.Channels[2].AddXY(DateTime.Now, double.Parse(CH3));
                        plot4.Channels[3].AddXY(DateTime.Now, double.Parse(CH4));
                        plot4.Channels[4].AddXY(DateTime.Now, double.Parse(CH5));
                        plot4.Channels[5].AddXY(DateTime.Now, double.Parse(CH6));
                        plot4.Channels[6].AddXY(DateTime.Now, double.Parse(CH7));
                        plot4.Channels[7].AddXY(DateTime.Now, double.Parse(CH8));

                    }

                }
            }
            catch (SystemException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        #endregion

        #region 显示/隐藏曲线—复选框点击事件
        //电压页
        private void CH_V_CheckedChanged(object sender, EventArgs e)
        {
            MessageBox.Show("??");
            CheckBox boxTemp = (CheckBox)sender;
            int temp = Array.IndexOf(Voltage, boxTemp.Name);
            if (temp >= 0 && temp < Voltage.Length)
                plotgraph.Channels[temp].Visible = boxTemp.Checked ? true : false;
        }

        //离子页
        private void CH_2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox boxTemp = (CheckBox)sender;
            int temp = Array.IndexOf(Ion, boxTemp.Name);
            if (temp >= 0 && temp < Ion.Length)
                plot1.Channels[temp].Visible = boxTemp.Checked ? true : false;
        }

        //恒温页
        private void CH_3_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox boxTemp = (CheckBox)sender;
            int temp = Array.IndexOf(Contem, boxTemp.Name);
            if (temp >= 0 && temp < Contem.Length)
                plot2.Channels[temp].Visible = boxTemp.Checked ? true : false;
        }

        //接收机页
        private void CH_4_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox boxTemp = (CheckBox)sender;
            int temp = Array.IndexOf(Receive, boxTemp.Name);
            if (temp >= 0 && temp < Receive.Length)
                plot3.Channels[temp].Visible = boxTemp.Checked ? true : false;
        }

        // 隔离器页
        private void CH_5_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox boxTemp = (CheckBox)sender;
            int temp = Array.IndexOf(Isolation, boxTemp.Name);
            if (temp >= 0 && temp < Isolation.Length)
                plot4.Channels[temp].Visible = boxTemp.Checked ? true : false;
        }

        //历史查看页：选择Item
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBox.Text == "电压页")
            {
                ComboBox_SelectedCong(true, false, false, false, false);
            }
            else if (ComboBox.Text == "离子泵页")
            {
                ComboBox_SelectedCong(false, true, false, false, false);
            }
            else if (ComboBox.Text == "恒温页")
            {
                ComboBox_SelectedCong(false, false, true, false, false);
            }
            else if (ComboBox.Text == "接收机页")
            {
                ComboBox_SelectedCong(false, false, false, true, false);
            }
            else if (ComboBox.Text == "隔离器页")
            {
                ComboBox_SelectedCong(false, false, false, false, true);
            }
        }

        //曲线隐藏/显示
        private void checkBoxHistory_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox boxTemp = (CheckBox)sender;
            int temp = Array.IndexOf(history, boxTemp.Name);
            if (temp >= 0 && temp < history.Length)
                plot5.Channels[temp].Visible = boxTemp.Checked ? true : false;
        }
        #endregion

        // 用户关闭窗口并制定关闭原因前发生
        private void MainForms_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("点击确定退出当前运行的程序？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        //网络配置图标点击事件
        private void label6_Click(object sender, EventArgs e)
        {
            NetDebugForm sub = new NetDebugForm(this);
            sub.Show();
           // NetDebugForm.getInstance().Show();
        }
        //网络配置图标提示
        private void label6_MouseEnter(object sender, EventArgs e)
        {
            ToolTip p = new ToolTip();
            p.ShowAlways = true;
            p.SetToolTip(this.label6, "网络配置");
        }

        #region 自定义函数
        //历史查看页上组件隐藏/显示设置函数
        private void ComboBox_SelectedCong(bool vol, bool ion, bool contem, bool Receive, bool Isolation)
        {
            checkBox25.Visible = vol;
            checkBox26.Visible = vol;
            checkBox27.Visible = vol;
            checkBox28.Visible = vol;
            checkBox29.Visible = vol;
            checkBox30.Visible = vol;
            checkBox31.Visible = vol;
            checkBox17.Visible = ion;
            checkBox16.Visible = ion;
            checkBox15.Visible = ion;
            checkBox14.Visible = ion;
            checkBox24.Visible = contem;
            checkBox23.Visible = contem;
            checkBox22.Visible = contem;
            checkBox21.Visible = contem;
            checkBox20.Visible = contem;
            checkBox19.Visible = contem;
            checkBox18.Visible = contem;
            checkBox9.Visible = Receive;
            checkBox8.Visible = Receive;
            checkBox7.Visible = Receive;
            checkBox35.Visible = Isolation;
            checkBox34.Visible = Isolation;
            checkBox33.Visible = Isolation;
            checkBox32.Visible = Isolation;
            checkBox13.Visible = Isolation;
            checkBox12.Visible = Isolation;
            checkBox11.Visible = Isolation;
            checkBox10.Visible = Isolation;
        }

        //判断一个字符串是否为数字字符串
        private bool isNumberic(string message)
        {
            System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");
            return rex.IsMatch(message);
        }

        //根据服务器状态设置系统设置里相应组件的可用性
        private void setControlsAvailability(bool avail)
        {
          
            button6.Enabled = avail;
            button7.Enabled = avail;
            button8.Enabled = avail;
            button9.Enabled = avail;
            button5.Enabled = avail;
            checkBox36.Enabled = avail;
            button12.Enabled = avail;
            button15.Enabled = avail;
            button16.Enabled = avail;
            button1.Enabled = avail;
            button20.Enabled = avail;
            radioButton1.Enabled = avail;
            radioButton2.Enabled = avail;
            button10.Enabled = avail;
            if (avail == true)
            {
                radioButton1.Checked = true;
                button10.Enabled = true;
                setCheckBoxAvailability(false);

            }
        }

        //删除数据
        private void deleteData(bool IsAll, string startdat, string enddat)
        {
            for (int temp = 0; temp < 29; temp++)
                plot5.Channels[temp].Clear();
            dataGridView1.Rows.Clear();

            OleDbConnection mycon = null;
            string sql = null;
            try
            {
                mycon = new OleDbConnection(Program.ConStr);
                mycon.Open();
                if (IsAll)
                {
                    sql = "delete *from sensor";
                }
                else
                {
                    DateTime starTime = Convert.ToDateTime(startdat);//2010-7-1 00:00:00
                    DateTime endTime = Convert.ToDateTime(enddat);//2010-7-1 00:00:00
                    sql = "delete *from sensor where date between #" + starTime + "# and #" + endTime + "#";
                }
                OleDbCommand mycom = new OleDbCommand(sql, mycon);
                mycom.ExecuteNonQuery();
            }
            finally
            {
                mycon.Close();
            }
        }
        #endregion

        private void label4_Click(object sender, EventArgs e)
        {

        }
        private void setCheckBoxAvailability(bool avail)
        {
           
            button12.Enabled = avail;
            button15.Enabled = avail;
            button16.Enabled = avail;
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (NetDebugForm.hasOpen && MessageBox.Show("确定进行操作吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes) 
            {
                if (button10.Text == "打开")
                 {
                    relayStateAutoSend(true); 
                 button10.Text = "关闭";
                 }
                  else if (button10.Text == "关闭")
                 {
                     relayStateAutoSend(false); 
                    button10.Text = "打开";
                 }
             }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
          
            if (radioButton1.Checked) {
                button10.Enabled = true;
                setCheckBoxAvailability(false);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                setCheckBoxAvailability(true);
                button10.Enabled = false;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (NetDebugForm.hasOpen && MessageBox.Show("确定进行操作吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                byte relayID = 0;
                bool OnOff = false;
                if (button12.Text == "打开")
                {
                    relayID = 0x01;
                    OnOff = true;
                    button12.Text = "关闭";
                }
                else if (button12.Text == "关闭")
                {
                    relayID = 0x01;
                    OnOff = false;
                    button12.Text = "打开";
                }

                relayStateSend(relayID, OnOff);
            }
        }


        private void button15_Click(object sender, EventArgs e)
        {
            if (NetDebugForm.hasOpen && MessageBox.Show("确定进行操作吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                byte relayID = 0;
                bool OnOff = false;
                if (button15.Text == "打开")
                {
                    relayID = 0x03;
                    OnOff = true;
                    button15.Text = "关闭";
                }
                else if (button15.Text == "关闭")
                {
                    relayID = 0x03;
                    OnOff = false;
                    button15.Text = "打开";
                }
                relayStateSend(relayID, OnOff);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (NetDebugForm.hasOpen && MessageBox.Show("确定进行操作吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                byte relayID = 0;
                bool OnOff = false;
                if (button16.Text == "打开")
                {
                    relayID = 0x05;
                    OnOff = true;
                    button16.Text = "关闭";
                }
                else if (button16.Text == "关闭")
                {
                    relayID = 0x05;
                    OnOff = false;
                    button16.Text = "打开";
                }
                relayStateSend(relayID, OnOff);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定进行操作吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
            int CntTimeDay, CntTimeHour, CntTimeMin, CntTimeSec;
            if (NetDebugForm.hasOpen)
            {
                Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                byte[] sendbytes = new byte[17];
                CntTimeDay = int.Parse(textBox65.Text);
                CntTimeHour = int.Parse(textBox66.Text);
                CntTimeMin = int.Parse(textBox69.Text);
                CntTimeSec = int.Parse(textBox70.Text);


                sendbytes[0] = 0x2e;
                sendbytes[1] = 0x2e;
                sendbytes[2] = 0x00;
                sendbytes[3] = 0x11;
                sendbytes[4] = 0x0a;
                sendbytes[5] = (byte)(CntTimeDay / 1000);
                sendbytes[6] = (byte)(CntTimeDay % 1000 / 100);
                sendbytes[7] = (byte)(CntTimeDay % 100 / 10);
                sendbytes[8] = (byte)(CntTimeDay % 10 / 1);
                sendbytes[9] = (byte)(CntTimeHour % 100 / 10);
                sendbytes[10] = (byte)(CntTimeHour % 10 / 1);
                sendbytes[11] = (byte)(CntTimeMin % 100 / 10);
                sendbytes[12] = (byte)(CntTimeMin % 10 / 1);
                sendbytes[13] = (byte)(CntTimeSec % 100 / 10);
                sendbytes[14] = (byte)(CntTimeSec % 10 / 1);
                sendbytes[15] = 0x2f;
                sendbytes[16] = 0x2f;

                foreach (int conn in dictsocket.Keys)
                    try
                    {
                        //检测客户端Socket的状态
                        if (dictsocket[conn].Connected)
                        {
                            dictsocket[conn].Send(sendbytes);
                          
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
            }
            }
         
        }

        private void textBox65_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {

                MessageBox.Show("只能输入数字");
                e.Handled = true;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定进行操作吗？", "询问", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                if (NetDebugForm.hasOpen)
                {
                    Dictionary<int, Socket> dictsocket = NetDebugForm.getInstance().getDictionary();
                    byte[] sendbytes = new byte[8];
                    if (textBox67.Text == "")
                    {
                        MessageBox.Show("请输入周期");
                        return;
                    }
                    byte t = (byte)int.Parse(textBox67.Text);

                       #region  写入数据库
                        OleDbConnection con = new OleDbConnection(Program.ConStr);
                        int cycle; //模拟数据保存周期
                        cycle = t;


                        try
                        {
                            con.Open();
                            if (con.State == ConnectionState.Open)
                            {
                                string tx = "insert into cycle values('"
                                    + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToLongTimeString().ToString() + "','"
                                    + cycle + "')";
                                OleDbCommand cmd = new OleDbCommand(tx, con);
                                cmd.ExecuteNonQuery();
                                cmd.Dispose();
                            }
                        }
                        catch (SystemException ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                        finally
                        {
                            con.Close();
                        }
                 
                    #endregion


                    //sMessageBox.Show(button19.Text + "19" + button18.Text + "18" + button17.Text + "17" + button16.Text + "16" + button15.Text + "15" + button12.Text + "12");
                    sendbytes[0] = 0x2e;
                    sendbytes[1] = 0x2e;
                    sendbytes[2] = 0x00;
                    sendbytes[3] = 0x08;
                    sendbytes[4] = 0x08;
                    sendbytes[5] = t;
                    sendbytes[6] = 0x2f;
                    sendbytes[7] = 0x2f;

                    foreach (int conn in dictsocket.Keys)
                        try
                        {
                            //检测客户端Socket的状态
                            if (dictsocket[conn].Connected)
                            {
                                dictsocket[conn].Send(sendbytes);

                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                }
            }

        }

        private void textBox67_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {

                MessageBox.Show("只能输入数字");
                e.Handled = true;
            }
        }

    

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                textBox5.MaxLength = 7;

                textBox5.Text = float.Parse(textBox5.Text).ToString("0.0000").PadRight(4);
            }
            if (radioButton3.Checked)
            {
                textBox5.MaxLength = 9;
                textBox5.Text = float.Parse(textBox5.Text).ToString("0.000000").PadRight(4);
            }
        }

        private void plot4_Click(object sender, EventArgs e)
        {

        }

       

     

      

      

      

     

        
        

      

       

      


       

       

       

       
    }
}
