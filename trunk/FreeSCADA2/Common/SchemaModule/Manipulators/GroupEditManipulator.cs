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
    public class GroupEditManipulator : BaseManipulator
    {
        Thumb topLeft, topRight, bottomLeft, bottomRight;
        Thumb topRightRotate;
        Matrix Trans;
        Rectangle boundceRect;
        Point startMovedPos;
        bool Moved;
        VisualCollection visualChildren;
        public List<UIElement> selectedElements;


        public GroupEditManipulator(UIElement adornedElement,SchemaDocument schema)
            : base(adornedElement, schema)
        {
            visualChildren = new VisualCollection(this);
            // Call a helper method to initialize the Thumbs
            // with a customized cursors.
            topRightRotate = new Thumb();
            topRightRotate.Width = topRightRotate.Height = 20;
            topRightRotate.Cursor = Cursors.Hand;
            topRightRotate.Opacity = 0.0;
            visualChildren.Add(topRightRotate);
            topRightRotate.DragDelta += new DragDeltaEventHandler(topRightRotate_DragDelta);

            this.Cursor = Cursors.Cross;
            BuildAdornerCorner(ref topLeft, Cursors.SizeNWSE);
            BuildAdornerCorner(ref topRight, Cursors.SizeNESW);
            BuildAdornerCorner(ref bottomLeft, Cursors.SizeNESW);
            BuildAdornerCorner(ref bottomRight, Cursors.SizeNWSE);
            Moved = false;
            boundceRect = new Rectangle();
            boundceRect.Stroke = Brushes.Black;
            boundceRect.Opacity = 0.5;
            boundceRect.Fill = Brushes.Gray;
            boundceRect.StrokeThickness = 2;
            visualChildren.Add(boundceRect);


            // Add handlers for resizing.
            bottomLeft.DragDelta += new DragDeltaEventHandler(HandleBottomLeft);
            bottomRight.DragDelta += new DragDeltaEventHandler(HandleBottomRight);
            topLeft.DragDelta += new DragDeltaEventHandler(HandleTopLeft);
            topRight.DragDelta += new DragDeltaEventHandler(HandleTopRight);
            selectedElements = new List<UIElement>();
            ContextMenu = new ContextMenu();
            MenuItem mi = new MenuItem();
            mi.Header = "Ungroup";
            mi.CommandParameter = AdornedElement;
            mi.Command = new UngroupCommand();
            ContextMenu.Items.Add(mi);
            MenuItem mii = new MenuItem();
            mii.Header = "Group";
            mii.CommandParameter = this;

            mii.Command = new GroupCommand();
            ContextMenu.Items.Add(mii);
            Trans = AdornedElement.RenderTransform.Value;



        }

        public void Add(UIElement el)
        {
            if (selectedElements.Contains(el))
                selectedElements.Remove(el);
            else
                selectedElements.Add(el);

            AdornerLayer.GetAdornerLayer(AdornedElement).Update();
        }
        void topRightRotate_DragDelta(object sender, DragDeltaEventArgs e)
        {

        }
        void HandleBottomRight(object sender, DragDeltaEventArgs args)
        {

        }

        void HandleBottomLeft(object sender, DragDeltaEventArgs args)
        {

        }

        void HandleTopRight(object sender, DragDeltaEventArgs args)
        {

        }

        void HandleTopLeft(object sender, DragDeltaEventArgs args)
        {

        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect r = EditorHelper.CalculateBoundce(selectedElements, (Canvas)AdornedElement);

            boundceRect.Arrange(r);
            return finalSize;
        }
        void BuildAdornerCorner(ref Thumb cornerThumb, Cursor customizedCursor)
        {
            if (cornerThumb != null) return;

            cornerThumb = new Thumb();

            // Set some arbitrary visual characteristics.
            cornerThumb.Cursor = customizedCursor;
            cornerThumb.Height = cornerThumb.Width = 10;
            cornerThumb.Opacity = 0.5;
            cornerThumb.Background = Brushes.Gray;//new SolidColorBrush(Colors.MediumBlue);
            cornerThumb.BorderBrush = Brushes.Gray;
            visualChildren.Add(cornerThumb);
        }
        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

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

            }

        }


        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            Matrix elM = AdornedElement.RenderTransform.Value;
            elM.Invert();
            Matrix m = elM * ((Transform)transform).Value;
            //ParentTransform.Matrix = m;

            topLeft.Width = 10 / m.M11;
            topLeft.Height = 10 / m.M22;
            topRight.Width = 10 / m.M11;
            topRight.Height = 10 / m.M22;
            bottomLeft.Width = 10 / m.M11;
            bottomLeft.Height = 10 / m.M22;
            bottomRight.Width = 10 / m.M11;
            bottomRight.Height = 10 / m.M22;
            return transform;
        }
    }

}
