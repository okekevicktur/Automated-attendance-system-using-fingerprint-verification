using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace login0307
{
    public partial class AdminMain : Form
    {
          //Add two instance of the UserControls

    //    public AdminMain()
    //    {
    //        this.firstUserControl.MyEvent += MainWindow_myevent;
    //    }

         //   void MainWindow_myevent(object sender, EventArgs e)
    //{
    //    this.secondUserControl.MyText = this.firstUserControl.MyText;
    //}

        public AdminMain()
        {


            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void AddLectModule_Click(object sender, EventArgs e)
        {
            if (!panelAdmin.Controls.Contains(ucAddLectModule.Instance))
            {
                panelAdmin.Controls.Add(ucAddLectModule.Instance);
                ucAddLectModule.Instance.Dock = DockStyle.Fill;
                ucAddLectModule.Instance.BringToFront();
            }
            else
                ucAddLectModule.Instance.BringToFront(); 
        }

        private void AddStudentModule_Click(object sender, EventArgs e)
        {

            if (!panelAdmin.Controls.Contains(ucAddStudModule.Instance))
            {
                panelAdmin.Controls.Add(ucAddStudModule.Instance);
                ucAddStudModule.Instance.Dock = DockStyle.Fill;
                ucAddStudModule.Instance.BringToFront();
            }
            else
                ucAddStudModule.Instance.BringToFront();
        }

        private void ViewLecturers_Click(object sender, EventArgs e)
        {
            
            if (!panelAdmin.Controls.Contains(ucViewLecturers.Instance ))
            {
                panelAdmin.Controls.Add(ucViewLecturers.Instance);
                ucViewLecturers.Instance.Dock = DockStyle.Fill;
                ucViewLecturers.Instance.BringToFront();
            }
            else
                ucViewLecturers.Instance.BringToFront(); 
           
        }


        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialog1 = MessageBox.Show("Are you Sure you want to Log Out as Admin?", "Biometric Attendance system", MessageBoxButtons.YesNo);
            if (dialog1 == DialogResult.Yes)
            {
                this.Hide();
                LoginPg lgout = new LoginPg();
                lgout.Show();
              //  this.Dispose();
            }
            else if (dialog1 == DialogResult.No)
            {
                // CHANGE THIS LATER TO E.CANCEL
                this.Show();
            }
           
           
            

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

     

        private void button6_Click(object sender, EventArgs e)
        {
            if (!panelAdmin.Controls.Contains( ucAddDelCourse.Instance))
            {
                panelAdmin.Controls.Add(ucAddDelCourse.Instance);
                ucAddDelCourse.Instance.Dock = DockStyle.Fill;
                ucAddDelCourse.Instance.BringToFront();
            }
            else
                ucAddDelCourse.Instance.BringToFront(); 
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!panelAdmin.Controls.Contains(ucViewStudents.Instance))
            {
                panelAdmin.Controls.Add(ucViewStudents.Instance);
                ucViewStudents.Instance.Dock = DockStyle.Fill;
                ucViewStudents.Instance.BringToFront();
            }
            else
                ucViewStudents.Instance.BringToFront(); 
        }

        private void AdminMain_FormClosing(object sender, FormClosingEventArgs e)
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

        private void panelAdmin_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!panelAdmin.Controls.Contains(ucAssignCourse.Instance))
            {
                panelAdmin.Controls.Add(ucAssignCourse.Instance);
                ucAssignCourse.Instance.Dock = DockStyle.Fill;
                ucAssignCourse.Instance.BringToFront();
            }
            else
                ucAssignCourse.Instance.BringToFront(); 
        }
    }
}
