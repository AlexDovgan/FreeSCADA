using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Common;

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
        ISelectionManager _selManeger;

        public Point LastClickedPoint
        {
            get;
            set;
        }

        public SelectionTool(UIElement element,ISelectionManager selManager)
            : base(element)
        {
            boundceRect.Stroke = Brushes.Black;
            boundceRect.Opacity = 0.25;
            boundceRect.Fill = Brushes.Gray;
            boundceRect.StrokeThickness = 1;
            visualChildren.Add(boundceRect);

            selectionRectangle.Opacity = 0.5;
            visualChildren.Add(selectionRectangle);
            _selManeger = selManager;

            //need in refectoring 
        }

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (isDragged)
            {
                DrawingContext drawingContext = selectionRectangle.RenderOpen();

                // Create a rectangle and draw it in the DrawingContext.
                finalSize = GridManager.GetGridManagerFor(AdornedElement).GetMousePos() - startPos;
                Rect rect = new Rect(startPos, finalSize);

                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 0.2), rect);
                drawingContext.Close();


            }
        
            
            if (isSelectionMoved)
            {
                Vector newPosDelta;
                newPosDelta = GridManager.GetGridManagerFor(AdornedElement).GetMousePos() - movePos;
                movePos = GridManager.GetGridManagerFor(AdornedElement).GetMousePos();
                MoveHelper(newPosDelta.X, newPosDelta.Y);
                
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
            }
            
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {

            if (isDragged)
                _selManeger.SelectObject(null);
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

                    _selManeger.AddObject(el);
                }

            }
            selectionRectangle.RenderOpen().Close();
            InvalidateVisual();
            LastClickedPoint = GridManager.GetGridManagerFor(AdornedElement).GetMousePos();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //base.OnPreviewMouseLeftButtonDown(e);
            Point pt = e.GetPosition(this);
        

            DependencyObject documentHit = null;
            if (VisualTreeHelper.HitTest(AdornedElement, pt) != null)
                documentHit = VisualTreeHelper.HitTest(AdornedElement, pt).VisualHit;
            UIElement selObj = null;
            if(_selManeger.SelectedObjects.Count>0)
                selObj=_selManeger.SelectedObjects.Cast<UIElement>().FirstOrDefault();
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
                startPos = GridManager.GetGridManagerFor(AdornedElement).GetMousePos();
                isDragged = true;
                _selManeger.SelectObject(null);

            }
            else if (documentHit != AdornedElement )
            {
                var el = (FrameworkElement)EditorHelper.FindTopParentUnder(AdornedElement, documentHit);
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
                {

                    isSelectionMoved = true;

                    movePos = GridManager.GetGridManagerFor(AdornedElement).GetMousePos();
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                    moveUndoInfo = true;
                    if (!_selManeger.SelectedObjects.Contains(el))
                        _selManeger.SelectObject(el);
                    foreach (var elm in _selManeger.SelectedObjects.Cast<UIElement>())
                    {
                        var ub = UndoRedoManager.GetUndoBufferFor(elm);
                        ub.AddCommand(new ModifyGraphicsObject(elm));
                    }
            
                }
                else
                {
                    if (_selManeger.SelectedObjects.Contains(el))
                    {
                        _selManeger.DeleteObject(el);
                    }
                    else
                    {
                        _selManeger.AddObject(el);
                    }

                }

            }

            InvalidateVisual();
            
            e.Handled = false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {

            var r = EditorHelper.CalculateBounds(_selManeger.SelectedObjects.Cast<UIElement>().ToList(), AdornedElement);
            if (_selManeger.SelectedObjects.Count>1&&!r.IsEmpty)
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
            foreach (UIElement se in _selManeger.SelectedObjects)
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
