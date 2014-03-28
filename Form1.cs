using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ShareDrive
{
    public partial class shareDrive : Form
    {
        public shareDrive()
        {
            InitializeComponent();
        }

        [StructLayout(LayoutKind.Sequential)]
        public class NETRESOURCE
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }
        [DllImport("mpr.dll")]
        public static extern int WNetAddConnection2(NETRESOURCE netResource, string password, string username, int flags);

        private void Form1_Load(object sender, EventArgs e)
        {  
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NETRESOURCE myResource = new NETRESOURCE();
            myResource.dwScope = 0;
            myResource.dwType = 0;
            myResource.dwDisplayType = 0;
            myResource.LocalName = "h:";
            myResource.RemoteName = @"\\192.168.1.192\testShare";
            myResource.dwUsage = 0;
            myResource.Comment = "";
            myResource.Provider = "";

            int returnValue = WNetAddConnection2(myResource, password.Text, username.Text, 0);
            MessageBox.Show(returnValue.ToString());
        }
    }
}