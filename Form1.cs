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

        // Import WNetAddConnection2 from mpr.dll
        [DllImport("mpr.dll")]
        public static extern int WNetAddConnection2(NETRESOURCE netResource, string password, string username, int flags);

        // Import WNetCancelConnection2 from mpr.dll
        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2
            (string sLocalName, uint iFlags, int iForce);

        private void bnLogin_Click(object sender, EventArgs e)
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

            // Try the H: Drive
            int hReturnValue = WNetAddConnection2(myResource, password.Text, username.Text, 0);

            // Try the S: Drive
            myResource.LocalName = "s:";
            myResource.RemoteName = @"\\192.168.1.192\testShare2";
            int sReturnValue = WNetAddConnection2(myResource, password.Text, username.Text, 0);

            // Check the return value from WNetAddConnection2
            if (hReturnValue == 0) // Sucessful Connection
            {
                // Hide Form1
                this.Hide();

                // Create a new instance of the sleeper class
                sleeper sleeperForm = new sleeper();

                // Show the sleeper form
                if (sleeperForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    WNetCancelConnection2("h:", 0, 1);
                    WNetCancelConnection2("s:", 0, 1);
                    this.Close();
                }
            }
            else // Failed Connection
            {
                MessageBox.Show("Invalid username or password, please try again.\nError Code: " + hReturnValue,"Login Attempt Failed");
            }
        }
    }
}