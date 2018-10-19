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

namespace login0307
{
    public partial class ucAddDelCourse : UserControl
    {

        private static ucAddDelCourse _instance;
        public static ucAddDelCourse Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucAddDelCourse();
                return _instance;
            }
        }
        public ucAddDelCourse()
        {
            InitializeComponent();
           
        }
     

      
        public SQLiteConnection sql_con;
        public SQLiteCommand sql_cmd;
        public SQLiteDataAdapter DB = new SQLiteDataAdapter();
        public DataSet DS = new DataSet();
        public DataTable DT = new DataTable();

        private void ucAddDelCourse_Load(object sender, EventArgs e)
        {
                  
            try
            {
                fillCombo();
                clearData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
           
        }

        public void fillCombo()
        {
             string TxtQuery= ("SELECT * FROM courses ORDER by course_title");
             ExecuteQuery(TxtQuery);
             loadData();
           // DB.Fill(DS);
             comboBox1.Items.Clear();
                 for (int i = 0; i < DT.Rows.Count; i++)
                 {
                     comboBox1.Items.Add(DT.Rows[i]["course_title"]);

                 }
                     
        }

        public void SetConnection()
        {
            sql_con = new SQLiteConnection (@"Data Source=C:\Users\DELL\Desktop\vicky\login0307\BiometricAttSysDB.db");
              
        }

        public void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();

        }



       
       // Load data from database
        private void loadData()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM courses";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
           // comboBox1.DataSource = DT;
            sql_con.Close();
        }

        private void clearData()
        {
            AddCourseTxtBox.Text = "";
            txt_coursecode.Text = "";
            comboBox1.Text = "";
           
        }
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadData();
            //fillCombo();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
          fillCombo();
          clearData();
        }

        private void AddCourseBtn_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (AddCourseTxtBox.Text == "" && txt_coursecode.Text == "")
                {
                    AddWarnLabel.ForeColor = Color.Red;
                    //  AddWarnLabel.Text = "Some fields are Empty";
                    AddWarnLabel.Visible = true;
                    Timer timer = new Timer();
                    timer.Interval = 3000;
                    timer.Tick += (source, E) =>
                    {
                        AddWarnLabel.Visible = false;
                        timer.Stop();
                    };
                    timer.Start();
                }
                else
                {
                    string txtQuery = "INSERT INTO courses(course_title,course_code) VALUES('" + AddCourseTxtBox.Text + "','" + txt_coursecode.Text + "')";
                    ExecuteQuery(txtQuery);
                    // loadData();
                    loadData();
                    fillCombo();
                    clearData();
                    AddWarnLabel.ForeColor = Color.Green;
                    AddWarnLabel.Text = "Course Added Successfully";
                    AddWarnLabel.Visible = true;
                    Timer timer = new Timer();
                    timer.Interval = 5000;
                    timer.Tick += (source, E) =>
                    {
                        AddWarnLabel.Visible = false;
                        timer.Stop();
                    };
                    timer.Start();

                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
 
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (comboBox1.Text == "")
            {
                DelWarnlabel.ForeColor = Color.Red;
                //  AddWarnLabel.Text = "Some fields are Empty";
                DelWarnlabel.Visible = true;
                Timer timer = new Timer();
                timer.Interval = 4000;
                timer.Tick += (source, E) =>
                {
                    DelWarnlabel.Visible = false;
                    timer.Stop();
                };
                timer.Start();
            }
            else
            {

                try
                {
                    string cmb = comboBox1.Text;
                    string txtQuery = ("DELETE FROM courses WHERE course_title='" + comboBox1.Text + "'");
                    ExecuteQuery(txtQuery);
                    clearData();
                    DelWarnlabel.ForeColor = Color.Green;
                    AddWarnLabel.Text = "Course Deleted Successfully";
                    DelWarnlabel.Visible = true;
                    Timer timer = new Timer();
                    timer.Interval = 4000;
                    timer.Tick += (source, E) =>
                    {
                        DelWarnlabel.Visible = false;
                        timer.Stop();
                    };
                    timer.Start();
                    fillCombo();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
           
        }
              
      
    }
}
