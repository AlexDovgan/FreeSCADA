using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors
{
	/// <summary>
	/// The form displays a complete list of media items of current Project.
	/// </summary>
	public partial class ContentEditorDialog : Form
	{
		System.Windows.Controls.Image m_image_preview;
        public System.Windows.Forms.ListView ImagesList
        {
            get { return imageList; }
        }
		/// <summary>
		/// Constructor
		/// </summary>
        public ContentEditorDialog()
		{
			InitializeComponent();

			System.Windows.Controls.Border border = new System.Windows.Controls.Border();
			border.BorderThickness = new System.Windows.Thickness(2);
			border.BorderBrush = System.Windows.SystemColors.ActiveBorderBrush;

			m_image_preview = new System.Windows.Controls.Image();

			border.Child = m_image_preview;
			wpfHost.Child = border;

			UpdateImageList();

			if (imageList.SelectedIndices.Count == 0 && imageList.Items.Count > 0)
				imageList.Items[0].Selected = true;

		}

		

		void UpdateImageList()
		{
			ListView.SelectedIndexCollection selection = imageList.SelectedIndices;
			imageList.Items.Clear();

			string[] entries = Env.Current.Project.GetEntities(ProjectEntityType.Image);

			for(int i=0;i<entries.Length;i++)
			{
				string imageName = entries[i];

				using(Stream stream = Env.Current.Project.GetData(ProjectEntityType.Image, imageName))
				{
					BitmapDecoder img = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
					ListViewItem item = imageList.Items.Add(imageName);
					string size = "";
					
					if(img.Frames.Count > 0)
						size = string.Format("{0}x{1}", img.Frames[0].PixelWidth, img.Frames[0].PixelHeight);
					item.SubItems.Add(size);
					item.Tag = img;

					if (selection.Contains(i))
						item.Selected = true;
				}
			}

			columnHeader1.Width = -2;
			columnHeader2.Width = -2;
		}

		private void imageList_SelectedIndexChanged(object sender, EventArgs e)
		{
			foreach (ListViewItem item in imageList.SelectedItems)
			{
				BitmapDecoder img = (BitmapDecoder)item.Tag;
				m_image_preview.Source = img.Frames[0];
			}
		}
	
	}
}
