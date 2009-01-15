
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
        Int32,
        UInt32,
        Float
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

}
