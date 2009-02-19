using System;
using System.Windows.Automation;
using Core;
using Core.UIItems;
using Core.UIItems.Finders;
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
				System.Drawing.Point pt = new System.Drawing.Point();
				
				pt.X = Convert.ToInt32(schemaView.Bounds.Left + 100);
				pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 100);
				mainWindow.Mouse.Location = pt;

				Core.InputDevices.Mouse.LeftDown();

				pt.X = Convert.ToInt32(schemaView.Bounds.Left + 200);
				pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 200);
				mainWindow.Mouse.Location = pt;

				System.Threading.Thread.Sleep(500);
				Core.InputDevices.Mouse.LeftUp();
				System.Threading.Thread.Sleep(500);

				//Select Selection tool
				toolbox.Select(ToolBoxWrapper.Entries.Selection);

				//Select object
				pt.X = Convert.ToInt32(schemaView.Bounds.Left + 150);
				pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 150);
				mainWindow.Mouse.Location = pt;
				mainWindow.Mouse.Click();

				//Check values of property grid
				Assert.IsTrue(Helpers.GetPropertyGridValue(mainWindow, "Height") as string == "100");
                Assert.IsTrue(Helpers.GetPropertyGridValue(mainWindow, "Width") as string == "100");
			}
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
			pt.X = Convert.ToInt32(schemaView.Bounds.Left + 100);
			pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 100);
			mainWindow.Mouse.Location = pt;
			mainWindow.Mouse.Click();

			//Check values of property grid
            Assert.IsTrue(Helpers.GetPropertyGridValue(mainWindow, "Height") as string == "100");
            Assert.IsTrue(Helpers.GetPropertyGridValue(mainWindow, "Width") as string == "200");
		}
	}
}
