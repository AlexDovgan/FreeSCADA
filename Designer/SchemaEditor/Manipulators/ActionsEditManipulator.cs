using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;
using FreeSCADA.Common.Schema.Actions;


namespace FreeSCADA.Designer.SchemaEditor.Manipulators
{
    class ActionsEditManipulator:BaseManipulator
    {

        StackPanel AddActionPanel = new StackPanel();
        StackPanel ActionsPanel = new StackPanel();
        public delegate void ActionSelectedDelegate(BaseAction a);
        public event ActionSelectedDelegate ActionSelected;

        public ActionsEditManipulator(UIElement el)
            : base(el)
        {
            
            AddActionPanel.Orientation = Orientation.Vertical;
            foreach(Type actionType in ActionsCollection.ActionsTypes)
            {

                Button b = new Button();
                b.Content = "Add"+actionType.Name;
                AddActionPanel.Children.Add(b);
                BaseAction a = (BaseAction)System.Activator.CreateInstance(actionType);
                if (a != null && !a.CheckActionFor(AdornedElement))
                    b.IsEnabled = false;
                else b.Tag = actionType;
                b.Click += new RoutedEventHandler(AddEventClick);

            }
            AddActionPanel.HorizontalAlignment = HorizontalAlignment.Left;
            AddActionPanel.VerticalAlignment = VerticalAlignment.Top;
            
            FillActions();
            ActionsPanel.HorizontalAlignment = HorizontalAlignment.Right;
            ActionsPanel.VerticalAlignment = VerticalAlignment.Top;

            visualChildren.Add(AddActionPanel);
            visualChildren.Add(ActionsPanel);
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
        void ActionClicked(object sender, RoutedEventArgs e)
        {
            if (ActionSelected!=null)
                ActionSelected(((RadioButton)sender).Tag as BaseAction);
        }
        protected override Size MeasureOverride(Size finalSize)
        {
            //Rect ro = LayoutInformation.GetLayoutSlot(AdornedElement as FrameworkElement);
            foreach (UIElement control in visualChildren)
            {
                control.Measure(finalSize);
            }
            return finalSize;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            //Rect ro = new Rect(0, 0, AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height);
            Rect ro = LayoutInformation.GetLayoutSlot(AdornedElement as FrameworkElement);
            
           foreach (StackPanel control in visualChildren)
            {
                Rect aligmentRect = new Rect();
                aligmentRect.Width=control.DesiredSize.Width;
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
