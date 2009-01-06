using FreeSCADA.Common;

namespace FreeSCADA.Archiver
{
	class PropertyCommand: BaseCommand
	{
		public PropertyCommand()
		{
			CanExecute = true;
		}

		public override string Name { get { return StringConstants.PropertyCommandName; } }
		public override string Description { get { return StringConstants.PropertyCommandName; } }
		public override System.Drawing.Bitmap Icon { get { return null; } }

		public override void Execute()
		{
			DatabaseSettingsForm form = new DatabaseSettingsForm();
			form.ShowDialog(Env.Current.MainWindow);
		}
	}
}
