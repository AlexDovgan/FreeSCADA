//-----------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Diagnostics;
using FreeSCADA.Common.Schema.Gestures;
using System.Globalization;

namespace FreeSCADA.Common.Schema
{
    /// <summary>
    /// This demo shows the VirtualCanvas managing up to 50,000 random WPF shapes providing smooth scrolling and
    /// zooming while creating those shapes on the fly.  This helps make a WPF canvas that is a lot more
    /// scalable.
    /// </summary>
    public partial class VirtualSchemaContainer : System.Windows.Forms.Integration.ElementHost
    {
        MapZoom zoom;
        Pan pan;
        RectangleSelectionGesture rectZoom;
        AutoScroll autoScroll;
        VirtualCanvas grid;
        ScrollViewer scroller;
        Canvas mapedCanvas;
        bool _showGridLines;
        bool _animateStatus = true;

        double _tileWidth = 0;
        double _tileHeight = 0;
        double _tileMargin = 0;
        int _totalVisuals = 0;
        int rows = 100;
        int cols = 100;

        public VirtualSchemaContainer()
        {
            //    InitializeComponent();

            scroller = new ScrollViewer();
            scroller.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scroller.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scroller.CanContentScroll = true;
            Child = scroller;
            grid = new VirtualCanvas();
            scroller.Content = grid;

            grid.SmallScrollIncrement = new Size(_tileWidth + _tileMargin, _tileHeight + _tileMargin);
            //Scroller.Content = grid;
            object v = scroller.GetValue(ScrollViewer.CanContentScrollProperty);

            Canvas target = grid.ContentCanvas;
            zoom = new MapZoom(target);
            pan = new Pan(target, zoom);
            rectZoom = new RectangleSelectionGesture(target, zoom, System.Windows.Input.ModifierKeys.Control);
            rectZoom.ZoomSelection = true;
            autoScroll = new AutoScroll(target, zoom);
            zoom.ZoomChanged += new EventHandler(OnZoomChanged);

            grid.VisualsChanged += new EventHandler<VisualChangeEventArgs>(OnVisualsChanged);
            //ZoomSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(OnZoomSliderValueChanged);

            grid.Scale.Changed += new EventHandler(OnScaleChanged);
            grid.Translate.Changed += new EventHandler(OnScaleChanged);

            grid.Background = new SolidColorBrush(Color.FromRgb(0xd0, 0xd0, 0xd0));
            grid.ContentCanvas.Background = Brushes.White;
         }

        public Canvas MappedCanvas
        {
            get { return mapedCanvas; }
            set
            {

                mapedCanvas = value;
                grid.ContentCanvas.MinWidth = mapedCanvas.Width;
                grid.ContentCanvas.MinHeight= mapedCanvas.Height;
                AllocateNodes();
            }
        }

        private void AllocateNodes()
        {
            
            grid.VirtualChildren.Clear();
            while (mapedCanvas.Children.Count>0)
            {
                grid.Resources = mapedCanvas.Resources;
                UIElement el = mapedCanvas.Children[0];
                mapedCanvas.Children.RemoveAt(0);
                grid.AddVirtualChild(new VirtualElement(el as FrameworkElement));
             
            }
            
            
        }


        void OnSaveLog(object sender, RoutedEventArgs e)
        {
#if DEBUG_DUMP
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "quadtree.xml";
            if (s.ShowDialog() == true)
            {
                grid.Dump(s.FileName);
            }
#else
            MessageBox.Show("You need to build the assembly with 'DEBUG_DUMP' to get this feature");
#endif
        }

        void OnScaleChanged(object sender, EventArgs e)
        {
            // Make the grid lines get thinner as you zoom in
            //double t = _gridLines.StrokeThickness = 0.1 / grid.Scale.ScaleX;
           // grid.Backdrop.BorderThickness = new Thickness(t);
        }

        int lastTick = Environment.TickCount;
        int addedPerSecond = 0;
        int removedPerSecond = 0;

        void OnVisualsChanged(object sender, VisualChangeEventArgs e)
        {
            if (_animateStatus)
            {

                int tick = Environment.TickCount;
                if (e.Added != 0 || e.Removed != 0)
                {
                    addedPerSecond += e.Added;
                    removedPerSecond += e.Removed;

                }
                if (tick > lastTick + 1000)
                {
                    lastTick = tick;
                }
            }
        }

        void OnAnimateStatus(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            _animateStatus = item.IsChecked = !item.IsChecked;

        }

        delegate void BooleanEventHandler(bool arg);


        void ShowQuadTree(bool arg)
        {
#if DEBUG_DUMP
            grid.ShowQuadTree(arg);
            StatusText.Text = "Done.";
#else
            MessageBox.Show("You need to build the assembly with 'DEBUG_DUMP' to get this feature");
#endif
        }

        void OnRowColChange(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            int d = int.Parse((string)item.Tag, CultureInfo.InvariantCulture);
            rows = cols = d;
            AllocateNodes();
        }

