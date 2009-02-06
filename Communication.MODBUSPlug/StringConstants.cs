
namespace FreeSCADA.Communication.MODBUSPlug
{
	public abstract class StringConstants
	{
		public static string PluginName = "MODBUS Connection Plugin";
		public static string PluginId = "modbus_connection_plug";

        public static string PropertyCommandName = "MODBUS properties...";
		public static string CommunicationGroupName = "Communication";

        public static string NameAssigned = "Name already assigned to another Station!";
        public static string VariablesExist = "Variables of this station exist! Cannot be deleted!";
        public static string Error = "Error!";
        public static string CannotCreateVariable = "Cannot create variable - no station definition exists";
    }
}
