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
    class ControlCreateTool<T>:BaseTool
    {
        Point startPos;
		bool isDragging;
		Control createdObject;
        DrawingVisual boundce = new DrawingVisual();

        public ControlCreateTool(UIElement element)
            : base(element)
		{
            if(!typeof(T).IsSubclassOf(typeof(Control)))
                throw new Exception();
            visualChildren.Add(boundce);
		}
		protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonDown(e);
			if (!e.Handled)
			{
				startPos = e.GetPosition(this);

				createdObject = (Control)System.Activator.CreateInstance(typeof(T));
    			createdObject.Opacity = 0.75;
				Canvas.SetLeft(createdObject, startPos.X);
				Canvas.SetTop(createdObject, startPos.Y);
				createdObject.Width = 0;
				createdObject.Height = 0;
				//buttonObject.Content = "Button";

				visualChildren.Add(createdObject);

				isDragging = true;
				CaptureMouse();

				e.Handled = true;
			}
            e.Handled = false;
		}

		protected override void OnPreviewMouseMove(MouseEventArgs e)
		{
			if (isDragging)
			{
				Vector v = e.GetPosition(this) - startPos;
                if (v.X <=0 || v.Y <= 0)
                {
                    
                }
                else
                {
                    createdObject.Width = v.X;
                    createdObject.Height = v.Y;
                    
                    DrawingContext drawingContext = boundce.RenderOpen();
                    Rect rect = new Rect(startPos, v);
                    Pen pen = new Pen(Brushes.Black, 0.1);
                    pen.DashStyle = DashStyles.DashDotDot;
                    drawingContext.DrawRectangle(null,pen , rect);

                    drawingContext.Close();
                }

				InvalidateArrange();

				e.Handled = true;
			}

			base.OnPreviewMouseMove(e);
		}

		protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			if(isDragging)
			{
                boundce.RenderOpen().Close();
                visualChildren.Remove(createdObject);
				createdObject.Opacity = 1;
                ReleaseMouseCapture();
                if (createdObject.Width < 10 || createdObject.Height < 10)
                {
                    createdObject = null;
                    isDragging = false;
                    return;
                }
    			
				
                NotifyObjectCreated(createdObject);
				SelectedObject = createdObject;

				isDragging = false;
				createdObject = null;
				

				e.Handled = true;
			}
			base.OnPreviewMouseLeftButtonUp(e);
		}

		protected override Size MeasureOverride(Size finalSize)
		{
			base.MeasureOverride(finalSize);
			if (createdObject != null)
				createdObject.Measure(finalSize);
			return finalSize;
		}
		protected override Size ArrangeOverride(Size finalSize)
		{
			base.ArrangeOverride(finalSize);
			if (createdObject != null)
			{
				double x = Canvas.GetLeft(createdObject);
				double y = Canvas.GetTop(createdObject);
				createdObject.Arrange(new Rect(new Point(x, y), createdObject.DesiredSize));
			}

			return finalSize;
		}
        protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return ObjectsFactory.CreateDefaultManipulator(obj);
        }

    }
    
}
