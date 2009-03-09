using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Interfaces;
using FreeSCADA.Common;

namespace FreeSCADA.VisualControls.FS2EasyControls
{
    /// <summary>
    /// Class is intended for a quick setup of a measured value display
    /// INTENTIONALLY implemented unflexible in the WPF sense, drawing its content in the OnRender method
    /// and sets Dependency properties in the constructor so that it cannot inherit its values from the container
    /// </summary>
    public class BinaryColorText : Control
    {
        // Dependency properties.
        public static readonly DependencyProperty Background1Property;
        public static readonly DependencyProperty Foreground1Property;
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty Text1Property;
        public static readonly DependencyProperty ChannelNameProperty;
        public static readonly DependencyProperty ChannelBadEdgeProperty;

        IChannel fs2channel = null;
        ChannelStatusFlags statusFlags = ChannelStatusFlags.NotUsed;
        const double nokPenThickness = 4.0;
        const double okPenThickness = 1.0;
        Pen nokPen = new Pen(Brushes.DarkGray, nokPenThickness);
        Pen okPen = new Pen(Brushes.Black, okPenThickness);
        Typeface font;

        // Public interfaces to dependency properties.
        public Brush Background1
        {
            set
            {
                SetValue(Background1Property, value);
            }
            get
            {
                return (Brush)GetValue(Background1Property);
            }
        }

        public Brush Foreground1
        {
            set
            {
                SetValue(Foreground1Property, value);
            }
            get
            {
                return (Brush)GetValue(Foreground1Property);
            }
        }

        public string Text
        {
            set
            {
                SetValue(TextProperty, value);
            }
            get
            {
                return (string)GetValue(TextProperty);
            }
        }

        public string Text1
        {
            set
            {
                SetValue(Text1Property, value);
            }
            get
            {
                return (string)GetValue(Text1Property);
            }
        }

        public string ChannelName
        {
            set
            {
                SetValue(ChannelNameProperty, value);
            }
            get
            {
                return (string)GetValue(ChannelNameProperty);
            }
        }

