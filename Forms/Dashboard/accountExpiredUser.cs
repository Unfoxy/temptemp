using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace desktopDashboard___Y_Lee.Forms
{
    public partial class accountExpiredUser : Form
    {
        public accountExpiredUser()
        {
            InitializeComponent();
        }

        private void btnAccountExpiredUser_Click(object sender, EventArgs e)
        {
            rtxtAccountExpiredUser.Clear();
            string site = comboxAccountExpiredUser.Text;
            string filter = lbAccountExpiredUserTop.Text;

            var (name, ntid, count) = Functions.queryAD(site, filter);
            rtxtAccountExpiredUser.AppendText(string.Format("{0,-7}{1,-25}{2,-20}", "Count", "Name", "NTID"));

            for (int i = 0; i < count; i++)
            {
                string rtxtCount = (i + 1).ToString();
                rtxtAccountExpiredUser.AppendText(Environment.NewLine);
                rtxtAccountExpiredUser.AppendText(string.Format("{0,-7}{1, -25}{2, -20}", rtxtCount, name[i], ntid[i]));
            }
            rtxtAccountExpiredUser.AppendText(Environment.NewLine);
            rtxtAccountExpiredUser.AppendText(Environment.NewLine + "Total Count: " + count);
        }
    }
}