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
    public partial class ucRegisterCourse : UserControl
    {
        private static ucRegisterCourse _instance;

        public static ucRegisterCourse Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucRegisterCourse();
                return _instance;
            }
        }
        public ucRegisterCourse()
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

        private void loadDataCheckStudents()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM Students";
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
        private void fillCheckedListBox_Students()
        {
            string TxtQuery = ("SELECT * FROM Students ORDER by Mat_No");
            ExecuteQuery(TxtQuery);
            loadDataCheckStudents();
            checkedListBox1.Items.Clear();
            for (int i = 0; i < DT.Rows.Count; i++)
            {
                checkedListBox1.Items.Add(DT.Rows[i]["Mat_No"]);

            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            fillCheckedListBox_Courses();
        }
        
        private void Btn_back_Click(object sender, EventArgs e)
        {

            ucAddStudModule.Instance.BringToFront();
            
            
        }

        private void Btn_assign_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedItems.Count == 0)
            {
                label2.Visible = true;
                label2.Text = "Please select student";
            }
            else if (checkedListBox1.CheckedItems.Count > 1)
            {
                label2.Visible = true;
                label2.Text = "You cannot select more than one student";
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
                string selected_student = checkedListBox1.CheckedItems.ToString();
                for (int x = 0; x <= checkedListBox1.CheckedItems.Count - 1; x++)
                {
                    selected_student = checkedListBox1.CheckedItems[x].ToString();

                }

                try
                {
                    sql_con.Open();
                    SQLiteDataAdapter sda = new SQLiteDataAdapter("SELECT * FROM Students WHERE Mat_No = '" + selected_student + "' ", sql_con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        string student_mat = dt.Rows[0][6].ToString();
                        string student_Level = dt.Rows[0][7].ToString();
                        sql_con.Close();

                        sql_con.Open();
                        SQLiteDataAdapter sdac = new SQLiteDataAdapter("SELECT * FROM registered_courses WHERE matnum = '" + student_mat + "' AND course_title = '"+ selected_course +"' ", sql_con);
                        DataTable dtc = new DataTable();
                        sdac.Fill(dtc);
                        
                        if(dtc.Rows.Count == 1){

                            label2.Visible = true;
                            label2.Text = "Sorry this course has been registered";
                            sql_con.Close();
                        }
                        else
                        {
                            string txtQuery = "INSERT INTO registered_courses (matnum, level, course_title) VALUES('" + student_mat + "', '" + student_Level + "', '" + selected_course + "') ";

                            ExecuteQuery(txtQuery);


                            if (txtQuery.Count() != 0)
                            {
                                MessageBox.Show("Course Registered Successfully");


                                SetConnection();
                                sql_con.Open();
                                sql_cmd = sql_con.CreateCommand();
                                string CommandText = "SELECT * FROM registered_courses";
                                DB = new SQLiteDataAdapter(CommandText, sql_con);
                                DS.Reset();
                                DB.Fill(DS);
                                DT = DS.Tables[0];
                                dataGridView1.DataSource = DT;
                                sql_con.Close();

                            }
                            sql_con.Close();
                        }
                        

                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            fillCheckedListBox_Students();
        }
    }
}
