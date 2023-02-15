using desktopDashboard___Y_Lee;
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
    public partial class deactivatedUser : Form
    {
        public deactivatedUser()
        {
            InitializeComponent();
        }
        private void btnDeactivatedUser_Click(object sender, EventArgs e)
        {
            rtxtDeactivatedUser.Clear();
            string site = comboxDeactivatedUser.Text;
            string filter = lbDeactivatedUserTop.Text;

            var (name, ntid, count) = Functions.queryAD(site, filter);
            rtxtDeactivatedUser.AppendText(string.Format("{0,-7}{1,-25}{2,-20}", "Count", "Name", "NTID"));

            for (int i = 0; i < count; i++)
            {
                string rtxtCount = (i + 1).ToString();
                rtxtDeactivatedUser.AppendText(Environment.NewLine);
                rtxtDeactivatedUser.AppendText(string.Format("{0,-7}{1, -25}{2, -20}", rtxtCount, name[i], ntid[i]));
            }
            rtxtDeactivatedUser.AppendText(Environment.NewLine);
            rtxtDeactivatedUser.AppendText(Environment.NewLine + "Total Count: " + count);
        }
    }
}

//private void btnDeactivatedUser_Click(object sender, EventArgs e)
//{
//    rtxtDeactivatedUser.Clear();
//    string site = comboxDeactivatedUser.Text;
//    string filter = lbDeactivatedUserTop.Text;

//    var (name, ntid, count) = Functions.queryAD(site, filter);
//    rtxtDeactivatedUser.AppendText("1. " + name[0]);
//    rtxtDeactivatedUser.AppendText(" - " + ntid[0]);

//    for (int i = 1; i < count; i++)
//    {
//        string rtxtCount = (i + 1).ToString();
//        rtxtDeactivatedUser.AppendText(Environment.NewLine + rtxtCount + ".  " + name[i]);
//        rtxtDeactivatedUser.AppendText(" - " + ntid[i]);
//    }
//    rtxtDeactivatedUser.AppendText(Environment.NewLine);
//    rtxtDeactivatedUser.AppendText(Environment.NewLine + "Total Count: " + count);
//}