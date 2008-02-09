
namespace FreeSCADA.ShellInterfaces
{
	public interface IEnvironment
	{
		ICommands Commands
		{
			get;
		}

		System.Windows.Forms.Control MainWindow
		{
			get;
		}

		FreeSCADA.Common.Project Project
		{
			get;
		}
	}
}
