using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Input;
using FreeSCADA.Common.Schema;
using System.Windows.Controls;
using FreeSCADA.Designer.SchemaEditor.Manipulators;
using FreeSCADA.Designer.SchemaEditor.UndoRedo;
using FreeSCADA.ShellInterfaces;
using System.Windows.Documents;
namespace FreeSCADA.Designer.SchemaEditor.Tools
{
    class TextBoxTool:BaseTool,ITool
    {
        Point startPos;
        bool isDragged;
        DrawingVisual objectPrview = new DrawingVisual();

        public TextBoxTool(SchemaDocument doc)
            : base(doc)
        {

            objectPrview.Opacity = 0.5;
            visualChildren.Add(objectPrview);

        }
        #region ITool implementation
        public String ToolName
        {
            get { return "TextBox Tool"; }
        }

        public String ToolGroup
        {
            get { return "Content Tools"; }
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
                Vector v = e.GetPosition(this) - startPos;
                DrawingContext drawingContext = objectPrview.RenderOpen();
                Rect rect = new Rect(startPos, v);
                drawingContext.DrawRectangle(Brushes.Gray, new Pen(Brushes.Black, 0.2), rect);

                drawingContext.Close();
            }
            base.OnPreviewMouseMove(e);
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (isDragged)
            {
                Rect b = VisualTreeHelper.GetContentBounds(objectPrview);
                if (!b.IsEmpty)
                {
                    TextBlock text = new TextBlock();
                    Canvas.SetLeft(text, b.X);
                    Canvas.SetTop(text, b.Y);
          
                    text.Width = b.Width;
                    text.Height = b.Height;
                    text.Text = "You can write text here"; 
                    text.TextWrapping = TextWrapping.Wrap;
                    UndoRedoManager.GetUndoBuffer(workedSchema).AddCommand(new AddGraphicsObject(text));
                    SelectedObject = text;

                }
                isDragged = false;
                objectPrview.RenderOpen().Close();

            }
            ReleaseMouseCapture();
            base.OnPreviewMouseLeftButtonUp(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {

            base.OnPreviewMouseLeftButtonDown(e);
            if (!e.Handled)
            {
                CaptureMouse();
                startPos = e.GetPosition(this);
                isDragged = true;
            }

            e.Handled = false;
        }
        protected override BaseManipulator CreateToolManipulator(UIElement obj)
        {
            return new TextBoxManipulator(obj);
        }

    }
}
