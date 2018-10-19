using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;
using System.Drawing.Imaging;



namespace login0307
{
    delegate void Function(); // A simple delegate for Marshmalling calls from event handler to the GUI Thread
    public partial class ucAddStudModule : UserControl, DPFP.Capture.EventHandler
    {
     
        private DPFP.Processing.Enrollment Enroller;
        
        protected override void ProcessEnroll(DPFP.Sample Sample)
        {
            Process(Sample);

            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

            // Check quality of the sample and add to enroller if it's good
            if (features != null) try
                {
                    MakeReport("The fingerprint feature set was created.");
                    Enroller.AddFeatures(features);		// Add feature set to template.
                }
                finally
                {
                    UpdateStatus();

                    // Check if template has been created.
                    switch (Enroller.TemplateStatus)
                    {
                        case DPFP.Processing.Enrollment.Status.Ready:	// report success and stop capturing
                            OnTemplateEnroll(Enroller.Template);
                            SetPrompt("Click Close, and then click Fingerprint Verification.");
                            Stop();
                            break;
                            
                            //byte[] serializedtemplate = null;
                            //string strTemp = null;
                            //DateTime cur_date = DateTime.Now;
                            //Enroller.Template.Serialize(ref serializedtemplate);
                            //if (serializedtemplate != null)
                            //{
                            //    string result = System.Text.Encoding.UTF8.GetString(serializedtemplate);
                            //    Console.Write(result);


                            //    MessageBox.Show(serializedtemplate.ToString());

                            //}


                        case DPFP.Processing.Enrollment.Status.Failed:	// report failure and restart capturing
                            Enroller.Clear();
                            Stop();
                            UpdateStatus();
                            OnTemplateEnroll(null);
                            Start();
                            break;
                    }
                }
        }
        private void UpdateStatus()
        {
            // Show number of samples needed.
            SetStatus(String.Format("Fingerprint samples needed: {0}", Enroller.FeaturesNeeded));
        }

        public delegate void OnTemplateEventHandler(DPFP.Template template);

        public event OnTemplateEventHandler OnTemplateEnroll;
        
