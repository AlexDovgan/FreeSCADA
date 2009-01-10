
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

    public enum ModbusInternalType
    {
        Integer,
        Unsigned,
        Float
    }

    public enum ModbusSerialType
    {
        RTU,
        ASCII
    }
}
