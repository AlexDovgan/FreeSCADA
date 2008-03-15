using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FreeSCADA.Common.Schema;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class ButtonTool : BaseTool, ITool
    {
        Point startPos;
        bool isDragging;
        //DrawingVisual objectPrview = new DrawingVisual();
        Button objectPrview;

        public ButtonTool(SchemaDocument doc)
            : base(doc)
        {

            //objectPrview.Children.Add(new Button());

        }

        #region ITool implementation
        public String ToolName
        {
            get { return "BootonTool"; }
        }

        public String ToolGroup
        {
            get { return "Controlls"; }
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
            if (visualChildren.Contains(objectPrview))
            {
                Point p = e.GetPosition(this);
                Canvas.SetLeft(objectPrview, p.X);
                Canvas.SetTop(objectPrview, p.Y);

                InvalidateArrange();
            }
            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {

            if (visualChildren.Contains(objectPrview))
            {
                visualChildren.Remove(objectPrview);
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            if (!visualChildren.Contains(objectPrview))
            {
                objectPrview = new Button();
                objectPrview.Opacity = 0.5;
                Point p = e.GetPosition(this);
                Canvas.SetLeft(objectPrview, p.X);
                Canvas.SetTop(objectPrview, p.Y);

                objectPrview.Width = 60;
                objectPrview.Height = 20;
                objectPrview.Content = "Button";

                visualChildren.Add(objectPrview);
                CaptureMouse();
            }
            else
            {
                   visualChildren.Remove(objectPrview);
                   objectPrview.Opacity = 1;    
                UndoRedoManager.GetUndoBuffer(workedSchema).AddCommand(new AddGraphicsObject(objectPrview));
                    SelectedObject = objectPrview;
                    
                ReleaseMouseCapture();
            }
            e.Handled = false;
        }

        protected override Size MeasureOverride(Size finalSize)
        {
            base.MeasureOverride(finalSize);
            if (objectPrview != null)
                objectPrview.Measure(finalSize);
            return finalSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            base.ArrangeOverride(finalSize);
            if (objectPrview != null)
            {
                double x = Canvas.GetLeft(objectPrview);
                double y = Canvas.GetTop(objectPrview);
                objectPrview.Arrange(new Rect(new Point(x, y), objectPrview.DesiredSize));
            }

            return finalSize;
        }
    }
}
