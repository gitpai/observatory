using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace 上位机服务器
{
    static class Program
    {
        public static bool Login = false;
        public static string ConStr = "Provider=Microsoft.jet.OLEDB.4.0;Data source=" + Application.StartupPath + @"\Database.mdb";
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //LoginForm loginForm = new LoginForm();
            //loginForm.ShowDialog();
            //if (Login == true)
            //{
            //    Application.Run(new MainForms());
            //}
            Application.Run(new MainForms());
        }
    }
}
