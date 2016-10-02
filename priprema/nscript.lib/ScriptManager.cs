using System;
using System.IO;
using System.CodeDom;
using System.Configuration;
using System.Reflection;
using System.CodeDom.Compiler;

namespace NScript
{
	/// <summary>
	/// Summary description for ScriptManager.
	/// </summary>
	public class ScriptManager : MarshalByRefObject, IScriptManager
	{
		public ScriptManager()
		{
		}
		
		private void AddReferencesFromFile(CompilerParameters compilerParams, string nrfFile)
		{
			using(StreamReader reader = new StreamReader(nrfFile))
			{
				string line;;

				while((line  = reader.ReadLine()) != null)
					compilerParams.ReferencedAssemblies.Add(line);
			}
		}

		#region Implementation of IScriptManager
		public void CompileAndExecuteFile(string file, string[] args, IScriptManagerCallback callback)
		{
			//Currently only csharp scripting is supported
			CodeDomProvider provider;
			
			string extension = Path.GetExtension(file);

			switch(extension)
			{
				case ".cs":
				case ".ncs":
					provider = new Microsoft.CSharp.CSharpCodeProvider();
					break;
				case ".vb":
				case ".nvb":
					provider = new Microsoft.VisualBasic.VBCodeProvider();
					break;
				case ".njs":
				case ".js":
					provider = (CodeDomProvider)Activator.CreateInstance("Microsoft.JScript", "Microsoft.JScript.JScriptCodeProvider").Unwrap();
					break;
				default:
					throw new UnsupportedLanguageExecption(extension);
			}

            ICodeCompiler compiler = provider.CreateCompiler();

            CompilerParameters compilerparams = new CompilerParameters();
			compilerparams.GenerateInMemory = true;
			compilerparams.GenerateExecutable = true;
			
			//Add assembly references from nscript.nrf or <file>.nrf
			string nrfFile = Path.ChangeExtension(file, "nrf");

			if (File.Exists(nrfFile))
				AddReferencesFromFile(compilerparams, nrfFile);
			else
			{
				//Use nscript.nrf
				nrfFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "nscript.nrf");

				if (File.Exists(nrfFile))
					AddReferencesFromFile(compilerparams, nrfFile);
			}

			System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, file);

			if (results.Errors.HasErrors)
			{
				System.Collections.ArrayList templist = new System.Collections.ArrayList();

				foreach(System.CodeDom.Compiler.CompilerError error in results.Errors)
				{
					templist.Add(new CompilerError(error));
				}

				callback.OnCompilerError((CompilerError[])templist.ToArray(typeof(CompilerError)));
			}
			else
			{
				results.CompiledAssembly.EntryPoint.Invoke(null, BindingFlags.Static, null, new object[]{args}, null);
			}
		}
		#endregion
	}
}
