using FreeSCADA.Interfaces;

namespace FreeSCADA.Common
{
	public class SubMenuCommand : BaseCommand
	{
        string _text;
        string _description;
		public SubMenuCommand (string text,string descr)
		{
            _text = text;
            _description = descr;
		}
        public SubMenuCommand(int priority)
		{
			Priority = priority; 
		}

		public override string Name { get { return _text; } }
		public override string Description { get { return _description; } }
		public override CommandType Type { get { return CommandType.Submenu; } }
	}
}
