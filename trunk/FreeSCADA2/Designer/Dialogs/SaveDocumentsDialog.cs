using System.Collections.Generic;
using System.Windows.Forms;

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
