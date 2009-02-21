﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators;


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
            UIElement selObj = SelectedObject;
            if (pointsCollection.Count == 0)
                base.OnPreviewMouseLeftButtonDown(e);
            if (SelectedObject==null)
            {
                CaptureMouse();
                pointsCollection.Add(e.GetPosition(this));

            }
            // creating a new polyline point when clicking to the line with Ctrl key
            else if (selObj == SelectedObject && (System.Windows.Forms.Control.ModifierKeys & System.Windows.Forms.Keys.Control) != 0)
            {
                if (selObj is Polyline)
                {
                    for (int i = 0; i < (selObj as Polyline).Points.Count - 1; i++)
                    {
                        // Hit test
                        LineGeometry lg = new LineGeometry((selObj as Polyline).Points[i], (selObj as Polyline).Points[i + 1]);
                        EllipseGeometry eg = new EllipseGeometry(e.GetPosition(this), (selObj as Polyline).StrokeThickness, (selObj as Polyline).StrokeThickness);
                        IntersectionDetail id = eg.FillContainsWithDetail(lg);
                        if (id == IntersectionDetail.Intersects)
                        {
                            // Insert point to the polyline
                            (selObj as Polyline).Points.Insert(i+1, e.GetPosition(this));
                            // Rendering (new thumbs)
                            if (ToolManipulator != null)
                            {
                                (ToolManipulator as PolylineEditManipulantor).AddThumb(e.GetPosition(this));
                            }
                            break;
                        }
                    }
                }
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
                
                Polyline poly = new Polyline();
                Rect b = new Rect();
                b.Y = b.X = System.Double.MaxValue;
                b.Width = b.Height = 0;
                foreach(Point p in pointsCollection)
                {
                    if (p.X < b.X)
                        b.X = p.X;
                    if (p.Y < b.Y)
                        b.Y = p.Y;
                    if (p.X > b.Width)
                        b.Width = p.X;
                    if (p.Y > b.Height)
                        b.Height = p.Y;
                }
                b.Width -= b.X;
                b.Height -= b.Y;
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
            e.Handled = true;
        }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            // We do not want the context menu when closing line
            e.Handled = true;
        }

        protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            if (obj is Polyline)
                return new PolylineEditManipulantor(obj as Polyline);
            else return base.CreateToolManipulator(obj);
        }

    }
}
