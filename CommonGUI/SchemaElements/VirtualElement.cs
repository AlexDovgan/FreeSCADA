using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;


namespace FreeSCADA.Common.Schema
{
    public class VirtualElement : IVirtualChild
    {
        Rect _bounds;
        FrameworkElement _visual = null;
        public event EventHandler BoundsChanged;
        FrameworkElement _el;
        static int created = 0;
        static int disposed = 0;
        public VirtualElement(FrameworkElement el)
        {
            _bounds.X = Canvas.GetLeft(el);
            _bounds.Y = Canvas.GetTop(el);
            _bounds.Width = el.Width;
            _bounds.Height = el.Height;
            _el = el;

        }
        Dictionary<DependencyProperty, Binding> dict = new Dictionary<DependencyProperty, Binding>();



        public FrameworkElement Visual
        {
            get { return _visual; }
        }

        public FrameworkElement CreateVisual(VirtualCanvas parent)
        {
            if (_visual == null)
            {
                _visual = _el;
                Func<DependencyProperty, bool> f = p =>
                    {
                        Binding b = BindingOperations.GetBinding(_visual, p);
                        if (b!=null && b.Source is ChannelDataProvider)
                        {
                            (b.Source as ChannelDataProvider).StopUpdate = false;
                            (b.Source as ChannelDataProvider).Refresh();

                        }
                        return true;
                    };
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
                    (b.Source as ChannelDataProvider).Refresh();
                }
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

                    action(entry.Property);


                }

            }

        }
        public Rect Bounds
        {
            get { return _bounds; }
        }
/*
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
        */
    }
}
