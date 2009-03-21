using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Designer.SchemaEditor.Manipulators;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class ControlCreateTool<T>:DrawTool
    {
        //Point startPos;
		//bool isDragging;
		FrameworkElement createdObject;
        DrawingVisual boundce = new DrawingVisual();

        public ControlCreateTool(UIElement element)
            : base(element)
		{
            if(!typeof(T).IsSubclassOf(typeof(FrameworkElement)))
                throw new Exception();
            visualChildren.Add(boundce);
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
        public override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            //return ObjectsFactory.CreateDefaultManipulator(obj);
            return new DragResizeRotateManipulator(obj);
        }


        protected override void DrawPreview(DrawingContext context, Rect rect)
        {
            context.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 1), rect);
        }
        protected override UIElement DrawEnded(Rect rect)
        {
            createdObject = (FrameworkElement)System.Activator.CreateInstance(typeof(T));
            createdObject.Opacity = 0.75;
            if (typeof(T) == typeof(ContentControl) || typeof(T).IsSubclassOf(typeof(ContentControl)))
                (createdObject as ContentControl).Content = "Content";

            Canvas.SetLeft(createdObject, rect.X);
            Canvas.SetTop(createdObject, rect.Y);
            createdObject.Width = rect.Width;
            createdObject.Height = rect.Height;
            return createdObject;
        }
        public override Type ToolEditingType()
        {
            return typeof(T);
        }
    }
    
}
