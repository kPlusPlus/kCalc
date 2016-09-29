using System;

namespace NScript
{
	/// <summary>
	/// Summary description for CompilerError.
	/// </summary>
	[Serializable]
	public class CompilerError
	{
		private int line;
		private string file;
		private int column;
		private string text;
		private string number;
		
		public int Line
		{
			get
			{
				return line;
			}
			set
			{
				line = value;
			}
		}

		public string File
		{
			get
			{
				return file;
			}
			set
			{
				file = value;
			}
		}

		public int Column
		{
			get
			{
				return column;
			}
			set
			{
				column = value;
			}
		}

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		public string Number
		{
			get
			{
				return number;
			}
			set
			{
				number = value;
			}
		}

		public CompilerError()
		{
		}
	
		public CompilerError(System.CodeDom.Compiler.CompilerError error)
		{
			this.column = error.Column;
			this.file = error.FileName;
			this.line = error.Line;
			this.number = error.ErrorNumber;
			this.text = error.ErrorText;
		}
	}
}
