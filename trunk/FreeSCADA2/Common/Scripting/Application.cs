using FreeSCADA.Interfaces;

namespace FreeSCADA.Common.Scripting
{
	public class Application
	{
		public delegate void OpenEntityHandler(ProjectEntityType entity_type, string entityName);
		public event OpenEntityHandler OpenEntity;

		public IChannel GetChannel(string name)
		{
			return Env.Current.CommunicationPlugins.GetChannel(name);
		}

		public void OpenSchema(string name)
		{
			if (OpenEntity != null)
				OpenEntity(ProjectEntityType.Schema, name);
		}

		public void OpenVariables()
		{
			if (OpenEntity != null)
				OpenEntity(ProjectEntityType.VariableList, "");
		}
	}
}
