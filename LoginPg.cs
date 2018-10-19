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
    public partial class LoginPg : Form
    {
        string dbConnectionstring = (@"Data Source=C:\Users\DELL\Desktop\vicky\login0307\BiometricAttSysDB.db");
        public LoginPg()
        {
            InitializeComponent();
        }

        //public string fullname;
        //DataTable dt0 = new DataTable();
        //public string Mywel
        //{
        //    get { return fullname; }
        //    set { fullname = dt0.Rows[0][1].ToString(); }

        //}

        public static string SetValueForUsername = "";
        public static string SetValueForPassword = "";
        
        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
                       
            SQLiteConnection sqliteCon = new SQLiteConnection(dbConnectionstring);
            //Open Connection With Database
            try
            {
                if (usernameMain.Text == "" || passwordMain.Text == "")
                {
                    ErrDisp.Visible = true;
                }
                else if( usernameMain.Text == "Admin" && passwordMain.Text == "pass1")
                {
                    this.Hide();
                    AdminMain Admin = new AdminMain();
                    Admin.Show();
                   
                }
                else
                {
                    sqliteCon.Open();
                    SQLiteDataAdapter sda = new SQLiteDataAdapter("select * from users where USERNAME = '" + usernameMain.Text + "' and PASSWORD = '" + passwordMain.Text + "' ", sqliteCon);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);


                    if (dt.Rows.Count == 1)
                    {
                        String Role = (dt.Rows[0][3].ToString());
                        if (Role == "ADMIN")
                        {
                            this.Hide();
                            AdminMain Admin = new AdminMain();
                            Admin.Show();

                        }
                        else if(Role == "LECTURER")
                        {
                            String uname = dt.Rows[0][2].ToString();
                            String pwrd = dt.Rows[0][1].ToString();

                            //sqliteCon.Open();
                            SQLiteDataAdapter sda0 = new SQLiteDataAdapter("select * from lecturers where user_id = '" + uname + "' and password = '" + pwrd + "' ", sqliteCon);
                            //public string fullname;
                            DataTable dt0 = new DataTable();
                            sda0.Fill(dt0);
                            if (dt0.Rows.Count == 1)
                            {
                               string fullname = dt0.Rows[0][1].ToString();
                               string course = dt0.Rows[0][2].ToString();
                              
                               SetValueForPassword = fullname; //passwordMain.Text;
                               SetValueForUsername = course; //usernameMain.Text;
                                this.Hide();
                                UserForm user = new UserForm();
                                user.Show();
                               //MessageBox.Show("Welcome " + fullname + "Your Course is " + course);
                            
                            }
                            
                        }

                    }

                    else
                    {
                        ErrDisp.Visible = true;
                        ErrDisp.Text = "Username or Password is Incorrect";
                        this.usernameMain.Text = "";
                        this.passwordMain.Text = "";

                    }
                    sqliteCon.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //this.Close();
            this.Hide();
            UserForm user = new UserForm();
            user.Show();

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void LoginPg_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //DialogResult dialog = MessageBox.Show("Are youu Sure you want to close Biometric Attendance system ?", "Biometric Attendance system", MessageBoxButtons.YesNo);
                //if (dialog == DialogResult.Yes)
                //{
                    this.Dispose();
                    Application.Exit();
                //}
                //else if (dialog == DialogResult.No)
                //{
                //    e.Cancel = true;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      
    }
}
