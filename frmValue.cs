using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kCalc
{
    public partial class frmValue : Form
    {
        public common com;

        public frmValue()
        {
            InitializeComponent();
        }

        private void frmValue_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save dataset
            dsVariables1.WriteXml(com.fileVariables);
        }
    }
}
