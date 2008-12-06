
namespace FreeSCADA.Interfaces
{
	public enum EnvironmentMode
	{
		Designer,
		Runtime
	}

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

		EnvironmentMode Mode
		{
			get;
		}
	}
}
