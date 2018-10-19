using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace login0307
{
    public partial class ucReport : UserControl
    {
        private static ucReport _instance;
        public static ucReport Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ucReport();
                return _instance;
            }
        }


        public ucReport()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        
    }
}
