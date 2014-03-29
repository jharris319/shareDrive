using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShareDrive
{
    public partial class sleeper : Form
    {
        public sleeper()
        {
            InitializeComponent();
        }

        private void bnDisconnect_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sleeper_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
