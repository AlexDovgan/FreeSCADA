
namespace FreeSCADA.Communication.SNMPPlug
{
	public abstract class StringConstants
	{
		public static string PluginName = "SNMP Connection Plugin";
		public static string PluginId = "snmp_connection_plug";

        public static string PropertyCommandName = "SNMP properties...";
		public static string CommunicationGroupName = "Communication";

        public static string NameAssigned = "Name already assigned to another Entity!";
        public static string VariablesExist = "Variables of this agent exist! Cannot be deleted!";
        public static string Error = "Error!";
        public static string CannotCreateVariable = "Cannot create variable - no agent definition exists";
    }
}
