using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using System.Globalization;


namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class HelperSelectorTool:BaseTool
    {
        DrawingVisual objectPrview = new DrawingVisual();

        UIElement helperFor;
        public HelperSelectorTool(UIElement element,UIElement helperfor)
            : base(element)
        {
            helperFor = helperfor;
            visualChildren.Add(objectPrview);
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
                
            DrawingContext drawingContext = objectPrview.RenderOpen();
            Brush b =  Brushes.Black.Clone();
            b.Opacity = 0.3;
            FormattedText formattedText = new FormattedText(
                "Please select trajectory object",
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                14,
                b);
            Rect bounds=VisualTreeHelper.GetContentBounds(helperFor);
            drawingContext.DrawText(formattedText, new Point(Canvas.GetLeft(helperFor), Canvas.GetTop(helperFor) - 20));
            drawingContext.DrawLine(new Pen(b, 1), new Point(Canvas.GetLeft(helperFor) + bounds.Width / 2, Canvas.GetTop(helperFor) + bounds.Height / 2), e.GetPosition(AdornedElement));
            Shape sh;
            if ((sh=ValidateMousePos(e.GetPosition(AdornedElement)))!=null)//TODO: candidate to refectoring, ned take type of object from caller
            {
                
                Geometry g;
                g= sh.RenderedGeometry; ;
                g.Transform = (Transform)sh.TransformToVisual(AdornedElement);
                Brush hb=Brushes.Yellow.Clone();
                hb.Opacity=0.2;
                drawingContext.DrawGeometry(null,new Pen(hb,5),g);
                        
            }


            drawingContext.Close();
    
        }
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();
            base.OnPreviewMouseLeftButtonUp(e);
            
        }
        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = false;
            if(ValidateMousePos(e.GetPosition(AdornedElement))!=null)
                base.OnPreviewMouseLeftButtonDown(e);
         }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
        //    ReleaseMouseCapture();
        //    if (pointsCollection.Count == 0)
        //        NotifyToolFinished();
        //    if (pointsCollection.Count > 1)
        //    {
        //        Rect b = VisualTreeHelper.GetContentBounds(objectPrview);
        //        Polyline poly = new Polyline();
        //        for (int i = 0; i < pointsCollection.Count; i++)
        //        {
        //            Point p=pointsCollection[i];
        //            p.X -= b.X;
        //            p.Y -= b.Y;
        //            pointsCollection[i] = p;
        //        }
        //        poly.Points = pointsCollection.Clone();
        //        pointsCollection.Clear();
        //        Canvas.SetLeft(poly, b.X);
        //        Canvas.SetTop(poly, b.Y);
        //        poly.Width = b.Width;
        //        poly.Height = b.Height;
        //        poly.Stroke = Brushes.Black;
        //        poly.Fill = Brushes.Transparent;
        //        poly.Stretch = Stretch.Fill;
        //        NotifyObjectCreated(poly);
        //        SelectedObject = poly;
        //    }
        //    pointsCollection.Clear();
        //    objectPrview.RenderOpen().Close();
        }
        protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return null;
        }
        protected Shape ValidateMousePos(Point p)
        {
            HitTestResult res = VisualTreeHelper.HitTest(AdornedElement, p);

            if (res.VisualHit is Shape && res.VisualHit!=helperFor)//TODO: candidate to refectoring, ned take type of object from caller
                return res.VisualHit as Shape;
            return null;
        }
    }
}
