using System;
using System.Xml;
using FreeSCADA.Interfaces;

namespace FreeSCADA.Communication.MODBUSPlug
{
    sealed class ChannelFactory
    {
        //Prevent class reation
        private ChannelFactory() { }

        public static IChannel CreateChannel(XmlElement node, Plugin plugin)
        {
            string name = node.Attributes["name"].Value;
            string type = node.Attributes["type"].Value;
            string modbusStation = node.Attributes["modbusStation"].Value;
            string modbusType = node.Attributes["modbusType"].Value;
            ModbusDataTypeEx modbusDataType;
            switch (modbusType)
            {
                case "InputRegister":
                    modbusDataType = ModbusDataTypeEx.InputRegister;
                    break;
                case "Coil":
                    modbusDataType = ModbusDataTypeEx.Coil;
                    break;
                case "Input":
                    modbusDataType = ModbusDataTypeEx.Input;
                    break;
                case "HoldingRegister":
                    modbusDataType = ModbusDataTypeEx.HoldingRegister;
                    break;
                default:
                    modbusDataType = ModbusDataTypeEx.DeviceFailureInfo;
                    break;
            }
            string modbusAddress = node.Attributes["modbusAddress"].Value;
            string slaveId;
            try
            {
                slaveId = node.Attributes["slaveId"].Value;
            }
            catch { slaveId = "0"; };

            Type t = Type.GetType("System." + type);

            return CreateChannel(name, plugin, t, modbusStation, modbusDataType, ushort.Parse(modbusAddress), byte.Parse(slaveId));
        }

        public static IChannel CreateChannel(string name, Plugin plugin, Type type, string modbusStation, ModbusDataTypeEx modbusType, ushort modbusAddress, byte slaveId)
        {
            return new ModbusChannelImp(name, plugin, type, modbusStation, modbusType, modbusAddress, slaveId);
        }

        public static void SaveChannel(XmlElement node, IChannel channel)
        {
            ModbusChannelImp channelBase = (ModbusChannelImp)channel;
            node.SetAttribute("name", channelBase.Name);
            node.SetAttribute("type", channelBase.ModbusInternalType.ToString());
            node.SetAttribute("modbusStation", channelBase.ModbusStation);
            node.SetAttribute("modbusType", channelBase.ModbusDataType.ToString());
            node.SetAttribute("modbusAddress", channelBase.ModbusDataAddress.ToString());
            node.SetAttribute("slaveId", channelBase.SlaveId.ToString());
        }
    }
}
