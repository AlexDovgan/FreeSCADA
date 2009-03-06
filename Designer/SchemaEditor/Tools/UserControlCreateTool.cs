using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FreeSCADA.Designer.SchemaEditor.Manipulators;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public class UserControlCreateTool:DrawTool
    {
        /// <summary>
        /// 
        /// </summary>
        static Type controlType = typeof(Control);      // Trick / this field will be set to another value in a dynamically defined child class

		FrameworkElement createdObject;
        DrawingVisual boundce = new DrawingVisual();
        /// <summary>
        /// 
        /// </summary>
        public UserControlCreateTool()
            :base()
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="element"></param>
        public UserControlCreateTool(UIElement element)
            : base(element)
		{
            if (!controlType.IsSubclassOf(typeof(FrameworkElement)))
                throw new Exception();
            visualChildren.Add(boundce);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="finalSize"></param>
		/// <returns></returns>
		protected override Size MeasureOverride(Size finalSize)
		{
			base.MeasureOverride(finalSize);
			if (createdObject != null)
				createdObject.Measure(finalSize);
			return finalSize;
		}
        ///
        /// 
        /// 
        public void setControlType(Type ctrlType)
        {
            controlType = ctrlType;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return ObjectsFactory.CreateDefaultManipulator(obj);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="rect"></param>
        protected override void DrawPreview(DrawingContext context, Rect rect)
        {
            context.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 1), rect);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        protected override UIElement DrawEnded(Rect rect)
        {
            createdObject = (FrameworkElement)System.Activator.CreateInstance(controlType);
            createdObject.Opacity = 1.0;
            //if (controlType == typeof(ContentControl) || controlType.IsSubclassOf(typeof(ContentControl)))
            //    (createdObject as ContentControl).Content = "Content";

            Canvas.SetLeft(createdObject, rect.X);
            Canvas.SetTop(createdObject, rect.Y);
            createdObject.Width = rect.Width;
            createdObject.Height = rect.Height;
            return createdObject;
        }
    }
    
}
