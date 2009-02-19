using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Drawing;
using Core;
using Core.UIItems;
using Core.UIItems.Finders;
using System.Windows.Automation;

namespace Designer.Tests
{
    class Helpers
    {
        static public void CreateNewSchema(Core.UIItems.WindowItems.Window mainWindow)
        {
            IUIItem new_schema_button = mainWindow.Get(SearchCriteria.ByText("New Schema"));
            Assert.IsNotNull(new_schema_button);
            Point pt = new Point(Convert.ToInt32(new_schema_button.Location.X) + 5, Convert.ToInt32(new_schema_button.Location.Y) + 5);
            mainWindow.Mouse.Click(pt);
        }

        static public object GetPropertyGridValue(Core.UIItems.WindowItems.Window mainWindow, string Name)
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

        static public Core.UIItems.WindowItems.Window FindTopWindow(Application app, string id)
        {
            foreach (Core.UIItems.WindowItems.Window wnd in app.GetWindows())
            {
                if (wnd.PrimaryIdentification == id)
                    return wnd;
            }

            return null;
        }
    }
}
