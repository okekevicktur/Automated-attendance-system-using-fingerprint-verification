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
  
    public partial class ucAssignCourse : UserControl
    {
        //public NffvUser Enroll(uint timeout, out NffvStatus status);
        private static ucAssignCourse _instance;
        public static ucAssignCourse Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucAssignCourse();
                return _instance;
            }
        }
        public ucAssignCourse()
        {
            InitializeComponent();
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

        public SQLiteConnection sql_con;
        public SQLiteCommand sql_cmd;
        public SQLiteDataAdapter DB;
        public DataSet DS = new DataSet();
        public DataTable DT = new DataTable();
        public void SetConnection()
        {
            sql_con = new SQLiteConnection
                (@"Data Source=C:\Users\DELL\Desktop\vicky\login0307\BiometricAttSysDB.db");
        }

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

        private void loadDataCheckLecturers()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM lecturers";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            //  checkedListBox1.DataSource = DT;
            sql_con.Close();
        }

        private void fillCheckedListBox_Courses()
        {
            string TxtQuery = ("SELECT * FROM courses ORDER by course_title");
            ExecuteQuery(TxtQuery);
            loadDataCheck();
            checkedListBox2.Items.Clear();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                checkedListBox2.Items.Add(DT.Rows[i]["course_title"]);

            }
        }
        private void fillCheckedListBox_Lecturers()
        {
            string TxtQuery = ("SELECT * FROM lecturers ORDER by fullname");
            ExecuteQuery(TxtQuery);
            loadDataCheckLecturers();
            checkedListBox1.Items.Clear();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                checkedListBox1.Items.Add(DT.Rows[i]["fullname"]);

            }
        }

        private void ucAssignCourse_Load(object sender, EventArgs e)
        {
            fillCheckedListBox_Lecturers();
            fillCheckedListBox_Courses();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                label2.Visible = true;
                label2.Text = "Please select Lecturer";
            }
            else if (checkedListBox1.CheckedItems.Count > 1)
            {
                label2.Visible = true;
                label2.Text = "You cannot select more than one lecturer";
            }
            else if (checkedListBox2.CheckedItems.Count == 0)
            {
                label2.Visible = true;
                label2.Text = "Please select Course";
            }
            else if (checkedListBox2.CheckedItems.Count > 1)
            {
                label2.Visible = true;
                label2.Text = "You cannot select more than one course";
            }
            else
            {
                string selected_course = checkedListBox2.CheckedItems.ToString();
                for (int x = 0; x <= checkedListBox2.CheckedItems.Count - 1; x++)
                {
                    selected_course = checkedListBox2.CheckedItems[x].ToString();

                }
                string selected_lecturer = checkedListBox1.CheckedItems.ToString();
                for (int x = 0; x <= checkedListBox1.CheckedItems.Count - 1; x++)
                {
                    selected_lecturer = checkedListBox1.CheckedItems[x].ToString();

                }

                try
                {
                    sql_con.Open();
                    SQLiteDataAdapter sda = new SQLiteDataAdapter("SELECT * FROM lecturers WHERE fullname = '" + selected_lecturer + "' ", sql_con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        string lecturer_id = dt.Rows[0][0].ToString();
                         string txtQuery = "UPDATE courses SET lecturer_id = '"+lecturer_id+"' WHERE course_title = '"+selected_course+"' ";

                        ExecuteQuery(txtQuery);


                        if (txtQuery.Count() != 0)
                        {
                            MessageBox.Show("Course Assigned Successfully");
                        }

                    }
                    sql_con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
               
            fillCheckedListBox_Lecturers();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fillCheckedListBox_Courses();
        }

                    

    }
}
