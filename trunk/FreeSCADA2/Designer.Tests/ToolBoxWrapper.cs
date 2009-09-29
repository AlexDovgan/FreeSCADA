using System;
using Core.UIItems;
using Core.UIItems.Finders;
using NUnit.Framework;

namespace Designer.Tests
{
	class ToolBoxWrapper
	{
		public enum Entries
		{
			Selection,
			ActionEdit,
			Rectangle,
			Ellipse,
			TextBox,
			Polyline,
			Button,
			ToggleButton,
			ProgressBar,
			ScrollBar,
			Image,
			Slider,
			Checkbox
		}

		IUIItem uiItem;
		Core.UIItems.WindowItems.Window mainWindow;

		public ToolBoxWrapper(Core.UIItems.WindowItems.Window mainWindow)
		{
			Assert.IsNotNull(mainWindow);
			uiItem = mainWindow.Get<Panel>(SearchCriteria.ByAutomationId("_toolBox"));
			Assert.IsNotNull(uiItem);

			this.mainWindow = mainWindow;
		}

		public void Select(Entries item)
		{
			switch (item)
			{
				case Entries.Selection:
					ClickToolboxItem(0, true);
					ClickToolboxItem(1, true);
					break;
				case Entries.ActionEdit:
					ClickToolboxItem(0, true);
					ClickToolboxItem(2, true);
					break;
				case Entries.Rectangle:
					ClickToolboxItem(0, true);
					ClickToolboxItem(2, false);
					ClickToolboxItem(2, true);
					break;
				case Entries.Ellipse:
					ClickToolboxItem(0, true);
					ClickToolboxItem(2, false);
					ClickToolboxItem(3, true);
					break;
				case Entries.TextBox:
					ClickToolboxItem(0, true);
					ClickToolboxItem(2, false);
					ClickToolboxItem(4, true);
					break;
				case Entries.Polyline:
					ClickToolboxItem(0, true);
					ClickToolboxItem(2, false);
					ClickToolboxItem(5, true);
					break;
				case Entries.Button:
					ClickToolboxItem(0, true);
					ClickToolboxItem(1, false);
					ClickToolboxItem(3, true);
					break;
				case Entries.ToggleButton:
					ClickToolboxItem(0, true);
					ClickToolboxItem(1, false);
					ClickToolboxItem(4, true);
					break;
				case Entries.ProgressBar:
					ClickToolboxItem(0, true);
					ClickToolboxItem(1, false);
					ClickToolboxItem(5, true);
					break;
				case Entries.ScrollBar:
					ClickToolboxItem(0, true);
					ClickToolboxItem(1, false);
					ClickToolboxItem(6, true);
					break;
				case Entries.Image:
					ClickToolboxItem(0, true);
					ClickToolboxItem(1, false);
					ClickToolboxItem(7, true);
					break;
				case Entries.Slider:
					ClickToolboxItem(0, true);
					ClickToolboxItem(1, false);
					ClickToolboxItem(8, true);
					break;
				case Entries.Checkbox:
					ClickToolboxItem(0, true);
					ClickToolboxItem(1, false);
					ClickToolboxItem(9, true);
					break;
			}
		}

		void ClickToolboxItem(int group, bool top)
		{
			System.Drawing.Point pt = new System.Drawing.Point();
			pt.X = Convert.ToInt32(uiItem.Bounds.Left + uiItem.Bounds.Width / 2);

			const int groupHeight = 20; //TODO: Get height based on window default font
			int offset = 4;
			if (group > 0)
				offset += group * groupHeight;
			offset += groupHeight / 2;

			if (top)
				pt.Y = Convert.ToInt32(uiItem.Bounds.Top) + offset;
			else
				pt.Y = Convert.ToInt32(uiItem.Bounds.Bottom) - offset;

			mainWindow.Mouse.Click(pt);
			System.Threading.Thread.Sleep(500);
		}
	}
}
