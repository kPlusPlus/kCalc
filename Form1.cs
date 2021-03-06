using System;
using System.Collections;
using System.Drawing;
using System.Runtime.InteropServices;

using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Diagnostics;



namespace kCalc
{


    /// <summary>
    /// Summary description for FormKCalc.
    /// </summary>    
    public class FormKCalc : Form
    {
        [DllImport("user32.dll", EntryPoint = "LockWindowUpdate", SetLastError = true,
ExactSpelling = true, CharSet = CharSet.Auto,
CallingConvention = CallingConvention.StdCall)]

        private static extern long LockWindow(long Handle);


        # region |   Declare variable and controls  |

        private int i = Math.Abs(-123);
        private Boolean rtfBoxScriptChange = false;
        private Boolean bAutoComplete = false;
        private int intPosCursor = 0;
        //private ListBox lbAutoComplete;
        //private Boolean boDialogDisplay = true;
        public RichTextBox rtbDisplayResult;
        public int CurrentTagStart = 0;
        private string currentFile = "";
        private uint FilePointer; // = Properties.Settings.Default.RFilePointer + 1;
        XmlDocument xmlDoc = new XmlDocument(); // function loadder from external xml files        
        Control AccControl;

        //String strAccObjectName;

        public ArrayList arrKey = new ArrayList(); // array with declaration word for highlighting
        public ArrayList arrComp = new ArrayList();
        public ArrayList arrOper = new ArrayList();
        public ArrayList arrRegexKey = new ArrayList();
        public ArrayList arrRegexComp = new ArrayList();
        public ArrayList arrRegexOper = new ArrayList();

        private Int32 iOrigpanelDisplayWidht;
        private Int32 iOrigpanelDisplayLeft;
        private Int32 iOrigRtfBoxScriptWidth;
        private Int32 iOrigRtfBoxScriptHeight;
        private Int32 iOrigTxtHistoryWidth;
        private Int32 iOrigTxtHistoryHeight;

        public XmlDocument xmlReserved = new XmlDocument();
        private System.Windows.Forms.Button btnSin;
        private System.Windows.Forms.Button btnCos;
        private System.Windows.Forms.Button btnTan;
        public TextBox txtHistory;
        private System.Windows.Forms.Button btnIzracunaj;
        private System.Windows.Forms.Button btnLog;
        private System.Windows.Forms.Button btnLN;
        private System.Windows.Forms.Button btn1;
        private System.Windows.Forms.Button btn2;
        private System.Windows.Forms.Button btn3;
        private System.Windows.Forms.Button btn4;
        private System.Windows.Forms.Button btn5;
        private System.Windows.Forms.Button btn6;
        private System.Windows.Forms.Button btn7;
        private System.Windows.Forms.Button btn8;
        private System.Windows.Forms.Button btn9;
        private System.Windows.Forms.Button btnPlus;
        private System.Windows.Forms.Button btnMinus;
        private System.Windows.Forms.Button btnPuta;
        private System.Windows.Forms.Button btnDjeljenje;
        private System.Windows.Forms.Button btnZagLijeva;
        private System.Windows.Forms.Button btnZagDesna;
        private System.Windows.Forms.Button btn0;
        private System.Windows.Forms.Button btnZarez;
        private System.Windows.Forms.Button btnABS;
        private System.Windows.Forms.Button btnSQRL;
        private System.Windows.Forms.Button btnRound;
        public System.Windows.Forms.ToolBar toolBar1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolBarButton tbntOpen;
        private System.Windows.Forms.ToolBarButton tbtnSave;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnFaktorijel;
        public System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.Button btnFunkcija;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Button btnPI;
        private System.Windows.Forms.ToolBarButton tbtnAbout;
        private System.Windows.Forms.MenuItem menuItem4;
        private System.Windows.Forms.MenuItem menuItem5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFuntionInfo;
        private System.Windows.Forms.MenuItem menuItem6;
        private System.Windows.Forms.MenuItem menuItem7;
        private Button btnVarijabla;
        private System.Windows.Forms.ListBox lbFunction;        
        // declaration variable
        protected int fWidth = 800;
        protected int fHeight = 430;
        public common com = new common();
        public delegate void ParseAllGeneral(string sALL, RichTextBox rtb);
        public int pozicija;
        public string funkcija = "";
        public string[] sFunName = new string[200];	// all from xml-a Hardcoded number is not good !!!
        public string[] sFunFormula = new string[200];
        public string[] sFunCursor = new string[200];
        public string[] sFunDesc = new string[200];
        private Panel panelDisplay;
        private Button btnPower;
        public RichTextBox rtfBoxScript;
        private ListBox lbIntelli;
        private MenuItem menuItem8;
        private FontDialog fontDialog1;
        private ListBox lbAutoComplete;
        private MenuItem menuItem9;
        private MenuItem miRF;
        private MenuItem menuItem10;
        private MenuItem menuItemPrint;
        private PrintDialog printDia;
        private System.Drawing.Printing.PrintDocument printDoc;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private CheckBox chkJavaScript;
        private CheckBox chkVisualBasic;
        private CheckBox chkCSharp;
        private CheckBox chkF;
        public string[] sFunCat = new string[200];
        public string lastScript;

        #endregion

        public FormKCalc()
        {
            InitializeComponent();
            // shortcut            
        }

        ////public void FormKCalcRTF(string iniFileName)
        ////{
        ////    // kada je kao argument datoteka
        ////    InitializeComponent();
        ////    System.IO.StreamReader sr = new System.IO.StreamReader(iniFileName);
        ////    //*** TEST TEST TEST --- initialize file
        ////    rtfBoxScript.Rtf = sr.ReadToEnd();
        ////    //rtfBoxScript.AppendText(sr.ReadToEnd()); //+= sr.ReadToEnd();
        ////    sr.Close();
        ////    currentFile = iniFileName;
        ////}

        public FormKCalc(string iniFileName, Boolean rtfFile)
        {
            // kada je kao argument datoteka
            InitializeComponent();
            System.IO.StreamReader sr = new System.IO.StreamReader(iniFileName);
            //*** TEST TEST TEST --- initialize file
            if (rtfFile)
                rtfBoxScript.Rtf = sr.ReadToEnd();
            else
                rtfBoxScript.AppendText(sr.ReadToEnd()); //+= sr.ReadToEnd();
            sr.Close();
            currentFile = iniFileName;
        }


