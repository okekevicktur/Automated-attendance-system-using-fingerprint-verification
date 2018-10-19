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
using System.Data.SqlClient;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Drawing.Imaging;
namespace login0307
{
    public partial class ucViewLecturers : UserControl
    {
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

        //Load data into database
        public void loadData()
        {

            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM lecturers";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
            dataGridView1.DataSource = DT;
            sql_con.Close();
        }

        public void clearData()
        {
            txt_name.Text = "" ;
            txt_password.Text = "";
            txt_username.Text = "";
            txt_phone_Numbr.Text = "";
            txt_Id.Text= "";
            checkBox_Female.Checked = false;
            checkBox_male.Checked = false;
        }
        public void hideView()
        {
            label3.Visible =false;
            lectame.Visible =false;
            userId.Visible =false;
            pass.Visible =false;
            label2.Visible =false;
            pictureBox1.Visible = false;
            txt_Id.Visible =false;
            txt_name.Visible =false;
            txt_username.Visible =false;
            checkBox_male.Visible =false;
            checkBox_Female.Visible =false;
            txt_password.Visible =false;
            txt_phone_Numbr.Visible = false;

        }

        private static ucViewLecturers _instance;
        public static ucViewLecturers Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucViewLecturers();
                return _instance;
            }
        }
        public ucViewLecturers()
        {
            InitializeComponent();
           
            
        }

        private void ucViewLecturers_Load(object sender, EventArgs e)
        {
             hideView();
             loadData();
        }

        int Id = 0;
           // Deletes a leturer from the database
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (txt_Id.Text != "" && txt_name.Text != "" && (checkBox_Female.Checked != false || checkBox_male.Checked != false) && txt_password.Text != "" && txt_username.Text != "" && txt_phone_Numbr.Text != "" && pictureBox1.Image != null)
                {
                    sql_con.Open();
                    SQLiteDataAdapter sda = new SQLiteDataAdapter("Select * from lecturers where user_id ='" + txt_username.Text + "' ", sql_con);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        String picPath = (dt.Rows[0][6].ToString());
                        if (File.Exists(picPath))
                        {
                            pictureBox1.Image.Dispose();
                            pictureBox1.Image = null;
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            File.Delete(picPath);
                            MessageBox.Show("Picture deleted from folder");

                            string txtQuery = "Delete from lecturers where user_id='" + txt_username.Text + "'";
                            ExecuteQuery(txtQuery);
                            string txtQ = "Delete from users where USERNAME='" + txt_username.Text + "'";
                            ExecuteQuery(txtQ);

                            MessageBox.Show("Record Deleted Successfully!");
                            loadData();
                            clearData();
                            hideView();
                            btn_Delete.Enabled = false;
                            btn_Update.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Picture not found in folder");

                        }
                    }
                    sql_con.Close();
                }
                else MessageBox.Show("Please Select Lecturer to Delete");
            }   
            catch (IOException ex)
                {
                  MessageBox.Show(ex.Message);
                }
        }
             
           string sex1;
 
         private void checkBox_male_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox_male.Checked == true)
            {
                checkBox_Female.Checked = false;
            }
        }

        private void checkBox_Female_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox_Female.Checked == true)
            {
                checkBox_male.Checked = false;
            }

        
        }

        //dataGridView1 RowHeaderMouseClick Event
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                Id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                txt_Id.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                txt_name.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                string gender = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                if (gender == "Male")
                {
                    checkBox_male.Checked = true;
                }
                else if (gender == "Female")
                {
                    checkBox_Female.Checked = true;
                }
                txt_username.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                txt_password.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                txt_phone_Numbr.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                string avatar = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                string pathstring = Path.Combine(avatar);
                pictureBox1.Image = Image.FromFile(pathstring);

               // MessageBox.Show(Id.ToString());
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (checkBox_male.Checked == true)
                {
                    sex1 = checkBox_male.Text;

                }
                else if (checkBox_Female.Checked == true)
                {
                    sex1 = checkBox_Female.Text;
                }

                if (txt_Id.Text !="" || txt_name.Text != "" || (checkBox_Female.Checked != false || checkBox_male.Checked != false) || txt_password.Text != "" || txt_username.Text != "" || txt_phone_Numbr.Text != "")
                {
                    string txtQuery = "UPDATE lecturers SET  fullname= '" + txt_name.Text + "', gender= '" + sex1 +"',user_id= '"+txt_username.Text+"',password='"+txt_password.Text+"',phone= '"+txt_phone_Numbr.Text+"', Profile_pic= '"+pictureBox1.Image+"'  where Id='" + txt_Id.Text + "'";
                    ExecuteQuery(txtQuery);

                   if (txtQuery.Count() != 0)
                     {
                         string txtQ = "UPDATE users SET  USERNAME='" + txt_username.Text + "', PASSWORD='" + txt_password.Text + "' WHERE Id='" + txt_Id.Text + "' ";
                        ExecuteQuery(txtQ);

                     }
                    MessageBox.Show("Record Updated Successfully");
                    loadData();
                    clearData();
                    hideView();
                }
                else
                {
                    MessageBox.Show("Oops... all fields are Empty, Please Provide Details!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            hideView();
            loadData();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            btn_Delete.Enabled = true;
            btn_Update.Enabled = true;
            label3.Visible = true;
            lectame.Visible = true;
            userId.Visible = true;
            pass.Visible = true;
            label2.Visible = true;
            pictureBox1.Visible = true;
            txt_Id.Visible = true;
            txt_name.Visible = true;
            txt_username.Visible = true;
            checkBox_male.Visible = true;
            checkBox_Female.Visible = true;
            txt_password.Visible = true;
            txt_phone_Numbr.Visible = true;
        }

        
      
    }
}
