using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace login0307
{
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
        }

        private void Btn_Cancel_Click(object sender, EventArgs e)
        {
            if (UserPanel.Controls.Contains(ucAttendance.Instance)||UserPanel.Controls.Contains(ucReport.Instance))
            {
                UserPanel.Controls.Remove(ucAttendance.Instance);
                UserPanel.Controls.Remove(ucReport.Instance);
                UserPanel.Show();
            }
            //else if (UserPanel.Controls.Contains(ucReport.Instance))
            //{
            //    UserPanel.Controls.Remove(ucReport.Instance);
            //    UserPanel.Show();
            //}       
        }

        private void btn_Logout_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you Sure you want to Log Out ?", "Biometric Attendance system", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                this.Hide();
                LoginPg lgout = new LoginPg();
                lgout.Show();
            }
            else if (dialog == DialogResult.No)
            {
                // CHANGE THIS LATER TO E.CANCEL
                this.Show();
                
            }
           
        }

        private void btn_Attendance_Click(object sender, EventArgs e)
        {
           // UserPanel.Visible = true;
            if (!UserPanel.Controls.Contains(ucAttendance.Instance))
            {
                UserPanel.Controls.Add(ucAttendance.Instance);
                ucAttendance.Instance.Dock = DockStyle.Fill;
                ucAttendance.Instance.BringToFront();
            }
            else
                ucAttendance.Instance.BringToFront();
            
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            label2.Text = LoginPg.SetValueForPassword;
            checkedListBox1.Items.Add( LoginPg.SetValueForUsername);
        }

        private void btn_Report_Click(object sender, EventArgs e)
        {
            if (!UserPanel.Controls.Contains(ucReport.Instance))
            {
                UserPanel.Controls.Add(ucReport.Instance);
                ucReport.Instance.Dock = DockStyle.Fill;
                ucReport.Instance.BringToFront();
            }
            else
                ucReport.Instance.BringToFront();
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void UserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult dialog = MessageBox.Show("Are you Sure you want to close Biometric Attendance system ?", "Biometric Attendance system", MessageBoxButtons.YesNo);
                if (dialog == DialogResult.Yes)
                {
                    this.Dispose();
                    Application.Exit();
                }
                else if (dialog == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

       

           
    }
}
