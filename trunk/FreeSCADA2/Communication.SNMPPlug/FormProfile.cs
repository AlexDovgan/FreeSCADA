using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using Lextm.SharpSnmpLib;
using System.Collections.Generic;

namespace FreeSCADA.Communication.SNMPPlug
{
    internal partial class FormProfile : Form
    {
        private SNMPAgent _agent;
        List<string> _forbiddenNames;

        public FormProfile(SNMPAgent agent)
        {
            InitializeComponent();
            _agent = agent;
        }

        public FormProfile(SNMPAgent agent, List<string> forbiddenNames)
        {
            InitializeComponent();
            _agent = agent;
            _forbiddenNames = forbiddenNames;
        }

        internal IPAddress IP
        {
            get { return IPAddress.Parse(txtIP.Text); }
        }

        internal VersionCode VersionCode
        {
            get { return (VersionCode)cbVersionCode.SelectedIndex; }
        }

        internal string AgentName
        {
            get { return txtName.Text; }
        }

        internal string GetCommunity
        {
            get { return txtGet.Text; }
        }

        internal string SetCommunity
        {
            get { return txtSet.Text; }
        }
        
        internal int Port
        {
            get { return int.Parse(txtPort.Text, CultureInfo.CurrentCulture); }
        }

        private void FormProfile_Load(object sender, EventArgs e)
        {
            if (_agent != null)
            {
                txtIP.Text = _agent.AgentIP.Address.ToString();
                txtPort.Text = _agent.AgentIP.Port.ToString();
                txtGet.Text = _agent.GetCommunity;
                txtSet.Text = _agent.SetCommunity;
                txtName.Text = _agent.Name;
                if ((int)_agent.VersionCode != 3)
                    cbVersionCode.SelectedIndex = (int)_agent.VersionCode;
                else
                    cbVersionCode.SelectedIndex = 2;
                this.PauseNumericUpDown.Value = _agent.CycleTimeout;
                this.TimeoutNumericUpDown.Value = _agent.RetryTimeout;
                this.NuberNumericUpDown.Value = _agent.RetryCount;
                this.failedNumericUpDown.Value = _agent.FailedCount;
                this.loggingComboBox.Items.Add(0);
                this.loggingComboBox.Items.Add(1);
                this.loggingComboBox.Items.Add(2);
                this.loggingComboBox.Items.Add(3);
                this.loggingComboBox.Items.Add(4);
                this.loggingComboBox.SelectedItem = _agent.LoggingLevel;
            }
            else
            {
                cbVersionCode.SelectedIndex = 1;
            }
        }
        
        private void txtPort_Validating(object sender, CancelEventArgs e)
        {
            int result;
            bool isInt = int.TryParse(txtPort.Text, out result);
            if (!isInt || result < 0)
            {
                e.Cancel = true;
                txtPort.SelectAll();
                errorProvider1.SetError(txtPort, "Please provide a valid port number");
            }
        }
        
        private void txtPort_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtPort, string.Empty);
        }
        
        private void txtSet_Validating(object sender, CancelEventArgs e)
        {
            if (txtSet.Text.Length == 0)
            {
                e.Cancel = true;
                txtSet.SelectAll();
                errorProvider1.SetError(txtSet, "Community name cannot be empty");
            }
        }
        
        private void txtSet_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtSet, string.Empty);
        }

        private void txtGet_Validating(object sender, CancelEventArgs e)
        {
            if (txtGet.Text.Length ==0)
            {
                e.Cancel = true;
                txtGet.SelectAll();
                errorProvider1.SetError(txtGet, "Community name cannot be empty");
            }
        }
        
        private void txtGet_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtGet, string.Empty);
        }

        private void txtIP_Validating(object sender, CancelEventArgs e)
        {
            IPAddress ip;
            if (!SNMPAgent.IsValidIPAddress(txtIP.Text, out ip))
            {
                e.Cancel = true;
                txtIP.SelectAll();
                errorProvider1.SetError(txtIP, "IP address is not valid");
            }
        }
        
        private void txtIP_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtIP, string.Empty);
        }
        
        private void BtnOKClick(object sender, EventArgs e)
        {
            ValidateAllChildren(this);
        }
        
        private void ValidateAllChildren(Control parent)
        {
            if (DialogResult == DialogResult.None)
            {
                return;    
            }
            
            foreach (Control c in parent.Controls)
            {
                c.Focus();
                if (!Validate())
                {
                    DialogResult = DialogResult.None;
                    return;
                }
                
                ValidateAllChildren(c);
            }
            _agent.Name = AgentName;
            _agent.AgentIP = new IPEndPoint(IP, Port);
            _agent.GetCommunity = GetCommunity;
            _agent.SetCommunity = SetCommunity;
            if (cbVersionCode.SelectedIndex != 2)
                _agent.VersionCode = (VersionCode)cbVersionCode.SelectedIndex;
            else
                _agent.VersionCode = (VersionCode)3;
            _agent.CycleTimeout = (int)this.PauseNumericUpDown.Value;
            _agent.RetryTimeout = (int)this.TimeoutNumericUpDown.Value;
            _agent.RetryCount = (int)this.NuberNumericUpDown.Value;
            _agent.FailedCount = (int)this.failedNumericUpDown.Value;
            _agent.LoggingLevel = (int)this.loggingComboBox.SelectedItem;
        }
    }
}
