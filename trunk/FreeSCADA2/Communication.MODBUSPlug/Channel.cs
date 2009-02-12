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
        ushort deviceDataLen;
        ModbusConversionType conversionType;
        ModbusReadWrite modbusReadWrite;

        public ModbusChannelImp(string name, Plugin plugin, Type type, string modbusStation, ModbusDataTypeEx modbusType, ushort modbusAddress,
                                byte slaveId, ModbusDeviceDataType deviceDataType, ushort deviceDataLen, ModbusConversionType conversionType, ModbusReadWrite modbusReadWrite)
            : base(name, false, plugin, type)
		{
            this.modbusStation = modbusStation;
            this.modbusDataAddress = modbusAddress;
            this.modbusDataType = modbusType;
            if (type == typeof(int))
                modbusInternalType = ModbusFs2InternalType.Int32;
            else if (type == typeof(uint))
                modbusInternalType = ModbusFs2InternalType.UInt32;
            else if (type == typeof(double))
                modbusInternalType = ModbusFs2InternalType.Double;
            else if (type == typeof(bool))
                modbusInternalType = ModbusFs2InternalType.Boolean;
            else if (type == typeof(string))
                modbusInternalType = ModbusFs2InternalType.String;
            this.slaveId = slaveId;
            this.deviceDataType = deviceDataType;
            this.deviceDataLen = deviceDataLen;
            this.conversionType = conversionType;
            this.modbusReadWrite = modbusReadWrite;
            if (modbusReadWrite == ModbusReadWrite.ReadOnly)
                this.readOnly = true;
            this.BitIndex = 0;
            this.K = 1.0;
            this.D = 0.0;
        }

        public ModbusChannelImp(string name, ModbusChannelImp ch)
            : base(name, false, ch.plugin, ch.Type)
        {
            this.modbusStation = ch.modbusStation;
            this.modbusDataAddress = ch.modbusDataAddress;
            this.modbusDataType = ch.modbusDataType;
            this.modbusInternalType = ch.modbusInternalType;
            this.slaveId = ch.slaveId;
            this.deviceDataType = ch.deviceDataType;
            this.deviceDataLen = ch.deviceDataLen;
            this.conversionType = ch.conversionType;
            this.modbusReadWrite = ch.modbusReadWrite;
            if (modbusReadWrite == ModbusReadWrite.ReadOnly)
                this.readOnly = true;
            this.BitIndex = ch.BitIndex;
            this.K = ch.K;
            this.D = ch.D;
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

        public ModbusFs2InternalType ModbusFs2InternalType
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

        public ushort DeviceDataLen
		{
            get { return deviceDataLen; }
            set { deviceDataLen = value; }
        }

        public ModbusConversionType ConversionType
		{
            get { return conversionType; }
            set { conversionType = value; }
        }

        public ModbusReadWrite ModbusReadWrite
		{
            get { return modbusReadWrite; }
            set {
                modbusReadWrite = value;
                if (modbusReadWrite == ModbusReadWrite.ReadOnly)
                    this.readOnly = true;
                else
                    this.readOnly = false;
            }
        }

        public int BitIndex { get; set; }

        public double K { get; set; }
        public double D { get; set; }

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
