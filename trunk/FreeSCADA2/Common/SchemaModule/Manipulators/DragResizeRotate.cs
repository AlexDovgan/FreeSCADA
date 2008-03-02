using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using FreeSCADA.Schema.Context_Menu;
using FreeSCADA.Schema.UndoRedo;
using System.Windows.Documents;

namespace FreeSCADA.Schema.Manipulators
{
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
        
        public DragResizeRotateManipulator(FrameworkElement el, SchemaDocument sch)
            : base(el, sch)
        {
            if (!((AdornedElement as FrameworkElement).RenderTransform is TransformGroup))
            {
                TransformGroup t = new TransformGroup();
                t.Children.Add(new MatrixTransform());
                t.Children.Add(new RotateTransform());
                (AdornedElement as FrameworkElement).RenderTransform = t;
            }
            //el.RenderTransformOrigin = new Point(0.5, 0.5); ;
            ThumbsResources tr = new ThumbsResources();
            tr.InitializeComponent();
            Resources = tr;


            dragControl.DataContext = el;
            dragControl.VerticalAlignment = VerticalAlignment.Stretch;
            dragControl.HorizontalAlignment = HorizontalAlignment.Stretch;
            dragControl.Cursor = Cursors.SizeAll;
            dragControl.RenderTransform = el.RenderTransform;

            visualChildren.Add(dragControl);

            rotateTopLeft.DataContext = el;
            rotateTopLeft.VerticalAlignment = VerticalAlignment.Top;
            rotateTopLeft.HorizontalAlignment = HorizontalAlignment.Left;
            visualChildren.Add(rotateTopLeft);

            rotateBottomLeft.DataContext = el;
            //rotateBottomLeft.RenderTransform = new RotateTransform(270);
            rotateBottomLeft.VerticalAlignment = VerticalAlignment.Bottom;
            rotateBottomLeft.HorizontalAlignment = HorizontalAlignment.Left;
            visualChildren.Add(rotateBottomLeft);

            rotateTopRight.DataContext = el;
            //rotateTopRight.RenderTransform = new RotateTransform(90);
            rotateTopRight.VerticalAlignment = VerticalAlignment.Top;
            rotateTopRight.HorizontalAlignment = HorizontalAlignment.Right;
            visualChildren.Add(rotateTopRight);

            rotateBottomRight.DataContext = el;
            //rotateBottomRight.RenderTransform = new RotateTransform(180);
            rotateBottomRight.VerticalAlignment = VerticalAlignment.Bottom;
            rotateBottomRight.HorizontalAlignment = HorizontalAlignment.Right;
            visualChildren.Add(rotateBottomRight);

            resizeTopLeft.DataContext = el;
            resizeTopLeft.Cursor = Cursors.SizeNWSE;
            resizeTopLeft.VerticalAlignment = VerticalAlignment.Top;
            resizeTopLeft.HorizontalAlignment = HorizontalAlignment.Left;
            visualChildren.Add(resizeTopLeft);

            resizeBottomLeft.DataContext = el;
            resizeBottomLeft.Cursor = Cursors.SizeNESW;
            resizeBottomLeft.VerticalAlignment = VerticalAlignment.Bottom;
            resizeBottomLeft.HorizontalAlignment = HorizontalAlignment.Left;
            visualChildren.Add(resizeBottomLeft);

            resizeTopRight.DataContext = el;
            resizeTopRight.Cursor = Cursors.SizeNESW;
            resizeTopRight.VerticalAlignment = VerticalAlignment.Top;
            resizeTopRight.HorizontalAlignment = HorizontalAlignment.Right;
            visualChildren.Add(resizeTopRight);

            resizeBottomRight.DataContext = el;
            resizeBottomRight.Cursor = Cursors.SizeNWSE;
            resizeBottomRight.VerticalAlignment = VerticalAlignment.Bottom;
            resizeBottomRight.HorizontalAlignment = HorizontalAlignment.Right;
            visualChildren.Add(resizeBottomRight);

            resizeLeft.DataContext = el;
            resizeLeft.Cursor = Cursors.SizeWE;
            resizeLeft.HorizontalAlignment = HorizontalAlignment.Left;
            resizeLeft.VerticalAlignment = VerticalAlignment.Center;
            visualChildren.Add(resizeLeft);

            resizeRight.DataContext = el;
            resizeRight.Cursor = Cursors.SizeWE;
            resizeRight.HorizontalAlignment = HorizontalAlignment.Right;
            resizeRight.VerticalAlignment = VerticalAlignment.Center;
            visualChildren.Add(resizeRight);

            resizeTop.DataContext = el;
            resizeTop.Cursor = Cursors.SizeNS;
            resizeTop.HorizontalAlignment = HorizontalAlignment.Center;
            resizeTop.VerticalAlignment = VerticalAlignment.Top;
            visualChildren.Add(resizeTop);

            resizeBottom.DataContext = el;
            resizeBottom.Cursor = Cursors.SizeNS;
            resizeBottom.HorizontalAlignment = HorizontalAlignment.Center;
            resizeBottom.VerticalAlignment = VerticalAlignment.Bottom;
            visualChildren.Add(resizeBottom);

            foreach (Thumb control in visualChildren)
            {
                control.DragStarted += new DragStartedEventHandler(control_DragStarted);
                control.DragCompleted += new DragCompletedEventHandler(control_DragCompleted);
                control.DragDelta += new DragDeltaEventHandler(control_DragDelta);
            }

        }

        void control_DragDelta(object sender, DragDeltaEventArgs e)
        {
            RaiseObjectChamnedEvent();
        }

        void control_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            RaiseObjectChamnedEvent();
        }
        

        void control_DragStarted(object sender, DragStartedEventArgs e)
        {
            UndoRedoManager.GetUndoBuffer(workedSchema).AddCommand(new ModifyGraphicsObject(AdornedElement));
            RaiseObjectChamnedEvent();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect ro = new Rect(0, 0, AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height);
            
            foreach (Thumb control in visualChildren)
            {
                Rect aligmentRect = new Rect();
                aligmentRect.Width = control.Width;
                aligmentRect.Height = control.Height;
                
                switch (control.VerticalAlignment)
                {   
                    case VerticalAlignment.Top: aligmentRect.Y = 0;
                        break;
                    case VerticalAlignment.Bottom: aligmentRect.Y = ro.Height;
                        break;
                    case VerticalAlignment.Center: aligmentRect.Y = ro.Height / 2;
                        break;
                    case VerticalAlignment.Stretch: aligmentRect.Height = ro.Height;
                        break;
                    default:
                        break;
                }
                switch (control.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left: aligmentRect.X = 0;
                        break;
                    case HorizontalAlignment.Right: aligmentRect.X = ro.Width;
                        break;
                    case HorizontalAlignment.Center: aligmentRect.X = ro.Width / 2;
                        break;
                    case HorizontalAlignment.Stretch: aligmentRect.Width = ro.Width;
                        break;
                    default:
                        break;
                }
                Matrix m = AdornedElement.RenderTransform.Value;
                m.OffsetX =- control.RenderTransform.Value.OffsetX;
                m.OffsetY= - control.RenderTransform.Value.OffsetY;

                Point p = m.Transform(new Point(aligmentRect.X, aligmentRect.Y));
               
                aligmentRect.X = p.X - (double.IsNaN(control.Width) ? 0 : control.Width) / 2;
                aligmentRect.Y = p.Y - (double.IsNaN(control.Height) ? 0 : control.Height) / 2;
            
                control.Arrange(aligmentRect);
            }
            return finalSize;
        }

       
    }
}
