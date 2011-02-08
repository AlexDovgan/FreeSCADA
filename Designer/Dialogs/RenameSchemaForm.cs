using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreeSCADA.Designer.Dialogs
{
	public partial class RenameEntityForm : Form
	{
		string schemaName = "";

		public string EntityName
		{
			get { return schemaName; }
		}

		public RenameEntityForm(string schemaName)
		{
			this.schemaName = schemaName;

			InitializeComponent();

			textBox1.Text = schemaName;
			button1.Enabled = textBox1.Text.Length > 0;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			schemaName = textBox1.Text;
			DialogResult = DialogResult.OK;
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			button1.Enabled = textBox1.Text.Length > 0;
		}
	}
}
