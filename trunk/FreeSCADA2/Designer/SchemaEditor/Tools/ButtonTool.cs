using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class ButtonTool : BaseTool, ITool
	{
		Point startPos;
		bool isDragging;
		Button buttonObject;

		public ButtonTool(UIElement element)
            : base(element)
		{
		}

		#region ITool implementation
		public String ToolName
		{
			get { return StringResources.ToolButtonName; }
		}

		public String ToolGroup
		{
			get { return StringResources.ToolControlsGroupName; }
		}
		public System.Drawing.Bitmap ToolIcon
		{
			get
			{
				return new System.Drawing.Bitmap(10, 10);
			}
		}
		#endregion


		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonDown(e);
			if (!e.Handled)
			{
				startPos = e.GetPosition(this);

				buttonObject = new Button();
				buttonObject.Opacity = 0.75;
				Canvas.SetLeft(buttonObject, startPos.X);
				Canvas.SetTop(buttonObject, startPos.Y);
				buttonObject.Width = 0;
				buttonObject.Height = 0;
				buttonObject.Content = "Button";

				visualChildren.Add(buttonObject);

				isDragging = true;
				CaptureMouse();

				e.Handled = true;
			}
		}

		protected override void OnPreviewMouseMove(MouseEventArgs e)
		{
			if (isDragging)
			{
				Vector v = e.GetPosition(this) - startPos;
				buttonObject.Width = v.X;
				buttonObject.Height = v.Y;

				InvalidateArrange();

				e.Handled = true;
			}

			base.OnPreviewMouseMove(e);
		}

		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if(isDragging)
			{
				visualChildren.Remove(buttonObject);
				buttonObject.Opacity = 1;
				
				if (buttonObject.Width < 10)
					buttonObject.Width = 10;
				if (buttonObject.Height < 10)
					buttonObject.Height = 10;

                NotifyObjectCreated(buttonObject);
				SelectedObject = buttonObject;

				isDragging = false;
				buttonObject = null;
				ReleaseMouseCapture();

				e.Handled = true;
			}
			base.OnPreviewMouseLeftButtonUp(e);
		}

		protected override Size MeasureOverride(Size finalSize)
		{
			base.MeasureOverride(finalSize);
			if (buttonObject != null)
				buttonObject.Measure(finalSize);
			return finalSize;
		}
		protected override Size ArrangeOverride(Size finalSize)
		{
			base.ArrangeOverride(finalSize);
			if (buttonObject != null)
			{
				double x = Canvas.GetLeft(buttonObject);
				double y = Canvas.GetTop(buttonObject);
				buttonObject.Arrange(new Rect(new Point(x, y), buttonObject.DesiredSize));
			}

			return finalSize;
		}
	}
}
