using System.Windows.Forms;
using System.Collections.Generic;

namespace FreeSCADA.Designer.Dialogs
{
	partial class SaveDocumentsDialog : Form
	{
		public SaveDocumentsDialog(List<string> documents)
		{
			InitializeComponent();
			foreach (string doc in documents)
				documentsListBox.Items.Add(doc);
		}
	}
}
