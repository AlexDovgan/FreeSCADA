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
using System.Diagnostics;
using FreeSCADA.Schema.Commands;

namespace FreeSCADA.Schema.Manipulators
{
    public class MoveResizeRotateManipulator : BaseManipulator
    {
        // Resizing adorner uses Thumbs for visual elements.  
        // The Thumbs have built-in mouse input handling.
        Thumb topLeft, topRight, bottomLeft, bottomRight;
        Thumb topRightRotate;
        RotateTransform rotTrans;


        Rectangle boundceRect;
        Point startMovedPos;
        bool Moved;

        // To store and manage the adorner's visual children.
        VisualCollection visualChildren;

        // Initialize the ResizingAdorner.
        public MoveResizeRotateManipulator(UIElement adornedElement,SchemaDocument schema)
            : base(adornedElement,schema)
        {
            visualChildren = new VisualCollection(this);
            // Call a helper method to initialize the Thumbs
            // with a customized cursors.
            topRightRotate = new Thumb();
            topRightRotate.Width = topRightRotate.Height = 20;
            topRightRotate.Cursor = Cursors.Hand;
            topRightRotate.Background = Brushes.Blue;
            topRightRotate.Opacity = 0.2;
            visualChildren.Add(topRightRotate);
            topRightRotate.DragDelta += new DragDeltaEventHandler(topRightRotate_DragDelta);
            topRightRotate.DragCompleted += new DragCompletedEventHandler(topRightRotate_DragCompleted);
            this.Cursor = Cursors.Cross;
            BuildAdornerCorner(ref topLeft, Cursors.SizeNWSE);
            BuildAdornerCorner(ref topRight, Cursors.SizeNESW);
            BuildAdornerCorner(ref bottomLeft, Cursors.SizeNESW);
            BuildAdornerCorner(ref bottomRight, Cursors.SizeNWSE);
            Moved = false;
            boundceRect = new Rectangle();

            boundceRect.Stroke = Brushes.Blue;
            Brush b = new SolidColorBrush(Colors.Gray);
            b.Opacity = 0.0;
            boundceRect.Fill = b;
            boundceRect.StrokeThickness = 1;

            visualChildren.Add(boundceRect);



            // Add handlers for resizing.
            bottomLeft.DragDelta += new DragDeltaEventHandler(HandleBottomLeft);
            bottomRight.DragDelta += new DragDeltaEventHandler(HandleBottomRight);
            topLeft.DragDelta += new DragDeltaEventHandler(HandleTopLeft);
            topRight.DragDelta += new DragDeltaEventHandler(HandleTopRight);
            ContextMenu = new ContextMenu();
            MenuItem mi = new MenuItem();
            mi.Header = "Ungroup";
            mi.CommandParameter = AdornedElement;
            mi.Command = new UngroupCommand();
            ContextMenu.Items.Add(mi);
            MenuItem mii = new MenuItem();
            mii.Header = "Group";
            mii.CommandParameter = AdornedElement;
            mii.Command = new GroupCommand();
            ContextMenu.Items.Add(mii);
            rotTrans = new RotateTransform();

            if (!(AdornedElement.RenderTransform is TransformGroup))
            {
                TransformGroup t = new TransformGroup();
                t.Children.Add(new MatrixTransform());
                t.Children.Add(new RotateTransform());
                AdornedElement.RenderTransform = t;
            }
            rotTrans.Angle = ((RotateTransform)((TransformGroup)AdornedElement.RenderTransform).Children[1]).Angle;
            boundceRect.RenderTransform = new MatrixTransform(AdornedElement.RenderTransform.Value);
        }

        void topRightRotate_DragCompleted(object sender, DragCompletedEventArgs e)
        {

            Point p = new Point(rotTrans.Value.OffsetX, rotTrans.Value.OffsetY);;
            Canvas.SetLeft(AdornedElement, Canvas.GetLeft(AdornedElement) + p.X);
            Canvas.SetTop(AdornedElement, Canvas.GetTop(AdornedElement) + p.Y);
            ((RotateTransform)((TransformGroup)AdornedElement.RenderTransform).Children[1]).Angle += rotTrans.Angle;
            boundceRect.RenderTransform = new MatrixTransform(AdornedElement.RenderTransform.Value);
            rotTrans.Angle = 0;
        }


        void topRightRotate_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Point p = AdornedElement.RenderTransform.Inverse.Transform(new Point(((FrameworkElement)AdornedElement).Width, ((FrameworkElement)AdornedElement).Height));
            Point p2 = new Point(e.HorizontalChange, e.VerticalChange);
            Point p3 = AdornedElement.RenderTransform.Transform(new Point(((FrameworkElement)AdornedElement).Width, ((FrameworkElement)AdornedElement).Height));


