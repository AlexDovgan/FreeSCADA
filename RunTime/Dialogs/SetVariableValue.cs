using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FreeSCADA.Interfaces;
using System.Reflection;

namespace FreeSCADA.RunTime.Dialogs
{
    public partial class SetVariableValue : Form
    {
        IChannel chan;

        public SetVariableValue()
        {
            InitializeComponent();
        }

        public SetVariableValue(IChannel chan)
        {
            this.chan = chan;
            InitializeComponent();
            if (chan != null && chan.Value != null)
                this.valueTextBox.Text = chan.Value.ToString();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                Type t = chan.Value.GetType();
                Type[] types = { typeof(string) };
                MethodInfo mi = t.GetMethod("Parse", types);
                if (mi != null)
                {
                    object[] parameters = { valueTextBox.Text };
                    object res = mi.Invoke(chan.Value, parameters);
                    chan.Value = res;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Cannot convert text to channel value, exception message: {0}", ex.Message));
            }
        }
    }
}