        private static void OnChannelNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as BinaryColorText).fs2channel != null)
            {
                (d as BinaryColorText).fs2channel.ValueChanged -= new EventHandler((d as BinaryColorText).OnChannelValueChanged);
            }
            if ((d as BinaryColorText).ChannelName != null) (d as BinaryColorText).fs2channel = Env.Current.CommunicationPlugins.GetChannel((d as BinaryColorText).ChannelName);
            if ((d as BinaryColorText).fs2channel != null)
                (d as BinaryColorText).fs2channel.ValueChanged += new EventHandler((d as BinaryColorText).OnChannelValueChanged);
        }

        public Brush ChannelBadEdge
        {
            set
            {
                SetValue(ChannelBadEdgeProperty, value);
            }
            get
            {
                return (Brush)GetValue(ChannelBadEdgeProperty);
            }
        }

        private static void OnChannelBadEdgeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as BinaryColorText).nokPen = new Pen((Brush)e.NewValue, nokPenThickness);
        }

        private delegate void UpdateChannelDelegate(IChannel channel);
        private void UpdateChannelFunc(IChannel channel)
        {
            if (statusFlags != channel.StatusFlags)
            {
                statusFlags = channel.StatusFlags;
                (ToolTip as ToolTip).Content = "Variable: " + channel.PluginId + "." + channel.Name + ", Status: " + channel.Status;
            }
            this.InvalidateVisual();
        }

        delegate void InvokeDelegate();
        void OnChannelValueChanged(object sender, EventArgs e)
        {
            IChannel ch = (IChannel)sender;
            object[] args = { ch };
            this.Dispatcher.BeginInvoke(new UpdateChannelDelegate(UpdateChannelFunc), args);
        }

        // Constructor
        public BinaryColorText()
        {
            Background = Brushes.WhiteSmoke;
            FontFamily = new FontFamily("Arial");
            FontSize = 25;
            ToolTip = new ToolTip();
            (ToolTip as ToolTip).Content = "Channel name set to non-existent variable";
            Foreground = Brushes.Black;
        }

        // Destructor
        ~BinaryColorText()
        {
            if (fs2channel != null)
                fs2channel.ValueChanged -= new EventHandler(OnChannelValueChanged);
        }

        // Static constructor.
        static BinaryColorText()
        {
            Background1Property =
            DependencyProperty.Register("Background1", typeof(Brush),
            typeof(BinaryColorText), new FrameworkPropertyMetadata(Brushes.LightGreen,
            FrameworkPropertyMetadataOptions.AffectsRender));
            Foreground1Property =
            DependencyProperty.Register("Foreground1", typeof(Brush),
            typeof(BinaryColorText), new FrameworkPropertyMetadata(Brushes.Black,
            FrameworkPropertyMetadataOptions.AffectsRender));
            TextProperty =
            DependencyProperty.Register("Text", typeof(string),
            typeof(BinaryColorText), new FrameworkPropertyMetadata("Text0",
            FrameworkPropertyMetadataOptions.AffectsRender));
            Text1Property =
            DependencyProperty.Register("Text1", typeof(string),
            typeof(BinaryColorText), new FrameworkPropertyMetadata("Text1",
            FrameworkPropertyMetadataOptions.AffectsRender));
            ChannelNameProperty =
            DependencyProperty.Register("ChannelName", typeof(string),
            typeof(BinaryColorText), new FrameworkPropertyMetadata("",
            FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnChannelNameChanged)));
            ChannelBadEdgeProperty =
            DependencyProperty.Register("ChannelBadEdge", typeof(Brush),
            typeof(BinaryColorText), new FrameworkPropertyMetadata(Brushes.DarkGray,
            FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnChannelBadEdgeChanged)));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == BackgroundProperty ||
                e.Property == ForegroundProperty ||
                e.Property == FontSizeProperty)
            {
                InvalidateVisual();
            }
            if (e.Property == FontFamilyProperty)
            {
                font = new Typeface(FontFamily, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal);
            }
            base.OnPropertyChanged(e);
        }

        // Override of MeasureOverride.
        protected override Size MeasureOverride(Size sizeAvailable)
        {
            Size sizeDesired = base.MeasureOverride(sizeAvailable);
            Pen actualPen = nokPen;

            if (fs2channel != null)
            {
                if (!(fs2channel.StatusFlags != ChannelStatusFlags.Good))
                    actualPen = okPen;
            }
            sizeDesired = new Size(actualPen.Thickness, actualPen.Thickness);

            return sizeDesired;
        }
        // Override of OnRender.
        protected override void OnRender(DrawingContext dc)
        {
            Size size = RenderSize;
            string actualText = null;
            Brush actualBackground;
            Brush actualForeground;
            Pen actualPen = nokPen;
            bool actualValue = false;
            bool stringValue = false;

            if (fs2channel != null)
            {
                if (fs2channel.StatusFlags == ChannelStatusFlags.Good)
                    actualPen = okPen;
                if (fs2channel.Value != null)
                {
                    if (fs2channel.Value.GetType() == typeof(bool))
                    {
                        actualValue = (bool)fs2channel.Value;
                    }
                    else
                    {
                        if (fs2channel.Value.GetType() == typeof(string))
                        {
                            actualText = (string)fs2channel.Value;
                            stringValue = true;
                        }
                        else
                        {
                            try
                            {
                                if (Convert.ToDouble(fs2channel.Value) != 0)
                                {
                                    actualValue = true;
                                }
                            }
                            catch { }
                        }
                    }
                }
            }
            if (actualValue)
            {
                actualBackground = Background1;
                actualForeground = Foreground1;
                actualText = Text1;
            }
            else
            {
                actualBackground = Background;
                actualForeground = Foreground;
                if (!stringValue) actualText = Text;
            }
            // Adjust rendering size for width of Pen.
            size.Width = Math.Max(0, size.Width - actualPen.Thickness);
            size.Height = Math.Max(0, size.Height - actualPen.Thickness);

            // Draw the rectangle.
            dc.DrawRectangle(actualBackground, actualPen, new Rect(actualPen.Thickness / 2, actualPen.Thickness / 2, size.Width, size.Height));

            FormattedText formtxt =
                new FormattedText(actualText, CultureInfo.CurrentCulture, FlowDirection, font, FontSize, actualForeground);
            Point textPt = new Point((RenderSize.Width - formtxt.Width) / 2, (RenderSize.Height - formtxt.Height) / 2);
            dc.DrawText(formtxt, textPt);
        }
    }
}
