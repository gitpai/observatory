using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace 上位机服务器
{
    public sealed partial class NetDebugForm : Form
    {
        #region 类变量定义
        private struct canshu
        {
            public int counter;                                                     //..............
        }
        public static bool hasOpen = false;
        private static int MAXSIZE = 100;
        private Socket hostSocket = null;                                           //负责监听客户端段连接请求的套接字
        private Thread[] threadMsg = new Thread[MAXSIZE];
        private Thread threadWatch = null;                                          //负责调用套接字，执行监听请求的线程
        private bool isWatch = true;
        private uint clientCounter = 0;                                             //已连接的客户端计数
        private int dictCounter = 0;                                                //dictionary里的有效成员计数 dictCounter作为dictionary的唯一标识
        private Socket[] clientarry = new Socket[MAXSIZE];                          //客户端列表
        Dictionary<int, Socket> dictsocket = new Dictionary<int, Socket>();
        private int[] lengthSum = new int[MAXSIZE];                                 //单个客户端的收到数据的总字节长度
        private bool[] isRec = new bool[MAXSIZE];                                   //客户端接收数据线程标识
        private bool[] FlagEnd = new bool[MAXSIZE];                                 //接收数据结束标识
        private string[] SaveTime = new string[MAXSIZE];                            //保存的时间
        private int ccflag = 0;
        private int SaveIndex = -1;
        #endregion

        //设置为单例模式
        private static NetDebugForm instance = null;
        private static readonly object padlock = new object();
        MainForms referenceToMainForm = null;
        public NetDebugForm(MainForms mf){
         InitializeComponent();
         referenceToMainForm = mf;
    }
        private NetDebugForm()
        {
            InitializeComponent();
            net_tb_localport.Text = SystemConfig.GetConfigData("LocalPort", string.Empty);
            net_tb_localip.Text = SystemConfig.GetConfigData("LocalIP", string.Empty);
        }
        public static NetDebugForm getInstance()
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new NetDebugForm();
                }
                return instance;
            }
        }

        //返回Dictionary<int, Socket> dictsocket
        public Dictionary<int, Socket> getDictionary()
        {
            return dictsocket;
        }


        #region 按钮点击事件
        // 网络调试：打开服务器 
        private void net_bt_opennet_Click(object sender, EventArgs e)
        {
           
            //检查IP地址输入格式是否正确
            String regx = @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))";
            System.Text.RegularExpressions.Regex check = new System.Text.RegularExpressions.Regex(regx);
            if (!check.IsMatch(net_tb_localip.Text))
            {
                MessageBox.Show("本地IP地址输入错误，请重新输入！", "警告");
                return;
            }
            //检查端口号是否为空
            if (net_tb_localport.Text.Equals(""))
            {
                MessageBox.Show("请输入本地端口号！", "警告");
                return;
            }
            //建立Socket连接
            try
            {
                if (hostSocket == null)
                {
                  
                    hostSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                  
                    hostSocket.Bind(new IPEndPoint(IPAddress.Parse(net_tb_localip.Text), Convert.ToInt32(net_tb_localport.Text)));
                    //设置 监听队列 长度为50(同时能够处理 50个连接请求)
                  
                    hostSocket.Listen(50);
                    showTip1.Text = "服务器已启动，开始监听";
                   
                    //开始接受客户端连接请求
                    //HostSocket.BeginAccept(new AsyncCallback(ClientAccepted), HostSocket);
                    isWatch = true;
                   
                    threadWatch = new Thread(ClientAccepted0);
                    threadWatch.IsBackground = true;
                    threadWatch.Start();

                    showTip1.Text = "服务器已启动，开始接收数据";
                    showTip2.Text = "建立连接：0";
                    net_bt_opennet.Enabled = false;
                    net_bt_closenet.Enabled = true;

                    SystemConfig.WriteConfigData("LocalPort", net_tb_localport.Text.Trim());
                    SystemConfig.WriteConfigData("LocalIP", net_tb_localip.Text.Trim());
                    net_tb_localip.Enabled = false;
                    net_tb_localport.Enabled = false;
                    net_btnSend.Enabled = true;
                    net_btnSendAll.Enabled = true;
                    net_tx_send.Enabled = true;

                    hasOpen = true;
                    MainForms.ServerChange = true;
                   
                }
            }
            catch (Exception)
            {
                MessageBox.Show("本地IP地址错误或端口已被占用！", "警告");
                return; 
            }
        }

        // 网络调试：关闭服务器
        private void net_bt_closenet_Click(object sender, EventArgs e)
        {

           
            MainForms.ServerChange = true;
            hasOpen = false;
          

                try
                {
                         int[] a = new int[MAXSIZE];
                             int counter = 0;
                             isWatch = false;
                          isWatch = false;
                    foreach (int conn in dictsocket.Keys)//关闭线程
                    {

                        isRec[conn] = false;
                        if (threadMsg[conn].IsAlive) threadMsg[conn].Abort();
                        //从list中移除选中的选项
                        net_listBox1.Items.Clear();
                        //从tab中移除
                        //tabControl1.TabPages.Clear();//;
                        //从dictionary中移除选中项
                        dictsocket[conn].Close();
                        //dictsocket.Remove(conn);
                        a[conn] = conn;
                        counter = conn;
                    }

                    for (int k = 1; k <= counter; k++)
                    {
                        if (a[k] == k)
                        {
                            dictsocket.Remove(k);
                        }
                    }

                    //关闭客户端Socket,清理资源
                    if (!threadWatch.IsAlive)//关闭监听线程
                        threadWatch.Abort();

                    hostSocket.Close();
                    hostSocket = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    clientCounter = 0;

                    showTip1.Text = "点击启动服务器即可.";
                    showTip2.Text = "服务器停止监听.";



                    net_bt_opennet.Enabled = true;
                    net_bt_closenet.Enabled = false;
                    net_tb_localip.Enabled = true;
                    net_tb_localport.Enabled = true;
                    net_btnSend.Enabled = false;
                    net_btnSendAll.Enabled = false;
                    net_tx_send.Enabled = false;
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show("关闭无效！" + ex.ToString());
                }
        }

        // 网络调试：移除连接
        private void net_btn_remove_Click(object sender, EventArgs e)
        {
             if (!string.IsNullOrEmpty(net_listBox1.Text))
            {
                try
                {
                    //从dictionary中移除选中项
                    string end = ":";
                    int length = net_listBox1.Text.IndexOf(end);
                    int a = int.Parse(net_listBox1.Text.Substring(0, length));

                    isRec[a] = false;
                    if (threadMsg[a].IsAlive) threadMsg[a].Abort();
                    dictsocket[a].Close();
                    dictsocket.Remove(a);
                    //从list中移除选中的选项
                    net_listBox1.Items.Remove(net_listBox1.Text);
                    //从tab中移除
                    // tabControl1.Controls.RemoveByKey("tabpage" + a);
                    //tabControl1.TabPages.Remove(pagearry[a]);
                    clientCounter--;
                    showTip2.Text = "建立连接." + clientCounter;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("请选择要移除的对象.");
            }
        }

        // 网络调试：清除所有
        private void net_btn_delete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(net_listBox1.Text))
            {
                try
                {
                    //从dictionary中移除选中项
                    string end = ":";
                    int length = net_listBox1.Text.IndexOf(end);
                    int a = int.Parse(net_listBox1.Text.Substring(0, length));

                    isRec[a] = false;
                    if (threadMsg[a].IsAlive) threadMsg[a].Abort();
                    dictsocket[a].Close();
                    dictsocket.Remove(a);
                    //从list中移除选中的选项
                    net_listBox1.Items.Remove(net_listBox1.Text);
                    //从tab中移除
                    // tabControl1.Controls.RemoveByKey("tabpage" + a);
                    //tabControl1.TabPages.Remove(pagearry[a]);
                    clientCounter--;
                    showTip2.Text = "建立连接." + clientCounter;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("请选择要移除的对象.");
            }
        }

        // 网络调试: 选中列表事件
        private void net_listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SaveIndex != net_listBox1.SelectedIndex && net_listBox1.SelectedIndex != -1)
            {
                SaveIndex = net_listBox1.SelectedIndex;
                string end = ":";
                int length = net_listBox1.Text.IndexOf(end);
                int a = int.Parse(net_listBox1.Text.Substring(0, length));
                //点击客户端显示相对应的tab
                //tabControl1.SelectTab("tabpage" + a);
            }
        }

        // 网络调试: 清空接收框
        private void net_link_clearreceived_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            net_lb_received.Clear();
        }

        // 网络调试: 清空发送框
        private void net_link_clearSend_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            net_tx_send.Clear();
        }

        // 网络调试： 16进制
        private void net_cb_sendhex_CheckedChanged(object sender, EventArgs e)
        {
            net_tx_send.Text = net_tx_send.Text.Replace(" ", "");
            if (!net_tx_send.Text.Equals(""))
            {
                if (net_cb_sendhex.Checked)
                {
                    StringBuilder stringBuilder = new StringBuilder(net_tx_send.Text.Length * 2);
                    for (int i = 0; i < net_tx_send.Text.Length; i++)
                    {
                        stringBuilder.Append(((int)net_tx_send.Text[i]).ToString("X2") + " ");//x2是小写
                    }
                    net_tx_send.Text = stringBuilder.ToString();
                }
                else
                {
                    if (net_tx_send.Text.Length % 2 == 0)
                    {
                        try
                        {
                            String result = "";
                            for (int i = 0; i < net_tx_send.Text.Length / 2; i++)
                            {
                                int value = Convert.ToInt32(net_tx_send.Text.Substring(2 * i, 2), 16);
                                char charValue = (char)value;
                                result += charValue.ToString();
                            }
                            net_tx_send.Text = result;
                        }
                        catch (SystemException)
                        {
                            net_tx_send.Text = "";
                        }
                    }
                    else
                    {
                        net_tx_send.Text = "";
                    }
                }
            }
        }

        // 网络调试: 文本框输入文字
        private void net_tx_send_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (net_cb_sendhex.Checked)
            {
                e.Handled = "0123456789ABCDEF ".IndexOf(char.ToUpper(e.KeyChar)) < 0;
            }
        }

        // 网络调试： 发送数据
        private void net_btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(net_tx_send.Text))
            {
                MessageBox.Show("发送字节为空.");
            }
            else
            {
                if (net_listBox1.SelectedIndex != -1)
                {
                    string end = ":";
                    int length = net_listBox1.Text.IndexOf(end);
                    int a = int.Parse(net_listBox1.Text.Substring(0, length));
                    //检测客户端Socket的状态
                    if (dictsocket[a].Connected)
                    {
                        byte[] sendbytes;
                        if (net_cb_sendhex.Checked)
                        {
                            string message = net_tx_send.Text.Trim();
                            message = message.Replace(" ", "");
                            if (message.Length % 2 != 0) return;
                            sendbytes = new byte[message.Length / 2];
                            for (int i = 0; i < message.Length; i += 2)
                            {
                                sendbytes[i / 2] = Convert.ToByte(message.Substring(i, 2), 16);
                            }
                            dictsocket[a].Send(sendbytes);
                        }
                        else
                        {
                            dictsocket[a].Send(Encoding.ASCII.GetBytes(net_tx_send.Text.Trim()));
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择客户端.");
                }
            }
        }

        // 网络调试：数据群发
        private void net_btnSendAll_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(net_tx_send.Text))
            {
                foreach (int conn in dictsocket.Keys)
                    try
                    {
                        //检测客户端Socket的状态
                        if (dictsocket[conn].Connected)
                        {
                            byte[] sendbytes;
                            if (net_cb_sendhex.Checked)
                            {
                                string message = net_tx_send.Text.Trim();
                                message = message.Replace(" ", "");
                                if (message.Length % 2 != 0) return;
                                sendbytes = new byte[message.Length / 2];
                                for (int i = 0; i < message.Length; i += 2)
                                {
                                    sendbytes[i / 2] = Convert.ToByte(message.Substring(i, 2), 16);
                                }
                                dictsocket[conn].Send(sendbytes);
                            }
                            else
                            {
                                dictsocket[conn].Send(Encoding.ASCII.GetBytes(net_tx_send.Text.Trim()));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
            }
            else
            {
                MessageBox.Show("请输入发送的数据.");
            }
        }

        // 网络调试: 调试
        private void net_cb_debug_CheckedChanged(object sender, EventArgs e)
        {
            if (net_cb_debug.Checked)
            {
                net_cb_autowrap.Checked = false;
                net_cb_reveivedhex.Enabled = false;
                net_cb_autowrap.Enabled = false;
                return;
            }
            net_cb_reveivedhex.Enabled = true;
            net_cb_autowrap.Enabled = true;
        }

        #endregion



        #region 数据接收/客户端连接
        /**
        * 有客户端连接
        **/
        public void ClientAccepted(IAsyncResult ar)
        {
            
            if (hostSocket == null)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
               
            }
            else
            {
                var sokWatch = ar.AsyncState as Socket;
                //这就是客户端的Socket实例，我们后续可以将其保存起来
                var client = sokWatch.EndAccept(ar);
                dictCounter++;//从1开始标识
                clientarry[dictCounter] = client;
                //将client保存到dictionary
                dictsocket.Add(dictCounter, client);
                //将client显示在listbox中
                net_listBox1.Items.Add(dictCounter + ":" + client.RemoteEndPoint);
                //成功连接的客户端数量显示在状态栏
                clientCounter++;
                
                showTip1.Text = "建立连接." + clientCounter;
                //接收客户端的消息
                threadMsg[dictCounter] = new Thread(ReceiveMessage0);
                canshu aa = new canshu();
                isRec[dictCounter] = true;
                aa.counter = dictCounter;
                threadMsg[dictCounter].IsBackground = true;
                threadMsg[dictCounter].Start((object)aa);
                //准备接受下一个客户端请求
                sokWatch.BeginAccept(new AsyncCallback(ClientAccepted), sokWatch);
            }
        }

        /**
        * 检测是否收到数据/客户端掉线
        **/
        public void ReceiveMessage0(object o)
        {
          
            canshu aa = (canshu)o;
            SaveTime[aa.counter] = System.DateTime.Now.ToString("HHmmss");
            while (isRec[aa.counter])
            {
                if (!dictsocket[dictCounter].Connected)
                {
                    isRec[aa.counter] = false;
                    //从list中移除选中的选项
                    net_listBox1.Items.Remove(aa.counter + ":" + clientarry[aa.counter].RemoteEndPoint);
                    //从dictionary中移除选中项
                    dictsocket[aa.counter].Close();
                    dictsocket.Remove(aa.counter);
                    clientCounter--;
                    showTip1.Text = "建立连接." + clientCounter;
                }
                else
                {
                    try
                    {
                        byte[] arrMsg = new byte[1024 * 1024];
                        //接收对应客户端发来的消息
                        int length = dictsocket[aa.counter].Receive(arrMsg);
                        ccflag++;
                        //保存原始数据
                        FlagEnd[aa.counter] = SaveTxt(arrMsg, length, aa.counter);
                        if (FlagEnd[aa.counter]) //结束标识
                        {
                            SaveTime[aa.counter] = System.DateTime.Now.ToString("HHmmss");
                        }
                        lengthSum[aa.counter] += length;
                        //将接收到的消息数组里真实消息转成字符串（关键函数）
                        massageDeal(arrMsg, length);
                    }
                    catch
                    {
                        if (isRec[aa.counter])
                        {
                            isRec[aa.counter] = false;
                            net_listBox1.Items.Remove(aa.counter + ":" + dictsocket[aa.counter].RemoteEndPoint);
                            dictsocket[aa.counter].Close();
                            dictsocket.Remove(aa.counter);
                            clientCounter--;
                            showTip2.Text = "建立连接." + clientCounter;
                        }
                    }
                }
            }
        }

        /**
         * 客户端接入服务器
         **/
        public void ClientAccepted0()
        {
        
            while (isWatch)
            {
                if (!isWatch){
                    GC.WaitForPendingFinalizers();
                   
                }
                   
                 
                else
                {
                 
                    //这就是客户端的Socket实例，我们后续可以将其保存起来


                    try
                    {
                        var client = hostSocket.Accept();




                        dictCounter++;//从1开始标识
                        clientarry[dictCounter] = client;

                        //将client保存到dictionary
                        dictsocket.Add(dictCounter, client);

                        //将client显示在listbox中
                        net_listBox1.Items.Add(dictCounter + ":" + client.RemoteEndPoint);

                        //将连接的客户端创建tabpage 
                        //d = new addDelegate(CreatTabpage);
                        // Thread t = new Thread(new ThreadStart(add));
                        //t.Start();

                        //成功连接的客户端数量显示在状态栏
                        clientCounter++;
                        showTip2.Text = "建立连接." + clientCounter;

                        //接收客户端的消息
                        threadMsg[dictCounter] = new Thread(ReceiveMessage0);
                        canshu aa = new canshu();
                        isRec[dictCounter] = true;
                        aa.counter = dictCounter;
                        threadMsg[dictCounter].IsBackground = true;
                        threadMsg[dictCounter].Start((object)aa);
                    }
                    catch {
                        return;
                    }
                }
            }
        }

        /**
         * 数据存储
         **/
        public bool SaveTxt(byte[] sContent, Int32 slength, int cc)
        {
            
            if (sContent[slength - 4] == 0x0d && sContent[slength - 3] == 0x0a)
            {
                return true;
            }
            else return false;
        } 
        #endregion 

        #region 自定义函数:数据存储massageDeal+调试Debug
        /**
         * 数据存储
         **/
        void massageDeal(byte[] arrMsg, int length)
        {
            if (net_cb_stopreceive.Checked) return;
            if (net_cb_debug.Checked)
            {
                
                Debug(arrMsg, length);
            }
            else
            {
                //十六进制接收数据
                if (net_cb_reveivedhex.Checked)
                {
                    string strMsg = "";
                    for (int i = 0; i < length; i++)
                    {
                        strMsg += string.Format("{0:X2} ", arrMsg[i]);             //字符串添加
                    }

                    net_lb_received.AppendText(strMsg);
                    if (net_cb_autowrap.Checked) net_lb_received.AppendText("\n");
                }//字符串形式接收数据
                else
                {
                    string strMsg = System.Text.Encoding.Default.GetString(arrMsg, 0, length);
                    net_lb_received.AppendText(strMsg);
                    if (net_cb_autowrap.Checked) net_lb_received.AppendText("\n");
                }
            }
        }

        /**
         * 调试
         **/
        void Debug(byte[] arrMsg, int length)
        {
           
            OleDbConnection con = new OleDbConnection(Program.ConStr);
            #region 总协议获取
            if (arrMsg[0] == 0x2E || arrMsg[1] == 0x2E || arrMsg[2] == 0x00 || arrMsg[3] == 0x7A || arrMsg[120] == 0x2F || arrMsg[121] == 0x2F) { 
            
            // 电压页
            float V1 = BitConverter.ToSingle(arrMsg, 4);
            float V2 = BitConverter.ToSingle(arrMsg, 8);
            float V3 = BitConverter.ToSingle(arrMsg, 12);
            float V4 = BitConverter.ToSingle(arrMsg, 16);
            float CFV = BitConverter.ToSingle(arrMsg, 20);
            float OSCV = BitConverter.ToSingle(arrMsg, 24);
            float OSCI = BitConverter.ToSingle(arrMsg, 28);
            // 离子泵页
            float IONV = BitConverter.ToSingle(arrMsg, 32);
            float IONI = BitConverter.ToSingle(arrMsg, 36);
            float FLUX = BitConverter.ToSingle(arrMsg, 40);
            float FLR = BitConverter.ToSingle(arrMsg, 44);
            // 恒温页
            float THB = BitConverter.ToSingle(arrMsg, 48);
            float THC = BitConverter.ToSingle(arrMsg, 52);
            float THD = BitConverter.ToSingle(arrMsg, 56);
            float NECK = BitConverter.ToSingle(arrMsg, 60);
            float OVN1 = BitConverter.ToSingle(arrMsg, 64);
            float OVN2 = BitConverter.ToSingle(arrMsg, 68);
            float ISOL = BitConverter.ToSingle(arrMsg, 72);
            // 接收机页
            float IFL = BitConverter.ToSingle(arrMsg, 76);
            float TUNE = BitConverter.ToSingle(arrMsg, 80);
            float DIO = BitConverter.ToSingle(arrMsg, 84);
            // 隔离器页
            float CH1 = BitConverter.ToSingle(arrMsg, 88);
            float CH2 = BitConverter.ToSingle(arrMsg, 92);
            float CH3 = BitConverter.ToSingle(arrMsg, 96);
            float CH4 = BitConverter.ToSingle(arrMsg, 100);
            float CH5 = BitConverter.ToSingle(arrMsg, 104);
            float CH6 = BitConverter.ToSingle(arrMsg, 108);
            float CH7 = BitConverter.ToSingle(arrMsg, 112);
            float CH8 = BitConverter.ToSingle(arrMsg, 116);
           
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    string tx = "insert into sensor values('"
                        + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToLongTimeString().ToString() + "','"
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
            if (arrMsg[0] == 0x2E || arrMsg[1] == 0x2E || arrMsg[2] == 0x00 || arrMsg[3] == 0x0B || arrMsg[4] == 0x02 || arrMsg[9] == 0x2F || arrMsg[10] == 0x2F) {
                MessageBox.Show("秒信号");
          
           int sspwValue= BitConverter.ToInt32(arrMsg, 5);//秒信号脉宽

           referenceToMainForm.textBox4.Text = sspwValue+"";
         
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    string tx = "insert into sspw values('"
                        + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToLongTimeString().ToString() + "','"
                        + sspwValue + "')";
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

            if (arrMsg[0] == 0x2E || arrMsg[1] == 0x2E || arrMsg[2] == 0x00 || arrMsg[3] == 0x0D || arrMsg[4] == 0x04 || arrMsg[9] == 0x2F || arrMsg[10] == 0x2F)
            {
                MessageBox.Show("综合频率");


                  int  sign;
                 float sys_zong;
                if (arrMsg[5] == 0x00)
                {
                    sign = 1;
                }
                else {
                    sign = -1;
                }
               
                if (arrMsg[6] == 0x02)
                {
                      //6位精度
                    sys_zong = float.Parse((sign * BitConverter.ToSingle(arrMsg, 7)).ToString("0.000000").PadRight(4));
                }
                else
                {
                    sys_zong = float.Parse((sign * BitConverter.ToSingle(arrMsg, 7)).ToString("0.000000").PadRight(4));
                  





                }
               //综合器频率
                referenceToMainForm.textBox5.Text = sys_zong + "";
                try
                {
                    con.Open();
                    if (con.State == ConnectionState.Open)
                    {
                        string tx = "insert into syszong values('"
                            + DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " + DateTime.Now.ToLongTimeString().ToString() + "','"
                            + sys_zong + "')";
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
            if (arrMsg[0] == 0x2E || arrMsg[1] == 0x2E || arrMsg[2] == 0x00 || arrMsg[3] == 0x08 || arrMsg[4] == 0x08 || arrMsg[6] == 0x2F || arrMsg[7] == 0x2F)
            {
                MessageBox.Show("模拟数据周期");

                int cycle; //模拟数据保存周期
                cycle = arrMsg[5];
                referenceToMainForm.textBox67.Text = cycle + "";

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
            }

        }
        #endregion

        // 关闭窗口
        private void NetDebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
         
            //MessageBox.Show("本地IP地址输入错误，请重新输入！", "警告");
            NetDebugForm netDebugger = NetDebugForm.getInstance();
            netDebugger.Hide();
            e.Cancel = true;
        }

        private void net_groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void net_tb_localip_TextChanged(object sender, EventArgs e)
        {

        }

        private void showTip1_Click(object sender, EventArgs e)
        {

        }

       


    }
}
        #endregion