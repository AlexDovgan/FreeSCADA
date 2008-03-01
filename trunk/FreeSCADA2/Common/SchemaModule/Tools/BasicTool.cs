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
    
    public abstract class BasicTool : Adorner
    {
        public BaseManipulator manipulator;
        public VisualCollection visualChildren;
        public  SchemaDocument workedSchema;
        
        public BasicTool(SchemaDocument schema)
            : base(schema.MainCanvas)
        {
            visualChildren = new VisualCollection(this);
           
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            Rect rect = new Rect(new Point(0,0), AdornedElement.DesiredSize);

            drawingContext.DrawRectangle(Brushes.White, new Pen(Brushes.Black, 0.2), rect);

            // Persist the drawing content.
            drawingContext.Close();
            drawingVisual.Opacity = 0;

            visualChildren.Add(drawingVisual);
            workedSchema = schema;
        }

        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        protected override void OnPreviewMouseLeftButtonDown( MouseButtonEventArgs e)
        {
            if(ToolStarted!=null)
                ToolStarted(this,e);
            if (manipulator != null )
            {
                AdornerLayer.GetAdornerLayer(AdornedElement).Remove(manipulator);
                manipulator = null;
            }
        }
        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if(ToolFinished!=null)
                ToolFinished(this,e);
        }
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {            
            if(ToolWorked!=null)
                ToolWorked(this,e);
        }
        public void Activate()
        {
            AdornerLayer.GetAdornerLayer(AdornedElement).Add(this);
            AdornedElement.Focus();
           
        }
        public void Deactivate()
        {
            AdornerLayer.GetAdornerLayer(AdornedElement).Remove(this);
            if (manipulator != null)
                AdornerLayer.GetAdornerLayer(AdornedElement).Remove(manipulator);
            
        }
        public delegate void ToolEvent(BasicTool tool,EventArgs e);
        public event ToolEvent ToolFinished;
        public event ToolEvent ToolStarted;
        public event ToolEvent ToolWorked;
        public void RaiseToolFinished(BasicTool tool, EventArgs e)
        {
            ToolFinished(tool, e);
        }
        public void RaiseToolStarted(BasicTool tool, EventArgs e)
        {
            ToolStarted(tool, e);
        }
        public void RaiseToolWorked(BasicTool tool, EventArgs e)
        {
            ToolWorked(tool, e);
        }
        
    }
  
}