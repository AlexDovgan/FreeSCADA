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
using FreeSCADA.Schema.Context_Menu;
using FreeSCADA.Schema.Manipulators;
using FreeSCADA.Schema.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Schema.Tools
{
    public class SelectionTool : BasicTool, ITool
    {
        Point startPos;
        bool isDragged = false;
        public List<UIElement> selectedElements=new List<UIElement>();
        Rectangle boundceRect = new Rectangle();
        DrawingVisual selectionRectangle = new DrawingVisual();
        SelectToolContextMenu menu;
        public SelectionTool(SchemaDocument schema)
            : base(schema)
        {
            boundceRect.Stroke = Brushes.Black;
            boundceRect.Opacity = 0.5;
            boundceRect.Fill = Brushes.Gray;
            boundceRect.StrokeThickness = 2;
            visualChildren.Add(boundceRect);

            selectionRectangle.Opacity = 0.5;
            visualChildren.Add(selectionRectangle);
            ContextMenu = menu = new SelectToolContextMenu();

        }
        #region ITool implementation
        public String ToolName
        {
            get { return "Selection Tool"; }
        }

        public String ToolGroup
        {
            get { return "Graphics Tools"; }
        }
        public System.Drawing.Bitmap ToolIcon
        {
            get
            {
                return new System.Drawing.Bitmap(10, 10);
            }
        }
        #endregion

        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            if (isDragged)
            {
                DrawingContext drawingContext = selectionRectangle.RenderOpen();

                // Create a rectangle and draw it in the DrawingContext.
                Vector v = e.GetPosition(this) - startPos;
                Rect rect = new Rect(startPos, v);

                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 0.2), rect);
                drawingContext.Close();
 

            }
            e.Handled = false;
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            //selectionRectangle.Drawing.Children.Clear();
            selectionRectangle.RenderOpen().Close();
            isDragged=false;
            ReleaseMouseCapture();
            e.Handled = false;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Point pt = e.GetPosition(this);
            CaptureMouse();



            // Perform the hit test against a given portion of the visual object tree.
            HitTestResult result = VisualTreeHelper.HitTest(workedSchema.MainCanvas, pt);
            if (manipulator != null && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
            {
                AdornerLayer.GetAdornerLayer(AdornedElement).Remove(manipulator);
                manipulator = null;
            }

            if (result.VisualHit == workedSchema.MainCanvas && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
            {

                startPos = e.GetPosition(this);
                isDragged = true;
                selectedElements.Clear();
                AdornerLayer.GetAdornerLayer(AdornedElement).Update();
            }
            else if (result.VisualHit != workedSchema.MainCanvas)
            {
                FrameworkElement el = (FrameworkElement)EditorHelper.FindTopParentUnder(workedSchema.MainCanvas, (FrameworkElement)result.VisualHit);
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
                {
                    AdornerLayer.GetAdornerLayer(AdornedElement).Add(manipulator = new DragResizeRotate(el, workedSchema));

                    RaiseToolFinished(this, e);
                }
                else
                {
                    
                    selectedElements.Add(el);
                    AdornerLayer.GetAdornerLayer(AdornedElement).Update();
                }

            }
            e.Handled = false;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Rect r = EditorHelper.CalculateBoundce(selectedElements, (Canvas)AdornedElement);
            if(!r.IsEmpty)
                boundceRect.Arrange(r);
            return finalSize;
        }


    }

}
