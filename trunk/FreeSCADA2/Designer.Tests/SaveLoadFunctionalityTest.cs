﻿using Core;
using Core.UIItems;
using Core.UIItems.Finders;
using NUnit.Framework;

namespace Designer.Tests
{
	[TestFixture]
	public class SaveLoadFunctionalityTest
	{
		const string designer_executable = @"Designer.exe";

		[Test]
		public void CloseMainFormWithEmptyProject()
		{
			//Should not show any additional dialogs
			Application app = Application.Launch(designer_executable);
			Assert.IsNotNull(app);
			Core.UIItems.WindowItems.Window window = app.GetWindow("Designer");
			window.Close();

			Assert.IsTrue(app.HasExited);
		}

		[Test]
		public void SaveProjectOnClosing()
		{
			Application app = Application.Launch(designer_executable);
			Assert.IsNotNull(app);
			Core.UIItems.WindowItems.Window window = app.GetWindow("Designer");
			
			//Create new schema
			IUIItem new_schema_button = window.Get(SearchCriteria.ByText("New Schema"));
			Assert.IsNotNull(new_schema_button);
			System.Drawing.Point pt = new System.Drawing.Point(System.Convert.ToInt32(new_schema_button.Location.X) + 5, System.Convert.ToInt32(new_schema_button.Location.Y) + 5);
			window.Mouse.Click(pt);

			window.Close();
			Assert.IsFalse(window.IsClosed);

			//Check that there is Save dialog
			Core.UIItems.WindowItems.Window saveDlg = null;
			foreach (Core.UIItems.WindowItems.Window wnd in app.GetWindows())
			{
				if (wnd.PrimaryIdentification == "SaveDocumentsDialog")
				{
					saveDlg = wnd;
					break;
				}
			}
			Assert.IsNotNull(saveDlg);
			Button saveDlgNoBtn = saveDlg.Get<Button>(SearchCriteria.ByAutomationId("noButton"));
			Assert.IsNotNull(saveDlgNoBtn);
			saveDlgNoBtn.Click();

			Assert.IsTrue(window.IsClosed);
			Assert.IsTrue(app.HasExited);
		}

        [Test]
        public void ClickOnNewProjectCrashTest()
        {
            Application app = Application.Launch(designer_executable);
            Assert.IsNotNull(app);
            Core.UIItems.WindowItems.Window window = app.GetWindow("Designer");

            //Create new schema
            IUIItem new_project_button = window.Get(SearchCriteria.ByText("New Project"));
            Assert.IsNotNull(new_project_button);
            System.Drawing.Point pt = new System.Drawing.Point(System.Convert.ToInt32(new_project_button.Location.X) + 5, System.Convert.ToInt32(new_project_button.Location.Y) + 5);
            
            //Do click several times
            window.Mouse.Click(pt);
            System.Threading.Thread.Sleep(1000);

            window.Mouse.Click(pt);
            System.Threading.Thread.Sleep(1000);

            window.Mouse.Click(pt);

            window.Close();

            Assert.IsTrue(window.IsClosed);
            Assert.IsTrue(app.HasExited);
        }
	}
}
