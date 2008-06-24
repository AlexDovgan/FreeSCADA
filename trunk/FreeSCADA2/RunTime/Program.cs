using System;
using System.Windows.Forms;

namespace FreeSCADA.RunTime
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args )
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
                Application.Run(new MainForm(args[0]));
            else
                Application.Run(new MainForm());
		}
    }
}
