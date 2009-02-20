using System;
using System.Windows.Automation;
using Core;
using Core.UIItems;
using Core.UIItems.Finders;
using Core.UIItems.MenuItems;
using Core.UIItems.WindowItems;
using NUnit.Framework;
using System.Drawing;

namespace Designer.Tests
{
    [TestFixture]
    public class BindingEditingTest
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

        void AddChannelsToSimulator()
        {
            Menu communicationMenu = mainWindow.MenuBar.MenuItem("Communication");
            Assert.IsNotNull(communicationMenu);
            communicationMenu.Click();
            communicationMenu.SubMenu("Simulator properties...").Click();

            Window simulatorDlg = Helpers.FindTopWindow(app, "SettingsForm");
            Assert.IsNotNull(simulatorDlg);

            Button addButton = simulatorDlg.Get<Button>(SearchCriteria.ByAutomationId("addButton"));
            Assert.IsNotNull(addButton);

            addButton.Click();

            //Change type to "Ramp channel"
            GridWrapper grid = new GridWrapper(mainWindow, simulatorDlg.Get(SearchCriteria.ByAutomationId("grid")));
            grid.MoveToCell(0, 1);
            grid.Select(2);

            Button okButton = simulatorDlg.Get<Button>(SearchCriteria.ByAutomationId("okButton"));
            Assert.IsNotNull(okButton);

            okButton.Click();
        }

        [Test]
        public void CreateNumbericBinding()
        {
            AddChannelsToSimulator();

            Helpers.CreateNewSchema(mainWindow);
            Panel schemaView = mainWindow.Get<Panel>(SearchCriteria.ByAutomationId("SchemaCanvas"));
            Assert.IsNotNull(schemaView);
            ToolBoxWrapper toolbox = new ToolBoxWrapper(mainWindow);

            toolbox.Select(ToolBoxWrapper.Entries.Rectangle);

            //Draw rect
            System.Drawing.Point pt = new System.Drawing.Point();

            pt.X = Convert.ToInt32(schemaView.Bounds.Left + 100);
            pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 100);
            mainWindow.Mouse.Location = pt;

            Core.InputDevices.Mouse.LeftDown();
            pt.X = Convert.ToInt32(schemaView.Bounds.Left + 200);
            pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 200);
            mainWindow.Mouse.Location = pt;
            Core.InputDevices.Mouse.LeftUp();

            //Select Selection tool
            toolbox.Select(ToolBoxWrapper.Entries.Selection);

            //Select object
            pt.X = Convert.ToInt32(schemaView.Bounds.Left + 150);
            pt.Y = Convert.ToInt32(schemaView.Bounds.Top + 150);
            mainWindow.Mouse.Location = pt;
            mainWindow.Mouse.Click();

            Menu editMenu = mainWindow.MenuBar.MenuItem("Edit");
            Assert.IsNotNull(editMenu);
            editMenu.Click();
            editMenu.SubMenu("Associate with data...").Click();

            //Tune binding
            BindingDialogWrapper bindingDlg = new BindingDialogWrapper(app);
            bindingDlg.SelectProperty("Height");
            bindingDlg.CreateBinding("Bind to numeric value");
            //bindingDlg.CreateBinding("Numeric expression binding");

            bindingDlg.DblClickChannel("Data Simulator", "variable_1");
            
            Label channelNameLabel = bindingDlg.Window.Get<Label>(SearchCriteria.ByAutomationId("channelNameLabel"));
            Assert.AreEqual("variable_1 [Data Simulator]", channelNameLabel.Text);

            bindingDlg.Close(true);
        }
    }
}
