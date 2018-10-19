using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Text.RegularExpressions;

namespace login0307
{
    
    public partial class ucAddLectModule : UserControl
    {
        
        String sex1;
        //string dbConnectionstring = @"Data Source=C:\Users\scot pipper\Documents\Visual Studio 2015\Projects\login0307\BiometricAttSysDB.db";
        
        private static ucAddLectModule _instance;
        public static ucAddLectModule Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucAddLectModule();
                return _instance;
            }
        }
        public ucAddLectModule()
        {
            InitializeComponent();
        }

        public SQLiteConnection sql_con;
        public SQLiteCommand sql_cmd;
        public SQLiteDataAdapter DB;
        public DataSet DS = new DataSet();
        public DataTable DT = new DataTable();

        private void ucAddLectModule_Load(object sender, EventArgs e)
        {
            //fillCheckedListBox();
        }
        
        //set connection
        public void SetConnection()
        {
            sql_con = new SQLiteConnection
               (@"Data Source=C:\Users\DELL\Desktop\vicky\login0307\BiometricAttSysDB.db");
        }

        //set ExecuteQuery
        public void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();

        }

        //private void fillCheckedListBox()
        //{
        //    string TxtQuery = ("SELECT * FROM courses ORDER by course_title");
        //    ExecuteQuery(TxtQuery);
        //    loadDataCheck();
        //    checkedListBox1.Items.Clear();
        //    for (int i = 0; i < DT.Rows.Count; i++)
        //    {
        //        checkedListBox1.Items.Add(DT.Rows[i]["course_title"]);

        //    }
        //}


        private void loadDataCheck()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM courses";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
          //  checkedListBox1.DataSource = DT;
            sql_con.Close();
        }


        //Load data from database
           private void loadData()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM lecturers ORDER BY Id";
            DB = new SQLiteDataAdapter(CommandText, sql_con);            
            DB.Fill(DS);
            DT = DS.Tables[0];
            DS.Reset();
           //dataGridView1.DataSource = DT;
            sql_con.Close();
        }

          
           private void clearData()
           {
               lectName.Text = "";
               lecUserId.Text = "";
               lectPass.Text = "";
               lecFemaleCheckbox.Checked = false;
               lectMaleCheckBox.Checked = false;
               lecPhonenum.Text = "";
               LectProfPic.Image = null;
               comboBox1.Text = "";
               labelfilename.Text = "";
               //while (checkedListBox1.CheckedIndices.Count > 0)
               //{
               //    //checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[0], false);
               //}
               
           }


        private void btn_clear_Click(object sender, EventArgs e)
        {
            clearData();
        }

         private void lectMaleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (lectMaleCheckBox.Checked == true)
            {
                lecFemaleCheckbox.Checked = false;
            }
        }

        private void lecFemaleCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (lecFemaleCheckbox.Checked == true)
            {
                lectMaleCheckBox.Checked = false;
            }
               
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        string chosen_file = "";
         private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFD = new OpenFileDialog() {Filter = "JPEG Image|*.jpg|GIF Image|*.gif|PNG Image|*.png", ValidateNames = true, Multiselect = false };
               

                openFD.Title = "Insert an image ";
                openFD.InitialDirectory = "Pictures";
                openFD.FileName = lectName.Text;
                


                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                    MessageBox.Show("Operation cancelled !");
                }

                else
                {
                    chosen_file = openFD.FileName.ToString();
                    labelfilename.Text = chosen_file;
                   // LectProfPic.ImageLocation = chosen_file;
                    LectProfPic.Image = Image.FromFile(chosen_file);
                }
           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
         }
       
        //string checked_Item_List = "";


        //private void btn_Save_Click_1(object sender, EventArgs e)
        //{
        //}

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          //fillCheckedListBox();
        }

       
           
        

        private void LectProfPic_Click(object sender, EventArgs e)
        {

        }

        private void lecPhonenum_KeyPress(object sender, KeyPressEventArgs e)
        {
            string dissallowedChars= ".<,>/?;:'][}{=+-_)(*&^~!#@$%|"  ;
            
            //Disallow  unwanted char
            if (char.IsLetter(e.KeyChar ) == true)
            {
                e.Handled = true;
                MessageBox.Show("phonenumbers must be numerical digits only");

                lecPhonenum.BackColor = Color.Red;
                lecPhonenum.ForeColor = Color.White;
            }
                //check if dissalowed char is entered
            else if (dissallowedChars.Contains(e.KeyChar))
            {
                e.Handled = true;

                MessageBox.Show("phonenumbers must be numerical digits only");
                lecPhonenum.BackColor = Color.Red;
                lecPhonenum.ForeColor = Color.White;
            }
                //if char entered is numbers
            else
            {
                lecPhonenum.BackColor = Color.White;
                lecPhonenum.ForeColor = Color.Black;
                 
            }
               
        }

        private void btn_Save_Click(object sender, EventArgs e)
        {

            try
            {
                string pathstring = "";

                if (lectName.Text != "" && lecUserId.Text != "" && lectPass.Text != "" && lecPhonenum.Text != "" && (lectMaleCheckBox.Checked == true || lecFemaleCheckbox.Checked == true) && comboBox1.Text != "" && LectProfPic.Image != null)
                {
                    if (lectMaleCheckBox.Checked == true)
                    {
                        sex1 = lectMaleCheckBox.Text;

                    }
                    else if (lecFemaleCheckbox.Checked == true)
                    {
                        sex1 = lecFemaleCheckbox.Text;
                    }

                    if (lecPhonenum.Text.Length < 11)
                    {
                        label9.ForeColor = Color.Red;
                        label9.Text = "Phone number must be 11 character long";
                        label9.Visible = true;
                        Timer timer = new Timer();
                        timer.Interval = 3000;
                        timer.Tick += (source, E) =>
                        {
                            label9.Visible = false;
                            timer.Stop();
                        };
                        timer.Start();
                    }
                    else if (LectProfPic.Image != null)
                    {
                        string fname = lecUserId.Text + ".jpg";
                        string folder = "C:\\Attendance\\Lecturers";
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                            pathstring = Path.Combine(folder, fname);
                            Image a = LectProfPic.Image;
                            a.Save(pathstring);
                        }
                        else
                        {
                            pathstring = Path.Combine(folder, fname);
                            Image a = LectProfPic.Image;
                            a.Save(pathstring);
                        }

                        //sql_con.Open();
                        //SQLiteDataAdapter sdac = new SQLiteDataAdapter("SELECT * FROM lecturers WHERE fullname = '" + lectName.Text + "' AND user_id = '" + lecUserId.Text + "' ", sql_con);
                        //DataTable dtc = new DataTable();
                        //sdac.Fill(dtc);

                        //if (dtc.Rows.Count == 1)
                        //{

                        //    label9.Visible = true;
                        //    label9.Text = "Sorry this lecturer has been registered";
                        //    sql_con.Close();
                        //}
                        //else
                        //{

                        string txtQuery = "INSERT INTO lecturers (fullname,gender,user_id,password,phone,Profile_pic) VALUES( '" + lectName.Text + "', '" + sex1 + "', '" + lecUserId.Text + "', '" + lectPass.Text + "', '" + lecPhonenum.Text + "',   '" + pathstring + "')";

                        ExecuteQuery(txtQuery);


                        if (txtQuery.Count() != 0)
                        {
                            string txtQ = "INSERT INTO users (USERNAME,PASSWORD,ROLE) VALUES('" + lecUserId.Text + "', '" + lectPass.Text + "', '" + comboBox1.Text + "')";
                            ExecuteQuery(txtQ);

                            label9.ForeColor = Color.Green;
                            label9.Text = "Account Successfully Created";
                            Timer timer = new Timer();
                            timer.Interval = 3000;
                            timer.Tick += (source, E) =>
                            {
                                label9.Visible = false;
                                timer.Stop();
                            };
                            timer.Start();

                            clearData();
                        }
                        else
                        {
                            label9.ForeColor = Color.Red;
                            label9.Text = "Oops! an error occured";
                        }
                        //}
                    }
                }

                else if (lectName.Text == "" || lecUserId.Text == "" || lectPass.Text == "" || lecPhonenum.Text == "" || (lectMaleCheckBox.Checked == false && lecFemaleCheckbox.Checked == false) || comboBox1.Text == "" || LectProfPic.Image == null)
                {

                    label9.ForeColor = Color.Red;
                    label9.Text = "Some fields are Empty";
                    label9.Visible = true;
                    Timer timer = new Timer();
                    timer.Interval = 3000;
                    timer.Tick += (source, E) =>
                    {
                        label9.Visible = false;
                        timer.Stop();
                    };
                    timer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error Message");
            }

        }

      

                 
    }
}
