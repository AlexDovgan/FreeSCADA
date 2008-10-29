using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.SchemaCommands;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.ShellInterfaces;


namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class PolylineTool:BaseTool
    {
        DrawingVisual objectPrview = new DrawingVisual();
        PointCollection pointsCollection = new PointCollection();


        public PolylineTool(UIElement element)
            : base(element)
        {
            visualChildren.Add(objectPrview);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (pointsCollection.Count>0)
            {
                
                DrawingContext drawingContext = objectPrview.RenderOpen();
                for (int i = 1; i < pointsCollection.Count;i++ )
                {

                    drawingContext.DrawLine(new Pen(Brushes.Black, 1), pointsCollection[i - 1], pointsCollection[i]);
                        
               }

                drawingContext.DrawLine(new Pen(Brushes.Black, 1), pointsCollection[pointsCollection.Count-1],e.GetPosition(this));

                drawingContext.Close();
            }

        }
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //ReleaseMouseCapture();
            //base.OnPreviewMouseLeftButtonUp(e);
            
        }
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            if (pointsCollection.Count == 0)
                base.OnPreviewMouseLeftButtonDown(e);
            if (SelectedObject==null)
            {
                CaptureMouse();
                pointsCollection.Add(e.GetPosition(this));

            }
            
            e.Handled = false;

        }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            if (pointsCollection.Count == 0)
                NotifyToolFinished();
            if (pointsCollection.Count > 1)
            {
                Rect b = VisualTreeHelper.GetContentBounds(objectPrview);
                Polyline poly = new Polyline();
                for (int i = 0; i < pointsCollection.Count; i++)
                {
                    Point p=pointsCollection[i];
                    p.X -= b.X;
                    p.Y -= b.Y;
                    pointsCollection[i] = p;
                }
                poly.Points = pointsCollection.Clone();
                pointsCollection.Clear();
                Canvas.SetLeft(poly, b.X);
                Canvas.SetTop(poly, b.Y);
                poly.Width = b.Width;
                poly.Height = b.Height;
                poly.Stroke = Brushes.Black;
                poly.Fill = Brushes.Transparent;
                poly.Stretch = Stretch.Fill;
                NotifyObjectCreated(poly);
                SelectedObject = poly;
            }
            pointsCollection.Clear();
            objectPrview.RenderOpen().Close();
        }
        protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            if (obj is Polyline)
                return new PolylineEditManipulantor(obj as Polyline);
            else return base.CreateToolManipulator(obj);
        }

    }
}
