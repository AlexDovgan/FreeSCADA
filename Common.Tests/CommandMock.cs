
namespace FreeSCADA.Common.Tests
{
	class CommandMock: BaseCommand
	{
		string name;
		public bool isExecuted = false;
		public CommandMock(string name)
		{
			this.name = name;
			CanExecute = true;
		}

		public override string Name { get { return name; } }
		public override string Description { get { return ""; } }

		public override void Execute()
		{
			isExecuted = true;
		}
	}
}
