using System;
using System.ComponentModel;
using FreeSCADA.Common;
using Modbus.Data;
using System.Collections.Generic;


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
            else if (ushort.Parse(x.ModbusAddress) > ushort.Parse(y.ModbusAddress))
                return 1;
            else if (ushort.Parse(x.ModbusAddress) < ushort.Parse(y.ModbusAddress))
                return -1;
            else
                return 0;
        }
    }
}
