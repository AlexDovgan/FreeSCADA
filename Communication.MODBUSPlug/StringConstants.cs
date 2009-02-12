
namespace FreeSCADA.Communication.MODBUSPlug
{
	public abstract class StringConstants
	{
		public static string PluginName = "MODBUS Connection Plugin";
		public static string PluginId = "modbus_connection_plug";

        public static string PropertyCommandName = "MODBUS properties...";
		public static string CommunicationGroupName = "Communication";

        public static string NameAssigned = "Name already assigned to another Entity!";
        public static string VariablesExist = "Variables of this station exist! Cannot be deleted!";
        public static string Error = "Error!";
        public static string CannotCreateVariable = "Cannot create variable - no station definition exists";
        public static string ReadingValues = "Error when reading values. Check parameters!";
        public static string ErrConvert = "MODBUS/TCP Station '{0}' channel '{1}', error converting from data type {2} to {3}";
        public static string ErrException = "MODBUS/TCP Station '{0}' exception: {1}";
        public static string InfoTCPStarting = "MODBUS/TCP Station '{0}' Info: Starting new TcpClient {1}, {2}";
        public static string InfoTCPStarted = "MODBUS/TCP Station '{0}' Info: TCP Socket to {1}, {2} successfully started";
        public static string ErrReceive = "MODBUS/TCP Station '{0}', Error reading buffer from slave {1}, data type {2}, data address {3}, number of adr. {4}, message: {5}.";
    }
}
