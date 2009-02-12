
namespace FreeSCADA.Communication.MODBUSPlug
{
    public enum ModbusStationType
    {
        TCPMaster,
        SerialMaster,
        //TCPSlave,
        //SerialSlave
    }

    public enum ModbusConversionType
    {
        SwapNone,
        SwapBytes,
        SwapWords,
        SwapAll
    }
    
    public enum ModbusFs2InternalType
    {
        Boolean,
        Int32,
        UInt32,
        Double,
        String
    }

    public enum ModbusSerialType
    {
        RTU,
        ASCII
    }

    public enum ModbusDataTypeEx
    {
        HoldingRegister = 0,
        InputRegister = 1,
        Coil = 2,
        Input = 3,
        DeviceFailureInfo = 4
    }

    public enum ModbusDeviceDataType
    {
        Bool,
        Int,
        UInt,
        DInt,
        DUInt,
        Float,
        String
    }

    public enum ModbusReadWrite
    {
        ReadOnly,
        WriteOnly,
        ReadAndWrite
    }

    public struct ModbusLog
    {
        public const int logNone = 0;
        public const int logErrors = 1;
        public const int logWarnings = 2;
        public const int logInfos = 3;
        public const int logDebug = 4;
    }
}
