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
            NETRESOURCE netResource = new NETRESOURCE();
            netResource.dwScope = 0;
            netResource.dwType = 0;
            netResource.dwDisplayType = 0;
            netResource.LocalName = "h:";
            netResource.RemoteName = @"\\$SERVER-IP\users\home\" + username.Text;
            netResource.dwUsage = 0;
            netResource.Comment = "";
            netResource.Provider = "";

            // Try the H: Drive - domain1
            int hReturnValue = WNetAddConnection2(netResource, password.Text, "$DOMAIN1\\" + username.Text, 0);

            // Try the S: Drive - domain1
            netResource.LocalName = "s:";
            netResource.RemoteName = @"\\$SERVER-IP\share";
            int sReturnValue = WNetAddConnection2(netResource, password.Text, "$DOMAIN1\\" + username.Text, 0);

            // If the ua-net domain fails, the user may still
            // be on the asnet domain, so try the username
            // with the asnet domain
            if (hReturnValue > 0 || sReturnValue > 0)
            {
                // Try the H: Drive - domain2
                netResource.LocalName = "h:";
                netResource.RemoteName = @"\\$SERVER-IP\users\home\" + username.Text;
                hReturnValue = WNetAddConnection2(netResource, password.Text, "$DOMAIN2\\" + username.Text, 0);

                // Try the S: Drive - domain2
                netResource.LocalName = "s:";
                netResource.RemoteName = @"\\SERVER-IP\share";
                sReturnValue = WNetAddConnection2(netResource, password.Text, "$DOMAIN2\\" + username.Text, 0);
            }

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
                // TODO -- Add statements to differentiate error numbers
                // the current method isn't all too helpful
                MessageBox.Show("Invalid username or password, please try again.\nError Code: " + hReturnValue,"Login Attempt Failed");
            }
        }

        private void shareDrive_Load(object sender, EventArgs e)
        {
            WNetCancelConnection2("h:", 0, 1);
            WNetCancelConnection2("s:", 0, 1);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            WNetCancelConnection2("h:", 0, 1);
            WNetCancelConnection2("s:", 0, 1);
        }
    }
}