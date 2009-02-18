using System;
using System.Collections.Generic;
using System.Text;
using White;
using NUnit.Framework;
using Core;
using Core.UIItems;
using White.NUnit;
using System.Windows;
using Core.UIItems.Finders;
using System.Windows.Automation;

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

		protected void CreateNewSchema()
		{
			IUIItem new_schema_button = mainWindow.Get(SearchCriteria.ByText("New Schema"));
			Assert.IsNotNull(new_schema_button);
			System.Drawing.Point pt = new System.Drawing.Point(System.Convert.ToInt32(new_schema_button.Location.X) + 5, System.Convert.ToInt32(new_schema_button.Location.Y) + 5);
			mainWindow.Mouse.Click(pt);
		}

		[TearDown]
		public void TearDown()
		{
			mainWindow.Close();

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

			if (saveDlg != null)
			{
				Button saveDlgNoBtn = saveDlg.Get<Button>(SearchCriteria.ByAutomationId("noButton"));
				Assert.IsNotNull(saveDlgNoBtn);
				saveDlgNoBtn.Click();
			}

			Assert.IsTrue(mainWindow.IsClosed);
			Assert.IsTrue(app.HasExited);
		}

		object GetPropertyGridValue(string Name)
		{
			Panel propertyView = mainWindow.Get<Panel>(SearchCriteria.ByAutomationId("propertyGrid"));
			Assert.IsNotNull(propertyView);
			System.Windows.Automation.AutomationElement elem = propertyView.GetElement(SearchCriteria.ByControlType(System.Windows.Automation.ControlType.Table));
			Assert.IsNotNull(elem);

			Core.UIItems.TableItems.Table table = new Core.UIItems.TableItems.Table(elem, new Core.UIItems.Actions.ProcessActionListener(elem));
			Assert.IsNotNull(table);

			System.Windows.Automation.AutomationElement row = table.GetElement(SearchCriteria.ByText(Name));
			Assert.IsNotNull(row);

			object obj = row.GetCurrentPropertyValue(ValuePattern.ValueProperty);
			Assert.IsNotNull(obj);
			return obj;
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
				CreateNewSchema();
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
				Assert.IsTrue(GetPropertyGridValue("Height") as string == "100");
				Assert.IsTrue(GetPropertyGridValue("Width") as string == "100");
			}
		}

		[Test]
		public void CreatePolyline()
		{
			CreateNewSchema();
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
			Assert.IsTrue(GetPropertyGridValue("Height") as string == "100");
			Assert.IsTrue(GetPropertyGridValue("Width") as string == "200");
		}
	}
}
