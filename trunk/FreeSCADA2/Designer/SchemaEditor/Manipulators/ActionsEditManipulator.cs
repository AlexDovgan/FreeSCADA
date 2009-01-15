using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using FreeSCADA.Common.Schema.Actions;

namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class ActionsEditManipulator : BaseManipulator
    {

        StackPanel AddActionPanel = new StackPanel();
        StackPanel ActionsPanel = new StackPanel();
        public delegate void ActionSelectedDelegate(BaseAction a);
        public event ActionSelectedDelegate ActionSelected;
        Tools.BaseTool helperTool;
        
        DrawingVisual helperObject= new DrawingVisual();
        public ActionsEditManipulator(UIElement element)
            : base(element)
        {

            AddActionPanel.Orientation = Orientation.Vertical;
            Button b ;
            foreach (Type actionType in ActionsCollection.ActionsTypes)
            {

                b= new Button();
                b.Content = "Add" + actionType.Name;
                AddActionPanel.Children.Add(b);
                b.Tag = actionType;
                b.Click += new RoutedEventHandler(AddEventClick);

            }
            b = new Button();
            b.Content = "Delete Action";
            AddActionPanel.Children.Add(b);
            b.Click += new RoutedEventHandler(DeleteActionClicked);
            AddActionPanel.HorizontalAlignment = HorizontalAlignment.Left;
            AddActionPanel.VerticalAlignment = VerticalAlignment.Top;
            ActionsPanel.HorizontalAlignment = HorizontalAlignment.Right;
            ActionsPanel.VerticalAlignment = VerticalAlignment.Top;

            visualChildren.Add(AddActionPanel);
            visualChildren.Add(ActionsPanel);
            Activate();

        }

        void DeleteActionClicked(object sender, RoutedEventArgs e)
        {
            foreach (RadioButton butt in ActionsPanel.Children)
            {
                if (butt.IsChecked.Value)
                {
                    ActionsCollection.GetActions(AdornedElement as FrameworkElement).ActionsList.Remove((BaseAction)butt.Tag);
                    break;
                }

            }
            FillActions();
        }

        public override void Activate()
        {
            foreach (Button b in AddActionPanel.Children)
            {
                if (b.Tag != null)
                {
                    BaseAction a = (BaseAction)System.Activator.CreateInstance(b.Tag as Type);
                    if (a != null && !a.CheckActionFor(AdornedElement))
                        b.IsEnabled = false;
                    else b.IsEnabled = true;
                }

            }
            FillActions();
            visualChildren.Add(helperObject);
            base.Activate();
        }
        public override void Deactivate()
        {
            
            if (helperTool != null)
            {
                helperTool.Deactivate();
                helperTool = null;
            }
            helperObject.RenderOpen().Close();
            base.Deactivate();
        }

        void AddEventClick(object sender, RoutedEventArgs e)
        {

            BaseAction a = (BaseAction)System.Activator.CreateInstance((Type)((Button)sender).Tag);
            ActionsCollection.GetActions(AdornedElement as FrameworkElement).ActionsList.Add(a);
            FillActions();
            UpdateLayout();
        }
        void FillActions()
        {
            ActionsPanel.Children.Clear();
            helperObject.RenderOpen().Close();
            foreach (BaseAction action in ActionsCollection.GetActions(AdornedElement as FrameworkElement).ActionsList)
            {
                RadioButton b = new RadioButton();
                b.BorderBrush = Brushes.Blue;
                b.BorderThickness = new Thickness(1);
                b.Background = Brushes.White;
                //TextBlock t = new TextBlock();
                //t.Text = action.GetType().Name;
                b.Content = action.GetType().Name;
                ActionsPanel.Children.Add(b);
                //b.MouseLeftButtonDown+=new MouseButtonEventHandler(ActionPresed);
                b.Click += new RoutedEventHandler(ActionClicked);
                b.Tag = action;
            }

        }
        protected void helperSelected(Object obj)
        {
            Shape shape;
            if (obj is Shape)
            {
                shape = obj as Shape;

                foreach (RadioButton b in ActionsPanel.Children)
                {
                    if (b.IsChecked.Value)
                    {

                        (b.Tag as BaseAction).HelperObject = shape.RenderedGeometry; ;
                        (b.Tag as BaseAction).HelperObject.Transform = (Transform)shape.TransformToVisual((Panel)VisualTreeHelper.GetParent(AdornedElement));
                        (b.Tag as BaseAction).HelperObject = (b.Tag as BaseAction).HelperObject.GetFlattenedPathGeometry();
                        //PathGeometry.CreateFromGeometry((b.Tag as BaseAction).HelperObject);
                        ((Panel)VisualTreeHelper.GetParent(AdornedElement)).Children.Remove(shape);
                        ((Panel)VisualTreeHelper.GetParent(AdornedElement)).UpdateLayout();

                        DrawingContext c = helperObject.RenderOpen();
                        c.DrawGeometry(null, new Pen(Brushes.Black, 1), (b.Tag as BaseAction).HelperObject);
                        c.Close();
                        InvalidateVisual();
                        break;
                    }
                }
            }
            helperTool.ObjectSelected -= helperSelected;
            helperTool.Deactivate();
            helperTool = null;

        }
        void ActionClicked(object sender, RoutedEventArgs e)
        {
            if (ActionSelected != null)
                ActionSelected(((RadioButton)sender).Tag as BaseAction);
            helperObject.RenderOpen().Close();
            if (!(((RadioButton)sender).Tag as BaseAction).IsHelperObjectNeded())
                return;

            if (helperTool != null)
            {
                helperTool.Deactivate();
                helperTool.ObjectSelected -= helperSelected;
                helperTool = null;
            }
 
           
            
            if ((((RadioButton)sender).Tag as BaseAction).HelperObject == null)
            {
                helperTool = new Tools.HelperSelectorTool((UIElement)VisualTreeHelper.GetParent(AdornedElement), AdornedElement);
                helperTool.Activate();
                helperTool.ObjectSelected += helperSelected;
            }
            else 
            {
                if(!((((RadioButton)sender).Tag as BaseAction).HelperObject is PathGeometry))
                    (((RadioButton)sender).Tag as BaseAction).HelperObject= PathGeometry.CreateFromGeometry((((RadioButton)sender).Tag as BaseAction).HelperObject);
                DrawingContext c = helperObject.RenderOpen();
                c.DrawGeometry(null, new Pen(Brushes.Black, 1), (((RadioButton)sender).Tag as BaseAction).HelperObject);
                
                c.Close();
                InvalidateVisual();
            }
            
        }
        protected override Size MeasureOverride(Size finalSize)
        {
            //Rect ro = LayoutInformation.GetLayoutSlot(AdornedElement as FrameworkElement);
            foreach (Visual v in visualChildren)
            {
                if (v is FrameworkElement)
                    (v as FrameworkElement).Measure(finalSize);
            }
            return finalSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            //Rect ro = new Rect(0, 0, AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height);
            Rect ro = LayoutInformation.GetLayoutSlot(AdornedElement as FrameworkElement);
            FrameworkElement control;
            foreach (Visual v in visualChildren)
            {
                if (v is FrameworkElement)
                    control = v as FrameworkElement;
                else continue;

                Rect aligmentRect = new Rect();
                aligmentRect.Width = control.DesiredSize.Width;
                aligmentRect.Height = control.DesiredSize.Height;
                aligmentRect.Y = ro.Y;
                aligmentRect.X = ro.X;
                switch (control.VerticalAlignment)
                {
                    case VerticalAlignment.Top: aligmentRect.Y += 0;
                        break;
                    case VerticalAlignment.Bottom: aligmentRect.Y += ro.Height;
                        break;
                    case VerticalAlignment.Center: aligmentRect.Y += ro.Height / 2;
                        break;
                    case VerticalAlignment.Stretch: aligmentRect.Height = ro.Height;// *Math.Abs((AdornedElement.RenderTransform as TransformGroup).Children[0].Value.M22);
                        break;
                    default:
                        break;
                }
                switch (control.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left: aligmentRect.X += -aligmentRect.Width;
                        break;
                    case HorizontalAlignment.Right: aligmentRect.X += ro.Width;
                        break;
                    case HorizontalAlignment.Center: aligmentRect.X += ro.Width / 2;
                        break;
                    case HorizontalAlignment.Stretch: aligmentRect.Width = ro.Width;// *Math.Abs((AdornedElement.RenderTransform as TransformGroup).Children[0].Value.M11);
                        break;
                    default:
                        break;
                }

          

                control.Arrange(aligmentRect);
            }
            return finalSize;
        }
    }
}