            Vector vector1 = new Vector((p.Y / 2 - p2.Y), (p.X / 2 + p2.X));
            Vector vector2 = new Vector(p.Y / 2, p.X / 2);
			//double angle = (Math.Atan(p.Y / p.X) - Math.Atan((p.Y / 2 - p2.Y) / (p.X / 2 + p2.X))) * 180 / Math.PI;


            //Trace.WriteLine(p.X+" "+p.Y+" "+Vector.AngleBetween(vector2, vector1));

            rotTrans.CenterX = p3.X / 2;
            rotTrans.CenterY = p3.Y / 2;
            rotTrans.Angle = Vector.AngleBetween(vector2, vector1);
            boundceRect.RenderTransform = new MatrixTransform(AdornedElement.RenderTransform.Value * rotTrans.Value);


        }

        // Handler for resizing from the bottom-right.
        void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);
            double x = 0, y = 0;
            Matrix m = AdornedElement.RenderTransform.Value;
            m.Invert();
            m.OffsetX = 0; m.OffsetY = 0;

            Point p = m.Transform(new Point(args.HorizontalChange, args.VerticalChange));

            if (adornedElement.Width + p.X > hitThumb.Width)
                x = p.X;
            if (adornedElement.Height + p.Y > hitThumb.Height)
                y = p.Y;

            adornedElement.Width = adornedElement.Width + x;
            adornedElement.Height = adornedElement.Height + y;


        }

        // Handler for resizing from the bottom-left.
        void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            double x = 0, y = 0;
            Matrix m = AdornedElement.RenderTransform.Value;
            m.Invert();
            m.OffsetX = 0; m.OffsetY = 0;

            Point p = m.Transform(new Point(args.HorizontalChange, args.VerticalChange));

            if (adornedElement.Width - p.X > hitThumb.Width)
                x = p.X;
            if (adornedElement.Height + p.Y > hitThumb.Height)
                y = p.Y;

            adornedElement.Width = adornedElement.Width - x;
            adornedElement.Height = adornedElement.Height + y;
            p = AdornedElement.RenderTransform.Transform(new Point(x, 0));


            if (adornedElement.Width > hitThumb.Width)
                Canvas.SetLeft(adornedElement, Canvas.GetLeft(adornedElement) + p.X - AdornedElement.RenderTransform.Value.OffsetX);
            if (adornedElement.Height > hitThumb.Height)
                Canvas.SetTop(adornedElement, Canvas.GetTop(adornedElement) + p.Y - AdornedElement.RenderTransform.Value.OffsetY);


        }

        // Handler for resizing from the top-right.
        void HandleTopRight(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = this.AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;
            FrameworkElement parentElement = adornedElement.Parent as FrameworkElement;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            double x = 0, y = 0;
            Matrix m = AdornedElement.RenderTransform.Value;
            m.Invert();
            m.OffsetX = 0; m.OffsetY = 0;

            Point p = m.Transform(new Point(args.HorizontalChange, args.VerticalChange));

            if (adornedElement.Width + p.X > hitThumb.Width)
                x = p.X;
            if (adornedElement.Height - p.Y > hitThumb.Height)
                y = p.Y;

            adornedElement.Width = adornedElement.Width + x;
            adornedElement.Height = adornedElement.Height - y;

            p = AdornedElement.RenderTransform.Value.Transform(new Point(0, y));
            if (adornedElement.Height > hitThumb.Height)
                Canvas.SetTop(adornedElement, Canvas.GetTop(adornedElement) + p.Y - AdornedElement.RenderTransform.Value.OffsetY);
            if (adornedElement.Width > hitThumb.Width)
                Canvas.SetLeft(adornedElement, Canvas.GetLeft(adornedElement) + p.X - AdornedElement.RenderTransform.Value.OffsetX);

        }

        // Handler for resizing from the top-left.
        void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {
            FrameworkElement adornedElement = AdornedElement as FrameworkElement;
            Thumb hitThumb = sender as Thumb;

            if (adornedElement == null || hitThumb == null) return;

            // Ensure that the Width and Height are properly initialized after the resize.
            EnforceSize(adornedElement);

            // Change the size by the amount the user drags the mouse, as long as it's larger 
            // than the width or height of an adorner, respectively.
            double x = 0, y = 0;
            Matrix m = AdornedElement.RenderTransform.Value;
            m.Invert();
            m.OffsetX = 0; m.OffsetY = 0;

            Point p = m.Transform(new Point(args.HorizontalChange, args.VerticalChange));

            if (adornedElement.Width - p.X > hitThumb.Width)
                x = p.X;
            if (adornedElement.Height - p.Y > hitThumb.Height)
                y = p.Y;

            adornedElement.Width = adornedElement.Width - x;
            adornedElement.Height = adornedElement.Height - y;

            p = AdornedElement.RenderTransform.Transform(new Point(x, y));
            if (adornedElement.Width > hitThumb.Width)
                Canvas.SetLeft(adornedElement, Canvas.GetLeft(adornedElement) + p.X - AdornedElement.RenderTransform.Value.OffsetX);
            if (adornedElement.Height > hitThumb.Height)
                Canvas.SetTop(adornedElement, Canvas.GetTop(adornedElement) + p.Y - AdornedElement.RenderTransform.Value.OffsetY);
        }

        // Arrange the Adorners.
        protected override Size ArrangeOverride(Size finalSize)
        {
            Point p;
            Rect ro = new Rect(0, 0, AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height);
            topLeft.Arrange(new Rect(-topLeft.Width / 2, -topLeft.Height / 2, topLeft.Width, topLeft.Height));
            p = AdornedElement.RenderTransform.Transform(new Point(AdornedElement.DesiredSize.Width, 0));
            topRight.Arrange(new Rect(p.X - topLeft.Width / 2, p.Y - topLeft.Height / 2, topLeft.Width, topLeft.Height));
            topRightRotate.Arrange(new Rect(p.X - topRightRotate.Width / 2, p.Y - topRightRotate.Height / 2, topRightRotate.Width, topRightRotate.Height));

            p = AdornedElement.RenderTransform.Transform(new Point(0, AdornedElement.DesiredSize.Height));
            bottomLeft.Arrange(new Rect(p.X - topLeft.Width / 2, p.Y - topLeft.Height / 2, topLeft.Width, topLeft.Height));
            p = AdornedElement.RenderTransform.Transform(new Point(AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height));
            bottomRight.Arrange(new Rect(p.X - topLeft.Width / 2, p.Y - topLeft.Height / 2, topLeft.Width, topLeft.Height));

            boundceRect.Arrange(ro);
            // Return the final size.
            return finalSize;
        }
        // Helper method to instantiate the corner Thumbs, set the Cursor property, 
        // set some appearance properties, and add the elements to the visual tree.
        void BuildAdornerCorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            if (cornerThumb != null) return;

            cornerThumb = new Thumb();

            // Set some arbitrary visual characteristics.
            cornerThumb.Cursor = customizedCursor;
            cornerThumb.Height = cornerThumb.Width = 10;
            cornerThumb.Opacity = 1;
            //cornerThumb.Background = Brushes.Gray;//new SolidColorBrush(Colors.MediumBlue);


            visualChildren.Add(cornerThumb);
        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        // This method ensures that the Widths and Heights are initialized.  Sizing to content produces
        // Width and Height values of Double.NaN.  Because this Adorner explicitly resizes, the Width and Height
        // need to be set first.  It also sets the maximum size of the adorned element.
        void EnforceSize(FrameworkElement adornedElement)
        {
            if (adornedElement.Width.Equals(Double.NaN))
                adornedElement.Width = adornedElement.DesiredSize.Width;
            if (adornedElement.Height.Equals(Double.NaN))
                adornedElement.Height = adornedElement.DesiredSize.Height;

            FrameworkElement parent = adornedElement.Parent as FrameworkElement;
            if (parent != null)
            {
                adornedElement.MaxHeight = parent.ActualHeight;
                adornedElement.MaxWidth = parent.ActualWidth;
            }
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            startMovedPos = e.GetPosition(this); ;
            //Point aaa =  e.GetPosition(this); ;
            CaptureMouse();
            Moved = true;
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            Moved = false;
            ReleaseMouseCapture();

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {

            if (Moved == true)
            {
                if (AdornedElement is Canvas) return;
                Point newPos = e.GetPosition(this);
                Vector v = newPos - startMovedPos;

                Vector off = VisualTreeHelper.GetOffset(AdornedElement);

                Point p = /*AdornedElement.RenderTransform.Transform(*/new Point(v.X, v.Y);//);
                Canvas.SetLeft(AdornedElement, off.X + p.X - AdornedElement.RenderTransform.Value.OffsetX);
                Canvas.SetTop(AdornedElement, off.Y + p.Y - AdornedElement.RenderTransform.Value.OffsetY);
            }

        }


        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {

            TranslateTransform ttr = new TranslateTransform();
            ttr.X = ((Transform)transform).Value.OffsetX;
            ttr.Y = ((Transform)transform).Value.OffsetY;
            return ttr;
        }

    }


}
