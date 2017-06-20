using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace 上位机服务器
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.TopMost = true;
        }

        private void bt_cancel_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void bt_login_Click(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection(Program.ConStr);
            OleDbDataAdapter oleAdapter = new OleDbDataAdapter();
            try
            {
                con.Open();
                if (con.State == ConnectionState.Open)
                {
                    string tx = "Select * from [users] where user='" + tx_user.Text + "' and password='" + tx_password.Text + "'";
                    OleDbCommand sqlCmd = new OleDbCommand(tx, con);
                    oleAdapter.SelectCommand = sqlCmd;
                    DataSet ds = new DataSet();
                    oleAdapter.Fill(ds);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Program.Login = true;
                        System.Threading.Thread.Sleep(100);
                        this.Close();
                    }
                    else
                    {
                        tx_user.Clear();
                        tx_password.Clear();
                        System.Threading.Thread.Sleep(100);
                        MessageBox.Show("用户名或密码不正确！");
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

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
