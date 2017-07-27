namespace 上位机服务器
{
    partial class NetDebugForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            foreach (int conn in dictsocket.Keys)
            {
                if (threadMsg[conn].IsAlive) threadMsg[conn].Abort();
            }
            
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.GroupBox net_groupBox1;
            this.net_tb_localip = new System.Windows.Forms.TextBox();
            this.net_bt_closenet = new System.Windows.Forms.Button();
            this.net_bt_opennet = new System.Windows.Forms.Button();
            this.net_tb_localport = new System.Windows.Forms.TextBox();
            this.net_la_port = new System.Windows.Forms.Label();
            this.net_la_IP_adress = new System.Windows.Forms.Label();
            this.net_lb_received = new System.Windows.Forms.TextBox();
            this.net_tx_send = new System.Windows.Forms.TextBox();
            this.net_link_clearSend = new System.Windows.Forms.LinkLabel();
            this.net_cb_sendhex = new System.Windows.Forms.CheckBox();
            this.net_link_clearreceived = new System.Windows.Forms.LinkLabel();
            this.net_cb_stopreceive = new System.Windows.Forms.CheckBox();
            this.net_cb_autowrap = new System.Windows.Forms.CheckBox();
            this.net_cb_debug = new System.Windows.Forms.CheckBox();
            this.net_cb_reveivedhex = new System.Windows.Forms.CheckBox();
            this.net_groupBox3 = new System.Windows.Forms.GroupBox();
            this.net_btnSendAll = new System.Windows.Forms.Button();
            this.net_btnSend = new System.Windows.Forms.Button();
            this.net_btn_remove = new System.Windows.Forms.Button();
            this.net_listBox1 = new System.Windows.Forms.ListBox();
            this.net_btn_delete = new System.Windows.Forms.Button();
            this.net_groupBox2 = new System.Windows.Forms.GroupBox();
            this.showTip1 = new System.Windows.Forms.Label();
            this.showTip2 = new System.Windows.Forms.Label();
            net_groupBox1 = new System.Windows.Forms.GroupBox();
            net_groupBox1.SuspendLayout();
            this.net_groupBox3.SuspendLayout();
            this.net_groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // net_groupBox1
            // 
            net_groupBox1.BackColor = System.Drawing.Color.Transparent;
            net_groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            net_groupBox1.Controls.Add(this.net_tb_localip);
            net_groupBox1.Controls.Add(this.net_bt_closenet);
            net_groupBox1.Controls.Add(this.net_bt_opennet);
            net_groupBox1.Controls.Add(this.net_tb_localport);
            net_groupBox1.Controls.Add(this.net_la_port);
            net_groupBox1.Controls.Add(this.net_la_IP_adress);
            net_groupBox1.Font = new System.Drawing.Font("华文楷体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            net_groupBox1.Location = new System.Drawing.Point(39, 39);
            net_groupBox1.Name = "net_groupBox1";
            net_groupBox1.Size = new System.Drawing.Size(271, 180);
            net_groupBox1.TabIndex = 31;
            net_groupBox1.TabStop = false;
            net_groupBox1.Text = "网络设置";
            net_groupBox1.Enter += new System.EventHandler(this.net_groupBox1_Enter);
            // 
            // net_tb_localip
            // 
            this.net_tb_localip.Location = new System.Drawing.Point(33, 53);
            this.net_tb_localip.Name = "net_tb_localip";
            this.net_tb_localip.Size = new System.Drawing.Size(209, 27);
            this.net_tb_localip.TabIndex = 6;
            this.net_tb_localip.Text = "111.186.98.14";
            this.net_tb_localip.TextChanged += new System.EventHandler(this.net_tb_localip_TextChanged);
            // 
            // net_bt_closenet
            // 
            this.net_bt_closenet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.net_bt_closenet.Enabled = false;
            this.net_bt_closenet.Font = new System.Drawing.Font("华文楷体", 10F);
            this.net_bt_closenet.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.net_bt_closenet.Location = new System.Drawing.Point(152, 143);
            this.net_bt_closenet.Margin = new System.Windows.Forms.Padding(1);
            this.net_bt_closenet.Name = "net_bt_closenet";
            this.net_bt_closenet.Size = new System.Drawing.Size(90, 28);
            this.net_bt_closenet.TabIndex = 5;
            this.net_bt_closenet.Text = "关闭服务器";
            this.net_bt_closenet.UseVisualStyleBackColor = true;
            this.net_bt_closenet.Click += new System.EventHandler(this.net_bt_closenet_Click);
            // 
            // net_bt_opennet
            // 
            this.net_bt_opennet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.net_bt_opennet.Font = new System.Drawing.Font("华文楷体", 10F);
            this.net_bt_opennet.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.net_bt_opennet.Location = new System.Drawing.Point(34, 142);
            this.net_bt_opennet.Margin = new System.Windows.Forms.Padding(1);
            this.net_bt_opennet.Name = "net_bt_opennet";
            this.net_bt_opennet.Size = new System.Drawing.Size(90, 28);
            this.net_bt_opennet.TabIndex = 4;
            this.net_bt_opennet.Text = "打开服务器";
            this.net_bt_opennet.UseVisualStyleBackColor = true;
            this.net_bt_opennet.Click += new System.EventHandler(this.net_bt_opennet_Click);
            // 
            // net_tb_localport
            // 
            this.net_tb_localport.Location = new System.Drawing.Point(34, 108);
            this.net_tb_localport.MaxLength = 5;
            this.net_tb_localport.Name = "net_tb_localport";
            this.net_tb_localport.Size = new System.Drawing.Size(209, 27);
            this.net_tb_localport.TabIndex = 3;
            this.net_tb_localport.Text = "8899";
            // 
            // net_la_port
            // 
            this.net_la_port.AutoSize = true;
            this.net_la_port.Location = new System.Drawing.Point(23, 84);
            this.net_la_port.Name = "net_la_port";
            this.net_la_port.Size = new System.Drawing.Size(120, 17);
            this.net_la_port.TabIndex = 2;
            this.net_la_port.Text = "（2）本地端口号";
            // 
            // net_la_IP_adress
            // 
            this.net_la_IP_adress.AutoSize = true;
            this.net_la_IP_adress.Location = new System.Drawing.Point(20, 26);
            this.net_la_IP_adress.Name = "net_la_IP_adress";
            this.net_la_IP_adress.Size = new System.Drawing.Size(118, 17);
            this.net_la_IP_adress.TabIndex = 0;
            this.net_la_IP_adress.Text = "（1）本地IP地址";
            // 
            // net_lb_received
            // 
            this.net_lb_received.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.net_lb_received.Location = new System.Drawing.Point(14, 71);
            this.net_lb_received.Multiline = true;
            this.net_lb_received.Name = "net_lb_received";
            this.net_lb_received.ReadOnly = true;
            this.net_lb_received.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.net_lb_received.Size = new System.Drawing.Size(211, 47);
            this.net_lb_received.TabIndex = 19;
            // 
            // net_tx_send
            // 
            this.net_tx_send.Font = new System.Drawing.Font("宋体", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.net_tx_send.Location = new System.Drawing.Point(11, 166);
            this.net_tx_send.Multiline = true;
            this.net_tx_send.Name = "net_tx_send";
            this.net_tx_send.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.net_tx_send.Size = new System.Drawing.Size(212, 36);
            this.net_tx_send.TabIndex = 18;
            this.net_tx_send.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.net_tx_send_KeyPress);
            // 
            // net_link_clearSend
            // 
            this.net_link_clearSend.AutoSize = true;
            this.net_link_clearSend.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.net_link_clearSend.Location = new System.Drawing.Point(91, 136);
            this.net_link_clearSend.Name = "net_link_clearSend";
            this.net_link_clearSend.Size = new System.Drawing.Size(68, 17);
            this.net_link_clearSend.TabIndex = 15;
            this.net_link_clearSend.TabStop = true;
            this.net_link_clearSend.Text = "清空发送";
            this.net_link_clearSend.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.net_link_clearSend_LinkClicked);
            // 
            // net_cb_sendhex
            // 
            this.net_cb_sendhex.AutoSize = true;
            this.net_cb_sendhex.Location = new System.Drawing.Point(14, 136);
            this.net_cb_sendhex.Name = "net_cb_sendhex";
            this.net_cb_sendhex.Size = new System.Drawing.Size(71, 21);
            this.net_cb_sendhex.TabIndex = 14;
            this.net_cb_sendhex.Text = "16进制";
            this.net_cb_sendhex.UseVisualStyleBackColor = true;
            this.net_cb_sendhex.CheckedChanged += new System.EventHandler(this.net_cb_sendhex_CheckedChanged);
            // 
            // net_link_clearreceived
            // 
            this.net_link_clearreceived.AutoSize = true;
            this.net_link_clearreceived.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.net_link_clearreceived.Location = new System.Drawing.Point(121, 45);
            this.net_link_clearreceived.Name = "net_link_clearreceived";
            this.net_link_clearreceived.Size = new System.Drawing.Size(68, 17);
            this.net_link_clearreceived.TabIndex = 5;
            this.net_link_clearreceived.TabStop = true;
            this.net_link_clearreceived.Text = "清空接收";
            this.net_link_clearreceived.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.net_link_clearreceived_LinkClicked);
            // 
            // net_cb_stopreceive
            // 
            this.net_cb_stopreceive.AutoSize = true;
            this.net_cb_stopreceive.Location = new System.Drawing.Point(14, 44);
            this.net_cb_stopreceive.Name = "net_cb_stopreceive";
            this.net_cb_stopreceive.Size = new System.Drawing.Size(87, 21);
            this.net_cb_stopreceive.TabIndex = 4;
            this.net_cb_stopreceive.Text = "停止接收";
            this.net_cb_stopreceive.UseVisualStyleBackColor = true;
            // 
            // net_cb_autowrap
            // 
            this.net_cb_autowrap.AutoSize = true;
            this.net_cb_autowrap.Enabled = false;
            this.net_cb_autowrap.Location = new System.Drawing.Point(164, 18);
            this.net_cb_autowrap.Name = "net_cb_autowrap";
            this.net_cb_autowrap.Size = new System.Drawing.Size(87, 21);
            this.net_cb_autowrap.TabIndex = 3;
            this.net_cb_autowrap.Text = "自动换行";
            this.net_cb_autowrap.UseVisualStyleBackColor = true;
            // 
            // net_cb_debug
            // 
            this.net_cb_debug.AutoSize = true;
            this.net_cb_debug.Checked = true;
            this.net_cb_debug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.net_cb_debug.Location = new System.Drawing.Point(11, 18);
            this.net_cb_debug.Name = "net_cb_debug";
            this.net_cb_debug.Size = new System.Drawing.Size(57, 21);
            this.net_cb_debug.TabIndex = 2;
            this.net_cb_debug.Text = "调试";
            this.net_cb_debug.UseVisualStyleBackColor = true;
            this.net_cb_debug.CheckedChanged += new System.EventHandler(this.net_cb_debug_CheckedChanged);
            // 
            // net_cb_reveivedhex
            // 
            this.net_cb_reveivedhex.AutoSize = true;
            this.net_cb_reveivedhex.Enabled = false;
            this.net_cb_reveivedhex.Location = new System.Drawing.Point(84, 18);
            this.net_cb_reveivedhex.Name = "net_cb_reveivedhex";
            this.net_cb_reveivedhex.Size = new System.Drawing.Size(71, 21);
            this.net_cb_reveivedhex.TabIndex = 1;
            this.net_cb_reveivedhex.Text = "16进制";
            this.net_cb_reveivedhex.UseVisualStyleBackColor = true;
            // 
            // net_groupBox3
            // 
            this.net_groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.net_groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.net_groupBox3.Controls.Add(this.net_lb_received);
            this.net_groupBox3.Controls.Add(this.net_tx_send);
            this.net_groupBox3.Controls.Add(this.net_link_clearSend);
            this.net_groupBox3.Controls.Add(this.net_btnSendAll);
            this.net_groupBox3.Controls.Add(this.net_cb_sendhex);
            this.net_groupBox3.Controls.Add(this.net_btnSend);
            this.net_groupBox3.Controls.Add(this.net_link_clearreceived);
            this.net_groupBox3.Controls.Add(this.net_cb_stopreceive);
            this.net_groupBox3.Controls.Add(this.net_cb_autowrap);
            this.net_groupBox3.Controls.Add(this.net_cb_debug);
            this.net_groupBox3.Controls.Add(this.net_cb_reveivedhex);
            this.net_groupBox3.Font = new System.Drawing.Font("华文楷体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.net_groupBox3.Location = new System.Drawing.Point(51, 388);
            this.net_groupBox3.Name = "net_groupBox3";
            this.net_groupBox3.Size = new System.Drawing.Size(248, 241);
            this.net_groupBox3.TabIndex = 35;
            this.net_groupBox3.TabStop = false;
            this.net_groupBox3.Text = "调试/测试";
            // 
            // net_btnSendAll
            // 
            this.net_btnSendAll.Location = new System.Drawing.Point(134, 208);
            this.net_btnSendAll.Name = "net_btnSendAll";
            this.net_btnSendAll.Size = new System.Drawing.Size(91, 23);
            this.net_btnSendAll.TabIndex = 25;
            this.net_btnSendAll.Text = "数据群发";
            this.net_btnSendAll.UseVisualStyleBackColor = true;
            this.net_btnSendAll.Click += new System.EventHandler(this.net_btnSendAll_Click);
            // 
            // net_btnSend
            // 
            this.net_btnSend.Location = new System.Drawing.Point(9, 208);
            this.net_btnSend.Name = "net_btnSend";
            this.net_btnSend.Size = new System.Drawing.Size(95, 23);
            this.net_btnSend.TabIndex = 12;
            this.net_btnSend.Text = "发送数据";
            this.net_btnSend.UseVisualStyleBackColor = true;
            this.net_btnSend.Click += new System.EventHandler(this.net_btnSend_Click);
            // 
            // net_btn_remove
            // 
            this.net_btn_remove.Location = new System.Drawing.Point(32, 86);
            this.net_btn_remove.Name = "net_btn_remove";
            this.net_btn_remove.Size = new System.Drawing.Size(95, 23);
            this.net_btn_remove.TabIndex = 27;
            this.net_btn_remove.Text = "移除连接";
            this.net_btn_remove.UseVisualStyleBackColor = true;
            this.net_btn_remove.Click += new System.EventHandler(this.net_btn_remove_Click);
            // 
            // net_listBox1
            // 
            this.net_listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.net_listBox1.BackColor = System.Drawing.SystemColors.Control;
            this.net_listBox1.FormattingEnabled = true;
            this.net_listBox1.HorizontalScrollbar = true;
            this.net_listBox1.ItemHeight = 17;
            this.net_listBox1.Location = new System.Drawing.Point(32, 30);
            this.net_listBox1.Name = "net_listBox1";
            this.net_listBox1.Size = new System.Drawing.Size(213, 21);
            this.net_listBox1.TabIndex = 22;
            this.net_listBox1.SelectedIndexChanged += new System.EventHandler(this.net_listBox1_SelectedIndexChanged);
            // 
            // net_btn_delete
            // 
            this.net_btn_delete.Location = new System.Drawing.Point(153, 87);
            this.net_btn_delete.Name = "net_btn_delete";
            this.net_btn_delete.Size = new System.Drawing.Size(91, 23);
            this.net_btn_delete.TabIndex = 26;
            this.net_btn_delete.Text = "清除所有";
            this.net_btn_delete.UseVisualStyleBackColor = true;
            this.net_btn_delete.Click += new System.EventHandler(this.net_btn_delete_Click);
            // 
            // net_groupBox2
            // 
            this.net_groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.net_groupBox2.Controls.Add(this.net_btn_remove);
            this.net_groupBox2.Controls.Add(this.net_listBox1);
            this.net_groupBox2.Controls.Add(this.net_btn_delete);
            this.net_groupBox2.Font = new System.Drawing.Font("华文楷体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.net_groupBox2.Location = new System.Drawing.Point(39, 272);
            this.net_groupBox2.Name = "net_groupBox2";
            this.net_groupBox2.Size = new System.Drawing.Size(270, 372);
            this.net_groupBox2.TabIndex = 34;
            this.net_groupBox2.TabStop = false;
            this.net_groupBox2.Text = "客户端信息";
            // 
            // showTip1
            // 
            this.showTip1.Location = new System.Drawing.Point(37, 227);
            this.showTip1.Name = "showTip1";
            this.showTip1.Size = new System.Drawing.Size(272, 15);
            this.showTip1.TabIndex = 36;
            this.showTip1.Click += new System.EventHandler(this.showTip1_Click);
            // 
            // showTip2
            // 
            this.showTip2.Location = new System.Drawing.Point(38, 251);
            this.showTip2.Name = "showTip2";
            this.showTip2.Size = new System.Drawing.Size(272, 15);
            this.showTip2.TabIndex = 37;
            // 
            // NetDebugForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 674);
            this.Controls.Add(this.showTip2);
            this.Controls.Add(this.showTip1);
            this.Controls.Add(this.net_groupBox3);
            this.Controls.Add(this.net_groupBox2);
            this.Controls.Add(net_groupBox1);
            this.Name = "NetDebugForm";
            this.Text = "网络调试助手";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NetDebugForm_FormClosing);
            net_groupBox1.ResumeLayout(false);
            net_groupBox1.PerformLayout();
            this.net_groupBox3.ResumeLayout(false);
            this.net_groupBox3.PerformLayout();
            this.net_groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox net_tb_localip;
        private System.Windows.Forms.Button net_bt_closenet;
        private System.Windows.Forms.Button net_bt_opennet;
        private System.Windows.Forms.TextBox net_tb_localport;
        private System.Windows.Forms.Label net_la_port;
        private System.Windows.Forms.Label net_la_IP_adress;
        private System.Windows.Forms.TextBox net_lb_received;
        private System.Windows.Forms.TextBox net_tx_send;
        private System.Windows.Forms.LinkLabel net_link_clearSend;
        private System.Windows.Forms.CheckBox net_cb_sendhex;
        private System.Windows.Forms.LinkLabel net_link_clearreceived;
        private System.Windows.Forms.CheckBox net_cb_stopreceive;
        private System.Windows.Forms.CheckBox net_cb_autowrap;
        private System.Windows.Forms.CheckBox net_cb_debug;
        private System.Windows.Forms.CheckBox net_cb_reveivedhex;
        private System.Windows.Forms.GroupBox net_groupBox3;
        private System.Windows.Forms.Button net_btnSendAll;
        private System.Windows.Forms.Button net_btnSend;
        private System.Windows.Forms.Button net_btn_remove;
        private System.Windows.Forms.ListBox net_listBox1;
        private System.Windows.Forms.Button net_btn_delete;
        private System.Windows.Forms.GroupBox net_groupBox2;
        private System.Windows.Forms.Label showTip1;
        private System.Windows.Forms.Label showTip2;
    }
}