using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators.Controlls;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    //TODO:reimplement this manipulator on transform whith aplying changens on DragCompleted

    class DragResizeRotateManipulator : BaseManipulator
    {
        
        public DragThumb dragControl = new DragThumb();
        RotateThumb rotateTopLeft = new RotateThumb();
        RotateThumb rotateBottomLeft = new RotateThumb();
        RotateThumb rotateTopRight = new RotateThumb();
        RotateThumb rotateBottomRight = new RotateThumb();
        ResizeThumb resizeTopLeft = new ResizeThumb();
        ResizeThumb resizeBottomLeft = new ResizeThumb();
        ResizeThumb resizeTopRight = new ResizeThumb();
        ResizeThumb resizeBottomRight = new ResizeThumb();
        ResizeThumb resizeLeft = new ResizeThumb();
        ResizeThumb resizeRight = new ResizeThumb();
        ResizeThumb resizeTop = new ResizeThumb();
        ResizeThumb resizeBottom = new ResizeThumb();
        Rectangle visualCopy = new Rectangle();

        
        public DragResizeRotateManipulator(UIElement element)
            : base(element)
        {
            

            
            dragControl.VerticalAlignment = VerticalAlignment.Stretch;
            dragControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            dragControl.Cursor = Cursors.SizeAll;
            visualChildren.Add(dragControl);
            
            
            rotateTopLeft.VerticalAlignment = VerticalAlignment.Top;
            rotateTopLeft.HorizontalAlignment = HorizontalAlignment.Left;
            visualChildren.Add(rotateTopLeft);

            
            rotateBottomLeft.VerticalAlignment = VerticalAlignment.Bottom;
            rotateBottomLeft.HorizontalAlignment = HorizontalAlignment.Left;
            visualChildren.Add(rotateBottomLeft);

            
            rotateTopRight.VerticalAlignment = VerticalAlignment.Top;
            rotateTopRight.HorizontalAlignment = HorizontalAlignment.Right;
            visualChildren.Add(rotateTopRight);

            
            rotateBottomRight.VerticalAlignment = VerticalAlignment.Bottom;
            rotateBottomRight.HorizontalAlignment = HorizontalAlignment.Right;
            visualChildren.Add(rotateBottomRight);

            resizeTopLeft.Cursor = Cursors.SizeNWSE;
            resizeTopLeft.VerticalAlignment = VerticalAlignment.Top;
            resizeTopLeft.HorizontalAlignment = HorizontalAlignment.Left;
            visualChildren.Add(resizeTopLeft);

            resizeBottomLeft.Cursor = Cursors.SizeNESW;
            resizeBottomLeft.VerticalAlignment = VerticalAlignment.Bottom;
            resizeBottomLeft.HorizontalAlignment = HorizontalAlignment.Left;
            visualChildren.Add(resizeBottomLeft);

            resizeTopRight.Cursor = Cursors.SizeNESW;
            resizeTopRight.VerticalAlignment = VerticalAlignment.Top;
            resizeTopRight.HorizontalAlignment = HorizontalAlignment.Right;
            visualChildren.Add(resizeTopRight);

            resizeBottomRight.Cursor = Cursors.SizeNWSE;
            resizeBottomRight.VerticalAlignment = VerticalAlignment.Bottom;
            resizeBottomRight.HorizontalAlignment = HorizontalAlignment.Right;
            visualChildren.Add(resizeBottomRight);

            resizeLeft.Cursor = Cursors.SizeWE;
            resizeLeft.HorizontalAlignment = HorizontalAlignment.Left;
            resizeLeft.VerticalAlignment = VerticalAlignment.Center;
            visualChildren.Add(resizeLeft);

            resizeRight.Cursor = Cursors.SizeWE;
            resizeRight.HorizontalAlignment = HorizontalAlignment.Right;
            resizeRight.VerticalAlignment = VerticalAlignment.Center;
            visualChildren.Add(resizeRight);

            resizeTop.Cursor = Cursors.SizeNS;
            resizeTop.HorizontalAlignment = HorizontalAlignment.Center;
            resizeTop.VerticalAlignment = VerticalAlignment.Top;
            visualChildren.Add(resizeTop);

            resizeBottom.Cursor = Cursors.SizeNS;
            resizeBottom.HorizontalAlignment = HorizontalAlignment.Center;
            resizeBottom.VerticalAlignment = VerticalAlignment.Bottom;
            visualChildren.Add(resizeBottom);
            Activate();

        }
        public override void Activate()
        {
            if (!(AdornedElement is FrameworkElement))
                throw new Exception("This is not FrameworkElement");

            foreach (Thumb control in visualChildren)
            {

                control.DataContext = AdornedElement;
                control.DragStarted += new DragStartedEventHandler(control_DragStarted);
                control.DragCompleted += new DragCompletedEventHandler(control_DragCompleted);
                control.DragDelta += new DragDeltaEventHandler(control_DragDelta);
                if (control.Cursor == Cursors.SizeAll) control.RenderTransform = AdornedElement.RenderTransform;

            }
            base.Activate();
            
        }

        void control_DragDelta(object sender, DragDeltaEventArgs e)
        {
            RaiseObjectChangedEvent();
            InvalidateArrange();
        }

        void control_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            
  
            InvalidateArrange();
        }
        

        void control_DragStarted(object sender, DragStartedEventArgs e)
        {
        
            InvalidateArrange();
            RaiseObjectChangedPrevewEvent();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            //if (Visibility == Visibility.Hidden) return finalSize;
            Rect ro = new Rect(0, 0, AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height);
            //Rect ro = LayoutInformation.GetLayoutSlot(AdornedElement as FrameworkElement);
            foreach (Thumb control in visualChildren)
            {
                Rect aligmentRect = new Rect();
                aligmentRect.Width = control.Width;
                aligmentRect.Height = control.Height;
                aligmentRect.Y = ro.Y;
                aligmentRect.X = ro.X;
                switch (control.VerticalAlignment)
                {   
                    case VerticalAlignment.Top: aligmentRect.Y += 0;
                        break;
                    case VerticalAlignment.Bottom: aligmentRect.Y += ro.Height;
                        break;
                    case VerticalAlignment.Center: aligmentRect.Y += ro.Height / 2;
                        break;
                    case VerticalAlignment.Stretch: aligmentRect.Height = ro.Height;// *Math.Abs((AdornedElement.RenderTransform as TransformGroup).Children[0].Value.M22);
                        break;
                    default:
                        break;
                }
                switch (control.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left: aligmentRect.X += 0;
                        break;
                    case HorizontalAlignment.Right: aligmentRect.X += ro.Width;
                        break;
                    case HorizontalAlignment.Center: aligmentRect.X += ro.Width / 2;
                        break;
                    case HorizontalAlignment.Stretch: aligmentRect.Width = ro.Width;// *Math.Abs((AdornedElement.RenderTransform as TransformGroup).Children[0].Value.M11);
                        break;
                    default:
                        break;
                }
               
                //Point p = AdornedElement.TransformToVisual(this).Transform(new Point(aligmentRect.X, aligmentRect.Y));
                Point p = AdornedElement.TranslatePoint(new Point(aligmentRect.X, aligmentRect.Y), this);
                p.X -= control.RenderTransform.Value.OffsetX;
                p.Y -= control.RenderTransform.Value.OffsetY;
                
                aligmentRect.X =p.X-  (double.IsNaN(control.Width) ? 0 : control.Width) / 2;
                aligmentRect.Y =p.Y-  (double.IsNaN(control.Height) ? 0 : control.Height) / 2;
                
                //aligmentRect.X -= (double.IsNaN(control.Width) ? 0 : control.Width) / 2;
                //aligmentRect.Y -= (double.IsNaN(control.Height) ? 0 : control.Height) / 2; 
                control.Arrange(aligmentRect);
            }
            return finalSize;
        }
        protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
        }

       
    }
}