		protected virtual void Init()
		{
            try
            {
                Capturer = new DPFP.Capture.Capture();				// Create a capture operation.

                if ( null != Capturer )
                {
                    Capturer.EventHandler = this;					// Subscribe for capturing events.
                    UpdateStatus();
                }
                   
                else
                   SetPrompt("Can't initiate capture operation!");
            
            }
            catch
            {               
                MessageBox.Show("Can't initiate capture operation!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);            
            }
		}

        protected void InitVerify()
        {
            Init();
            Verificator =new DPFP.Verification.Verification(); // Create a Fingerprint verificator
            UpdateStatusVerify(0);
        }

        protected void InitEnroll()
        {
            Init();
            Enroller = new DPFP.Processing.Enrollment(); // Create an enrollment.
            UpdateStatus();
        }

        protected virtual void Process(DPFP.Sample Sample)
        {
            // Draw fingerprint sample image.
            DrawPicture(ConvertSampleToBitmap(Sample));
        }


		protected void Start()
		{
            if (null != Capturer)
            {
                try
                {
                    Capturer.StartCapture();
                   SetPrompt("Using the fingerprint reader, scan your fingerprint.");
                }
                catch
                {
                   SetPrompt("Can't initiate capture!");
                }
            }
		}

		protected void Stop()
		{
            if (null != Capturer)
            {
                try
                {
                    Capturer.StopCapture();
                }
                catch
                {
                    SetPrompt("Can't terminate capture!");
                }
            }
		}
		
        private DPFP.Template Template;

        protected override void ProcessVerify(DPFP.Sample Sample)
        {
            Process(Sample);

            // Process the sample and create a feature set for the enrollment purpose.
            DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

            // Check quality of the sample and start verification to enroller if it's good
            // TODO: move to a seperate task
            if (features != null)
                {
                    //Compare the feature set with our template
                DPFP.Verification.Verification.Result result= new DPFP.Verification.Verification.Result();
                Verificator.Verify(features, Template, ref result);
                UpdateStatusVerify(result.FARAchieved);
                if(result.Verified)
                    MakeReport("The fingerprint feature was Verified.");
                else
                    MakeReport("The fingerprint feature was  Not Verified.");
                }              
        }
        private void UpdateStatusVerify(int FAR)
        {
            //Show "False accept rate" value
            SetStatus(string.Format("False Accept Rate (FAR) = {0}", FAR));
        }
	#region Form Event Handlers:

        //private void CaptureForm_Load(object sender, EventArgs e)
        //{
        //    Init();
        //    Start();												// Start capture operation.
        //}

        //private void CaptureForm_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    Stop();
        //}
	#endregion

	#region EventHandler Members:

		public void OnComplete(object Capture, string ReaderSerialNumber, DPFP.Sample Sample)
		{
            MakeReport("The fingerprint sample was captured.");
            SetPrompt("Scan the same fingerprint again.");
            Process(Sample);


            //byte[] serializedTemplate = null;
            //string str_temp = null;
            //DateTime cur_date = DateTime.Now;
            //Enroller.Template.Serialize(ref serializedTemplate);
		}

		public void OnFingerGone(object Capture, string ReaderSerialNumber)
		{
            MakeReport("The finger was removed from the fingerprint reader.");
		}

		public void OnFingerTouch(object Capture, string ReaderSerialNumber)
		{
            MakeReport("The fingerprint reader was touched.");
		}

		public void OnReaderConnect(object Capture, string ReaderSerialNumber)
		{
			MakeReport("The fingerprint reader was connected.");
		}

		public void OnReaderDisconnect(object Capture, string ReaderSerialNumber)
		{
            MakeReport("The fingerprint reader was disconnected.");
		}

		public void OnSampleQuality(object Capture, string ReaderSerialNumber, DPFP.Capture.CaptureFeedback CaptureFeedback)
		{
			if (CaptureFeedback == DPFP.Capture.CaptureFeedback.Good)
                MakeReport("The quality of the fingerprint sample is good.");
			else
                MakeReport("The quality of the fingerprint sample is poor.");
		}
	#endregion

		protected Bitmap ConvertSampleToBitmap(DPFP.Sample Sample)
		{
			DPFP.Capture.SampleConversion Convertor = new DPFP.Capture.SampleConversion();	// Create a sample convertor.
			Bitmap bitmap = null;												            // TODO: the size doesn't matter
			Convertor.ConvertToPicture(Sample, ref bitmap);									// TODO: return bitmap as a result
			return bitmap;
		}
       
        
        //process the sample and create a feature set for the enrollment purpose
		protected DPFP.FeatureSet ExtractFeatures(DPFP.Sample Sample, DPFP.Processing.DataPurpose Purpose)
		{
			DPFP.Processing.FeatureExtraction Extractor = new DPFP.Processing.FeatureExtraction();	// Create a feature extractor
			DPFP.Capture.CaptureFeedback feedback = DPFP.Capture.CaptureFeedback.None;
			DPFP.FeatureSet features = new DPFP.FeatureSet();
			Extractor.CreateFeatureSet(Sample, Purpose, ref feedback, ref features);			// TODO: return features as a result?
			if (feedback == DPFP.Capture.CaptureFeedback.Good)
				return features;

			else
				return null;
		}

        protected void SetStatus(string status)
        {
            this.Invoke(new Function(delegate()
            {
                StatusLine.Text = status;
            }));
                
        }

        protected void SetPrompt(string prompt)
        {
            this.Invoke(new Function(delegate()
            {
                Prompt.Text = prompt;
            }));
           
        }
        protected void MakeReport(string message)
        {
            this.Invoke(new Function(delegate()
            {
                StatusText.AppendText(message + "\r\n");
            }));
                
          
        }


        private void DrawPicture(Bitmap bitmap)
        {
            
                pictureBox2.Image = new Bitmap(bitmap, pictureBox2.Size);	// fit the image into the picture box

              //  int captured = 0;
                //DialogResult dialog1 = MessageBox.Show("Fingerprint captured, do you want to save fingerprint?", "Save Fingerprint", MessageBoxButtons.YesNo);
                
                ////do
                ////{
                ////    captured++;
                ////}
                ////while (captured < 10);

                //    if (dialog1 == DialogResult.Yes)
                //    {
                //        if (pictureBox2.Image != null)
                //        {
                //            string pathstring = "";
                //             //  string vic = "Victor";
                                
                //                string folder = "C:\\Attendance\\Students\\" + txt_MatricNo.Text;
                //                string fname = StatusLine.Text + ".jpg";
                //                pathstring = Path.Combine(folder, fname);                               
                                
                                
                //                if (!Directory.Exists(folder))
                //                {

                //                    Directory.CreateDirectory(folder);
                //                    Image a = pictureBox2.Image;
                //                    a.Save(pathstring);

                //                }

                //                else
                //                {
                //                    pathstring = Path.Combine(folder, fname);
                //                    Image a = pictureBox2.Image;
                //                    a.Save(pathstring);
                //                }
                                
                            
                            
                //        }
                //    }
                //    else if (dialog1 == DialogResult.No)
                //    {
                //        //pictureBox2.Image = null;
                //        //StatusLine.Text = "";
                //    }
                   
            
        }

		private DPFP.Capture.Capture Capturer;
        private DPFP.Verification.Verification Verificator;

        //Bitmap img = null;
        //DPFP.Capture.SampleConversion sp = new DPFP.Capture.SampleConversion();
        //DPFP.Capture.Capture cp = new DPFP.Capture.Capture();
        //DPFP.Sample sample = new DPFP.Sample();
        
        private static ucAddStudModule _instance;

        public static ucAddStudModule Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucAddStudModule();
                return _instance;
            }
        }
        public ucAddStudModule()
        {
            InitializeComponent();
           
            
        }
        private void ucAddStudModule_Load(object sender, EventArgs e)
        {
            fillCheckedListBox();
            
        }


