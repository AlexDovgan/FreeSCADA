using System;
using System.Collections.Generic;
using System.Text;
using Core;
using Core.UIItems.ListBoxItems;
using Core.UIItems.WindowItems;
using Core.UIItems.Finders;
using Core.UIItems;
using Core.UIItems.TreeItems;
using NUnit.Framework;

namespace Designer.Tests
{
    class BindingDialogWrapper
    {
        Application app;
        Window window;

        public Window Window
        {
            get { return window; }
        }

        public BindingDialogWrapper(Application app)
        {
            this.app = app;
            this.window = Helpers.FindTopWindow(app, "CommonBindingDialog");
            Assert.IsNotNull(window);
        }

        public void Close(bool save)
        {
            if (save)
            {
                Button saveButton = window.Get<Button>(SearchCriteria.ByAutomationId("saveButton"));
                Assert.IsNotNull(saveButton);
                saveButton.Click();
            }
            else
            {
                Button cancelButton = window.Get<Button>(SearchCriteria.ByAutomationId("button1"));
                Assert.IsNotNull(cancelButton);
                cancelButton.Click();
            }
        }

        public void SelectProperty(string name)
        {
            ListBox propertyList = window.Get<ListBox>(SearchCriteria.ByAutomationId("propertyList"));
            Assert.IsNotNull(propertyList);
            Assert.IsTrue(propertyList.Visible);

            ListItem item = propertyList.Item(name);
            Assert.IsNotNull(item);
            item.Click();
        }

        public void DblClickChannel(string plugin, string name)
        {
            Tree tree = window.Get<Tree>(SearchCriteria.ByAutomationId("channelsTree"));
            Assert.IsNotNull(tree);
            Assert.IsTrue(tree.Visible);

            TreeNode node = tree.Node(plugin, name);
            Assert.IsNotNull(node);
            node.DoubleClick();
        }

        public void CreateBinding(string name)
        {
            ComboBox bindingTypes = window.Get<ComboBox>(SearchCriteria.ByAutomationId("bindingTypes"));
            Assert.IsNotNull(bindingTypes);
            Assert.IsTrue(bindingTypes.Visible);

            bindingTypes.Select(name);

            Button button = window.Get<Button>(SearchCriteria.ByAutomationId("CreateAssociationButton"));
            Assert.IsNotNull(button);
            button.Click();
        }
    }
}
