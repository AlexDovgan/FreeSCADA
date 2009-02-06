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
            byte slaveId = 0;
            string sdeviceDataType;
            ushort deviceDataLen = 1;
            string sconversionType;

            ModbusDataTypeEx modbusDataType;
            ModbusDeviceDataType deviceDataType;
            ModbusConversionType conversionType;

            modbusDataType = (ModbusDataTypeEx)Enum.Parse(typeof(ModbusDataTypeEx), modbusType);

            string modbusAddress = node.Attributes["modbusAddress"].Value;
            try
            {
                slaveId = byte.Parse(node.Attributes["slaveId"].Value);
            }
            catch { };

            try
            {
                sdeviceDataType = node.Attributes["deviceDataType"].Value;
            }
            catch { sdeviceDataType = "UInt"; };
            deviceDataType = (ModbusDeviceDataType)Enum.Parse(typeof(ModbusDeviceDataType), sdeviceDataType);
            
            try
            {
                deviceDataLen = ushort.Parse(node.Attributes["deviceDataLen"].Value);
            }
            catch { };
            
            try
            {
                sconversionType = node.Attributes["conversionType"].Value;
            }
            catch { sconversionType = "SwapNone"; };
            conversionType = (ModbusConversionType)Enum.Parse(typeof(ModbusConversionType), sconversionType);

            Type t = Type.GetType("System." + type);

            return CreateChannel(name, plugin, t, modbusStation, modbusDataType, ushort.Parse(modbusAddress), slaveId,
                                 deviceDataType, deviceDataLen, conversionType);
        }

        public static IChannel CreateChannel(string name, Plugin plugin, Type type, string modbusStation, ModbusDataTypeEx modbusType, ushort modbusAddress,
                                            byte slaveId, ModbusDeviceDataType deviceDataType, ushort deviceDataLen, ModbusConversionType conversionType)
        {
            return new ModbusChannelImp(name, plugin, type, modbusStation, modbusType, modbusAddress, slaveId, deviceDataType, deviceDataLen, conversionType);
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
            node.SetAttribute("deviceDataType", channelBase.DeviceDataType.ToString());
            node.SetAttribute("deviceDataLen", channelBase.DeviceDataLen.ToString());
            node.SetAttribute("conversionType", channelBase.ConversionType.ToString());
        }
    }
}
