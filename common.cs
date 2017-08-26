using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.JScript;
using Microsoft.JScript.Vsa;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace kCalc
{
    /// <summary>
    /// kCalc --- mmodul for run JavaScript
    /// </summary>
    public class common
    {
        // INIT
        public common()
        {
            // on INIT load all ReservedWords
        }

        public VariableS[] Variables;
        private VariableS _Var;
        public FunctionS[] Functions;
        private FunctionS _Fun;

        [System.Runtime.InteropServices.DllImport("user32.DLL")]
        private extern static int SendMessage(
            System.IntPtr hWnd, int wMsg,
            int wParam, string lParam);


        public void TextBoxScroll(TextBox tb, long HorizScroll, long VertScroll)
        {
            SendMessage((IntPtr)tb.Handle, 182, (int)HorizScroll, VertScroll.ToString());
        }


        public string backResult(string formula)
        {
            formula = formula.Replace(Environment.NewLine, "");
            formula = formula.Replace("\n", null);
            string expr = @" var bEval = eval('" + formula + "','unsafe');	";

            VsaEngine engine = VsaEngine.CreateEngine();
            object o = null;

            try
            {
                o = Eval.JScriptEvaluate(expr, engine);
                return o.ToString();
            }
            catch (Exception ex)
            {
                return "ERROR " + ex.Message.ToString() + Environment.NewLine + " >>> " + ex.Source + " " + ex.StackTrace;
            }
        }

        public void SetVariable(string sCont)
        {
            StringCollection resultList = new StringCollection();

            Regex regexObj = new Regex(@"var (?<varname>([a-zA-Z0-9_-]{1,}))\s{0,3}=(?<varval>\s{0,3}\d{0,12}.\d{0,12})\s{0,3};", RegexOptions.Multiline | RegexOptions.ExplicitCapture);
            //Match matchResult = regexObj.Match(sCont);
            MatchCollection mc = regexObj.Matches(sCont);

            if (Variables != null) Variables = null;

            foreach(Match m in mc)
            {
                //MessageBox.Show(m.Groups["varname"].Value);
                if (Variables == null)
                    Variables = new VariableS[] { };
                int count = Variables.Length;
                Array.Resize(ref Variables, count + 1);
                Variables[count] = new VariableS();
                Variables[count].Name = m.Groups["varname"].Value;
                //string varval = m.Groups["varval"].Value.Replace(".", ",");
                //Variables[count].ValueDouble = double.Parse( varval );
                Variables[count].Parsiralica(m.Groups["varval"].Value);
            }

            //Form frm = new frmValue();
            frmValue frm = new frmValue();
            frm.com = this;

            dsVariables dsv = new dsVariables();
            DataRow dr;

            for(int i = 0; i < Variables.Length; i++)
            {
                dr = dsv.Tables["Variables"].NewRow();
                dr["Name"] = Variables[i].Name;
                dr["ValInt"] = Variables[i].ValueInt;
                dr["ValDouble"] = Variables[i].ValueDouble;
                dr["ValueType"] = Variables[i].ValueType;
                dsv.Tables["Variables"].Rows.Add(dr);
            }

            dsv.WriteXml("Variables.xml");
            frm.dsVariables1BindingSource.DataSource = dsv;

            frm.Show();            
        }


        /* * * new options * * */
        public static CompilerResults CompileCode(CodeDomProvider provider,
                                                  String sourceFile,
                                                  String exeFile)
        {
            // Configure a CompilerParameters that links System.dll
            // and produces the specified executable file.
            String[] referenceAssemblies = { "System.dll" };
            CompilerParameters cp = new CompilerParameters(referenceAssemblies,
                                                           exeFile, false);
            // Generate an executable rather than a DLL file.
            cp.GenerateExecutable = true;

            // Invoke compilation.
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFile);
            // Return the results of compilation.
            return cr;
        }

    }

    public class VariableS
    {
        public string Name;
        public int ValueType = 0;
        public string ValueString;
        public int ValueInt;
        public double ValueDouble;


        public VariableS()
        {
            // to do something
        }

        //TODO testiraj parsiralicu
        public void Parsiralica(string sNumber)
        {
            if (Int32.TryParse(sNumber, out ValueInt) == true)
            {
                ValueType = 1;
                return;
            }
            string varval = sNumber.Replace(".", ",");
            if (double.TryParse(varval, out ValueDouble) == true)
            {
                ValueType = 2;
                return;
            }

        }

    }

    public class FunctionS
    {
        public string Name;
        public string Contest;

    }
}
