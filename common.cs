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


		public void TextBoxScroll(TextBox tb,long HorizScroll,long VertScroll )
		{
			SendMessage( (IntPtr) tb.Handle,182,(int) HorizScroll,VertScroll.ToString());
		}


		public string backResult(string formula)
		{
			formula = formula.Replace(Environment.NewLine,"");
			formula = formula.Replace("\n", null);
			string expr = @" var bEval = eval('" + formula + "','unsafe');	";            
			
			VsaEngine engine = VsaEngine.CreateEngine();            
			object o = null;
			
			try
			{
				o = Eval.JScriptEvaluate(expr,engine);
				return o.ToString();
			}
			catch(Exception ex)
			{
				return "ERROR " + ex.Message.ToString() + Environment.NewLine + " >>> " + ex.Source + " " + ex.StackTrace;
			}
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
        public int ValueInt;
        public string ValueString;

        public VariableS()
        {
            // to do something
        }        

    }

    public class FunctionS
    {
        public string Name;
        public string Contest;

    }
}
