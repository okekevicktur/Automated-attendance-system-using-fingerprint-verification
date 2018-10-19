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

namespace login0307
{
    public partial class ucViewStudents : UserControl
    {
        private static ucViewStudents _instance;
        public static ucViewStudents Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucViewStudents();
                return _instance;
            }
        }


        public ucViewStudents()
        {
            InitializeComponent();
            
        }

        private void ucViewStudents_Load_1(object sender, EventArgs e)
        {
            loadData(); HideData();

        }
       
        public SQLiteConnection sql_con;
        public SQLiteCommand sql_cmd;
        public SQLiteDataAdapter DB;
        public DataSet DS = new DataSet();
        public DataTable DT = new DataTable();
       
        //Setting Up DataBase connection
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

        //Load data from database
        public void loadData()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM Students";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            dataGridView_Std.DataSource = DT;
            sql_con.Close();
        }

        public void clearData()
        {

            txt_Id.Text = "";
            txt_FirstName.Text = "";
            txt_Surname.Text = "";
          //  txt_courseTitle.Text = "";           
            checkBox_Female.Checked = false;
            checkBox_Male.Checked = false;
            checkBox_1st.Checked = false;
            checkBox_2nd.Checked = false;
            checkBox_3rd.Checked = false;
            txt_MatricNo.Text = "";
            cmboBox_Level.Text = "";
        }

        public void HideData()
        {
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
         //   coursetitle.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            cmboBox_Level.Visible = false;
            txt_MatricNo.Visible = false;
            
            txt_Id.Visible = false;
            txt_FirstName.Visible = false;
            txt_Surname.Visible = false;
            checkBox_Male.Visible = false;
            checkBox_Female.Visible = false;
            checkBox_1st.Visible = false;
            checkBox_2nd.Visible = false;
            checkBox_3rd.Visible = false;
          //  txt_courseTitle.Visible = false;
        }
        public string _gender()
        {
            if (checkBox_Male.Checked == true)
            {
                sex = checkBox_Male.Text;

            }
            else if (checkBox_Female.Checked == true)
            {
                sex = checkBox_Female.Text;
            }
            return sex;
        }
        public string _semester()
        {
            if (checkBox_1st.Checked == true)
            {
                semesta = checkBox_1st.Text;
            }
            else if (checkBox_2nd.Checked == true)
            {
                semesta = checkBox_2nd.Text;
            }
            else if (checkBox_3rd.Checked == true)
            {
                semesta = checkBox_3rd.Text;

            }
            return semesta;
        }
           
         

        private void checkBox_Male_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Male.Checked == true)
            {
                checkBox_Female.Checked = false;
            }
        }

        private void checkBox_Female_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Female.Checked == true)
            {
                checkBox_Male.Checked = false;

            }
        }

        private void checkBox_1st_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_1st.Checked == true)
            {
                checkBox_2nd.Checked = false;
                checkBox_3rd.Checked = false;

            }
        }

        private void checkBox_2nd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_2nd.Checked == true)
            {
                checkBox_1st.Checked = false;
                checkBox_3rd.Checked = false;
            }
        }

        private void checkBox_3rd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_3rd.Checked == true)
            {
                checkBox_1st.Checked = false;
                checkBox_2nd.Checked = false;
            }
        }

        //refreshes datagrid to load any newly added data
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            loadData(); HideData();
        }

        int Id= 0;
        string semesta, sex;
        //private void dataGridView_Std_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //}

        private void dataGridView_Std_RowHeaderMouseClick_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                Id = Convert.ToInt32(dataGridView_Std.Rows[e.RowIndex].Cells[0].Value.ToString());
                txt_Id.Text = dataGridView_Std.Rows[e.RowIndex].Cells[0].Value.ToString();
                txt_Surname.Text = dataGridView_Std.Rows[e.RowIndex].Cells[1].Value.ToString();
                txt_FirstName.Text = dataGridView_Std.Rows[e.RowIndex].Cells[2].Value.ToString();
               // txt_courseTitle.Text = dataGridView_Std.Rows[e.RowIndex].Cells[3].Value.ToString();
                string gender = dataGridView_Std.Rows[e.RowIndex].Cells[3].Value.ToString();
                if (gender == "Male")
                {
                    checkBox_Male.Checked = true;
                }
                else if (gender == "Female")
                {
                    checkBox_Female.Checked = true;
                }
                string semester = dataGridView_Std.Rows[e.RowIndex].Cells[4].Value.ToString();
                if (semester == "1st")
                {
                    checkBox_1st.Checked = true;
                }
                else if (semester == "2nd")
                {
                    checkBox_2nd.Checked = true;
                }
                else if (semester == "3rd")
                {
                    checkBox_3rd.Checked = true;
                }
                string avatar = dataGridView_Std.Rows[e.RowIndex].Cells[5].Value.ToString();
                string pathstring = Path.Combine(avatar);
                pictureBox1.Image = Image.FromFile(pathstring);
                txt_MatricNo.Text = dataGridView_Std.Rows[e.RowIndex].Cells[6].Value.ToString();
                cmboBox_Level.SelectedText= dataGridView_Std.Rows[e.RowIndex].Cells[7].Value.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            
            try {
                                       
                     if (txt_Id.Text != "" || txt_Surname.Text != "" || txt_FirstName.Text != "" || (checkBox_Female.Checked != false || checkBox_Male.Checked != false) || (checkBox_1st.Checked != false || checkBox_2nd.Checked != false || checkBox_3rd.Checked != false))
                           {
                               string txtQuery = "UPDATE Students SET Id= '"+txt_Id.Text+"', firstname='"+txt_FirstName.Text+"', surname= '" + txt_Surname.Text + "',gender= '" + _gender() + "',semester= '" + _semester() + "' WHERE Id='" +  Id + "'";
                                ExecuteQuery(txtQuery);

                                MessageBox.Show("Record Updated Successfully");
                                loadData();
                                clearData();
                             }
                          else
                             {
                                 MessageBox.Show("Oops... All fields are Empty, Please Provide Details!");
                             }

                          // }
                }
                   
            catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Id.Text != "" && txt_Surname.Text != "" && txt_FirstName.Text != "" && (checkBox_Female.Checked != false || checkBox_Male.Checked != false) && (checkBox_1st.Checked != false || checkBox_2nd.Checked != false || checkBox_3rd.Checked != false) && pictureBox1.Image != null && cmboBox_Level.Text != "")
                {
                    sql_con.Open();
                    SQLiteDataAdapter sda = new SQLiteDataAdapter("Select * from Students where surname ='" + txt_Surname.Text + "' ", sql_con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        String picPath = (dt.Rows[0][5].ToString());
                        if (File.Exists(picPath))
                        {
                            pictureBox1.Image.Dispose();
                            pictureBox1.Image = null;
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            File.Delete(picPath);
                            MessageBox.Show("Picture deleted from folder");
                            string txtQuery = "Delete from Students where Id='" + Id + "'";
                            ExecuteQuery(txtQuery);
                            MessageBox.Show("Student Deleted Successfully!");
                            loadData();
                            clearData();
                            btn_Delete.Enabled = false;
                            btn_Update.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Invalid Operation");
                        }
                    }
                    sql_con.Close();
                }
                
                else
                {
                    MessageBox.Show("Please Select Student to Delete");
                }
                
           }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }           
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
                btn_Delete.Enabled = true;
                btn_Update.Enabled = true;
                clearData();
                label4.Visible=true;
                label5.Visible=true;
                label6.Visible=true;
                label2.Visible=true;
                label3.Visible=true;
              //  coursetitle.Visible=true;
                label7.Visible = true;
                label8.Visible = true;
                cmboBox_Level.Visible = true;
                txt_MatricNo.Visible = true;

                txt_Id.Visible=true;
                txt_FirstName.Visible=true;
                txt_Surname.Visible=true;
                checkBox_Male.Visible=true;
                checkBox_Female.Visible=true;
                checkBox_1st.Visible=true;
                checkBox_2nd.Visible=true;
                checkBox_3rd.Visible=true;
               // txt_courseTitle.Visible = true;
        }

    }


}
