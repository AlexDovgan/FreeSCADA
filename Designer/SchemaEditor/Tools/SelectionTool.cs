using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// Singale and multi selection tool
    /// multiselection implemented throw left button down and drag
    /// and throw clicks with shift down
    /// </summary>
    class SelectionTool : BaseTool
    {
        Point startPos;
        Point movePos;
        bool moveUndoInfo = false;  // first move event after start of dragging multiple objerts
        Vector finalSize;
        bool isDragged = false;
        bool isSelectionMoved = false;
        Rectangle boundceRect = new Rectangle();
        DrawingVisual selectionRectangle = new DrawingVisual();
        SelectionManager selManeger;

        public Point LastClickedPoint
        {
            get;
            set;
        }

        public SelectionTool(UIElement element)
            : base(element)
        {
            boundceRect.Stroke = Brushes.Black;
            boundceRect.Opacity = 0.25;
            boundceRect.Fill = Brushes.Gray;
            boundceRect.StrokeThickness = 1;
            visualChildren.Add(boundceRect);

            selectionRectangle.Opacity = 0.5;
            visualChildren.Add(selectionRectangle);
            selManeger= SelectionManager.GetSelectionManagerFor(AdornedElement);

            //need in refectoring 
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (isDragged)
            {
                DrawingContext drawingContext = selectionRectangle.RenderOpen();

                // Create a rectangle and draw it in the DrawingContext.
                finalSize = GridManager.GetMousePos() - startPos;
                Rect rect = new Rect(startPos, finalSize);

                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 0.2), rect);
                drawingContext.Close();


            }
        
            
            if (isSelectionMoved)
            {
                Vector newPosDelta;
                newPosDelta = GridManager.GetMousePos() - movePos;
                movePos = GridManager.GetMousePos();
                MoveHelper(newPosDelta.X, newPosDelta.Y);
                
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
            }
            
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {

            if (isDragged)
                SelectionManager.GetSelectionManagerFor(AdornedElement).SelectObject(null);
            isDragged = false;
            isSelectionMoved = false;
            ReleaseMouseCapture();
            e.Handled = false;
            Rect b = VisualTreeHelper.GetContentBounds(selectionRectangle);
            foreach (FrameworkElement el in (AdornedElement as Canvas).Children)
            {
                Rect itemRect = VisualTreeHelper.GetDescendantBounds(el);
                Rect itemBounds = el.TransformToAncestor
                        (AdornedElement).TransformBounds(itemRect);

                if (b.Contains(itemBounds))
                {

                    SelectionManager.GetSelectionManagerFor(AdornedElement).AddObject(el);
                }

            }
            selectionRectangle.RenderOpen().Close();
            InvalidateVisual();
            LastClickedPoint = GridManager.GetMousePos();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //base.OnPreviewMouseLeftButtonDown(e);
            Point pt = e.GetPosition(this);
        

            DependencyObject documentHit = null;
            if (VisualTreeHelper.HitTest(AdornedElement, pt) != null)
                documentHit = VisualTreeHelper.HitTest(AdornedElement, pt).VisualHit;
            UIElement selObj = null;
            if(selManeger.SelectedObjects.Count>0)
                selObj=selManeger.SelectedObjects[0];
            if (documentHit == selObj)
            {
                if (e.ClickCount > 1)
                {
                    //createDeffManipulator
                    //SelectionManager.GetSelectionManagerFor(AdornedElement)
                    //Assembly archiverAssembly = this.GetType().Assembly;
                    //foreach (Type type in archiverAssembly.GetTypes())
                    //{
                    //    if (type.IsSubclassOf(typeof(BaseTool)))
                    //    {
                    //        BaseBindingPanelFactory factory = (BaseBindingPanelFactory)Activator.CreateInstance(type);
                    //        if (factory != null && factory.CheckApplicability(element, property))
                    //            result.Add(factory);
                    //    }
                    //}
                    //((AdornedElement as FrameworkElement).Tag as Views.SchemaView).CurrentTool = ObjectsFactory.CreateDefaultManipulator(selObj); 


                    return;
                }
            }
            if (documentHit == AdornedElement)
            {

                CaptureMouse();
                startPos = GridManager.GetMousePos();
                isDragged = true;
                selManeger.SelectObject(null);

            }
            else if (documentHit != AdornedElement )
            {
                FrameworkElement el = (FrameworkElement)EditorHelper.FindTopParentUnder(AdornedElement, documentHit);
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
                {

                    isSelectionMoved = true;
                    movePos = GridManager.GetMousePos();
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                    moveUndoInfo = true;
                    if (!selManeger.SelectedObjects.Contains(el))
                        selManeger.SelectObject(el);
                }
                else
                {
                    if (selManeger.SelectedObjects.Contains(el))
                    {
                        selManeger.DeleteObject(el);
                    }
                    else
                    {
                        selManeger.AddObject(el);
                    }

                }

            }

            InvalidateVisual();
            
            e.Handled = false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            
            Rect r = selManeger.CalculateBounds();
            if (selManeger.SelectedObjects.Count>1&&!r.IsEmpty)
            {
                boundceRect.Visibility = Visibility.Visible;
                boundceRect.Arrange(r);
            }
            else
                boundceRect.Visibility = Visibility.Hidden;
            return base.ArrangeOverride(finalSize);
            
        }

        public override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new DragResizeRotateManipulator(obj as FrameworkElement);
        }

        public void MoveHelper(double delta_x, double delta_y)
        {
            foreach (UIElement se in selManeger.SelectedObjects)
            {
                // undo
                
                double x = Canvas.GetLeft((se as FrameworkElement));
                double y = Canvas.GetTop((se as FrameworkElement));
                Canvas.SetLeft((se as FrameworkElement), x + delta_x);
                Canvas.SetTop((se as FrameworkElement), y + delta_y);
               
            }
            moveUndoInfo = false;

            selectionRectangle.RenderOpen().Close();
            InvalidateVisual();
         

        }
        public override Type ToolEditingType()
        {
            return typeof(FrameworkElement);
        }

    }
}
