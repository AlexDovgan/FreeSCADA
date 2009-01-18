
namespace FreeSCADA.Communication.MODBUSPlug
{
    public enum ModbusStationType
    {
        TCPMaster,
        TCPSlave,
        SerialMaster,
        SerialSlave
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
        Float,
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

}
