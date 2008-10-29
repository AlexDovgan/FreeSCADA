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
using FreeSCADA.Common;
using FreeSCADA.Designer.SchemaEditor.ShortProperties;

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
        public List<UIElement> selectedElements = new List<UIElement>();
        Rectangle boundceRect = new Rectangle();
        DrawingVisual selectionRectangle = new DrawingVisual();

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

           
            //need in refectoring 
        }
  
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (isDragged)
            {
                DrawingContext drawingContext = selectionRectangle.RenderOpen();

                // Create a rectangle and draw it in the DrawingContext.
                finalSize = e.GetPosition(this) - startPos;
                Rect rect = new Rect(startPos, finalSize);

                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 0.2), rect);
                drawingContext.Close();


            }
            e.Handled = false;
            // Update PropertyView
            RaiseObjectSelected(SelectedObject);
            if (isSelectionMoved)
            {
                Vector newPosDelta;
                newPosDelta = e.GetPosition(this) - movePos;
                movePos = e.GetPosition(this);
                foreach (UIElement se in selectedElements)
                {
                    double x = Canvas.GetLeft((se as FrameworkElement));
                    double y = Canvas.GetTop((se as FrameworkElement));
                    Canvas.SetLeft((se as FrameworkElement), x + newPosDelta.X);
                    Canvas.SetTop((se as FrameworkElement), y + newPosDelta.Y);
                    if (moveUndoInfo)
                        this.OnObjectChanged(se);
                }
                moveUndoInfo = false;
                //selectionRectangle.RenderOpen().Close();
                AdornerLayer.GetAdornerLayer(AdornedElement).Update();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
            }
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            
            if (isDragged)
                SelectedObject = null;
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
                    
                    selectedElements.Add(el);
                }

            }
            selectionRectangle.RenderOpen().Close();
            AdornerLayer.GetAdornerLayer(AdornedElement).Update();
            RaiseObjectSelected(SelectedObject);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //base.OnPreviewMouseLeftButtonDown(e);
            Point pt = e.GetPosition(this);
            
            IInputElement manipulatorHit = null;
            if (ToolManipulator != null)
                manipulatorHit = ToolManipulator.InputHitTest(e.GetPosition(ToolManipulator));

            DependencyObject documentHit=null;
            if(VisualTreeHelper.HitTest(AdornedElement, pt)!=null)
                documentHit = VisualTreeHelper.HitTest(AdornedElement, pt).VisualHit;
            if (manipulatorHit != null)
            {
                if (e.ClickCount > 1)
                {
                    UIElement so = SelectedObject;
                    SelectedObject = null;
                    ToolManipulator = ObjectsFactory.CreateDefaultManipulator(so);
                    SelectedObject = so;
                }
            }
            else if (documentHit == AdornedElement)
            {

                CaptureMouse();
                startPos = e.GetPosition(this);
                isDragged = true;
                selectedElements.Clear();
                SelectedObject = null;

            }
            else if (documentHit != AdornedElement)
            {
                FrameworkElement el = (FrameworkElement)EditorHelper.FindTopParentUnder(AdornedElement, documentHit);
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
                {
                    if (selectedElements.Count > 0)
                    {
                        bool found = false;
                        for (int i = 0; i < selectedElements.Count; i++)
                        {
                            Rect r = new Rect(Canvas.GetLeft(selectedElements[i]), Canvas.GetTop(selectedElements[i]), selectedElements[i].RenderSize.Width, selectedElements[i].RenderSize.Height);
                            if (r.Contains(pt))
                            {
                                found = true;
                                break;
                            }
                        }
                        if (found)
                        {
                            isSelectionMoved = true;
                            movePos = e.GetPosition(this);
                            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.SizeAll;
                            moveUndoInfo = true;
                        }
                        else
                        {
                            SelectedObject = el;
                        }
                    }
                    else
                    {
                        SelectedObject = el;
                    }

                }
                else
                {
                    if (selectedElements.Contains(el))
                    {
                        selectedElements.Remove(el);
                    }
                    else{
                        if (SelectedObject != null)
                        {
                            selectedElements.Add(SelectedObject);
                            SelectedObject = null;
                        }
                        selectedElements.Add(el);
                    }

                }

            }
           
            AdornerLayer.GetAdornerLayer(AdornedElement).Update();
            RaiseObjectSelected(SelectedObject);
            e.Handled = false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(finalSize);
            Rect r = EditorHelper.CalculateBoundce(selectedElements, AdornedElement);
            if (!r.IsEmpty)
            {
                boundceRect.Visibility = Visibility.Visible;
                boundceRect.Arrange(r);
            }
            else
                boundceRect.Visibility = Visibility.Hidden;
            return finalSize;
        }

        protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new DragResizeRotateManipulator(obj as FrameworkElement);
        }

        public List<UIElement> MoveHelper(double delta_x, double delta_y)
        {
            if (SelectedObject != null)
            {
                // undo
                this.OnObjectChanged(SelectedObject);
                double x = Canvas.GetLeft((SelectedObject as FrameworkElement));
                double y = Canvas.GetTop((SelectedObject as FrameworkElement));
                Canvas.SetLeft((SelectedObject as FrameworkElement), x + delta_x);
                Canvas.SetTop((SelectedObject as FrameworkElement), y + delta_y);
                // Update PropertyView
                RaiseObjectSelected(SelectedObject);
                List<UIElement> lst = new List<UIElement>(1);
                lst.Add(SelectedObject);
                return lst;
            }
            else if (selectedElements.Count > 0)
            {
                foreach (UIElement se in selectedElements)
                {
                    // undo
                    this.OnObjectChanged(se);
                    double x = Canvas.GetLeft((se as FrameworkElement));
                    double y = Canvas.GetTop((se as FrameworkElement));
                    Canvas.SetLeft((se as FrameworkElement), x + delta_x);
                    Canvas.SetTop((se as FrameworkElement), y + delta_y);
                }
                selectionRectangle.RenderOpen().Close();
                AdornerLayer.GetAdornerLayer(AdornedElement).Update();
                return selectedElements;
            }
            return null;
        }

    }
}
