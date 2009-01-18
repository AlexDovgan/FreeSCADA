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
            string slaveId;
            string sdeviceDataType;
            string deviceDataLen;
            string sconversionType;

            ModbusDataTypeEx modbusDataType;
            ModbusDeviceDataType deviceDataType;
            ModbusConversionType conversionType;

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
            try
            {
                slaveId = node.Attributes["slaveId"].Value;
            }
            catch { slaveId = "0"; };

            try
            {
                sdeviceDataType = node.Attributes["deviceDataType"].Value;
            }
            catch { sdeviceDataType = "UInt"; };
            switch (sdeviceDataType)
            {
                case "Bool":
                    deviceDataType = ModbusDeviceDataType.Bool;
                    break;
                case "UInt":
                    deviceDataType = ModbusDeviceDataType.UInt;
                    break;
                case "DInt":
                    deviceDataType = ModbusDeviceDataType.DInt;
                    break;
                case "DUInt":
                    deviceDataType = ModbusDeviceDataType.DUInt;
                    break;
                case "Float":
                    deviceDataType = ModbusDeviceDataType.Float;
                    break;
                case "String":
                    deviceDataType = ModbusDeviceDataType.String;
                    break;
                default:    //Int
                    deviceDataType = ModbusDeviceDataType.Int;
                    break;
            }
            try
            {
                deviceDataLen = node.Attributes["deviceDataLen"].Value;
            }
            catch { deviceDataLen = "1"; };
            try
            {
                sconversionType = node.Attributes["conversionType"].Value;
            }
            catch { sconversionType = "SwapNone"; };
            switch (sconversionType)
            {
                case "SwapAll":
                    conversionType = ModbusConversionType.SwapAll;
                    break;
                case "SwapBytes":
                    conversionType = ModbusConversionType.SwapBytes;
                    break;
                case "SwapWords":
                    conversionType = ModbusConversionType.SwapWords;
                    break;
                default:
                    conversionType = ModbusConversionType.SwapNone;
                    break;
            }

            Type t = Type.GetType("System." + type);

            return CreateChannel(name, plugin, t, modbusStation, modbusDataType, ushort.Parse(modbusAddress), byte.Parse(slaveId),
                                 deviceDataType, int.Parse(deviceDataLen), conversionType);
        }

        public static IChannel CreateChannel(string name, Plugin plugin, Type type, string modbusStation, ModbusDataTypeEx modbusType, ushort modbusAddress,
                                            byte slaveId, ModbusDeviceDataType deviceDataType, int deviceDataLen, ModbusConversionType conversionType)
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
