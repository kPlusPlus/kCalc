using System;
using System.IO;
using System.Resources;
using System.Reflection;

namespace NScript
{
	/// <summary>
	/// Summary description for BaseApp.
	/// </summary>
	public abstract class BaseApp : MarshalByRefObject, IScriptManagerCallback
	{
		private static ResourceManager resMgr = new ResourceManager("NScript.NScript", typeof(BaseApp).Assembly);
		private AppDomain executionDomain;
		private string fileName;

		public BaseApp()
		{
		}
		
		#region Overridables for derived classes
		protected void TerminateExecution()
		{
			AppDomain.Unload(executionDomain);
		}
		
		protected abstract void ExecutionLoop(IAsyncResult result);
		protected abstract void TerminateExecutionLoop();
		protected abstract void ShowErrorMessage(string message);

		#endregion

		#region Utility function for derived classes		
		protected string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		protected string EntryAssemblyName
		{
			get
			{
				return Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
			}
		}

		protected void ShowErrorMessage(string message, params object[] args)
		{
			ShowErrorMessage(String.Format(message, args));
		}

		protected void ShowErrorMessageFromResource(string id, params object[] args)
		{
			ShowErrorMessage(resMgr.GetString(id), args);
		}
		
		protected static string GetResourceString(string name)
		{
			return resMgr.GetString(name);
		}

		protected static object GetResourceObject(string name)
		{
			return resMgr.GetObject(name);
		}
		#endregion

		#region Other Utility Methods
		private void ShowUsage()
		{
			ShowErrorMessageFromResource("Usage", EntryAssemblyName);
		}
		#endregion
		
		delegate void CompileAndExecuteRoutine(string file, string[] args, IScriptManagerCallback callback);

		private void CompileAndExecute(string file, string[] args, IScriptManagerCallback callback)
		{
			try
			{
				//Create an AppDomain to compile and execute the code
				//This enables us to cancel the execution if needed
				executionDomain = AppDomain.CreateDomain("ExecutionDomain");
				IScriptManager manager = (IScriptManager)executionDomain.CreateInstanceFromAndUnwrap(typeof(BaseApp).Assembly.Location, typeof(ScriptManager).FullName); 

				manager.CompileAndExecuteFile(file, args, this);
			}
			catch(UnsupportedLanguageExecption e)
			{
				ShowErrorMessageFromResource("UnsupportedLanguage", e.Extension);
			}
			catch(AppDomainUnloadedException e)
			{
				System.Diagnostics.Trace.WriteLine(e.Message);
			}
			catch(Exception e)
			{
				ShowErrorMessage(e.Message);
			}
			
			TerminateExecutionLoop();
		}

		protected void Run(string[] args)
		{
			if (args.Length < 1)
			{
				ShowUsage();
				return;
			}
			
			fileName = args[0];

			if (!File.Exists(fileName))
			{
				ShowErrorMessageFromResource("FileDoesnotExist", args[0]);
				return;
			}
		
			//Create new argument array removing the file name
			string[] newargs = new String[args.Length - 1];
			Array.Copy(args, 1, newargs, 0, args.Length - 1);
			
			CompileAndExecuteRoutine asyncDelegate = new CompileAndExecuteRoutine(this.CompileAndExecute);
			IAsyncResult result = asyncDelegate.BeginInvoke(fileName, newargs, this, null, null);
			
			//For a windows app a message loop and for a console app a simple wait
			ExecutionLoop(result);

			asyncDelegate.EndInvoke(result);
		}

		#region Implementation of IScriptManagerCallback
		public void OnCompilerError(CompilerError[] errors)
		{
			StringWriter writer = new StringWriter();
				
			string errorFormat = BaseApp.GetResourceString("CompilerErrorFormat");
				
			foreach(CompilerError error in errors)
			{
				writer.WriteLine(errorFormat, error.File, error.Number, error.Text, error.Line, error.Column);
			}
				
			throw new ApplicationException(writer.ToString());
		}
		#endregion
	}
}