        public SQLiteConnection sql_con;
        public SQLiteCommand sql_cmd;
        public SQLiteDataAdapter DB;
        public DataSet DS = new DataSet();
        public DataTable DT = new DataTable();


  
        //public void onComplete(object Capture, string readerSerialNumber, DPFP.Sample Sample)
        //{
        //    sp.ConvertToPicture(Sample, ref img);

        //    pictureBox2.Image = img;

        //}

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

        private void fillCheckedListBox()
        {
            string TxtQuery = ("SELECT * FROM courses ORDER by course_title");
            ExecuteQuery(TxtQuery);
            loadDataCheck();
            //checkedListBox_courseTitle.Items.Clear();
            //for (int i = 0; i < DT.Rows.Count; i++)
            //{
            //    checkedListBox_courseTitle.Items.Add(DT.Rows[i]["course_title"]);

            //}
        }


        //Load data from database
        private void loadData()
        {
            SetConnection();
            sql_con.Open();
            sql_cmd = sql_con.CreateCommand();
            string CommandText = "SELECT * FROM Students";
            DB = new SQLiteDataAdapter(CommandText, sql_con);
            DS.Reset();
            DB.Fill(DS);
            DT = DS.Tables[0];
           //dataGridView1.DataSource = DT;
            sql_con.Close();
        }

