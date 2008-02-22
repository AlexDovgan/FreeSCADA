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
using FreeSCADA.Scheme.Commands;
using FreeSCADA.Scheme.Manipulators;
using FreeSCADA.Scheme.Helpers;
using FreeSCADA.Scheme.UndoRedo;
using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Scheme.Tools
{
    
    public abstract class BasicTool : Adorner
    {
        public BaseManipulator manipulator;
        public VisualCollection visualChildren;
        public  FSScheme workedScheme;
        
        public BasicTool(FSScheme scheme)
            : base(scheme.MainCanvas)
        {
            visualChildren = new VisualCollection(this);
            workedScheme = scheme;
        }

        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }

        public virtual void OnCanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(ToolStarted!=null)
                ToolStarted(this,e);
            if (manipulator != null )
            {
                AdornerLayer.GetAdornerLayer(AdornedElement).Remove(manipulator);
                manipulator = null;
            }
        }
        public virtual void OnCanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(ToolFinished!=null)
                ToolFinished(this,e);
        }
        public virtual void OnCanvasMouseMove(object sender, MouseEventArgs e)
        {            
            if(ToolWorked!=null)
                ToolWorked(this,e);
        }
        public virtual void OnCanvasKeyDown(object sender, KeyEventArgs e)
        { }
        public virtual void OnCanvasKeyUp(object sender, KeyEventArgs e)
        { }
        public void Activate()
        {
            AdornedElement.MouseLeftButtonDown += new MouseButtonEventHandler(OnCanvasMouseLeftButtonDown);
            AdornedElement.MouseLeftButtonUp += new MouseButtonEventHandler(OnCanvasMouseLeftButtonUp);
            AdornedElement.MouseMove += new MouseEventHandler(OnCanvasMouseMove);
            AdornedElement.KeyDown += new KeyEventHandler(OnCanvasKeyDown);
            AdornedElement.KeyUp += new KeyEventHandler(OnCanvasKeyUp);
            AdornedElement.Focus();
        }
        public void Deactivate()
        {
            AdornedElement.MouseLeftButtonDown -= OnCanvasMouseLeftButtonDown;
            AdornedElement.MouseLeftButtonUp -= OnCanvasMouseLeftButtonUp;
            AdornedElement.MouseMove -= OnCanvasMouseMove;
            AdornedElement.KeyDown -= OnCanvasKeyDown;
            AdornedElement.KeyUp -= OnCanvasKeyUp;
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