using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

namespace kCalc
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		private System.Windows.Forms.RichTextBox rtbAbout;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public About()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.rtbAbout = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbAbout
            // 
            this.rtbAbout.Font = new System.Drawing.Font("Arial Black", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtbAbout.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.rtbAbout.Location = new System.Drawing.Point(12, 51);
            this.rtbAbout.Name = "rtbAbout";
            this.rtbAbout.ReadOnly = true;
            this.rtbAbout.Size = new System.Drawing.Size(362, 189);
            this.rtbAbout.TabIndex = 0;
            this.rtbAbout.Text = "QQQQQQQQQQQQQQ";
            this.rtbAbout.MouseDown += new System.Windows.Forms.MouseEventHandler(this.richTextBox1_MouseDown);
            // 
            // About
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(386, 272);
            this.Controls.Add(this.rtbAbout);
            this.ForeColor = System.Drawing.Color.Transparent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "About";
            this.Text = "About";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.DoubleClick += new System.EventHandler(this.About_DoubleClick);
            this.Load += new System.EventHandler(this.About_Load);
            this.ResumeLayout(false);

		}
		#endregion



		private void About_Load(object sender, System.EventArgs e)
		{			
			this.BackColor = Color.Black;
			this.TransparencyKey = BackColor;

            StreamReader sr = new StreamReader("About.rtf");
            rtbAbout.Rtf = sr.ReadToEnd();
		}


		private void About_DoubleClick(object sender, System.EventArgs e)
		{
			About.ActiveForm.Close();		
		}

		private void richTextBox1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			About.ActiveForm.Close();
		}


	}
}
