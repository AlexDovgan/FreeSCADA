using NUnit.Extensions.Forms;

namespace FreeSCADA.Archiver.Tests
{
	class GridTester : ControlTester
	{
		public GridTester(string controlName, string formName)
			: base(controlName, formName)
		{

		}

		new public SourceGrid.Grid Control
		{
			get
			{
				return (SourceGrid.Grid)base.Control;
			}
		}

		public void SetChannelType(int channelNo, string type)
		{
			Control[channelNo + 1, 1].Value = type;
		}

		public void SetChannelName(int channelNo, string name)
		{
			Control[channelNo + 1, 0].Value = name;
		}
	}
}
