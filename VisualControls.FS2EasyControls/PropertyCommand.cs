using FreeSCADA.Common;
using FreeSCADA.Interfaces;
using System.Windows.Forms;

namespace FreeSCADA.VisualControls.FS2EasyControls
{
	class PropertyCommand : BaseCommand
	{
		Plugin plugin;

		public PropertyCommand(Plugin plugin)
		{
			this.plugin = plugin;
			CanExecute = true;
		}

		public override string Name { get { return StringConstants.PropertyCommandName; } }
		public override string Description { get { return StringConstants.PropertyCommandName; } }
		public override System.Drawing.Bitmap Icon { get { return null; } }

		public override void Execute()
		{
            string s = "FreeSCADA2 EasyControls Library.\nAn example of building and using third-party user controls in FS2 environment.\n\nAvailable controls:\n";
            foreach (IVisualControlDescriptor c in plugin.Controls)
            {
                s += c.Name;
                s += "\n";
            }
            MessageBox.Show( s + "\nNo global settings available", "FreeSCADA2 EasyControls");
		}
	}
}
