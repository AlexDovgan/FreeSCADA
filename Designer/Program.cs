using System;
using System.Windows.Forms;

namespace FreeSCADA.Designer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(mf = new MainForm());
		}
        public static MainForm mf;
    }
}
