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
	class GuegeTool : BaseTool, ITool
	{
		Point startPos;
		bool isDragging;

        ProgressBar guageObject;
        public GuegeTool(SchemaDocument doc)
			: base(doc)
		{
		}

		#region ITool implementation
		public String ToolName
		{
			get { return "Guage Tool"; }
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

                guageObject = new ProgressBar();
   
                guageObject.Template =(ControlTemplate) EditorHelper.TemplateResources["guageTemplate"];
				guageObject.Opacity = 0.75;
				Canvas.SetLeft(guageObject, startPos.X);
				Canvas.SetTop(guageObject, startPos.Y);
				guageObject.Width = 0;
				guageObject.Height = 0;
				//guageObject.Content = "Button";

				visualChildren.Add(guageObject);

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
				guageObject.Width = v.X;
				guageObject.Height = v.Y;

				InvalidateArrange();

				e.Handled = true;
			}

			base.OnPreviewMouseMove(e);
		}

		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if(isDragging)
			{
				visualChildren.Remove(guageObject);
				guageObject.Opacity = 1;
				
				if (guageObject.Width < 10)
					guageObject.Width = 10;
				if (guageObject.Height < 10)
					guageObject.Height = 10;

				UndoRedoManager.GetUndoBuffer(workedSchema).AddCommand(new AddGraphicsObject(guageObject));
				SelectedObject = guageObject;

				isDragging = false;
				guageObject = null;
				ReleaseMouseCapture();

				e.Handled = true;
			}
			base.OnPreviewMouseLeftButtonUp(e);
		}

		protected override Size MeasureOverride(Size finalSize)
		{
			base.MeasureOverride(finalSize);
			if (guageObject != null)
				guageObject.Measure(finalSize);
			return finalSize;
		}
		protected override Size ArrangeOverride(Size finalSize)
		{
			base.ArrangeOverride(finalSize);
			if (guageObject != null)
			{
				double x = Canvas.GetLeft(guageObject);
				double y = Canvas.GetTop(guageObject);
				guageObject.Arrange(new Rect(new Point(x, y), guageObject.DesiredSize));
			}
            
			return finalSize;
		}
	}
}
