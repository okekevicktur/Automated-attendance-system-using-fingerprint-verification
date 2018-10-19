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
    public partial class ucAttendance : UserControl
    {
        private static ucAttendance _instance;
        public static ucAttendance Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucAttendance();
                return _instance;
            }
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
        public ucAttendance()
        {
            InitializeComponent();
        }

        private void ucAttendance_Load(object sender, EventArgs e)
        {
            //passwordMain.Text;
            //LoginPg Lg = new LoginPg();
            //Welcome_Label.Text = Lg.Mywel;
            //MessageBox.Show("MR. " + Welcome_Label.Text + " Welcome To Class");
          

        }


        //set ExecuteQuery
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public void ExecuteQuery(string txtQuery)
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            sql_cmd.CommandText = txtQuery;
            sql_cmd.ExecuteNonQuery();
            sql_con.Close();

        }

        //loaddata from database
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
          //  dataGridView1.DataSource = DT;
            sql_con.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            labTime.Text = DateTime.Now.ToString("HH:mm");
            labSec.Text  = DateTime.Now.ToString("ss");
            labDate.Text = DateTime.Now.ToString("MMM dd yyyy");
            labDay.Text = DateTime.Now.ToString("dddd");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            timer.Start();
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer.Stop();
            
        }

           
    
    }
}
