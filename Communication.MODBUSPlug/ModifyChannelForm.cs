using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace FreeSCADA.Communication.MODBUSPlug
{
    public partial class ModifyChannelForm : Form
    {
        bool test = false;
        List<string> forbiddenNames;
        List<string> stations;
        string selectedStation;

        public ModifyChannelForm(ModbusChannelImp ch, List<string> forbiddenNames, List<string> stations, string selectedStation)
        {
            InitializeComponent();
            InitializeTooltips();
            this.Tag = ch;
            this.forbiddenNames = forbiddenNames;
            this.stations = stations;
            this.selectedStation = selectedStation;
            this.FormClosing += new FormClosingEventHandler(ModifyChannelForm_FormClosing);

            nameTextBox.Text = ch.Name;
            foreach (ModbusDataTypeEx s in Enum.GetValues(typeof(ModbusDataTypeEx)))
            {
                modbusDataTypeComboBox.Items.Add(s);
            }
            modbusDataTypeComboBox.SelectedItem = ch.ModbusDataType;

            foreach (string s in stations)
            {
                stationComboBox.Items.Add(s);
            }
            stationComboBox.SelectedItem = ch.ModbusStation;

            slaveIdUpDown.Value = ch.SlaveId;
            modbusDataAddressNumericUpDown.Value = ch.ModbusDataAddress;
            deviceDataLenNumericUpDown.Value = ch.DeviceDataLen;
            bitIndexNumericUpDown.Value = ch.BitIndex;
            CultureInfo ci = CultureInfo.GetCultureInfo("en-US");
            kMaskedTextBox.Text = ch.K.ToString(ci.NumberFormat);
            dMaskedTextBox.Text = ch.D.ToString(ci.NumberFormat);


            MakeControlsValidation(ch);

            // must be AFTER makeControlValidation
            modbusDataTypeComboBox.SelectedIndexChanged += new EventHandler(modRegisterComboBox_SelectedIndexChanged);
            deviceDataTypeComboBox.SelectedIndexChanged += new EventHandler(modRegisterComboBox_SelectedIndexChanged);
            modbusFs2InternalTypeComboBox.SelectedIndexChanged += new EventHandler(modRegisterComboBox_SelectedIndexChanged);
        }

        void InitializeTooltips()
        {
            ToolTip expressionTooltip = new ToolTip();
            expressionTooltip.AutomaticDelay = 180000;
            expressionTooltip.InitialDelay = 100;
            expressionTooltip.ShowAlways = true;

            string tip = "Offset from the start register of the respective data space.\n\n" +
                "The IO ranges in MODICON PLC and other devices are:\n" +
                "00001 - 09999 Coils / Discrete Outputs\n" +
                "10001 - 19999 Inputs (Discrete, Read only)\n" +
                "30001 - 39999 Input registers/analog/counters (Read only)\n" +
                "40001 - 49999 Holding registers / analog output";
            expressionTooltip.SetToolTip(modbusDataAddressNumericUpDown, tip);
        }

        void ModifyChannelForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((sender as ModifyChannelForm).test)
            {
                ModbusChannelImp ch = (ModbusChannelImp)this.Tag;

                if (forbiddenNames != null && forbiddenNames.Contains(ch.Name))
                {
                    e.Cancel = true;
                    MessageBox.Show(StringConstants.NameAssigned);
                }
                (sender as ModifyChannelForm).test = false;
            }
        }

        void MakeControlsValidation(ModbusChannelImp ch)
        {
            modbusDataTypeComboBox.SelectedIndexChanged -= new EventHandler(modRegisterComboBox_SelectedIndexChanged);
            deviceDataTypeComboBox.SelectedIndexChanged -= new EventHandler(modRegisterComboBox_SelectedIndexChanged);
            modbusFs2InternalTypeComboBox.SelectedIndexChanged -= new EventHandler(modRegisterComboBox_SelectedIndexChanged);

            ch = new ModbusChannelImp(ch.Name, ch);         // Channel temporary copy
            if (modbusDataTypeComboBox.SelectedItem != null) ch.ModbusDataType = (ModbusDataTypeEx)modbusDataTypeComboBox.SelectedItem;
            if (deviceDataTypeComboBox.SelectedItem != null) ch.DeviceDataType = (ModbusDeviceDataType)deviceDataTypeComboBox.SelectedItem;
            if (conversionTypeComboBox.SelectedItem != null) ch.ConversionType = (ModbusConversionType)conversionTypeComboBox.SelectedItem;
            if (modbusReadWriteComboBox.SelectedItem != null) ch.ModbusReadWrite = (ModbusReadWrite)modbusReadWriteComboBox.SelectedItem;
            if (modbusFs2InternalTypeComboBox.SelectedItem != null) ch.ModbusFs2InternalType = (ModbusFs2InternalType)modbusFs2InternalTypeComboBox.SelectedItem;
            ch.DeviceDataLen = (ushort)deviceDataLenNumericUpDown.Value;
            //ch.K = 
            //---
            bitIndexNumericUpDown.Enabled = false;

            switch (ch.ModbusDataType)
            {
                case ModbusDataTypeEx.Input:
                case ModbusDataTypeEx.Coil:
                case ModbusDataTypeEx.DeviceFailureInfo:
                    ch.DeviceDataType = ModbusDeviceDataType.Bool;
                    deviceDataTypeComboBox.Items.Clear();
                    deviceDataTypeComboBox.Items.Add(ModbusDeviceDataType.Bool);
                    deviceDataTypeComboBox.SelectedItem = ModbusDeviceDataType.Bool;
                    deviceDataTypeComboBox.Enabled = false;
                    //---
                    ch.DeviceDataLen = 1;
                    deviceDataLenNumericUpDown.Value = ch.DeviceDataLen;
                    deviceDataLenNumericUpDown.Enabled = false;
                    //---
                    ch.ConversionType = ModbusConversionType.SwapNone;
                    conversionTypeComboBox.Items.Clear();
                    conversionTypeComboBox.Items.Add(ModbusConversionType.SwapNone);
                    conversionTypeComboBox.SelectedItem = ModbusConversionType.SwapNone;
                    conversionTypeComboBox.Enabled = false;
                    //---
                    modbusReadWriteComboBox.Items.Clear();
                    if (ch.ModbusDataType == ModbusDataTypeEx.Coil)
                    {
                        foreach (ModbusReadWrite r in Enum.GetValues(typeof(ModbusReadWrite)))
                        {
                            modbusReadWriteComboBox.Items.Add(r);
                        }
                        modbusReadWriteComboBox.Enabled = true;
                    }
                    else
                    {
                        ch.ModbusReadWrite = ModbusReadWrite.ReadOnly;
                        modbusReadWriteComboBox.Items.Add(ModbusReadWrite.ReadOnly);
                        modbusReadWriteComboBox.Enabled = false;
                    }
                    modbusReadWriteComboBox.SelectedItem = ch.ModbusReadWrite;
                    //---
                    ch.ModbusFs2InternalType = ModbusFs2InternalType.Boolean;
                    modbusFs2InternalTypeComboBox.Items.Clear();
                    modbusFs2InternalTypeComboBox.Items.Add(ModbusFs2InternalType.Boolean);
                    modbusFs2InternalTypeComboBox.SelectedItem = ModbusFs2InternalType.Boolean;
                    modbusFs2InternalTypeComboBox.Enabled = false;
                    //---
                    ch.BitIndex = 0;
                    bitIndexNumericUpDown.Value = ch.BitIndex;
                    bitIndexNumericUpDown.Enabled = false;
                    //---
                    ch.K = 1.0F;
                    kMaskedTextBox.Text = ch.K.ToString();
                    kMaskedTextBox.Enabled = false;
                    //---
                    ch.D = 0.0F;
                    dMaskedTextBox.Text = ch.D.ToString();
                    dMaskedTextBox.Enabled = false;
                    if (ch.ModbusDataType == ModbusDataTypeEx.DeviceFailureInfo)
                    {
                        ch.ModbusDataAddress = 1;
                        modbusDataAddressNumericUpDown.Value = ch.ModbusDataAddress;
                        modbusDataAddressNumericUpDown.Enabled = false;
                    }
                    else
                        modbusDataAddressNumericUpDown.Enabled = true;

                    break;
                case ModbusDataTypeEx.HoldingRegister:
                case ModbusDataTypeEx.InputRegister:
                    modbusDataAddressNumericUpDown.Enabled = true;
                    deviceDataTypeComboBox.Items.Clear();
                    foreach (ModbusDeviceDataType r in Enum.GetValues(typeof(ModbusDeviceDataType)))
                    {
                        deviceDataTypeComboBox.Items.Add(r);
                    }
                    deviceDataTypeComboBox.SelectedItem = ch.DeviceDataType;
                    deviceDataTypeComboBox.Enabled = true;
                    //---
                    switch (ch.DeviceDataType)
                    {
                        case ModbusDeviceDataType.Int:
                        case ModbusDeviceDataType.DInt:
                            conversionTypeComboBox.Items.Clear();
                            if (ch.DeviceDataType == ModbusDeviceDataType.Int)
                            {
                                ch.DeviceDataLen = 1;
                                if (ch.ConversionType != ModbusConversionType.SwapNone &&
                                    ch.ConversionType != ModbusConversionType.SwapBytes)
                                    ch.ConversionType = ModbusConversionType.SwapNone;
                                conversionTypeComboBox.Items.Add(ModbusConversionType.SwapNone);
                                conversionTypeComboBox.Items.Add(ModbusConversionType.SwapBytes);
                            }
                            else
                            {
                                ch.DeviceDataLen = 2;
                                foreach (ModbusConversionType t in Enum.GetValues(typeof(ModbusConversionType)))
                                {
                                    conversionTypeComboBox.Items.Add(t);
                                }
                            }
                            deviceDataLenNumericUpDown.Value = ch.DeviceDataLen;
                            deviceDataLenNumericUpDown.Enabled = false;
                            //---
                            conversionTypeComboBox.SelectedItem = ch.ConversionType;
                            conversionTypeComboBox.Enabled = true;
                            //---
                            if (ch.ModbusFs2InternalType != ModbusFs2InternalType.Int32 &&
                                ch.ModbusFs2InternalType != ModbusFs2InternalType.Double)
                                ch.ModbusFs2InternalType = ModbusFs2InternalType.Int32;
                            modbusFs2InternalTypeComboBox.Items.Clear();
                            modbusFs2InternalTypeComboBox.Items.Add(ModbusFs2InternalType.Int32);
                            modbusFs2InternalTypeComboBox.Items.Add(ModbusFs2InternalType.Double);
                            modbusFs2InternalTypeComboBox.SelectedItem = ch.ModbusFs2InternalType;
                            modbusFs2InternalTypeComboBox.Enabled = true;
                            break;
                        case ModbusDeviceDataType.UInt:
                        case ModbusDeviceDataType.DUInt:
                            conversionTypeComboBox.Items.Clear();
                            if (ch.DeviceDataType == ModbusDeviceDataType.UInt)
                            {
                                ch.DeviceDataLen = 1;
                                if (ch.ConversionType != ModbusConversionType.SwapNone &&
                                    ch.ConversionType != ModbusConversionType.SwapBytes)
                                    ch.ConversionType = ModbusConversionType.SwapNone;
                                conversionTypeComboBox.Items.Add(ModbusConversionType.SwapNone);
                                conversionTypeComboBox.Items.Add(ModbusConversionType.SwapBytes);
                            }
                            else
                            {
                                ch.DeviceDataLen = 2;
                                foreach (ModbusConversionType t in Enum.GetValues(typeof(ModbusConversionType)))
                                {
                                    conversionTypeComboBox.Items.Add(t);
                                }
                            }
                            deviceDataLenNumericUpDown.Value = ch.DeviceDataLen;
                            deviceDataLenNumericUpDown.Enabled = false;
                            //---
                            conversionTypeComboBox.SelectedItem = ch.ConversionType;
                            conversionTypeComboBox.Enabled = true;
                            //---
                            if (ch.ModbusFs2InternalType != ModbusFs2InternalType.UInt32 &&
                                ch.ModbusFs2InternalType != ModbusFs2InternalType.Double)
                                ch.ModbusFs2InternalType = ModbusFs2InternalType.UInt32;
                            modbusFs2InternalTypeComboBox.Items.Clear();
                            modbusFs2InternalTypeComboBox.Items.Add(ModbusFs2InternalType.UInt32);
                            modbusFs2InternalTypeComboBox.Items.Add(ModbusFs2InternalType.Double);
                            modbusFs2InternalTypeComboBox.SelectedItem = ch.ModbusFs2InternalType;
                            modbusFs2InternalTypeComboBox.Enabled = true;
                            break;
                        case ModbusDeviceDataType.Float:
                            ch.DeviceDataLen = 2;
                            deviceDataLenNumericUpDown.Value = ch.DeviceDataLen;
                            deviceDataLenNumericUpDown.Enabled = false;
                            //---
                            conversionTypeComboBox.Items.Clear();
                            foreach (ModbusConversionType t in Enum.GetValues(typeof(ModbusConversionType)))
                            {
                                conversionTypeComboBox.Items.Add(t);
                            }
                            conversionTypeComboBox.SelectedItem = ch.ConversionType;
                            conversionTypeComboBox.Enabled = true;
                            //---
                            ch.ModbusFs2InternalType = ModbusFs2InternalType.Double;
                            modbusFs2InternalTypeComboBox.Items.Clear();
                            modbusFs2InternalTypeComboBox.Items.Add(ModbusFs2InternalType.Double);
                            modbusFs2InternalTypeComboBox.SelectedItem = ch.ModbusFs2InternalType;
                            modbusFs2InternalTypeComboBox.Enabled = false;
                            break;
                        case ModbusDeviceDataType.String:
                            deviceDataLenNumericUpDown.Enabled = true;
                            //---
                            conversionTypeComboBox.Items.Clear();
                            if (ch.ConversionType != ModbusConversionType.SwapNone &&
                                ch.ConversionType != ModbusConversionType.SwapBytes)
                                ch.ConversionType = ModbusConversionType.SwapNone;
                            conversionTypeComboBox.Items.Add(ModbusConversionType.SwapNone);
                            conversionTypeComboBox.Items.Add(ModbusConversionType.SwapBytes);
                            conversionTypeComboBox.SelectedItem = ch.ConversionType;
                            conversionTypeComboBox.Enabled = true;
                            //---
                            ch.ModbusFs2InternalType = ModbusFs2InternalType.String;
                            modbusFs2InternalTypeComboBox.Items.Clear();
                            modbusFs2InternalTypeComboBox.Items.Add(ModbusFs2InternalType.String);
                            modbusFs2InternalTypeComboBox.SelectedItem = ch.ModbusFs2InternalType;
                            modbusFs2InternalTypeComboBox.Enabled = false;
                            break;
                        case ModbusDeviceDataType.Bool:
                            ch.DeviceDataLen = 1;
                            deviceDataLenNumericUpDown.Value = ch.DeviceDataLen;
                            deviceDataLenNumericUpDown.Enabled = false;
                            //---
                            ch.ConversionType = ModbusConversionType.SwapNone;
                            conversionTypeComboBox.Items.Clear();
                            conversionTypeComboBox.Items.Add(ModbusConversionType.SwapNone);
                            conversionTypeComboBox.SelectedItem = ModbusConversionType.SwapNone;
                            conversionTypeComboBox.Enabled = false;
                            //---
                            ch.ModbusFs2InternalType = ModbusFs2InternalType.Boolean;
                            modbusFs2InternalTypeComboBox.Items.Clear();
                            modbusFs2InternalTypeComboBox.Items.Add(ModbusFs2InternalType.Boolean);
                            modbusFs2InternalTypeComboBox.SelectedItem = ch.ModbusFs2InternalType;
                            modbusFs2InternalTypeComboBox.Enabled = false;
                            //---
                            bitIndexNumericUpDown.Enabled = true;
                            bitIndexNumericUpDown.Minimum = 0;
                            bitIndexNumericUpDown.Maximum = 15;
                            break;
                    }
                    //---
                    modbusReadWriteComboBox.Items.Clear();
                    if (ch.ModbusDataType == ModbusDataTypeEx.HoldingRegister && ch.DeviceDataType != ModbusDeviceDataType.Bool)
                    {
                        foreach (ModbusReadWrite r in Enum.GetValues(typeof(ModbusReadWrite)))
                        {
                            modbusReadWriteComboBox.Items.Add(r);
                        }
                        modbusReadWriteComboBox.Enabled = true;
                    }
                    else
                    {
                        ch.ModbusReadWrite = ModbusReadWrite.ReadOnly;
                        modbusReadWriteComboBox.Items.Add(ModbusReadWrite.ReadOnly);
                        modbusReadWriteComboBox.Enabled = false;
                    }
                    modbusReadWriteComboBox.SelectedItem = ch.ModbusReadWrite;
                    //---
                    if (ch.ModbusFs2InternalType == ModbusFs2InternalType.Double)
                    {
                        kMaskedTextBox.Enabled = true;
                        dMaskedTextBox.Enabled = true;
                    }
                    else
                    {
                        //---
                        ch.K = 1.0;
                        kMaskedTextBox.Text = ch.K.ToString();
                        kMaskedTextBox.Enabled = false;
                        //---
                        ch.D = 0.0;
                        dMaskedTextBox.Text = ch.D.ToString();
                        dMaskedTextBox.Enabled = false;
                    }
                    break;
            }
            modbusDataTypeComboBox.SelectedIndexChanged += new EventHandler(modRegisterComboBox_SelectedIndexChanged);
            deviceDataTypeComboBox.SelectedIndexChanged += new EventHandler(modRegisterComboBox_SelectedIndexChanged);
            modbusFs2InternalTypeComboBox.SelectedIndexChanged += new EventHandler(modRegisterComboBox_SelectedIndexChanged);
            setRegisterLabel();
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            ModbusChannelImp ch = (ModbusChannelImp)this.Tag;
            ch.ModbusDataType = (ModbusDataTypeEx)modbusDataTypeComboBox.SelectedItem;
            ch.DeviceDataType = (ModbusDeviceDataType)deviceDataTypeComboBox.SelectedItem;
            ch.ConversionType = (ModbusConversionType)conversionTypeComboBox.SelectedItem;
            ch.ModbusReadWrite = (ModbusReadWrite)modbusReadWriteComboBox.SelectedItem;
            ch.ModbusFs2InternalType = (ModbusFs2InternalType)modbusFs2InternalTypeComboBox.SelectedItem;
            ch.DeviceDataLen = (ushort)deviceDataLenNumericUpDown.Value;
            ch.BitIndex = (int)bitIndexNumericUpDown.Value;
            CultureInfo ci = CultureInfo.GetCultureInfo("en-US");
            try { ch.D = double.Parse(dMaskedTextBox.Text, NumberStyles.Float, ci.NumberFormat); }
            catch { MessageBox.Show("Constant D - bad format"); }
            try { ch.K = double.Parse(kMaskedTextBox.Text, NumberStyles.Float, ci.NumberFormat); }
            catch { MessageBox.Show("Constant K - bad format"); }
            ch.ModbusDataAddress = (ushort)modbusDataAddressNumericUpDown.Value;
            ch.ModbusReadWrite = (ModbusReadWrite)modbusReadWriteComboBox.SelectedItem;
            ch.ModbusStation = (string)stationComboBox.SelectedItem;
            ch.SlaveId = (byte)slaveIdUpDown.Value;

            test = true;
        }

        public ModbusChannelImp DoShow()
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                // renaming - special treatment
                if (this.nameTextBox.Text != (this.Tag as ModbusChannelImp).Name)
                {
                    this.Tag = new ModbusChannelImp(this.nameTextBox.Text, (ModbusChannelImp)this.Tag);
                }
                return (ModbusChannelImp)this.Tag;
            }
            else
                return null;
        }

        private void modRegisterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show("Zmena");
            MakeControlsValidation((ModbusChannelImp)this.Tag);
        }

        private void modbusDataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setRegisterLabel();
        }

        private void modbusDataAddressNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            setRegisterLabel();
        }

        private void setRegisterLabel()
        {
            ModbusDataTypeEx data = (ModbusDataTypeEx)modbusDataTypeComboBox.SelectedItem;
            switch (data)
            {
                case ModbusDataTypeEx.Input:
                    registerLabel.Text = "1" + ((int)(modbusDataAddressNumericUpDown.Value + 1)).ToString("D4");
                    break;
                case ModbusDataTypeEx.Coil:
                    registerLabel.Text = "0" + ((int)(modbusDataAddressNumericUpDown.Value + 1)).ToString("D4");
                    break;
                case ModbusDataTypeEx.InputRegister:
                    registerLabel.Text = "3" + ((int)(modbusDataAddressNumericUpDown.Value + 1)).ToString("D4");
                    break;
                case ModbusDataTypeEx.HoldingRegister:
                    registerLabel.Text = "4" + ((int)(modbusDataAddressNumericUpDown.Value + 1)).ToString("D4");
                    break;
                case ModbusDataTypeEx.DeviceFailureInfo:
                    registerLabel.Text = "FS2 internal";
                    break;
            }
        }
    }
}
