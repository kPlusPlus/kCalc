using System;

namespace NScript
{
	/// <summary>
	/// Summary description for IScriptManagerCallback.
	/// </summary>
	public interface IScriptManagerCallback
	{
		void OnCompilerError(CompilerError[] errors);
	}
}
