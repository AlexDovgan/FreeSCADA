using System;
using System.Windows.Automation;
using Core;
using Core.UIItems;
using Core.UIItems.Finders;
using Core.UIItems.MenuItems;
using NUnit.Framework;

namespace Designer.Tests
{
	[TestFixture]
	public class SchemaEditingTest
	{
		const string designer_executable = @"Designer.exe";
		Application app;
		Core.UIItems.WindowItems.Window mainWindow;

		[SetUp]
		public void SetUp()
		{
			app = Application.Launch(designer_executable);
			Assert.IsNotNull(app);

			mainWindow = app.GetWindow("Designer");
			Assert.IsNotNull(mainWindow);
		}

		[TearDown]
		public void TearDown()
		{
			mainWindow.Close();

			//Check that there is Save dialog
			Core.UIItems.WindowItems.Window saveDlg = Helpers.FindTopWindow(app, "SaveDocumentsDialog");
			if (saveDlg != null)
			{
				Button saveDlgNoBtn = saveDlg.Get<Button>(SearchCriteria.ByAutomationId("noButton"));
				Assert.IsNotNull(saveDlgNoBtn);
				saveDlgNoBtn.Click();
			}

			Assert.IsTrue(mainWindow.IsClosed);
			Assert.IsTrue(app.HasExited);
		}

		[Test]
		public void CreateRectangularElements()
		{
			ToolBoxWrapper.Entries[] elements = new ToolBoxWrapper.Entries[]
			{ 
				ToolBoxWrapper.Entries.Rectangle,
				ToolBoxWrapper.Entries.TextBox,
				ToolBoxWrapper.Entries.Button,
				ToolBoxWrapper.Entries.ProgressBar,
				ToolBoxWrapper.Entries.ScrollBar,
				ToolBoxWrapper.Entries.Image,
				ToolBoxWrapper.Entries.Slider,
				ToolBoxWrapper.Entries.Checkbox
			};
			
			foreach(ToolBoxWrapper.Entries entry in elements)
			{
                Helpers.CreateNewSchema(mainWindow);
				Panel schemaView = mainWindow.Get<Panel>(SearchCriteria.ByAutomationId("SchemaCanvas"));
				Assert.IsNotNull(schemaView);
				ToolBoxWrapper toolbox = new ToolBoxWrapper(mainWindow);

				toolbox.Select(entry);

				//Draw rect
                DoRectCreationByMouse(schemaView, 100, 100, 200, 200);

				//Select Selection tool
				toolbox.Select(ToolBoxWrapper.Entries.Selection);

				//Select object
                System.Drawing.Point pt = new System.Drawing.Point();
				pt.X = Convert.ToInt32(schemaView.Bounds.Left + 150);
				pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 150);
				mainWindow.Mouse.Location = pt;
				mainWindow.Mouse.Click();

				//Check values of property grid
				Assert.IsTrue(Helpers.GetPropertyGridValue(mainWindow, "Height") as string == "100");
                Assert.IsTrue(Helpers.GetPropertyGridValue(mainWindow, "Width") as string == "100");
			}
		}

        private void DoRectCreationByMouse(Panel schemaView, int x1, int y1, int x2, int y2)
        {
            System.Drawing.Point pt = new System.Drawing.Point();

            pt.X = Convert.ToInt32(schemaView.Bounds.Left + x1);
            pt.Y = Convert.ToInt32(schemaView.Bounds.Top + y1);
            mainWindow.Mouse.Location = pt;

            Core.InputDevices.Mouse.LeftDown();

            pt.X = Convert.ToInt32(schemaView.Bounds.Left + x2);
            pt.Y = Convert.ToInt32(schemaView.Bounds.Top + y2);
            mainWindow.Mouse.Location = pt;

            System.Threading.Thread.Sleep(500);
            Core.InputDevices.Mouse.LeftUp();
            System.Threading.Thread.Sleep(500);
        }

		[Test]
		public void CreatePolyline()
		{
            Helpers.CreateNewSchema(mainWindow);
			Panel schemaView = mainWindow.Get<Panel>(SearchCriteria.ByAutomationId("SchemaCanvas"));
			Assert.IsNotNull(schemaView);
			ToolBoxWrapper toolbox = new ToolBoxWrapper(mainWindow);

			toolbox.Select(ToolBoxWrapper.Entries.Polyline);

			//Draw rect
			System.Drawing.Point pt = new System.Drawing.Point();

			pt.X = Convert.ToInt32(schemaView.Bounds.Left + 100);
			pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 100);
			mainWindow.Mouse.Location = pt;
			mainWindow.Mouse.Click();

			pt.X = Convert.ToInt32(schemaView.Bounds.Left + 200);
			pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 200);
			mainWindow.Mouse.Location = pt;
			mainWindow.Mouse.Click();

			pt.X = Convert.ToInt32(schemaView.Bounds.Left + 300);
			pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 100);
			mainWindow.Mouse.Location = pt;
			mainWindow.Mouse.Click();

			mainWindow.Mouse.RightClick();
			

			//Select Selection tool
			toolbox.Select(ToolBoxWrapper.Entries.Selection);

			//Select object
			pt.X = Convert.ToInt32(schemaView.Bounds.Left + 200);
			pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 200);
			mainWindow.Mouse.Location = pt;
			mainWindow.Mouse.Click();

			//Check values of property grid
            Assert.IsTrue(Helpers.GetPropertyGridValue(mainWindow, "Height") as string == "100");
            Assert.IsTrue(Helpers.GetPropertyGridValue(mainWindow, "Width") as string == "200");
		}

        /// <summary>
        /// There is a bug, when copy, cut and binding commands are not available right
        /// after element creation
        /// </summary>
        [Test]
        public void CheckCommandsWithoutSelectionTool()
        {
            Helpers.CreateNewSchema(mainWindow);
            Panel schemaView = mainWindow.Get<Panel>(SearchCriteria.ByAutomationId("SchemaCanvas"));
            Assert.IsNotNull(schemaView);
            ToolBoxWrapper toolbox = new ToolBoxWrapper(mainWindow);

            toolbox.Select(ToolBoxWrapper.Entries.Rectangle);

            //Draw rect
            DoRectCreationByMouse(schemaView, 100, 100, 200, 200);


            Menu editMenu = mainWindow.MenuBar.MenuItem("Edit");
            Assert.IsNotNull(editMenu);
            editMenu.Click();
            
            Menu cutCmd = editMenu.SubMenu("Cut");
            Assert.IsNotNull(cutCmd);
            Assert.IsTrue(cutCmd.Visible);
            Assert.IsTrue(cutCmd.Enabled);

            Menu copyCmd = editMenu.SubMenu("Copy");
            Assert.IsNotNull(copyCmd);
            Assert.IsTrue(copyCmd.Visible);
            Assert.IsTrue(copyCmd.Enabled);

            Menu bindCmd = editMenu.SubMenu("Associate with data...");
            Assert.IsNotNull(bindCmd);
            Assert.IsTrue(bindCmd.Visible);
            Assert.IsTrue(bindCmd.Enabled);
        }
	}
}
