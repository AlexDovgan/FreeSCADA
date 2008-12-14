using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
	public class NullCommand : BaseCommand
	{
		public NullCommand()
		{
		}
		public NullCommand(int priority)
		{
			Priority = priority;
		}

		public override string Name { get { return ""; } }
		public override string Description { get { return ""; } }
		public override CommandType Type { get { return CommandType.Separator; } }
	}
}
