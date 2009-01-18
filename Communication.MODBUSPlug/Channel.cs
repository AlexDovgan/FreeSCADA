using System;
using System.Collections.Generic;
using FreeSCADA.Common;
using Modbus.Data;


namespace FreeSCADA.Communication.MODBUSPlug
{
    /// <summary>
    /// TODO:  may be need to implement one abstract base class for implementation base functionality with 
    /// events
    /// </summary>
    public class ModbusChannelImp:BaseChannel, IComparer<ModbusChannelImp>
	{
        string modbusStation;
        ModbusDataTypeEx modbusDataType;
        ModbusFs2InternalType modbusInternalType;
        ushort modbusDataAddress;
        byte slaveId;
        ModbusDeviceDataType deviceDataType;
        int deviceDataLen;
        ModbusConversionType conversionType;

        public ModbusChannelImp(string name, Plugin plugin, Type type, string modbusStation, ModbusDataTypeEx modbusType, ushort modbusAddress,
                                byte slaveId, ModbusDeviceDataType deviceDataType, int deviceDataLen, ModbusConversionType conversionType)
            : base(name, false, plugin, type)
		{
            this.modbusStation = modbusStation;
            this.modbusDataAddress = modbusAddress;
            this.modbusDataType = modbusType;
            if (type == typeof(int))
                modbusInternalType = ModbusFs2InternalType.Int32;
            else if (type == typeof(uint))
                modbusInternalType = ModbusFs2InternalType.UInt32;
            else if (type == typeof(float))
                modbusInternalType = ModbusFs2InternalType.Float;
            this.slaveId = slaveId;
            this.deviceDataType = deviceDataType;
            this.deviceDataLen = deviceDataLen;
            this.conversionType = conversionType;
        }

        public string ModbusStation
		{
            get { return modbusStation; }
            set { modbusStation = value; }
        }

        public ModbusDataTypeEx ModbusDataType
		{
            get { return modbusDataType; }
            set { modbusDataType =value; }
        }

        public ModbusFs2InternalType ModbusInternalType
        {
            get { return modbusInternalType; }
            set { modbusInternalType = value; }
        }

        public ushort ModbusDataAddress
		{
            get { return modbusDataAddress; }
            set { modbusDataAddress = value; }
        }

        public byte SlaveId
		{
            get { return slaveId; }
            set { slaveId = value; }
        }

        public ModbusDeviceDataType DeviceDataType
		{
            get { return deviceDataType; }
            set { deviceDataType = value; }
        }

        public int DeviceDataLen
		{
            get { return deviceDataLen; }
            set { deviceDataLen = value; }
        }

        public ModbusConversionType ConversionType
		{
            get { return conversionType; }
            set { conversionType = value; }
        }

        public override void DoUpdate()
        {
        }

        // Summary:
        //     Compares two objects and returns a value indicating whether one is less than,
        //     equal to, or greater than the other.
        //
        // Parameters:
        //   x:
        //     The first object to compare.
        //
        //   y:
        //     The second object to compare.
        //
        // Returns:
        //     Value Condition Less than zero x is less than y.  Zero x equals y.  Greater
        //     than zero x is greater than y.
        public int Compare(ModbusChannelImp x, ModbusChannelImp y)
        {
            if (x.SlaveId > y.SlaveId)
                return 1;
            if (x.SlaveId < y.SlaveId)
                return -1;
            if (x.ModbusDataType > y.ModbusDataType)
                return 1;
            if (x.ModbusDataType < y.ModbusDataType)
                return -1;
            else if (x.modbusDataAddress > y.modbusDataAddress)
                return 1;
            else if (x.modbusDataAddress < y.modbusDataAddress)
                return -1;
            else
                return 0;
        }
    }

}
