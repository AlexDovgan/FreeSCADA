﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{//drawingContext.DrawEllipse(Brushes.Gray, new Pen(Brushes.Black, 1), startPos, vec.X, vec.Y);
              
    /// <summary>
    /// Ellipse object creation tool
    /// </summary>
    /// 
    class EllipseTool : DrawTool
    {
        public EllipseTool(UIElement element)
            : base(element)
        {

        }

        protected override void DrawPreview(DrawingContext context, Rect rect)
        {
            context.DrawEllipse(Brushes.Gray, new Pen(Brushes.Black, 1),
                new Point(rect.X + rect.Width / 2,
                    rect.Y + rect.Height / 2),
                rect.Width / 2,
                rect.Height / 2);
        }
        protected override UIElement DrawEnded(Rect rect)
        {
            Ellipse ellipse = new Ellipse();
            Canvas.SetLeft(ellipse, rect.X);
            Canvas.SetTop(ellipse, rect.Y);
            ellipse.Width = rect.Width;
            ellipse.Height = rect.Height;
            ellipse.Stroke = Brushes.Black;
            ellipse.Fill = Brushes.Gray;
            return ellipse;
        }

        
    }
}
