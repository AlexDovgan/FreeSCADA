﻿using System;
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
using FreeSCADA.Designer.SchemaEditor.Context_Menu;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    /// <summary>
    /// Singale and multi selection tool
    /// multiselection implemented throw left button down and drag
    /// and throw clicks with shift down
    /// </summary>
    class SelectionTool : BaseTool, ITool
    {
        Point startPos;
        bool isDragged = false;
        public List<UIElement> selectedElements = new List<UIElement>();
        Rectangle boundceRect = new Rectangle();
        DrawingVisual selectionRectangle = new DrawingVisual();

        public SelectionTool(SchemaDocument schema)
            : base(schema)
        {
            boundceRect.Stroke = Brushes.Black;
            boundceRect.Opacity = 0.25;
            boundceRect.Fill = Brushes.Gray;
            boundceRect.StrokeThickness = 1;
            visualChildren.Add(boundceRect);

            selectionRectangle.Opacity = 0.5;
            visualChildren.Add(selectionRectangle);

            ContextMenu = menu = new ToolContextMenu();

            menu.groupMenuItem.CommandParameter = this;
            menu.unGroupMenuItem.CommandParameter = this;
            

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
            selectionRectangle.RenderOpen().Close();
            if (isDragged)
                SelectedObject = null;
            isDragged = false;
            ReleaseMouseCapture();
            e.Handled = false;
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //base.OnPreviewMouseLeftButtonDown(e);
            Point pt = e.GetPosition(this);
            
            IInputElement manipulatorHit = null;
            if (ActiveManipulator != null)
                manipulatorHit = ActiveManipulator.InputHitTest(e.GetPosition(ActiveManipulator));

            DependencyObject documentHit;
            documentHit=VisualTreeHelper.HitTest(workedSchema.MainCanvas, pt).VisualHit;
            if (manipulatorHit != null)
            {
                if (e.ClickCount > 1)
                {
                    UIElement tmp = SelectedObject;
                    SelectedObject = null;
                    SelectedObject =tmp; 
                }
            }
            else if (documentHit == workedSchema.MainCanvas)
            {

                CaptureMouse();
                //ActiveManipulator = null;
                startPos = e.GetPosition(this);
                isDragged = true;
                selectedElements.Clear();

            }
            else if (documentHit != workedSchema.MainCanvas)
            {
                FrameworkElement el = (FrameworkElement)EditorHelper.FindTopParentUnder(workedSchema.MainCanvas, documentHit);
                if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
                {

                    ActiveManipulator = new DragResizeRotateManipulator(el);
                    SelectedObject = el;

                }
                else
                {
                    if (SelectedObject != null)
                    {
                        selectedElements.Add(SelectedObject);
                        SelectedObject = null;
                    }
                    selectedElements.Add(el);

                }

            }
            (menu.groupMenuItem.Command as GroupCommand).RaiseCanExecuteChanged();
            (menu.unGroupMenuItem.Command as UngroupCommand).RaiseCanExecuteChanged();
            AdornerLayer.GetAdornerLayer(AdornedElement).Update();
            e.Handled = false;
        }




        protected override Size ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(finalSize);
            Rect r = EditorHelper.CalculateBoundce(selectedElements, (Canvas)AdornedElement);
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
    }

}
