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
	class ThermoTool : BaseTool, ITool
	{
		Point startPos;
		bool isDragging;

        ProgressBar thermoObject;
        public ThermoTool(UIElement element)
            : base(element)
		{
		}

		#region ITool implementation
		public String ToolName
		{
			get { return "Thermo Tool"; }
		}

		public String ToolGroup
		{
			get { return StringResources.ToolContentGroupName; }
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

                thermoObject = new ProgressBar();
                thermoObject.Orientation = Orientation.Vertical;
                thermoObject.Template = (ControlTemplate)EditorHelper.TemplateResources["thermoTemplate"];
				thermoObject.Opacity = 0.75;
				Canvas.SetLeft(thermoObject, startPos.X);
				Canvas.SetTop(thermoObject, startPos.Y);
				thermoObject.Width = 0;
				thermoObject.Height = 0;
				//guageObject.Content = "Button";

				visualChildren.Add(thermoObject);

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
                
				thermoObject.Width = v.X;
				thermoObject.Height = v.Y;

				InvalidateArrange();

				e.Handled = true;
			}

			base.OnPreviewMouseMove(e);
		}

		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if(isDragging)
			{
				visualChildren.Remove(thermoObject);
				thermoObject.Opacity = 1;
				
				if (thermoObject.Width < 10)
					thermoObject.Width = 10;
				if (thermoObject.Height < 10)
					thermoObject.Height = 10;

                NotifyObjectCreated(thermoObject);
				SelectedObject = thermoObject;

				isDragging = false;
				thermoObject = null;
				ReleaseMouseCapture();

				e.Handled = true;
			}
			base.OnPreviewMouseLeftButtonUp(e);
		}

		protected override Size MeasureOverride(Size finalSize)
		{
			base.MeasureOverride(finalSize);
			if (thermoObject != null)
				thermoObject.Measure(finalSize);
			return finalSize;
		}
		protected override Size ArrangeOverride(Size finalSize)
		{
			base.ArrangeOverride(finalSize);
			if (thermoObject != null)
			{
				double x = Canvas.GetLeft(thermoObject);
				double y = Canvas.GetTop(thermoObject);
				thermoObject.Arrange(new Rect(new Point(x, y), thermoObject.DesiredSize));
			}
            
			return finalSize;
		}
	}
}
