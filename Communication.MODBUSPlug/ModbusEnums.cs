
namespace FreeSCADA.Communication.MODBUSPlug
{
    enum ModbusStationType
    {
        TCPMaster,
        TCPSlave,
        SerialMaster,
        SerialSlave
    }
    
    enum ModbusConversionType
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
}