        /// <summary>
        /// kCalc
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Inicijalizacija svemoguæeg suèelja
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormKCalc));
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.menuItem5 = new System.Windows.Forms.MenuItem();
            this.menuItem6 = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuItemPrint = new System.Windows.Forms.MenuItem();
            this.rtbDisplayResult = new System.Windows.Forms.RichTextBox();
            this.btnIzracunaj = new System.Windows.Forms.Button();
            this.btnSin = new System.Windows.Forms.Button();
            this.btnCos = new System.Windows.Forms.Button();
            this.btnTan = new System.Windows.Forms.Button();
            this.txtHistory = new System.Windows.Forms.TextBox();
            this.btnLog = new System.Windows.Forms.Button();
            this.btnLN = new System.Windows.Forms.Button();
            this.btn1 = new System.Windows.Forms.Button();
            this.btn2 = new System.Windows.Forms.Button();
            this.btn3 = new System.Windows.Forms.Button();
            this.btn4 = new System.Windows.Forms.Button();
            this.btn5 = new System.Windows.Forms.Button();
            this.btn6 = new System.Windows.Forms.Button();
            this.btn7 = new System.Windows.Forms.Button();
            this.btn8 = new System.Windows.Forms.Button();
            this.btn9 = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPuta = new System.Windows.Forms.Button();
            this.btnDjeljenje = new System.Windows.Forms.Button();
            this.btnZagLijeva = new System.Windows.Forms.Button();
            this.btnZagDesna = new System.Windows.Forms.Button();
            this.btn0 = new System.Windows.Forms.Button();
            this.btnZarez = new System.Windows.Forms.Button();
            this.btnABS = new System.Windows.Forms.Button();
            this.btnSQRL = new System.Windows.Forms.Button();
            this.btnRound = new System.Windows.Forms.Button();
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.tbntOpen = new System.Windows.Forms.ToolBarButton();
            this.tbtnSave = new System.Windows.Forms.ToolBarButton();
            this.tbtnAbout = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnFaktorijel = new System.Windows.Forms.Button();
            this.btnFunkcija = new System.Windows.Forms.Button();
            this.btnPI = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPower = new System.Windows.Forms.Button();
            this.btnVarijabla = new System.Windows.Forms.Button();
            this.lbFunction = new System.Windows.Forms.ListBox();
            this.txtFuntionInfo = new System.Windows.Forms.TextBox();
            this.panelDisplay = new System.Windows.Forms.Panel();
            this.rtfBoxScript = new System.Windows.Forms.RichTextBox();
            this.lbAutoComplete = new System.Windows.Forms.ListBox();
            this.lbIntelli = new System.Windows.Forms.ListBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.printDia = new System.Windows.Forms.PrintDialog();
            this.printDoc = new System.Drawing.Printing.PrintDocument();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.chkJavaScript = new System.Windows.Forms.CheckBox();
            this.chkVisualBasic = new System.Windows.Forms.CheckBox();
            this.chkCSharp = new System.Windows.Forms.CheckBox();
            this.chkF = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.panelDisplay.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem3,
            this.menuItem2,
            this.menuItem4,
            this.menuItem5,
            this.menuItem6,
            this.menuItem7,
            this.menuItem8,
            this.menuItem9,
            this.menuItem10,
            this.menuItemPrint});
            this.contextMenu1.Popup += new System.EventHandler(this.contextMenu1_Popup);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "Cut";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 1;
            this.menuItem3.Text = "Copy";
            this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 2;
            this.menuItem2.Text = "Paste";
            this.menuItem2.Click += new System.EventHandler(this.menuItem2_Click);
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 3;
            this.menuItem4.Text = "Select All";
            this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
            // 
            // menuItem5
            // 
            this.menuItem5.Index = 4;
            this.menuItem5.Text = "K Calc";
            this.menuItem5.Click += new System.EventHandler(this.btnIzracunaj_Click);
            // 
            // menuItem6
            // 
            this.menuItem6.Index = 5;
            this.menuItem6.Text = "Open";
            this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 6;
            this.menuItem7.Text = "Save";
            this.menuItem7.Click += new System.EventHandler(this.menuItem7_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 7;
            this.menuItem8.Text = "Font";
            this.menuItem8.Click += new System.EventHandler(this.menuItem8_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 8;
            this.menuItem9.MdiList = true;
            this.menuItem9.Text = "Recent Files";
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 9;
            this.menuItem10.Text = "Clear";
            this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
            // 
            // menuItemPrint
            // 
            this.menuItemPrint.Index = 10;
            this.menuItemPrint.Text = "Print";
            this.menuItemPrint.Click += new System.EventHandler(this.menuItemPrint_Click);
            // 
            // rtbDisplayResult
            // 
            this.rtbDisplayResult.AllowDrop = true;
            this.rtbDisplayResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbDisplayResult.AutoSize = true;
            this.rtbDisplayResult.BackColor = System.Drawing.Color.LightGray;
            this.rtbDisplayResult.ContextMenu = this.contextMenu1;
            this.rtbDisplayResult.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::kCalc.Properties.Settings.Default, "myFontDisplay", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rtbDisplayResult.Font = global::kCalc.Properties.Settings.Default.myFontDisplay;
            this.rtbDisplayResult.Location = new System.Drawing.Point(4, 122);
            this.rtbDisplayResult.Name = "rtbDisplayResult";
            this.rtbDisplayResult.ReadOnly = true;
            this.rtbDisplayResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.rtbDisplayResult.Size = new System.Drawing.Size(795, 35);
            this.rtbDisplayResult.TabIndex = 2;
            this.rtbDisplayResult.Text = "";
            // 
            // btnIzracunaj
            // 
            this.btnIzracunaj.BackColor = System.Drawing.Color.Transparent;
            this.btnIzracunaj.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnIzracunaj.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnIzracunaj.Image = ((System.Drawing.Image)(resources.GetObject("btnIzracunaj.Image")));
            this.btnIzracunaj.Location = new System.Drawing.Point(0, 595);
            this.btnIzracunaj.Name = "btnIzracunaj";
            this.btnIzracunaj.Size = new System.Drawing.Size(1112, 36);
            this.btnIzracunaj.TabIndex = 5;
            this.btnIzracunaj.Text = "&kCalc";
            this.btnIzracunaj.UseVisualStyleBackColor = false;
            this.btnIzracunaj.Click += new System.EventHandler(this.btnIzracunaj_Click);
            // 
            // btnSin
            // 
            this.btnSin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnSin.Image = ((System.Drawing.Image)(resources.GetObject("btnSin.Image")));
            this.btnSin.Location = new System.Drawing.Point(9, 22);
            this.btnSin.Name = "btnSin";
            this.btnSin.Size = new System.Drawing.Size(41, 21);
            this.btnSin.TabIndex = 6;
            this.btnSin.Text = "SIN";
            this.btnSin.Click += new System.EventHandler(this.btnSin_Click);
            // 
            // btnCos
            // 
            this.btnCos.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnCos.Image = ((System.Drawing.Image)(resources.GetObject("btnCos.Image")));
            this.btnCos.Location = new System.Drawing.Point(50, 22);
            this.btnCos.Name = "btnCos";
            this.btnCos.Size = new System.Drawing.Size(41, 21);
            this.btnCos.TabIndex = 7;
            this.btnCos.Text = "COS";
            this.btnCos.Click += new System.EventHandler(this.btnCos_Click);
            // 
            // btnTan
            // 
            this.btnTan.BackColor = System.Drawing.Color.OrangeRed;
            this.btnTan.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnTan.ForeColor = System.Drawing.Color.Black;
            this.btnTan.Image = global::kCalc.Properties.Resources.tipka;
            this.btnTan.Location = new System.Drawing.Point(91, 22);
            this.btnTan.Name = "btnTan";
            this.btnTan.Size = new System.Drawing.Size(41, 21);
            this.btnTan.TabIndex = 8;
            this.btnTan.Text = "TAN";
            this.btnTan.UseVisualStyleBackColor = true;
            this.btnTan.Click += new System.EventHandler(this.btnTan_Click);
            // 
            // txtHistory
            // 
            this.txtHistory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHistory.BackColor = System.Drawing.SystemColors.Info;
            this.txtHistory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHistory.CausesValidation = false;
            this.txtHistory.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.txtHistory.ForeColor = System.Drawing.SystemColors.InfoText;
            this.txtHistory.Location = new System.Drawing.Point(4, 4);
            this.txtHistory.Margin = new System.Windows.Forms.Padding(2);
            this.txtHistory.Multiline = true;
            this.txtHistory.Name = "txtHistory";
            this.txtHistory.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtHistory.Size = new System.Drawing.Size(795, 111);
            this.txtHistory.TabIndex = 3;
            this.txtHistory.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.txtHistory_MouseDoubleClick);
            // 
            // btnLog
            // 
            this.btnLog.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnLog.Image = ((System.Drawing.Image)(resources.GetObject("btnLog.Image")));
            this.btnLog.Location = new System.Drawing.Point(9, 43);
            this.btnLog.Name = "btnLog";
            this.btnLog.Size = new System.Drawing.Size(41, 23);
            this.btnLog.TabIndex = 10;
            this.btnLog.Text = "LOG";
            this.btnLog.Click += new System.EventHandler(this.btnLog_Click);
            // 
            // btnLN
            // 
            this.btnLN.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnLN.Image = ((System.Drawing.Image)(resources.GetObject("btnLN.Image")));
            this.btnLN.Location = new System.Drawing.Point(50, 43);
            this.btnLN.Name = "btnLN";
            this.btnLN.Size = new System.Drawing.Size(41, 23);
            this.btnLN.TabIndex = 11;
            this.btnLN.Text = "LN";
            this.btnLN.Click += new System.EventHandler(this.btnLN_Click);
            // 
            // btn1
            // 
            this.btn1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn1.Image = ((System.Drawing.Image)(resources.GetObject("btn1.Image")));
            this.btn1.Location = new System.Drawing.Point(9, 95);
            this.btn1.Name = "btn1";
            this.btn1.Size = new System.Drawing.Size(24, 22);
            this.btn1.TabIndex = 12;
            this.btn1.Text = "1";
            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // btn2
            // 
            this.btn2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn2.Image = ((System.Drawing.Image)(resources.GetObject("btn2.Image")));
            this.btn2.Location = new System.Drawing.Point(33, 95);
            this.btn2.Name = "btn2";
            this.btn2.Size = new System.Drawing.Size(24, 22);
            this.btn2.TabIndex = 13;
            this.btn2.Text = "2";
            this.btn2.Click += new System.EventHandler(this.btn2_Click);
            // 
            // btn3
            // 
            this.btn3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn3.Image = ((System.Drawing.Image)(resources.GetObject("btn3.Image")));
            this.btn3.Location = new System.Drawing.Point(57, 95);
            this.btn3.Name = "btn3";
            this.btn3.Size = new System.Drawing.Size(25, 22);
            this.btn3.TabIndex = 14;
            this.btn3.Text = "3";
            this.btn3.Click += new System.EventHandler(this.btn3_Click);
            // 
            // btn4
            // 
            this.btn4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn4.Image = ((System.Drawing.Image)(resources.GetObject("btn4.Image")));
            this.btn4.Location = new System.Drawing.Point(9, 117);
            this.btn4.Name = "btn4";
            this.btn4.Size = new System.Drawing.Size(24, 21);
            this.btn4.TabIndex = 15;
            this.btn4.Text = "4";
            this.btn4.Click += new System.EventHandler(this.btn4_Click);
            // 
            // btn5
            // 
            this.btn5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn5.Image = ((System.Drawing.Image)(resources.GetObject("btn5.Image")));
            this.btn5.Location = new System.Drawing.Point(33, 117);
            this.btn5.Name = "btn5";
            this.btn5.Size = new System.Drawing.Size(24, 21);
            this.btn5.TabIndex = 16;
            this.btn5.Text = "5";
            this.btn5.Click += new System.EventHandler(this.btn5_Click);
            // 
            // btn6
            // 
            this.btn6.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn6.Image = ((System.Drawing.Image)(resources.GetObject("btn6.Image")));
            this.btn6.Location = new System.Drawing.Point(57, 117);
            this.btn6.Name = "btn6";
            this.btn6.Size = new System.Drawing.Size(25, 21);
            this.btn6.TabIndex = 17;
            this.btn6.Text = "6";
            this.btn6.Click += new System.EventHandler(this.btn6_Click);
            // 
            // btn7
            // 
            this.btn7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn7.Image = ((System.Drawing.Image)(resources.GetObject("btn7.Image")));
            this.btn7.Location = new System.Drawing.Point(9, 138);
            this.btn7.Name = "btn7";
            this.btn7.Size = new System.Drawing.Size(24, 22);
            this.btn7.TabIndex = 18;
            this.btn7.Text = "7";
            this.btn7.Click += new System.EventHandler(this.btn7_Click);
            // 
            // btn8
            // 
            this.btn8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn8.Image = ((System.Drawing.Image)(resources.GetObject("btn8.Image")));
            this.btn8.Location = new System.Drawing.Point(33, 138);
            this.btn8.Name = "btn8";
            this.btn8.Size = new System.Drawing.Size(24, 22);
            this.btn8.TabIndex = 19;
            this.btn8.Text = "8";
            this.btn8.Click += new System.EventHandler(this.btn8_Click);
            // 
            // btn9
            // 
            this.btn9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn9.Image = ((System.Drawing.Image)(resources.GetObject("btn9.Image")));
            this.btn9.Location = new System.Drawing.Point(57, 138);
            this.btn9.Name = "btn9";
            this.btn9.Size = new System.Drawing.Size(25, 22);
            this.btn9.TabIndex = 20;
            this.btn9.Text = "9";
            this.btn9.Click += new System.EventHandler(this.btn9_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPlus.Image = ((System.Drawing.Image)(resources.GetObject("btnPlus.Image")));
            this.btnPlus.Location = new System.Drawing.Point(115, 160);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(25, 22);
            this.btnPlus.TabIndex = 21;
            this.btnPlus.Text = "+";
            this.btnPlus.Click += new System.EventHandler(this.btnPlus_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnMinus.Image = ((System.Drawing.Image)(resources.GetObject("btnMinus.Image")));
            this.btnMinus.Location = new System.Drawing.Point(140, 160);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(25, 22);
            this.btnMinus.TabIndex = 22;
            this.btnMinus.Text = "-";
            this.btnMinus.Click += new System.EventHandler(this.btnMinus_Click);
            // 
            // btnPuta
            // 
            this.btnPuta.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPuta.Image = ((System.Drawing.Image)(resources.GetObject("btnPuta.Image")));
            this.btnPuta.Location = new System.Drawing.Point(115, 138);
            this.btnPuta.Name = "btnPuta";
            this.btnPuta.Size = new System.Drawing.Size(25, 22);
            this.btnPuta.TabIndex = 23;
            this.btnPuta.Text = "*";
            this.btnPuta.Click += new System.EventHandler(this.btnPuta_Click);
            // 
            // btnDjeljenje
            // 
            this.btnDjeljenje.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnDjeljenje.Image = ((System.Drawing.Image)(resources.GetObject("btnDjeljenje.Image")));
            this.btnDjeljenje.Location = new System.Drawing.Point(140, 138);
            this.btnDjeljenje.Name = "btnDjeljenje";
            this.btnDjeljenje.Size = new System.Drawing.Size(25, 22);
            this.btnDjeljenje.TabIndex = 24;
            this.btnDjeljenje.Text = "/";
            this.btnDjeljenje.Click += new System.EventHandler(this.btnDjeljenje_Click);
            // 
            // btnZagLijeva
            // 
            this.btnZagLijeva.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnZagLijeva.Image = ((System.Drawing.Image)(resources.GetObject("btnZagLijeva.Image")));
            this.btnZagLijeva.Location = new System.Drawing.Point(115, 117);
            this.btnZagLijeva.Name = "btnZagLijeva";
            this.btnZagLijeva.Size = new System.Drawing.Size(25, 21);
            this.btnZagLijeva.TabIndex = 25;
            this.btnZagLijeva.Text = "(";
            this.btnZagLijeva.Click += new System.EventHandler(this.btnZagLijeva_Click);
            // 
            // btnZagDesna
            // 
            this.btnZagDesna.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnZagDesna.Image = ((System.Drawing.Image)(resources.GetObject("btnZagDesna.Image")));
            this.btnZagDesna.Location = new System.Drawing.Point(140, 117);
            this.btnZagDesna.Name = "btnZagDesna";
            this.btnZagDesna.Size = new System.Drawing.Size(25, 21);
            this.btnZagDesna.TabIndex = 26;
            this.btnZagDesna.Text = ")";
            this.btnZagDesna.Click += new System.EventHandler(this.btnZagDesna_Click);
            // 
            // btn0
            // 
            this.btn0.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn0.Image = ((System.Drawing.Image)(resources.GetObject("btn0.Image")));
            this.btn0.Location = new System.Drawing.Point(9, 160);
            this.btn0.Name = "btn0";
            this.btn0.Size = new System.Drawing.Size(24, 22);
            this.btn0.TabIndex = 28;
            this.btn0.Text = "0";
            this.btn0.Click += new System.EventHandler(this.btn0_Click);
            // 
            // btnZarez
            // 
            this.btnZarez.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnZarez.Image = ((System.Drawing.Image)(resources.GetObject("btnZarez.Image")));
            this.btnZarez.Location = new System.Drawing.Point(33, 160);
            this.btnZarez.Name = "btnZarez";
            this.btnZarez.Size = new System.Drawing.Size(24, 22);
            this.btnZarez.TabIndex = 29;
            this.btnZarez.Text = ",";
            this.btnZarez.Click += new System.EventHandler(this.btnZarez_Click);
            // 
            // btnABS
            // 
            this.btnABS.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnABS.Image = ((System.Drawing.Image)(resources.GetObject("btnABS.Image")));
            this.btnABS.Location = new System.Drawing.Point(9, 189);
            this.btnABS.Name = "btnABS";
            this.btnABS.Size = new System.Drawing.Size(41, 23);
            this.btnABS.TabIndex = 30;
            this.btnABS.Text = "ABS";
            this.btnABS.Click += new System.EventHandler(this.btnABS_Click);
            // 
            // btnSQRL
            // 
            this.btnSQRL.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnSQRL.Image = ((System.Drawing.Image)(resources.GetObject("btnSQRL.Image")));
            this.btnSQRL.Location = new System.Drawing.Point(50, 189);
            this.btnSQRL.Name = "btnSQRL";
            this.btnSQRL.Size = new System.Drawing.Size(41, 23);
            this.btnSQRL.TabIndex = 31;
            this.btnSQRL.Text = "SQRT";
            this.btnSQRL.Click += new System.EventHandler(this.btnSQRL_Click);
            // 
            // btnRound
            // 
            this.btnRound.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnRound.Image = ((System.Drawing.Image)(resources.GetObject("btnRound.Image")));
            this.btnRound.Location = new System.Drawing.Point(9, 212);
            this.btnRound.Name = "btnRound";
            this.btnRound.Size = new System.Drawing.Size(82, 21);
            this.btnRound.TabIndex = 32;
            this.btnRound.Text = "ROUND";
            this.btnRound.Click += new System.EventHandler(this.btnRound_Click);
            // 
            // toolBar1
            // 
            this.toolBar1.AccessibleRole = System.Windows.Forms.AccessibleRole.MenuBar;
            this.toolBar1.AllowDrop = true;
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbntOpen,
            this.tbtnSave,
            this.tbtnAbout});
            this.toolBar1.ButtonSize = new System.Drawing.Size(30, 30);
            this.toolBar1.CausesValidation = false;
            this.toolBar1.Cursor = System.Windows.Forms.Cursors.Default;
            this.toolBar1.Divider = false;
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(1112, 54);
            this.toolBar1.TabIndex = 33;
            this.toolBar1.Wrappable = false;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // tbntOpen
            // 
            this.tbntOpen.ImageIndex = 0;
            this.tbntOpen.Name = "tbntOpen";
            this.tbntOpen.Text = "Open";
            this.tbntOpen.ToolTipText = "Open";
            // 
            // tbtnSave
            // 
            this.tbtnSave.ImageIndex = 1;
            this.tbtnSave.Name = "tbtnSave";
            this.tbtnSave.Text = "Save";
            this.tbtnSave.ToolTipText = "Save";
            // 
            // tbtnAbout
            // 
            this.tbtnAbout.ImageIndex = 2;
            this.tbtnAbout.Name = "tbtnAbout";
            this.tbtnAbout.Text = "About";
            this.tbtnAbout.ToolTipText = "About";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.White;
            this.imageList1.Images.SetKeyName(0, "open.jpg");
            this.imageList1.Images.SetKeyName(1, "save.jpg");
            this.imageList1.Images.SetKeyName(2, "About_info.jpg");
            this.imageList1.Images.SetKeyName(3, "return_arrow.gif");
            // 
            // btnFaktorijel
            // 
            this.btnFaktorijel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnFaktorijel.Image = ((System.Drawing.Image)(resources.GetObject("btnFaktorijel.Image")));
            this.btnFaktorijel.Location = new System.Drawing.Point(165, 117);
            this.btnFaktorijel.Name = "btnFaktorijel";
            this.btnFaktorijel.Size = new System.Drawing.Size(24, 21);
            this.btnFaktorijel.TabIndex = 35;
            this.btnFaktorijel.Text = "n!";
            this.btnFaktorijel.Click += new System.EventHandler(this.btnFaktorijel_Click);
            // 
            // btnFunkcija
            // 
            this.btnFunkcija.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnFunkcija.Image = ((System.Drawing.Image)(resources.GetObject("btnFunkcija.Image")));
            this.btnFunkcija.Location = new System.Drawing.Point(165, 95);
            this.btnFunkcija.Name = "btnFunkcija";
            this.btnFunkcija.Size = new System.Drawing.Size(73, 22);
            this.btnFunkcija.TabIndex = 36;
            this.btnFunkcija.Text = "Function";
            this.btnFunkcija.Click += new System.EventHandler(this.btnFunkcija_Click);
            // 
            // btnPI
            // 
            this.btnPI.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPI.Image = ((System.Drawing.Image)(resources.GetObject("btnPI.Image")));
            this.btnPI.Location = new System.Drawing.Point(165, 138);
            this.btnPI.Name = "btnPI";
            this.btnPI.Size = new System.Drawing.Size(24, 22);
            this.btnPI.TabIndex = 37;
            this.btnPI.Text = "button1";
            this.btnPI.Click += new System.EventHandler(this.btnPI_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.AutoSize = true;
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.groupBox1.Controls.Add(this.btnPower);
            this.groupBox1.Controls.Add(this.btnVarijabla);
            this.groupBox1.Controls.Add(this.btn4);
            this.groupBox1.Controls.Add(this.btn6);
            this.groupBox1.Controls.Add(this.btnFaktorijel);
            this.groupBox1.Controls.Add(this.btnFunkcija);
            this.groupBox1.Controls.Add(this.btnTan);
            this.groupBox1.Controls.Add(this.btnPI);
            this.groupBox1.Controls.Add(this.btn0);
            this.groupBox1.Controls.Add(this.btnLog);
            this.groupBox1.Controls.Add(this.btnZarez);
            this.groupBox1.Controls.Add(this.btn1);
            this.groupBox1.Controls.Add(this.btn2);
            this.groupBox1.Controls.Add(this.btn3);
            this.groupBox1.Controls.Add(this.btnABS);
            this.groupBox1.Controls.Add(this.btn5);
            this.groupBox1.Controls.Add(this.btnSQRL);
            this.groupBox1.Controls.Add(this.btn7);
            this.groupBox1.Controls.Add(this.btnRound);
            this.groupBox1.Controls.Add(this.btn9);
            this.groupBox1.Controls.Add(this.btnPlus);
            this.groupBox1.Controls.Add(this.btnMinus);
            this.groupBox1.Controls.Add(this.btnPuta);
            this.groupBox1.Controls.Add(this.btnDjeljenje);
            this.groupBox1.Controls.Add(this.btnZagLijeva);
            this.groupBox1.Controls.Add(this.btnSin);
            this.groupBox1.Controls.Add(this.btn8);
            this.groupBox1.Controls.Add(this.btnCos);
            this.groupBox1.Controls.Add(this.btnZagDesna);
            this.groupBox1.Controls.Add(this.btnLN);
            this.groupBox1.Location = new System.Drawing.Point(0, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(244, 515);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Function";
            // 
            // btnPower
            // 
            this.btnPower.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPower.Image = ((System.Drawing.Image)(resources.GetObject("btnPower.Image")));
            this.btnPower.Location = new System.Drawing.Point(140, 189);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(49, 23);
            this.btnPower.TabIndex = 39;
            this.btnPower.Text = "x^y";
            this.btnPower.Click += new System.EventHandler(this.btnPower_Click);
            // 
            // btnVarijabla
            // 
            this.btnVarijabla.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnVarijabla.Image = ((System.Drawing.Image)(resources.GetObject("btnVarijabla.Image")));
            this.btnVarijabla.Location = new System.Drawing.Point(115, 95);
            this.btnVarijabla.Name = "btnVarijabla";
            this.btnVarijabla.Size = new System.Drawing.Size(50, 22);
            this.btnVarijabla.TabIndex = 38;
            this.btnVarijabla.Text = "Var";
            this.btnVarijabla.Click += new System.EventHandler(this.btnVarijabla_Click);
            // 
            // lbFunction
            // 
            this.lbFunction.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbFunction.ItemHeight = 14;
            this.lbFunction.Location = new System.Drawing.Point(250, 68);
            this.lbFunction.Name = "lbFunction";
            this.lbFunction.Size = new System.Drawing.Size(329, 326);
            this.lbFunction.TabIndex = 39;
            this.lbFunction.SelectedIndexChanged += new System.EventHandler(this.lbFunction_SelectedIndexChanged);
            this.lbFunction.DoubleClick += new System.EventHandler(this.lbFunction_DoubleClick);
            this.lbFunction.MouseLeave += new System.EventHandler(this.lbFunction_MouseLeave);
            this.lbFunction.MouseHover += new System.EventHandler(this.lbFunction_MouseHover);
            // 
            // txtFuntionInfo
            // 
            this.txtFuntionInfo.AcceptsReturn = true;
            this.txtFuntionInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtFuntionInfo.Location = new System.Drawing.Point(379, 1822);
            this.txtFuntionInfo.Multiline = true;
            this.txtFuntionInfo.Name = "txtFuntionInfo";
            this.txtFuntionInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFuntionInfo.Size = new System.Drawing.Size(428, 145);
            this.txtFuntionInfo.TabIndex = 40;
            // 
            // panelDisplay
            // 
            this.panelDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDisplay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panelDisplay.Controls.Add(this.rtfBoxScript);
            this.panelDisplay.Controls.Add(this.txtHistory);
            this.panelDisplay.Controls.Add(this.rtbDisplayResult);
            this.panelDisplay.ForeColor = System.Drawing.Color.Yellow;
            this.panelDisplay.Location = new System.Drawing.Point(292, 50);
            this.panelDisplay.Margin = new System.Windows.Forms.Padding(1);
            this.panelDisplay.Name = "panelDisplay";
            this.panelDisplay.Size = new System.Drawing.Size(812, 542);
            this.panelDisplay.TabIndex = 0;
            this.panelDisplay.Resize += new System.EventHandler(this.panelDisplay_Resize);
            // 
            // rtfBoxScript
            // 
            this.rtfBoxScript.AcceptsTab = true;
            this.rtfBoxScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtfBoxScript.AutoWordSelection = true;
            this.rtfBoxScript.BackColor = System.Drawing.Color.Black;
            this.rtfBoxScript.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::kCalc.Properties.Settings.Default, "myFontScriptBox", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rtfBoxScript.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::kCalc.Properties.Settings.Default, "myFontScriptBoxColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.rtfBoxScript.EnableAutoDragDrop = true;
            this.rtfBoxScript.Font = global::kCalc.Properties.Settings.Default.myFontScriptBox;
            this.rtfBoxScript.ForeColor = global::kCalc.Properties.Settings.Default.myFontScriptBoxColor;
            this.rtfBoxScript.Location = new System.Drawing.Point(4, 164);
            this.rtfBoxScript.Margin = new System.Windows.Forms.Padding(4);
            this.rtfBoxScript.Name = "rtfBoxScript";
            this.rtfBoxScript.Size = new System.Drawing.Size(795, 374);
            this.rtfBoxScript.TabIndex = 1;
            this.rtfBoxScript.Text = "";
            this.rtfBoxScript.TextChanged += new System.EventHandler(this.rtfBoxScript_TextChanged);
            this.rtfBoxScript.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rtfBoxScript_KeyUp);
            this.rtfBoxScript.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rtfBoxScript_MouseDoubleClick);
            this.rtfBoxScript.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.rtfBoxScript_PreviewKeyDown);
            // 
            // lbAutoComplete
            // 
            this.lbAutoComplete.BackColor = System.Drawing.Color.Orange;
            this.lbAutoComplete.FormattingEnabled = true;
            this.lbAutoComplete.ItemHeight = 14;
            this.lbAutoComplete.Location = new System.Drawing.Point(120, 524);
            this.lbAutoComplete.Name = "lbAutoComplete";
            this.lbAutoComplete.Size = new System.Drawing.Size(100, 4);
            this.lbAutoComplete.TabIndex = 42;
            this.lbAutoComplete.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbAutoComplete_MouseUp);
            this.lbAutoComplete.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.lbAutoComplete_PreviewKeyDown);
            // 
            // lbIntelli
            // 
            this.lbIntelli.BackColor = System.Drawing.Color.LemonChiffon;
            this.lbIntelli.FormattingEnabled = true;
            this.lbIntelli.ItemHeight = 14;
            this.lbIntelli.Location = new System.Drawing.Point(100, 524);
            this.lbIntelli.Name = "lbIntelli";
            this.lbIntelli.Size = new System.Drawing.Size(89, 4);
            this.lbIntelli.TabIndex = 41;
            this.lbIntelli.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.lbIntelli_PreviewKeyDown);
            // 
            // printDia
            // 
            this.printDia.Document = this.printDoc;
            this.printDia.UseEXDialog = true;
            // 
            // printDoc
            // 
            this.printDoc.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDoc_PrintPage);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            // 
            // chkJavaScript
            // 
            this.chkJavaScript.AutoSize = true;
            this.chkJavaScript.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.chkJavaScript.Location = new System.Drawing.Point(252, 0);
            this.chkJavaScript.Name = "chkJavaScript";
            this.chkJavaScript.Size = new System.Drawing.Size(84, 18);
            this.chkJavaScript.TabIndex = 43;
            this.chkJavaScript.Text = "Java Script";
            this.chkJavaScript.UseVisualStyleBackColor = false;
            // 
            // chkVisualBasic
            // 
            this.chkVisualBasic.AutoSize = true;
            this.chkVisualBasic.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.chkVisualBasic.Location = new System.Drawing.Point(343, 0);
            this.chkVisualBasic.Name = "chkVisualBasic";
            this.chkVisualBasic.Size = new System.Drawing.Size(65, 18);
            this.chkVisualBasic.TabIndex = 44;
            this.chkVisualBasic.Text = "VB.Net";
            this.chkVisualBasic.UseVisualStyleBackColor = false;
            // 
            // chkCSharp
            // 
            this.chkCSharp.AutoSize = true;
            this.chkCSharp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.chkCSharp.Location = new System.Drawing.Point(412, 0);
            this.chkCSharp.Name = "chkCSharp";
            this.chkCSharp.Size = new System.Drawing.Size(46, 18);
            this.chkCSharp.TabIndex = 45;
            this.chkCSharp.Text = "C #";
            this.chkCSharp.UseVisualStyleBackColor = false;
            // 
            // chkF
            // 
            this.chkF.AutoSize = true;
            this.chkF.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.chkF.Location = new System.Drawing.Point(464, 0);
            this.chkF.Name = "chkF";
            this.chkF.Size = new System.Drawing.Size(41, 18);
            this.chkF.TabIndex = 46;
            this.chkF.Text = "F#";
            this.chkF.UseVisualStyleBackColor = false;
            // 
            // FormKCalc
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.BackColor = global::kCalc.Properties.Settings.Default.FormBackColor;
            this.ClientSize = new System.Drawing.Size(1112, 631);
            this.ContextMenu = this.contextMenu1;
            this.Controls.Add(this.chkF);
            this.Controls.Add(this.chkCSharp);
            this.Controls.Add(this.chkVisualBasic);
            this.Controls.Add(this.chkJavaScript);
            this.Controls.Add(this.lbAutoComplete);
            this.Controls.Add(this.lbIntelli);
            this.Controls.Add(this.panelDisplay);
            this.Controls.Add(this.txtFuntionInfo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolBar1);
            this.Controls.Add(this.btnIzracunaj);
            this.Controls.Add(this.lbFunction);
            this.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::kCalc.Properties.Settings.Default, "FormForeColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::kCalc.Properties.Settings.Default, "FormBackColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("Font", global::kCalc.Properties.Settings.Default, "myFont", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::kCalc.Properties.Settings.Default, "FormKCalcLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = global::kCalc.Properties.Settings.Default.myFont;
            this.ForeColor = global::kCalc.Properties.Settings.Default.FormForeColor;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = global::kCalc.Properties.Settings.Default.FormKCalcLocation;
            this.MinimumSize = new System.Drawing.Size(823, 415);
            this.Name = "FormKCalc";
            this.Text = "kCalc";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormKCalc_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.FormKCalc_PreviewKeyDown);
            this.Resize += new System.EventHandler(this.FormKCalc_Resize);
            this.groupBox1.ResumeLayout(false);
            this.panelDisplay.ResumeLayout(false);
            this.panelDisplay.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// kCalc
        /// </summary>
        [STAThread]
        static void Main(params string[] Args)
        {
            if (Args.Length > 0)
            {
                // ako je argument datoteka
                if (Args[0].Substring(Args[0].Length - 4, 4).ToLower() == "kcal")
                {
                    Application.Run(new FormKCalc(Args[0], false));
                }
                if (Args[0].Substring(Args[0].Length - 5, 5).ToLower() == "krcal")
                {
                    Application.Run(new FormKCalc(Args[0], true));
                }
            }
            else
            {
                Application.Run(new FormKCalc());
            }
        }


        #region |   keyboard and function   |
        private void btnIzracunaj_Click(object sender, System.EventArgs e)
        {
            if (rtfBoxScript.Text == "")
            {
                rtbDisplayResult.Text = "kCalc 1.010101";
                rtbDisplayResult.SelectAll();
                rtbDisplayResult.SelectionFont = new Font("Tahoma", 22, FontStyle.Bold);
                rtbDisplayResult.SelectionColor = Color.RoyalBlue;
                return;
            }
            lastScript = rtfBoxScript.Rtf;
            SaveObjects();
            if (chkJavaScript.Checked)
                racunaj(ScriptEngine.Languages.JScript);
            else if (chkVisualBasic.Checked)
                racunaj(ScriptEngine.Languages.VBasic);
            else if (chkCSharp.Checked)
                racunaj(ScriptEngine.Languages.CSharp);
            else
                racunaj();
        }

        private void btnSin_Click(object sender, System.EventArgs e)
        {
            upis("Math.sin( )", -1);
        }


        private void btnCos_Click(object sender, System.EventArgs e)
        {
            upis("Math.cos( )", -1);
        }

        private void btnTan_Click(object sender, System.EventArgs e)
        {
            upis("Math.tan( )", -1);
        }

        private void btnLog_Click(object sender, System.EventArgs e)
        {
            upis("Math.log( )", -1);
        }

        private void btnLN_Click(object sender, System.EventArgs e)
        {
            upis("Math.ln( )", -1);
        }

        private void btn1_Click(object sender, System.EventArgs e)
        {
            upis("1");
        }

        private void btn2_Click(object sender, System.EventArgs e)
        {
            upis("2");
        }

        private void btn3_Click(object sender, System.EventArgs e)
        {
            upis("3");
        }

        private void btn4_Click(object sender, System.EventArgs e)
        {
            upis("4");
        }

        private void btn5_Click(object sender, System.EventArgs e)
        {
            upis("5");
        }

        private void btn6_Click(object sender, System.EventArgs e)
        {
            upis("6");
        }

        private void btn7_Click(object sender, System.EventArgs e)
        {
            upis("7");
        }

        private void btn8_Click(object sender, System.EventArgs e)
        {
            upis("8");
        }

        private void btn9_Click(object sender, System.EventArgs e)
        {
            upis("9");
        }

        private void btn0_Click(object sender, System.EventArgs e)
        {
            upis("0");
        }

        private void btnPlus_Click(object sender, System.EventArgs e)
        {
            upis("+");
        }

        private void btnMinus_Click(object sender, System.EventArgs e)
        {
            upis("-");
        }

        private void btnPuta_Click(object sender, System.EventArgs e)
        {
            upis("*");
        }

        private void btnDjeljenje_Click(object sender, System.EventArgs e)
        {
            upis("/");
        }

        private void btnZagLijeva_Click(object sender, System.EventArgs e)
        {
            upis("(");
        }

        private void btnZagDesna_Click(object sender, System.EventArgs e)
        {
            upis(")");
        }

        private void btnVarijabla_Click(object sender, System.EventArgs e)
        {
            upis("var ", 1);
        }

        private void btnZarez_Click(object sender, System.EventArgs e)
        {
            upis(".");
        }

        private void btnABS_Click(object sender, System.EventArgs e)
        {
            upis("Math.abs( )", -1);
        }

        private void btnSQRL_Click(object sender, System.EventArgs e)
        {
            upis("Math.sqrt( )", -1);
        }

        private void btnRound_Click(object sender, System.EventArgs e)
        {
            upis("Math.round( )", -1);
        }

        private void btnPower_Click(object sender, EventArgs e)
        {
            upis("Math.pow( , )", -3);
        }

        private void btnFaktorijel_Click(object sender, System.EventArgs e)
        {
            rtfBoxScript.Text = @"/* za NN staviti neki cjeli broj */
var NN = ;
var fakt = 1;
var i;
for(i = 1;i <= NN;i++)
{
	fakt = fakt * i;
}";
            rtfBoxScript.SelectionStart = 46;
            rtfBoxScript.Focus();
        }

        private void btnPI_Click(object sender, System.EventArgs e)
        {
            upis("Math.PI");
        }

        private void btnFunkcija_Click(object sender, System.EventArgs e)
        {
            if ((funkcija.ToString() != rtfBoxScript.SelectedText.ToString()) && rtfBoxScript.SelectedText.Length > 0)
                funkcija = rtfBoxScript.SelectedText.ToString();
            else
            {
                int pozicija = rtfBoxScript.SelectionStart;
                rtfBoxScript.Text = rtfBoxScript.Text.Insert(pozicija, funkcija);
            }
        }

        #endregion

        #region Radni dio
        private void Form1_Load(object sender, System.EventArgs e)
        {
            urediFormu();
            if (Properties.Settings.Default.SaveObjects)
                LoadObjects();
            LoadRecentFile(); // extra calling
            rtfBoxScript.Focus();
            //efekt();            
        }


        //private void efekt(object sender, System.EventArgs e)
        private void racunaj()
        {
            string rezultat = com.backResult(rtfBoxScript.Text.ToString());
            string strBoxScript = rtfBoxScript.Text.ToString();
            string greska = "ERROR";

            strBoxScript = strBoxScript.Replace("\n", Environment.NewLine);

            if ((int)rezultat.IndexOf(greska) != -1)
            {// ovo ako je greška				
                rtbDisplayResult.Text = rezultat;
                rtbDisplayResult.SelectAll();
                rtbDisplayResult.SelectionFont = new Font("Tahoma", 8, FontStyle.Bold);
                rtbDisplayResult.SelectionColor = Color.Red;
            }
            else
            {
                rtbDisplayResult.Text = rezultat.Replace(",", ".");
                //txtHistory.Text += Environment.NewLine + rtfBoxScript.Text.ToString() +" = " + rtbDisplayResult.Text + Environment.NewLine + "---------------------------------";
                txtHistory.Text += Environment.NewLine + strBoxScript + " = " + rtbDisplayResult.Text + Environment.NewLine + "---------------------------------";
                rtfBoxScript.Text = "";
                com.TextBoxScroll(txtHistory, 0, 1);

                rtbDisplayResult.SelectAll();
                rtbDisplayResult.SelectionFont = new Font("LCD2", 16, FontStyle.Bold);
                //rtbDisplayResult.SelectionFont = kCalc.Properties.Settings.Default.myFontDisplay;
                rtbDisplayResult.SelectionColor = System.Drawing.Color.Black;
                rtfBoxScript.Focus();
            }
        }

        private void racunaj(ScriptEngine.Languages lang )
        {
            string rezultat = com.backResult(rtfBoxScript.Text.ToString());
            string strBoxScript = rtfBoxScript.Text.ToString();
            string greska = "ERROR";

            strBoxScript = strBoxScript.Replace("\n", Environment.NewLine);
            ScriptEngine sce = new ScriptEngine(lang);
            sce.Code = strBoxScript;
            if (sce.Compile())
            { 
                rtbDisplayResult.Text = string.Empty;
                for (i = 0; i < sce.results.Output.Count; i++)            
                    rtbDisplayResult.Text += sce.results.Output[i].ToString();
            }
            else
            {
                rtbDisplayResult.Text = string.Empty;
                for (i = 0; i < sce.Messages.Length; i++)
                    rtbDisplayResult.Text += sce.Messages[i].ToString();
            }
        }

        private void efekt()
        {
            for (int i = 0; i <= 100; i++)
            {
                this.Visible = true;
                this.Opacity = i;
                this.Update();
            }
        }

        private void urediFormu()
        {
            // custom menu
            txtHistory.ContextMenu = contextMenu1;
            rtfBoxScript.ContextMenu = contextMenu1;

            contextMenu1.MenuItems[5].Shortcut = Shortcut.CtrlO;
            contextMenu1.MenuItems[5].ShowShortcut = true;
            contextMenu1.MenuItems[6].Shortcut = Shortcut.CtrlS;
            contextMenu1.MenuItems[6].ShowShortcut = true;
            contextMenu1.MenuItems[10].Shortcut = Shortcut.CtrlP;
            contextMenu1.MenuItems[10].ShowShortcut = true;

            // specijalni znakovi na tipkama            
            btnSQRL.Text = "\x221A";
            btnPI.Text = "\x00B6";
            lbIntelli.Hide();
            lbAutoComplete.Hide();

            // function loader general definition
            //if (xmlDoc == null)
            xmlDoc.Load(Application.StartupPath + "\\xFunction.xml");

            int i = 0;

            lbFunction.BeginUpdate();

            foreach (XmlElement elem in xmlDoc.SelectNodes("Function/Func"))
            {
                sFunName[i] = elem.GetAttribute("name");
                sFunFormula[i] = elem.GetAttribute("formula");
                //rtfBoxScript.AutoCompleteCustomSource.Add( sFunFormula[i].ToString() );
                sFunCursor[i] = elem.GetAttribute("cursor");
                sFunDesc[i] = elem.GetAttribute("desc");
                sFunCat[i] = elem.GetAttribute("cat");
                lbFunction.Items.Add(sFunName[i]);
                lbIntelli.Items.Add(sFunName[i]);

                //*** higlight code
                if (elem.GetAttribute("type").ToString() == "")
                {
                    arrKey.Add(elem.GetAttribute("formula"));
                    //*** regex option
                    if (elem.GetAttribute("regex").ToString() == "")
                        arrRegexKey.Add(elem.GetAttribute("formula"));
                    else
                        arrRegexKey.Add(elem.GetAttribute("regex"));
                }

                if (elem.GetAttribute("type").ToString() == "keyword" /*|| elem.GetAttribute("type").ToString()==""*/)
                {
                    arrKey.Add(elem.GetAttribute("formula"));
                    //*** regex option
                    if (elem.GetAttribute("regex").ToString() == "")
                        arrRegexKey.Add(elem.GetAttribute("formula"));
                    else
                        arrRegexKey.Add(elem.GetAttribute("regex"));
                }

                if (elem.GetAttribute("type").ToString() == "compare")
                {
                    arrComp.Add(elem.GetAttribute("formula"));
                    //*** regex option
                    if (elem.GetAttribute("regex").ToString() == "")
                        arrRegexComp.Add(elem.GetAttribute("formula"));
                    else
                        arrRegexComp.Add(elem.GetAttribute("regex"));
                }

                if (elem.GetAttribute("type").ToString() == "operator")
                {
                    arrOper.Add(elem.GetAttribute("formula"));
                    //*** regex option
                    if (elem.GetAttribute("regex").ToString() == "")
                        arrRegexOper.Add(elem.GetAttribute("formula"));
                    else
                        arrRegexOper.Add(elem.GetAttribute("regex"));
                }
                i++;
            }

            lbFunction.EndUpdate();
        }

        void ParseALL(string sALL, RichTextBox rtb)
        {
            // find number of view lines
            ParseALL(sALL, rtb, rtfBoxScript.Lines.GetLength(0));
        }

        void ParseALL(string sALL, RichTextBox rtb, Int32 rangeWidth)
        {
            rtb.Invalidate();
            LockWindow(rtb.Handle.ToInt32());

            intPosCursor = rtb.SelectionStart;
            Int32 currLine = rtb.GetLineFromCharIndex(intPosCursor);
            Int32 compareLine;

            //Int32 rangeWidth = 5;
            rtb.SelectionStart = 0;
            rtb.SelectionLength = rtb.Rtf.Length;
            rtb.SelectionColor = rtb.ForeColor;


            Regex regexObj;
            MatchCollection matoColl;
            for (int i = 0; i < arrKey.Count; i++)
            {
                /* regexObj = new Regex(@"\b" + arrKey[i].ToString() + @"\b",
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline); */

                regexObj = new Regex(@"\b" + arrRegexKey[i].ToString() + @"\b",
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

                matoColl = regexObj.Matches(sALL);

                foreach (Match maca in matoColl)
                {
                    //rtb.SelectionStart = maca.Index;
                    compareLine = rtb.GetLineFromCharIndex(maca.Index);
                    if (compareLine <= currLine + rangeWidth && compareLine >= currLine - rangeWidth)
                    {
                        rtb.SelectionStart = maca.Index;
                        rtb.SelectionLength = maca.Length;
                        if (rtb.SelectionColor != Color.BlueViolet)
                            rtb.SelectionColor = Color.BlueViolet;
                    }
                }
            }

            for (int i = 0; i < arrComp.Count; i++)
            {
                /*regexObj = new Regex(@"\b" + arrComp[i].ToString() + @"\b",
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);*/
                regexObj = new Regex(@"\b" + arrRegexComp[i].ToString() + @"\b",
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

                matoColl = regexObj.Matches(sALL);

                foreach (Match maca in matoColl)
                {
                    //rtb.SelectionStart = maca.Index;
                    compareLine = rtb.GetLineFromCharIndex(maca.Index);
                    if (compareLine <= currLine + rangeWidth && compareLine >= currLine - rangeWidth)
                    {
                        rtb.SelectionStart = maca.Index;
                        rtb.SelectionLength = maca.Length;
                        //rtb.SelectionColor = rtb.SelectionColor = Color.Green;
                        if (rtb.SelectionColor != Color.Purple)
                            rtb.SelectionColor = Color.Purple;
                    }
                }
            }

            for (int i = 0; i < arrOper.Count; i++)
            {
                /*regexObj = new Regex(@"\b" + arrOper[i].ToString() + @"\b",
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);*/
                regexObj = new Regex(@"(\b" + arrRegexOper[i] + @"\b) | (" + arrRegexOper[i] + ")",
                        RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);

                matoColl = regexObj.Matches(sALL);

                foreach (Match maca in matoColl)
                {
                    //rtb.SelectionStart = maca.Index;
                    compareLine = rtb.GetLineFromCharIndex(maca.Index);
                    if (compareLine <= currLine + rangeWidth && compareLine >= currLine - rangeWidth)
                    {
                        rtb.SelectionStart = maca.Index;
                        rtb.SelectionLength = maca.Length;
                        if (rtb.SelectionColor != Color.Gray)
                            rtb.SelectionColor = Color.Gray;
                    }
                }
            }

            // jedno redni comment
            regexObj = new Regex("--.*$",
                RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

            matoColl = regexObj.Matches(sALL);

            foreach (Match maca in matoColl)
            {
                //rtb.SelectionStart = maca.Index;
                compareLine = rtb.GetLineFromCharIndex(maca.Index);
                if (compareLine <= currLine + rangeWidth && compareLine >= currLine - rangeWidth)
                {
                    rtb.SelectionStart = maca.Index;
                    rtb.SelectionLength = maca.Length;
                    if (rtb.SelectionColor != Color.Green)
                        rtb.SelectionColor = Color.Green;
                }
            }

            // blok comment može biti u više redova
            regexObj = new Regex(@"/\*.*?\*/",
                RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

            matoColl = regexObj.Matches(sALL);

            foreach (Match maca in matoColl)
            {
                //rtb.SelectionStart = maca.Index;
                compareLine = rtb.GetLineFromCharIndex(maca.Index);
                if (compareLine <= currLine + rangeWidth && compareLine >= currLine - rangeWidth)
                {
                    rtb.SelectionStart = maca.Index;
                    rtb.SelectionLength = maca.Length;
                    if (rtb.SelectionColor != Color.Green)
                        rtb.SelectionColor = Color.Green;
                }
            }

            // za tekst u navodnicima --- crvene boje
            regexObj = new Regex(@"(?:\').*?(?:\')",
                   RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);

            matoColl = regexObj.Matches(sALL);

            foreach (Match maca in matoColl)
            {
                //rtb.SelectionStart = maca.Index;
                compareLine = rtb.GetLineFromCharIndex(maca.Index);
                if (compareLine <= currLine + rangeWidth && compareLine >= currLine - rangeWidth)
                {
                    rtb.SelectionStart = maca.Index;
                    rtb.SelectionLength = maca.Length;
                    if (rtb.SelectionColor != Color.Red)
                        rtb.SelectionColor = Color.Red;
                }
            }

            rtb.SelectionStart = 0;
            LockWindow(0);

        }

        private void returnScript()
        {
            rtfBoxScript.Rtf = lastScript;
        }

        private void upis(string funkcija) // upiši i pomakni znak samo za jedan
        {
            pozicija = rtfBoxScript.SelectionStart;
            rtfBoxScript.Text = rtfBoxScript.Text.Insert(pozicija, funkcija);
            rtfBoxScript.SelectionStart = pozicija + 1;
            rtfBoxScript.Focus();
        }

        private void upis(string funkcija, int pomakniZnak)
        {
            pozicija = rtfBoxScript.SelectionStart;
            rtfBoxScript.Text = rtfBoxScript.Text.Insert(pozicija, funkcija);
            rtfBoxScript.SelectionStart = pozicija + funkcija.Length + pomakniZnak;
            rtfBoxScript.Focus();
        }

        private void upis(int index_function)
        {
            // funkciji predaš ime i onda on zove

            upis(sFunFormula[index_function], Convert.ToInt32(sFunCursor[index_function]));
        }

        private int GetIndex(string functionName)
        {
            return Array.IndexOf(sFunName, functionName);
        }

        #endregion

        #region Toolbar
        private void toolBar1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {

            if (e.Button == tbntOpen)
            {
                rtfBoxScriptChange = false;
                fnkOpen();
            }
            if (e.Button == tbtnSave)
            {
                fnkSave();
            }
            if (e.Button == tbtnAbout)
            {
                Form a = new About();
                a.ShowDialog();
            }
        }

        private void menuItem1_Click(object sender, System.EventArgs e) // cut
        {
            //rtfBoxScript.Cut();            
            if (AccControl is RichTextBox)
            {
                ((RichTextBox)AccControl).Cut();

            }
            if (AccControl is TextBox)
            {
                ((TextBox)AccControl).Cut();
            }
        }

        private void miRF_Click(object sender, System.EventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            //MessageBox.Show(mi.Text);

            fnkOpen(mi.Text);
        }

        private void menuItem2_Click(object sender, System.EventArgs e) // paste
        {
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == false)
                return; // nema teksta za paste

            IDataObject data = Clipboard.GetDataObject();
            string vrati = (string)data.GetData(DataFormats.Text);
            //int pozicija = rtfBoxScript.SelectionStart;			
            pozicija = rtfBoxScript.SelectionStart;
            rtfBoxScript.Text = rtfBoxScript.Text.Insert(pozicija, vrati);
        }

        private void menuItem3_Click(object sender, System.EventArgs e) // copy
        {
            Clipboard.SetDataObject(rtfBoxScript.SelectedText);
            if (AccControl is TextBox)
                Clipboard.SetDataObject(((TextBox)AccControl).SelectedText);
            if (AccControl is RichTextBox)
                Clipboard.SetDataObject(((RichTextBox)AccControl).SelectedText);
        }

        private void menuItem4_Click(object sender, System.EventArgs e)
        {
            rtfBoxScript.SelectAll();
            Clipboard.SetDataObject(rtfBoxScript.SelectedText);
        }

        private void menuItem6_Click(object sender, System.EventArgs e)
        {
            // kao da klikneš na toolbaru na open
            fnkOpen();
        }

        private void menuItem7_Click(object sender, System.EventArgs e)
        {
            // kao da klikneš na toolbaru save
            fnkSave();
        }

        private void menuItem8_Click(object sender, EventArgs e)
        {
            // font edit
            fontDialog1.ShowColor = true;
            fontDialog1.Font = Properties.Settings.Default.myFontScriptBox;
            fontDialog1.Color = Properties.Settings.Default.myFontScriptBoxColor;
            if (fontDialog1.ShowDialog() != DialogResult.Cancel)
            {
                //setup font and color                
                Properties.Settings.Default.myFontScriptBox = fontDialog1.Font;
                Properties.Settings.Default.myFontScriptBoxColor = fontDialog1.Color;
                Properties.Settings.Default.Save();
            }
        }


        #endregion

        #region list box with function
        private void lbFunction_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (lbFunction.SelectedIndex > -1)
                txtFuntionInfo.Text = "category: " +
                    sFunCat[lbFunction.SelectedIndex] +
                    Environment.NewLine +
                    "Description" +
                    Environment.NewLine + "\t" +
                    sFunDesc[lbFunction.SelectedIndex];
            //txtFuntionInfo.Top = 50;
            txtFuntionInfo.Top = lbFunction.Top;
            txtFuntionInfo.BringToFront();
        }


        #endregion

        private void lbFunction_MouseHover(object sender, System.EventArgs e)
        {
            lbFunction.Width = 120;
            lbFunction.BringToFront();
        }

        private void lbFunction_MouseLeave(object sender, System.EventArgs e)
        {
            lbFunction.Width = 41;
            txtFuntionInfo.Top = 1000;
            txtFuntionInfo.BringToFront();
            //lbFunction.SendToBack();
        }

        private void lbFunction_DoubleClick(object sender, System.EventArgs e)
        {
            string fName, fPosition;
            fName = sFunFormula[lbFunction.SelectedIndex];
            fPosition = sFunCursor[lbFunction.SelectedIndex];
            upis(fName, Convert.ToInt32(fPosition));
        }

        private Boolean fnkOpen()
        {
            DialogResult result;
            result = MessageBox.Show(this, "Open format", "Open", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (result == DialogResult.No)
            {
                openFileDialog1.Filter = "*Kcal file|*.kcal|All Files (*)|*.*";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    System.IO.StreamReader sr = new
                        System.IO.StreamReader(openFileDialog1.FileName);
                    AccControl.Text = sr.ReadToEnd();
                    sr.Close();
                    currentFile = openFileDialog1.FileName;
                    TitleFileName();
                    RecentFile(currentFile);
                }

                rtfBoxScriptChange = true;
                rtfBoxScript_TextChanged(null, null);
                return true;
            }
            else
                return fnkOpenRTF();
        }

        private Boolean fnkOpen(String fileName)
        {
            System.IO.StreamReader sr = new
                        System.IO.StreamReader(fileName);

            if (fileName.Substring(fileName.Length - 4, 4).ToLower() == "kcal")
            {
                AccControl.Text = sr.ReadToEnd();
                sr.Close();
                currentFile = fileName;
                TitleFileName();
                RecentFile(currentFile);

            }

            if (fileName.Substring(fileName.Length - 5, 5).ToLower() == "krcal")
            {
                ((RichTextBox)AccControl).Rtf = sr.ReadToEnd();
                sr.Close();
                currentFile = fileName;
                TitleFileName();
                RecentFile(currentFile);
            }

            rtfBoxScriptChange = true;
            rtfBoxScript_TextChanged(null, null);
            return true;
        }

        private Boolean fnkOpenRTF()
        {
            openFileDialog1.Filter = "*KRcal file|*.KRcal|All Files (*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                    System.IO.StreamReader(openFileDialog1.FileName);
                rtfBoxScript.Rtf = sr.ReadToEnd();
                sr.Close();
                currentFile = openFileDialog1.FileName;
                TitleFileName();
                RecentFile(currentFile);
            }
            rtfBoxScriptChange = true;
            rtfBoxScript_TextChanged(null, null);
            return true;
        }

        private Boolean fnkSave()
        {
            // format file or simple text
            DialogResult result;
            result = MessageBox.Show(this, "Format save", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk);
            if (result == DialogResult.No)
            {

                saveFileDialog1.Filter = "*Kcal file|*.kcal|All Files (*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    System.IO.StreamWriter sw = new
                        System.IO.StreamWriter(saveFileDialog1.FileName);
                    //sw.Write(rtfBoxScript.Text);
                    sw.Write(AccControl.Text);
                    sw.Close();
                    currentFile = saveFileDialog1.FileName;
                    TitleFileName();
                }
                return true;
            }
            else
                return fnkSaveRTF();

        }

        private Boolean fnkSaveRTF()
        {
            saveFileDialog1.Filter = "*KRcal file|*.KRcal|All Files (*)|*.*";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw = new
                    System.IO.StreamWriter(saveFileDialog1.FileName);
                sw.Write(rtfBoxScript.Rtf);
                sw.Close();
                currentFile = saveFileDialog1.FileName;
                TitleFileName();
            }
            return true;
        }


        private void rtfBoxScript_DragEnter(object sender, DragEventArgs e)
        {
            // make sure they're actually dropping files (not text or anything else)
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                // allow them to continue
                // (without this, the cursor stays a "NO" symbol
                e.Effect = DragDropEffects.All;
            }

            if (e.Data.GetDataPresent(DataFormats.Text) == true)
                e.Effect = DragDropEffects.All;

        }


        private void rtfBoxScript_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                // transfer the filenames to a string array
                // (yes, everything to the left of the "=" can be put in the 
                // foreach loop in place of "files", but this is easier to understand.)
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                rtfBoxScript.Clear();
                // loop through the string array, adding each filename to the ListBox
                foreach (string file in files)
                {
                    System.IO.StreamReader sr = new System.IO.StreamReader(file);
                    rtfBoxScript.Text += sr.ReadToEnd();
                    sr.Close();
                }
            }
            if (e.Data.GetDataPresent(DataFormats.Text) == true)
            {
                rtfBoxScript.Text += (string)e.Data.GetData(DataFormats.Text);
            }
        }


        private void txtHistory_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (txtHistory.Dock != DockStyle.Fill)
            {
                iOrigTxtHistoryWidth = txtHistory.Width;
                iOrigTxtHistoryHeight = txtHistory.Height;
                txtHistory.BringToFront();
                txtHistory.Dock = DockStyle.Fill;
                this.MaximizeBox = false;
            }
            else
            {
                rtfBoxScript.Visible = true;
                txtHistory.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right); 
                txtHistory.Dock = DockStyle.None;
                txtHistory.Width = iOrigTxtHistoryWidth;
                txtHistory.Height = iOrigTxtHistoryHeight;
                this.MaximizeBox = true;
            }
        }

        private void rtfBoxScript_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (rtfBoxScript.Dock != DockStyle.Fill)
            {
                iOrigRtfBoxScriptWidth = rtfBoxScript.Width;
                iOrigRtfBoxScriptHeight = rtfBoxScript.Height;
                rtfBoxScript.BringToFront();
                rtfBoxScript.Dock = DockStyle.Fill;
                this.MaximizeBox = false;
            }
            else
            {
                rtfBoxScript.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
                rtfBoxScript.Dock = DockStyle.None;
                rtfBoxScript.Width = iOrigRtfBoxScriptWidth;
                rtfBoxScript.Height = iOrigRtfBoxScriptHeight;
                this.MaximizeBox = true;
            }
        }

        private void FormKCalc_Resize(object sender, EventArgs e)
        {
            // this is preventive from reorder of objects
            ////if (rtfBoxScript.Dock == DockStyle.Fill)
            ////    rtfBoxScript.Dock = DockStyle.None; //rtfBoxScript_MouseDoubleClick(null, null); //ZAPI

            panelDisplay.Refresh();
            txtHistory.Refresh();            
            rtbDisplayResult.Refresh();
            //rtfBoxScript.Dock = DockStyle.None; // ZAPI promjena
            rtfBoxScript.Refresh();
            lbFunction.Refresh();
            Application.DoEvents();            
            // memorize new dimension
            // ZAPI
            ////iOrigpanelDisplayWidht = panelDisplay.Width;
            ////iOrigpanelDisplayLeft = panelDisplay.Left;
            ////iOrigRtfBoxScriptWidth = rtfBoxScript.Width;
            ////iOrigRtfBoxScriptHeight = rtfBoxScript.Height;

        }

        private void rtfBoxScript_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.Space))
            {
                CurrentTagStart = rtfBoxScript.SelectionStart;
                Point p = rtfBoxScript.GetPositionFromCharIndex(rtfBoxScript.SelectionStart);
                p.Y += panelDisplay.Location.Y + rtfBoxScript.Location.Y + (int)rtfBoxScript.Font.GetHeight() * 2;
                p.X += panelDisplay.Location.X + rtfBoxScript.Location.X;
                /* select word */
                string LastWord = GetLastWord(rtfBoxScript.Text);
                /* select item in list with this text */
                
                //lbIntelli.SelectedValue = LastWord;
                lbIntelli.SelectedIndex = lbIntelli.FindString(LastWord);
                
                //for (int i = 0; i < sFunFormula.Length; i++)
                //{
                //    if (sFunFormula[i] == LastWord)
                //    {
                //        lbIntelli.SelectedIndex = i;
                //        i = sFunFormula.Length;
                //    }
                //}

                rtfBoxScript.SelectionStart = rtfBoxScript.SelectionStart - LastWord.Length;
                rtfBoxScript.SelectedText = "";

                lbIntelli.Location = p;
                lbIntelli.Show();
                lbIntelli.Height = 46;
                ActiveControl = lbIntelli;
            }
        }


        public string GetLastWord(string phrase)
        {
            int lastSpace = phrase.Trim().LastIndexOf(" ");
            if (lastSpace == -1)
                return phrase;

            string result = phrase.Substring(lastSpace + 1);
            return result;
        }


        private void lbIntelli_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lbIntelli.Hide();
                rtfBoxScript.Focus();
                return; // if escape only return
            }
            string fName, fPosition;
            if (lbIntelli.SelectedIndex == -1) return;

            if (e.KeyCode == Keys.Enter)
            {
                fName = sFunFormula[lbIntelli.SelectedIndex];
                fPosition = sFunCursor[lbIntelli.SelectedIndex];
                upis(fName, Convert.ToInt32(fPosition));
                lbIntelli.Hide();
            }
        }

        private void lbAutoComplete_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Char ch = Convert.ToChar(e.KeyValue);
            if (((int)e.KeyCode >= 96) && ((int)e.KeyCode <= 105)) // numeric keyboard
                ch = Convert.ToChar(e.KeyCode - 48);

            if (e.KeyCode == Keys.Escape || Char.IsNumber(ch) || 190 == ch)
            {
                bAutoComplete = false;
                lbAutoComplete.Hide();
                rtfBoxScript.SelectionStart = intPosCursor;
                rtfBoxScript.SelectionLength = 0;
                if (Char.IsNumber(ch)) rtfBoxScript.SelectedText = ch.ToString();
                rtfBoxScript.Focus();
                return; // if escape only return
            }
            string fName, fPosition;
            if (lbAutoComplete.SelectedIndex == -1) return;

            if (e.KeyCode == Keys.Enter)
            {
                if (GetIndexByFunction(lbAutoComplete.SelectedItem.ToString()) != -1)
                {
                    fName = sFunFormula[GetIndexByFunction(lbAutoComplete.SelectedItem.ToString())];
                    fPosition = sFunCursor[GetIndexByFunction(lbAutoComplete.SelectedItem.ToString())];
                    upis(fName, Convert.ToInt32(fPosition));
                    bAutoComplete = false;
                    lbAutoComplete.Hide();
                }
            }
        }

        private void lbAutoComplete_MouseUp(object sender, MouseEventArgs e)
        {

            // simulate press ENTER key
            PreviewKeyDownEventArgs pkdea = new PreviewKeyDownEventArgs(Keys.Enter);
            lbAutoComplete_PreviewKeyDown(sender, pkdea);
        }


        private Int32 GetIndexByFunction(string sFuncName)
        {
            for (i = 0; i < sFunFormula.GetUpperBound(0) - 1; i++)
            {
                if (sFunName[i] != null)
                {
                    if (sFunName[i].ToString() == sFuncName)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void rtfBoxScript_TextChanged(object sender, EventArgs e)
        {
            Int32 intStartPosition = rtfBoxScript.SelectionStart;
            intPosCursor = intStartPosition;
            Int32 intPosition;
            Int32 intFind;
            Int32 i = 0;
            String sWord = "";

            if (rtfBoxScriptChange)
            {
                intPosition = rtfBoxScript.SelectionStart;
                i = intPosition - 1;
                // test test test
                if (e != null)
                {
                    if (rtfBoxScript.InvokeRequired)
                    {
                        ParseAllGeneral pa = new ParseAllGeneral(this.ParseALL);                        
                        backgroundWorker1.WorkerSupportsCancellation = true;
                        pa(rtfBoxScript.Text, rtfBoxScript);
                        //this.Invoke(pa, new object(this.rtfBoxScript.Text, rtfBoxScript));
                    }
                    else
                    {
                        Int32 selStart = rtfBoxScript.SelectionStart;
                        ParseALL(rtfBoxScript.Text, rtfBoxScript);
                        rtfBoxScript.SelectionStart = selStart;
                    }
                }
                rtfBoxScript.SelectionStart = intPosition;
                rtfBoxScript.SelectionLength = 0;
                rtfBoxScriptChange = false;
                return; // exit from procedure down is for autocompleted option
            }

            // ovdje ugradi intellisense
            // za pocetak recimo samo za naredbe Math.

            intPosition = rtfBoxScript.SelectionStart;
            i = intPosition - 1;
            if (intPosition > 0)
            {
                intFind = rtfBoxScript.Find(".", intPosition - 1, RichTextBoxFinds.None);
                /*&& ((intFind + 1) >= intPosition */
                if ((intFind != -1 && (intFind - 1) <= intPosition) && (intFind == i) && (rtfBoxScript.GetLineFromCharIndex(intPosition) == rtfBoxScript.GetLineFromCharIndex(intFind)))
                {
                    lbAutoComplete.Items.Clear();
                    // current Line
                    while ((i >= 0 && IsLetter(rtfBoxScript.Text.Substring(i, 1))) || (i >= 0 && rtfBoxScript.Text.Substring(i, 1) == "."))
                    {
                        sWord = rtfBoxScript.Text.Substring(i, 1) + sWord;
                        i -= 1;
                    }
                    //MessageBox.Show(" word " + sWord);
                    // prijeđi po funkcijama i pronađi iste

                    // extra glupo rješnje broja sa točkom npr. 12.1
                    if (sWord == ".")
                    {
                        bAutoComplete = false;
                        rtfBoxScript.SelectionStart = intStartPosition;
                        rtfBoxScript.SelectionLength = 0;
                        return;
                    }

                    for (i = 0; i < sFunFormula.GetUpperBound(0) - 1; i++)
                    {
                        if (sFunFormula[i] != null)
                        {
                            if (Regex.IsMatch(sFunFormula[i].ToString(), sWord))
                            {
                                // visible formul OR unvisible
                                bAutoComplete = true;
                                lbAutoComplete.Items.Add(sFunName[i]);
                            }
                        }
                    }
                    // if find them, display Autocomplete ComboBox

                    if (bAutoComplete)
                    {
                        rtfBoxScript.SelectionStart = intPosition - sWord.Length;
                        rtfBoxScript.SelectionLength = sWord.Length;
                        rtfBoxScript.SelectedText = "";

                        Point p = rtfBoxScript.GetPositionFromCharIndex(rtfBoxScript.SelectionStart);
                        p.Y += panelDisplay.Location.Y + rtfBoxScript.Location.Y + (int)rtfBoxScript.Font.GetHeight() * 2;
                        p.X += panelDisplay.Location.X + rtfBoxScript.Location.X;
                        lbAutoComplete.Location = p;
                        lbAutoComplete.Height = 150;
                        bAutoComplete = false;
                        lbAutoComplete.Show();
                        lbAutoComplete.Focus();
                    }
                }
                else
                {
                    rtfBoxScript.SelectionStart = intStartPosition;
                    rtfBoxScript.SelectionLength = 0;
                }
            }

        }
        private Boolean IsLetter(String s)
        {
            return (s.Length == 1 && char.IsLetter(s, 0));
        }


        private void rtfBoxScript_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                // ovo služi za bojanje
                rtfBoxScriptChange = true;
                rtfBoxScript_TextChanged(sender, e);
            }
            else
                rtfBoxScriptChange = false;
        }

        private void panelDisplay_Resize(object sender, EventArgs e)
        {
            iOrigpanelDisplayWidht = panelDisplay.Width;
            iOrigpanelDisplayLeft = panelDisplay.Left;
            panelDisplay.Refresh();
        }

        private void FormKCalc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.O) // open file
            {
                //fnkOpen();
                fnkOpenRTF();
            }

            if (e.KeyCode == Keys.Control || e.KeyCode == Keys.S) // save file
            {
                //fnkSave();
                fnkSaveRTF();
            }
        }

        private void FormKCalc_BeforeResize(object sender, EventArgs e)
        {
            if (txtHistory.Dock == DockStyle.Fill)
                txtHistory_MouseDoubleClick(null, null);
            if (rtfBoxScript.Dock == DockStyle.Fill)
                rtfBoxScript_MouseDoubleClick(null, null);

        }

        private void TitleFileName()
        {
            FormKCalc.ActiveForm.Text = "kCalc " + currentFile;
        }

        private void SaveObjects()
        {
            if (Properties.Settings.Default.SaveObjects == false) return;
            //return;
            // save all vital objects to binary file
            object obj = kCalc.FormKCalc.ActiveForm;

            BinaryFormatter serializer = new BinaryFormatter();
            FileStream saveFile = new FileStream("kCalc.bin", FileMode.Create, FileAccess.Write);
            serializer.Serialize(saveFile, rtfBoxScript.Text);
            serializer.Serialize(saveFile, txtHistory.Text);
            saveFile.Flush();
            saveFile.Close();
        }

        private void LoadObjects()
        {
            BinaryFormatter serializer = new BinaryFormatter();
            FileStream openFile = new FileStream("kCalc.bin", FileMode.OpenOrCreate, FileAccess.Read);
            try
            {
                if (currentFile.ToString() == "")
                {
                    rtfBoxScript.Text = serializer.Deserialize(openFile).ToString();
                    rtfBoxScriptChange = true;
                    rtfBoxScript_TextChanged(null, null);
                }
                txtHistory.Text = serializer.Deserialize(openFile).ToString();
                openFile.Close();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
                openFile.Close();
            }
        }

        private void RecentFile(string FileName)
        {
            FilePointer = Properties.Settings.Default.RFilePointer + 1;
            if (FilePointer > 5)
            {
                FilePointer = 1;
                Properties.Settings.Default.RFilePointer = FilePointer;
                Properties.Settings.Default.Save();
            }

            switch (FilePointer)
            {
                case 1:
                    Properties.Settings.Default.RFile1 = FileName;
                    Properties.Settings.Default.RFilePointer = FilePointer;
                    Properties.Settings.Default.Save();
                    miRF = new MenuItem(FileName);
                    this.miRF.Click += new System.EventHandler(this.miRF_Click);
                    if (menuItem9.MenuItems.Count == 0)
                        menuItem9.MenuItems.Add(miRF);
                    else
                        menuItem9.MenuItems[0].Text = FileName;
                    break;
                case 2:
                    Properties.Settings.Default.RFile2 = FileName;
                    Properties.Settings.Default.RFilePointer = FilePointer;
                    Properties.Settings.Default.Save();
                    miRF = new MenuItem(FileName);
                    this.miRF.Click += new System.EventHandler(this.miRF_Click);
                    if (menuItem9.MenuItems.Count <= 1)
                        menuItem9.MenuItems.Add(miRF);
                    else
                        menuItem9.MenuItems[1].Text = FileName;
                    break;
                case 3:
                    Properties.Settings.Default.RFile3 = FileName;
                    Properties.Settings.Default.RFilePointer = FilePointer;
                    Properties.Settings.Default.Save();
                    miRF = new MenuItem(FileName);
                    this.miRF.Click += new System.EventHandler(this.miRF_Click);
                    if (menuItem9.MenuItems.Count <= 2)
                        menuItem9.MenuItems.Add(miRF);
                    else
                        menuItem9.MenuItems[2].Text = FileName;
                    break;
                case 4:
                    Properties.Settings.Default.RFile4 = FileName;
                    Properties.Settings.Default.RFilePointer = FilePointer;
                    Properties.Settings.Default.Save();
                    miRF = new MenuItem(FileName);
                    this.miRF.Click += new System.EventHandler(this.miRF_Click);
                    if (menuItem9.MenuItems.Count <= 3)
                        menuItem9.MenuItems.Add(miRF);
                    else
                        menuItem9.MenuItems[3].Text = FileName;
                    break;
                case 5:
                    Properties.Settings.Default.RFile5 = FileName;
                    Properties.Settings.Default.RFilePointer = FilePointer;
                    Properties.Settings.Default.Save();
                    miRF = new MenuItem(FileName);
                    this.miRF.Click += new System.EventHandler(this.miRF_Click);
                    if (menuItem9.MenuItems.Count <= 4)
                        menuItem9.MenuItems.Add(miRF);
                    else
                        menuItem9.MenuItems[4].Text = FileName;
                    break;
            }
            // after all reorder to context menu
            //Properties.Settings.Default.Reload();

        }

        private void LoadRecentFile()
        {
            FilePointer = Properties.Settings.Default.RFilePointer;
            if (Properties.Settings.Default.RFile1 != "")
            {
                miRF = new MenuItem(Properties.Settings.Default.RFile1);
                menuItem9.MenuItems.Add(miRF);
            }
            if (Properties.Settings.Default.RFile2 != "")
            {
                miRF = new MenuItem(Properties.Settings.Default.RFile2);
                this.miRF.Click += new System.EventHandler(this.miRF_Click);
                menuItem9.MenuItems.Add(miRF);
            }
            if (Properties.Settings.Default.RFile3 != "")
            {
                miRF = new MenuItem(Properties.Settings.Default.RFile3);
                this.miRF.Click += new System.EventHandler(this.miRF_Click);
                menuItem9.MenuItems.Add(miRF);
            }
            if (Properties.Settings.Default.RFile4 != "")
            {
                miRF = new MenuItem(Properties.Settings.Default.RFile4);
                this.miRF.Click += new System.EventHandler(this.miRF_Click);
                menuItem9.MenuItems.Add(miRF);
            }
            if (Properties.Settings.Default.RFile5 != "")
            {
                miRF = new MenuItem(Properties.Settings.Default.RFile5);
                this.miRF.Click += new System.EventHandler(this.miRF_Click);
                menuItem9.MenuItems.Add(miRF);
            }
        }


        private void contextMenu1_Popup(object sender, EventArgs e)
        {
            // otkrij otkuda je ovo pozvano i postavi kao aktivni objekt
            AccControl = contextMenu1.SourceControl;
            //strAccObjectName = contextMenu1.SourceControl.Name;            
        }


        private void FormKCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Properties.Settings.Default.SaveObjects)
            {
                SaveObjects();
                Properties.Settings.Default.Save();
            }
        }

        private void menuItem10_Click(object sender, EventArgs e)
        {
            AccControl.Text = "";
        }

        

        # region Print Section
        private void menuItemPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //printDoc = new TextDocument(AccControl.Text.Split(Environment.NewLine) );

                /* nothing predefined */
                if (printDia.ShowDialog() == DialogResult.OK)
                    printDoc.Print();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Print(ex.Message);
            }
        }

        private void printDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //TODO riješi ispis na više stranica
            //int pageNum;

            // ovdje možeš staviti sadržaj za prinanje
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;
            Int32 linesPerPage;

            // Calculate the number of lines per page.
            linesPerPage = (Int32)Math.Floor(e.MarginBounds.Height /
             Properties.Settings.Default.myPrinterFont.GetHeight(e.Graphics));

            e.Graphics.DrawString("kCalc print" + DateTime.Now.ToString(), Properties.Settings.Default.myPrinterFont, Brushes.Black, leftMargin + 2, topMargin);
            e.Graphics.DrawString(AccControl.Text, Properties.Settings.Default.myPrinterFont, Brushes.Black, leftMargin + 12, topMargin + 30);
            e.HasMorePages = true;
        }

        #endregion

        
    }

    //public class TextDocument : System.Drawing.Printing.PrintDocument
    //{
    //    private string[] text;
    //    private int pageNumber;
    //    private int offset;

    //    public string[] Text
    //    {
    //        get { return text; }
    //        set { text = value; }
    //    }

    //    public int PageNumber
    //    {
    //        get { return pageNumber; }
    //        set { pageNumber = value; }
    //    }

    //    public int Offset
    //    {
    //        get { return offset; }
    //        set { offset = value; }
    //    }

    //    public TextDocument(string[] text)
    //    {
    //        this.Text = text;
    //    }
    //}
}
