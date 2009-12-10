using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Common.Schema;


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
                StringResources.HelperSelectingToolTooltip,
                CultureInfo.GetCultureInfo("en-us"),
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                14,
                b);
            Rect bounds=VisualTreeHelper.GetContentBounds(helperFor);
            drawingContext.DrawText(formattedText, new Point(Canvas.GetLeft(helperFor), Canvas.GetTop(helperFor) - 40));
            Pen p=new Pen(b, 1);
            p.DashStyle=DashStyles.DashDot;
            drawingContext.DrawLine(p, new Point(Canvas.GetLeft(helperFor) + bounds.Width / 2, Canvas.GetTop(helperFor) + bounds.Height / 2), e.GetPosition(AdornedElement));
            Shape sh;
            if ((sh=ValidateMousePos(e.GetPosition(AdornedElement)))!=null)//TODO: candidate to refectoring, ned take type of object from caller
            {
                
                Geometry g;
                g= sh.RenderedGeometry; ;
                g.Transform = (Transform)sh.TransformToVisual(AdornedElement);
                SolidColorBrush hb=new SolidColorBrush();//=Brushes.Yellow.Clone();
                System.Drawing.Color value = System.Drawing.SystemColors.Highlight;
                hb.Color = Color.FromArgb(value.A, value.R, value.G, value.B); 
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
            /*e.Handled = false;
            if (ValidateMousePos(e.GetPosition(AdornedElement)) != null)
                base.OnPreviewMouseLeftButtonDown(e);
            else
                SelectedObject = null;*/
         }
        protected override void OnPreviewMouseRightButtonDown(MouseButtonEventArgs e)
        {
            //SelectedObject = null;
        }
        public override BaseManipulator CreateToolManipulator(UIElement obj)
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