        void OnHelp(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(@"Click left mouse button and drag to pan the view
Hold Control-Key and run mouse wheel to zoom in and out
Click middle mouse button to turn on auto-scrolling
Hold Control-Key and drag the mouse with left button down to draw a rectangle to zoom into that region.",
                "User Interface", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void OnZoom(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            string tag = item.Tag as string;
            if (tag == "Fit")
            {
                double scaleX = grid.ViewportWidth / grid.Extent.Width;
                double scaleY = grid.ViewportHeight / grid.Extent.Height;
                zoom.Zoom = Math.Min(scaleX, scaleY);
                zoom.Offset = new Point(0, 0);
            }
            else
            {
                double zoomPercent;
                if (double.TryParse(tag, out zoomPercent))
                {
                    zoom.Zoom = zoomPercent / 100;
                }
            }

        }

        void OnZoomChanged(object sender, EventArgs e)
        {
            /*if (ZoomSlider.Value != zoom.Zoom)
            {
                ZoomSlider.Value = zoom.Zoom;
            }*/
        }


        void OnZoomSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (zoom.Zoom != e.NewValue)
            {
                zoom.Zoom = e.NewValue;
            }
        }

        enum TestShapeType { Ellipse, Curve, Rectangle, Last };

        class VirtualElement : IVirtualChild
        {
            Rect _bounds;
            UIElement _visual=null;
            public event EventHandler BoundsChanged;
            UIElement _el;
            static int created=0;
            static int disposed=0;
            public VirtualElement(FrameworkElement el)
            {
                _bounds.X = Canvas.GetLeft(el);
                _bounds.Y = Canvas.GetTop(el);
                _bounds.Width = el.Width;
                _bounds.Height= el.Height;
                _el = el;
            
            }
            Dictionary<DependencyProperty, Binding> dict = new Dictionary<DependencyProperty, Binding>();



            public UIElement Visual
            {
                get { return _visual; }
            }

            public UIElement CreateVisual(VirtualCanvas parent)
            {
                if (_visual == null)
                {
                    _visual=_el;
                    Func<DependencyProperty, bool> f = p =>
                        {
                            Binding b = BindingOperations.GetBinding(_visual, p);
                            if (b.Source is ChannelDataProvider)
                            {
                                (b.Source as ChannelDataProvider).StopUpdate = false;
                                Debug.WriteLine((b.Source as ChannelDataProvider).ChannelName + "Created");
                                (b.Source as ChannelDataProvider).Refresh();
                       
                            }
                            return true;
                        };
                    /*
                    foreach (DependencyProperty p in dict.Keys)
                    {
                        BindingOperations.SetBinding(_visual, p, dict[p]);
                    }*/
                    EnumerateBindings(_visual, f);
                }
                return _visual;
            }

            public void DisposeVisual()
            {
                Func<DependencyProperty, bool> f = p =>
                {
                    Binding b = BindingOperations.GetBinding(_visual, p);
                    if (b.Source is ChannelDataProvider)
                    {
                        (b.Source as ChannelDataProvider).StopUpdate = true;
                        Debug.WriteLine((b.Source as ChannelDataProvider).ChannelName + "Disposed");
                        (b.Source as ChannelDataProvider).Refresh();
                    }
                    /*
                    Binding b = BindingOperations.GetBinding(_visual, p);
                    dict[p] = b;
                    BindingOperations.ClearBinding(_visual, p);
                    _visual.SetValue(p, null);
                    _visual.InvalidateProperty(p);
                    _visual.InvalidateVisual();*/
                    return true;
                };
                
                EnumerateBindings(_visual, f);
                _visual = null;
                
            }
            private void EnumerateBindings(DependencyObject target, Func<DependencyProperty, bool> action)
            {

                LocalValueEnumerator lve = target.GetLocalValueEnumerator();

                while (lve.MoveNext())
                {

                    LocalValueEntry entry = lve.Current;

                    if (BindingOperations.IsDataBound(target, entry.Property))
                    {

                        //Binding binding = (entry.Value as BindingExpression).ParentBinding;
                        
                        action(entry.Property);
                        

                    }

                }

            }
            public Rect Bounds
            {
                get { return _bounds; }
            }

            VirtualCanvas _parent;
            Typeface _typeface;
            double _fontSize;

            public Size MeasureText(VirtualCanvas parent, string label)
            {
                if (_parent != parent)
                {
                    FontFamily fontFamily = (FontFamily)parent.GetValue(TextBlock.FontFamilyProperty);
                    FontStyle fontStyle = (FontStyle)parent.GetValue(TextBlock.FontStyleProperty);
                    FontWeight fontWeight = (FontWeight)parent.GetValue(TextBlock.FontWeightProperty);
                    FontStretch fontStretch = (FontStretch)parent.GetValue(TextBlock.FontStretchProperty);
                    _fontSize = (double)parent.GetValue(TextBlock.FontSizeProperty);
                    _typeface = new Typeface(fontFamily, fontStyle, fontWeight, fontStretch);
                    _parent = parent;
                }
                FormattedText ft = new FormattedText(label, CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight, _typeface, _fontSize, Brushes.Black);
                return new Size(ft.Width, ft.Height);
            }

        }
    }

}