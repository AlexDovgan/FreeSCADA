﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators.Controlls
{
    /// <summary>
    /// Drag controll for DragResizeRotateManipulator
    /// </summary>
    class PointDragThumb: Thumb
    {

        
        public PointDragThumb()
        {
        
            
        }
     
        
        void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            FrameworkElement item = this.DataContext as FrameworkElement;

            if (item != null)
            {
                double gridDelta = (double)item.FindResource("DesignerSettings_GridDelta");
                bool gridOn = (bool)item.FindResource("DesignerSettings_GridOn");
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                dragDelta = RenderTransform.Transform(dragDelta);

                double left = Canvas.GetLeft(item);
                double top = Canvas.GetTop(item);

                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                double x = left + dragDelta.X;
                double y = top + dragDelta.Y;
                if (gridOn)
                {
                    x -= x % gridDelta;
                    y -= y % gridDelta;
                }
                Canvas.SetLeft(item, x);
                Canvas.SetTop(item, y);
                
            }
            e.Handled = false;
        }
    }
}