        //string checked_Item_List = "";
        private void clearData()
        {
            pictureBox1.Image = null;
            txt_Surname.Text = "";
            txt_firstName.Text = "";
           
            //while (checkedListBox_courseTitle.CheckedIndices.Count > 0)
            //{
            //    checkedListBox_courseTitle.SetItemChecked(checkedListBox_courseTitle.CheckedIndices[0], false);
            //}
            combo_Level.Text = "";
            checkBox_Male.Checked = false;
            checkBox_Female.Checked = false;
            checkBox1st.Checked = false;
            checkBox2nd.Checked = false;
            checkBox3rd.Checked = false;
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

        private void checkBox1st_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1st.Checked == true)
            {
                checkBox2nd.Checked = false;
                checkBox3rd.Checked = false;
            }
        }

        private void checkBox2nd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2nd.Checked == true)
            {
                checkBox1st.Checked = false;
                checkBox3rd.Checked = false;
            }
        }

        private void checkBox3rd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3rd.Checked == true)
            {
                checkBox1st.Checked = false;
                checkBox2nd.Checked = false;
            }
        }

        string sex, _Semest;
        public void btn_Save_Click(object sender, EventArgs e)
        {
             try
            {
                   groupBox1.Visible = false;
                    if (checkBox_Male.Checked == true)
                    {
                        sex = checkBox_Male.Text;
                    }

                    else if (checkBox_Female.Checked == true)
                    {
                        sex = checkBox_Female.Text;
                    }

                    if (checkBox1st.Checked == true)
                    {
                        _Semest = checkBox1st.Text;
                    }
                    else if (checkBox2nd.Checked == true)
                    {
                        _Semest = checkBox2nd.Text;
                    }
                    else if (checkBox3rd.Checked == true)
                    {
                        _Semest = checkBox3rd.Text;
                    }

                   
                    else if (txt_Surname.Text != "" && txt_firstName.Text != "" && (checkBox_Female.Checked != false || checkBox_Male.Checked != false) && (checkBox1st.Checked != false || checkBox2nd.Checked != false || checkBox3rd.Checked != false) && pictureBox1.Image != null && combo_Level.Text != "" && txt_MatricNo.Text != "")
                    {
                        string pathstring = "";
                        if (txt_MatricNo.Text.Length < 10)
                        {
                            label13.ForeColor = Color.Red;
                            label13.Text = "Matric number must be 10 character long";
                            label13.Visible = true;
                            Timer timer = new Timer();
                            timer.Interval = 3000;
                            timer.Tick += (source, E) =>
                            {
                                label13.Visible = false;
                                timer.Stop();
                            };
                            timer.Start();
                        }
                        //for (int x = 0; x <= checkedListBox_courseTitle.CheckedItems.Count - 1; x++)
                        //{
                        //    checked_Item_List = checked_Item_List + checkedListBox_courseTitle.CheckedItems[x].ToString() + ",";

                        //}
                     
                        else if (pictureBox1.Image != null)
                        {
                            string fname = txt_Surname.Text + ".jpg";
                            string folder = "C:\\Attendance\\Students";
                            if (!Directory.Exists(folder))
                            {
                                Directory.CreateDirectory(folder);
                                pathstring = Path.Combine(folder, fname);
                                Image a = pictureBox1.Image;
                                a.Save(pathstring);

                                 
                            }
                            else
                            {
                                pathstring = Path.Combine(folder, fname);
                                Image a = pictureBox1.Image;
                                a.Save(pathstring);
                            }
                        }

                        sql_con.Open();
                        SQLiteDataAdapter sdac = new SQLiteDataAdapter("SELECT * FROM Students WHERE Mat_No = '" + txt_MatricNo.Text + "' ", sql_con);
                        DataTable dtc = new DataTable();
                        sdac.Fill(dtc);

                        if (dtc.Rows.Count == 1)
                        {

                            MessageBox.Show("Sorry this Matric Number already exist");
                            sql_con.Close();
                        }
                         else
                        {
                            string txtQuery = "INSERT INTO Students (surname,firstname,gender,semester,Profile_pic,Mat_No,level) VALUES('" + txt_Surname.Text + "', '" + txt_firstName.Text + "','" + sex + "', '" + _Semest + "', '" + pathstring + "', '" + txt_MatricNo.Text + "', '" + combo_Level.Text + "')";
                            ExecuteQuery(txtQuery);
                            loadData();
                            MessageBox.Show("Student is Successfully Added!");
                            clearData();
                        }
                        if (!panelAdmin.Controls.Contains(ucRegisterCourse.Instance))
                        {
                            panelAdmin.Controls.Add(ucRegisterCourse.Instance);
                            ucRegisterCourse.Instance.Dock = DockStyle.Fill;
                            ucRegisterCourse.Instance.BringToFront();
                            panelAdmin.Visible = true;

                        }
                        else
                        {
                            ucRegisterCourse.Instance.BringToFront();
                            panelAdmin.Visible = true;
                        }
                    }
                
                else
                {
                    MessageBox.Show("Oops... All fields are Empty, Please Provide Details!");
                }



                    
                   
                       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            if (txt_Surname.Text == "" && txt_firstName.Text == "" && (checkBox_Female.Checked == false || checkBox_Male.Checked == false) && (checkBox1st.Checked == false || checkBox2nd.Checked == false || checkBox3rd.Checked == false) && pictureBox1.Image == null && combo_Level.Text == "" && txt_MatricNo.Text == "")
            {
                MessageBox.Show("Nothing to clear");
            }
            else
            {
                clearData();
            } 
                    
          
        }
        string chosen_file = "";
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFD = new OpenFileDialog() { Filter = "JPEG Image|*.jpg|GIF Image|*.gif|PNG Image|*.png", ValidateNames = true, Multiselect = false };


                openFD.Title = "Insert an image ";
                openFD.InitialDirectory = "Pictures";
             //   openFD.FileName = txt_firstName.Text + txt_Surname.Text;



                if (openFD.ShowDialog() == DialogResult.Cancel)
                {
                    MessageBox.Show("Operation cancelled !");
                }

                else
                {
                    chosen_file = openFD.FileName.ToString();
                    // LectProfPic.ImageLocation = chosen_file;
                    pictureBox1.Image = Image.FromFile(chosen_file);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {

        }

        private void Btn_back_Click(object sender, EventArgs e)
        {
            ucAddStudModule.Instance.BringToFront();
            panelAdmin.Visible = false;
            groupBox1.Visible = true;

        }

        private void OnTemplate(DPFP.Template template)
        {
            Template = template;
            button1.Enabled =
        }
        private void Btn_enroll_Click(object sender, EventArgs e)
        {
            InitEnroll();
            Start();
            OnTemplateEnroll += this.OnTemplate;
            

            
            int cap = 3;

            try
            {
                int i;
                for (i = 1; i < cap; )
                {
                    DialogResult dialog2 = MessageBox.Show("Take fingerprint?", "Next Fingerprint", MessageBoxButtons.YesNo);

                    if (dialog2 == DialogResult.Yes)
                    {

                        StatusLine.Text = i.ToString();
                        i++;


                    }
                    else if (dialog2 == DialogResult.No)
                    {

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           

         }

        public void Verify(DPFP.Template template)
        {
            Template = template;        
        }
      
        private void button1_Click(object sender, EventArgs e)
        {
            //Bitmap bit1, bit2;
            try
            {
                Init();
             //   Process1(Sample);

                DPFP.Verification.Verification Verificator = new DPFP.Verification.Verification();          //Create a fingerprint template Verificator
               // Verificator.Verify(Template);
                UpdateStatus();
            //    pictureBox3.Image = null;
            //    string folder = "C:\\Attendance\\Students\\" + txt_MatricNo.Text;
            //    string fname = 1 + ".jpg";
            //    string pathstring = Path.Combine(folder, fname);
            //    pictureBox3.Image = Image.FromFile(pathstring);

            //    bit1 = new Bitmap(pictureBox3.Image);

            //    bit2 = new Bitmap(pictureBox2.Image);

            //    bool compare = ImageCompareString(bit1, bit2);
            //    if (compare == true)
            //    {
            //        MessageBox.Show("Match");
            //    }
            //    else
            //    {
            //        MessageBox.Show("Not Match");
            //    }
               
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private bool ImageCompareString(Bitmap bit11, Bitmap bit21)
        {
            MemoryStream ms = new MemoryStream();
            bit11.Save(ms, ImageFormat.Png);
            string first = Convert.ToBase64String(ms.ToArray());
            ms.Position = 0;
            bit21.Save(ms, ImageFormat.Png);
            string Second = Convert.ToBase64String(ms.ToArray());
            if (first.Equals(Second))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void txt_MatricNo_KeyPress(object sender, KeyPressEventArgs e)
        {
           string dissallowedChars= ".<,>/?;:'][}{=+-_)(*&^~!#@$%|"  ;
            
            ////Disallow  unwanted char
            //if (char.IsLetter(e.KeyChar ) == true)
            //{
            //    e.Handled = true;
            //    MessageBox.Show("phonenumbers must be numerical digits only");

            //    txt_MatricNo.BackColor = Color.Red;
            //    txt_MatricNo.ForeColor = Color.White;
            //}
                //check if dissalowed char is entered
             if (dissallowedChars.Contains(e.KeyChar))
            {
                e.Handled = true;

               
                 txt_MatricNo.BackColor = Color.Red;
                 txt_MatricNo.ForeColor = Color.White;
                 MessageBox.Show("Invalid format for a Mat. number");     
                        Timer timer = new Timer();
                        timer.Interval = 5000;
                        timer.Tick += (source, E) =>
                        {
                            
                            txt_MatricNo.BackColor = Color.White;
                           txt_MatricNo.ForeColor = Color.Black;
                           timer.Stop();
                        };
                        timer.Start();
               
            }
                //if char entered is numbers
            else
            {
                txt_MatricNo.BackColor = Color.White;
                txt_MatricNo.ForeColor = Color.Black;
                 
            }
               
        
        }
        
    }
}
