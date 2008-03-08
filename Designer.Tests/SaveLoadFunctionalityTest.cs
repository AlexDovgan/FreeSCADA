using System;
using NUnit.Framework;
using NUnit.Extensions.Forms;
using FreeSCADA.Designer;

namespace Designer.Tests
{
	[TestFixture]
	public class SaveLoadFunctionalityTest : NUnitFormTest
	{
		string projectFile;
		bool shownSaveProjectDialog = false;
		MainForm mainForm;

		public override void Setup()
		{
			projectFile = System.IO.Path.GetTempFileName();
			if (System.IO.File.Exists(projectFile))
				System.IO.File.Delete(projectFile);

			mainForm = new MainForm();
			mainForm.Show();
		}

		public override void TearDown()
		{
			mainForm.Dispose();
			System.IO.File.Delete(projectFile);
			System.GC.Collect();
		}

		[Test]
		public void CloseMainFormWithEmptyProject()
		{
			//Should not show any additional dialogs
			mainForm.Close();
			Assert.IsFalse(mainForm.Visible);
		}

		[Test]
		public void SaveProjectOnClosing()
		{
			ToolStripButtonTester newSchemaButton = new ToolStripButtonTester("toolStripButtonNewSchema");
			newSchemaButton.Click();

			ExpectModal("SaveDocumentsDialog", "SaveProjectDialog_Cancel");
			shownSaveProjectDialog = false;

			mainForm.Close();
			Assert.IsTrue(shownSaveProjectDialog);
			Assert.IsFalse(mainForm.Visible);
		}

		private void SaveProjectDialog_Cancel()
		{
			ButtonTester noButton = new ButtonTester("noButton", "SaveDocumentsDialog");
			noButton.Click();
			shownSaveProjectDialog = true;
		}
	}
}
