using System;

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
        AsIs,
        SwapBytes,
        SwapWords,
        SwapAll
    }

    enum ModbusInternalType
    {
        Integer,
        Unsigned,
        Float
    }
}
