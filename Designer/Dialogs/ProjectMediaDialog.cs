using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using FreeSCADA.Common;

namespace FreeSCADA.Designer.Dialogs
{
	/// <summary>
	/// The form displays a complete list of media items of current Project.
	/// </summary>
	public partial class ProjectMediaDialog : Form
	{
		System.Windows.Controls.Image m_image_preview;

		/// <summary>
		/// Constructor
		/// </summary>
		public ProjectMediaDialog()
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

		private void addButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog();

			fd.Filter = StringResources.OpenMediaDialogFilter;
			fd.FilterIndex = 0;
			fd.RestoreDirectory = true;
			fd.Multiselect = true;

			if (fd.ShowDialog() != DialogResult.OK)
				return;

			foreach (string fileName in fd.FileNames)
			{
				//Check that we don't have duplicates
				string entryName = Path.GetFileName(fileName);
				if (Env.Current.Project.ContainsEntity(ProjectEntityType.Image, entryName) == false)
				{
					MessageBox.Show(string.Format(DialogMessages.DuplicateImageFound, entryName),
						DialogMessages.ErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}

			foreach (string fileName in fd.FileNames)
			{
				string entryName = Path.GetFileName(fileName);
				//Check if the image can be loaded, then load it
				using (BinaryReader imageBits = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
				{
					BitmapDecoder imgEnc = BitmapDecoder.Create(imageBits.BaseStream, BitmapCreateOptions.None, BitmapCacheOption.Default);
					if (imgEnc.Frames.Count > 0)
					{
						imageBits.Close();
						using (BinaryReader binReader = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
						using (MemoryStream memStream = new MemoryStream())
						using (BinaryWriter writer = new BinaryWriter(memStream))
						{
							const int BufferSize = 4096;
							byte[] buff = new byte[BufferSize];
							int readCount;
							do
							{
								readCount = binReader.Read(buff, 0, BufferSize);
								if (readCount > 0)
									writer.Write(buff, 0, readCount);
							}
							while (readCount > 0);
							writer.Flush();

							Env.Current.Project.SetData(ProjectEntityType.Image, entryName, memStream);
						}
					}
				}
			}

			UpdateImageList();
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

		private void removeButton_Click(object sender, EventArgs e)
		{
			foreach (ListViewItem item in imageList.SelectedItems)
				Env.Current.Project.RemoveEntity(ProjectEntityType.Image, item.Text);

			UpdateImageList();
		}
	}
}
