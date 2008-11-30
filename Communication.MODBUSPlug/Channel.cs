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
        //ModbusDataType modType;

        string modbusStation;
        string modbusType;
        string modbusAddress;
        ModbusDataType modbusDataType;
        ModbusInternalType modbusInternalType;
        ushort modbusDataAddress;

        public ModbusChannelImp(string name, Plugin plugin, Type type, string modbusStation, string modbusType, string modbusAddress)
            : base(name, false, plugin, type)
		{
            this.modbusStation = modbusStation;
            this.modbusType = modbusType;
            this.modbusAddress = modbusAddress;
            this.modbusDataAddress = ushort.Parse(modbusAddress);
            switch (modbusType)
            {
                case "InputRegister":
                    modbusDataType = ModbusDataType.InputRegister;
                    break;
                case "Coil":
                    modbusDataType = ModbusDataType.Coil;
                    break;
                case "Input":
                    modbusDataType = ModbusDataType.Input;
                    break;
                case "HoldingRegister":
                    modbusDataType = ModbusDataType.HoldingRegister;
                    break;
            }
            if (type == typeof(int))
                modbusInternalType = ModbusInternalType.Integer;
            else if (type == typeof(uint))
                modbusInternalType = ModbusInternalType.Unsigned;
            else if (type == typeof(float))
                modbusInternalType = ModbusInternalType.Float;
        }

        public string ModbusStation
		{
            get { return modbusStation; }
		}

        public string ModbusType
        {
            get { return modbusType; }
        }

        public string ModbusAddress
		{
            get { return modbusAddress; }
		}

        public ModbusDataType ModbusDataType
		{
            get { return modbusDataType; }
		}

        public ModbusInternalType ModbusInternalType
        {
            get { return modbusInternalType; }
        }

        public ushort ModbusDataAddress
		{
            get { return modbusDataAddress; }
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

        /*public override void ExternalSetValue(object value)
        {
            base.ExternalSetValue(value);
        }*/
    }
}
