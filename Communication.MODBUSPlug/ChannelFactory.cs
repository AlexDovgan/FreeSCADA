using System;
using System.Xml;
using FreeSCADA.Interfaces;
using System.Globalization;

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
            string smodbusReadWrite;

            ModbusDataTypeEx modbusDataType;
            ModbusDeviceDataType deviceDataType;
            ModbusConversionType conversionType;
            ModbusReadWrite modbusReadWrite;

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
            catch { sconversionType = ModbusConversionType.SwapNone.ToString(); };
            conversionType = (ModbusConversionType)Enum.Parse(typeof(ModbusConversionType), sconversionType);

            try
            {
                smodbusReadWrite = node.Attributes["modbusReadWrite"].Value;
            }
            catch { smodbusReadWrite = ModbusReadWrite.ReadOnly.ToString(); };
            modbusReadWrite = (ModbusReadWrite)Enum.Parse(typeof(ModbusReadWrite), smodbusReadWrite);

            Type t = Type.GetType("System." + type);

            ModbusChannelImp ch = (ModbusChannelImp) CreateChannel(name, plugin, t, modbusStation, modbusDataType, ushort.Parse(modbusAddress), slaveId,
                                 deviceDataType, deviceDataLen, conversionType, modbusReadWrite);
            try { ch.BitIndex = int.Parse(node.Attributes["bitIndex"].Value); }
            catch { }
            CultureInfo ci = CultureInfo.GetCultureInfo("en-US");
            try
            {
                ch.K = double.Parse(node.Attributes["k"].Value, NumberStyles.Float, ci.NumberFormat);
            }
            catch { }
            try
            {
                ch.D = double.Parse(node.Attributes["d"].Value, NumberStyles.Float, ci.NumberFormat);
            }
            catch { }

            return ch;
        }

        public static IChannel CreateChannel(string name, Plugin plugin, Type type, string modbusStation, ModbusDataTypeEx modbusType, ushort modbusAddress,
                                            byte slaveId, ModbusDeviceDataType deviceDataType, ushort deviceDataLen, ModbusConversionType conversionType, ModbusReadWrite modbusReadWrite)
        {
            return new ModbusChannelImp(name, plugin, type, modbusStation, modbusType, modbusAddress, slaveId, deviceDataType, deviceDataLen, conversionType, modbusReadWrite);
        }

        public static void SaveChannel(XmlElement node, IChannel channel)
        {
            ModbusChannelImp channelBase = (ModbusChannelImp)channel;
            node.SetAttribute("name", channelBase.Name);
            node.SetAttribute("type", channelBase.ModbusFs2InternalType.ToString());
            node.SetAttribute("modbusStation", channelBase.ModbusStation);
            node.SetAttribute("modbusType", channelBase.ModbusDataType.ToString());
            node.SetAttribute("modbusAddress", channelBase.ModbusDataAddress.ToString());
            node.SetAttribute("slaveId", channelBase.SlaveId.ToString());
            node.SetAttribute("deviceDataType", channelBase.DeviceDataType.ToString());
            node.SetAttribute("deviceDataLen", channelBase.DeviceDataLen.ToString());
            node.SetAttribute("conversionType", channelBase.ConversionType.ToString());
            node.SetAttribute("modbusReadWrite", channelBase.ModbusReadWrite.ToString());
            if (channelBase.BitIndex != 0 ) node.SetAttribute("bitIndex", channelBase.BitIndex.ToString());
            CultureInfo ci = CultureInfo.GetCultureInfo("en-US");
            if (channelBase.K != 1.0) node.SetAttribute("k", channelBase.K.ToString(ci.NumberFormat));
            if (channelBase.D != 0.0) node.SetAttribute("d", channelBase.D.ToString(ci.NumberFormat));
        }
    }
}
