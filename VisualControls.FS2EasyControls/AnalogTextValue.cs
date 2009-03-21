using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FreeSCADA.Common;
using FreeSCADA.Interfaces;

namespace FreeSCADA.VisualControls.FS2EasyControls
{
    /// <summary>
    /// Class is intended for a quick setup of a measured value display
    /// INTENTIONALLY implemented unflexible in the WPF sense, drawing its content in the OnRender method
    /// and sets Dependency properties in the constructor so that it cannot inherit its values from the container
    /// </summary>
    public class AnalogTextValue : Control
    {
        // Dependency properties.
        public static readonly DependencyProperty ChannelBadEdgeProperty;
        public static readonly DependencyProperty UnitProperty;
        public static readonly DependencyProperty DecimalPlacesProperty;
        public static readonly DependencyProperty ChannelNameProperty;

        IChannel fs2channel = null;
        ChannelStatusFlags statusFlags = ChannelStatusFlags.NotUsed;
        string format = "{0:F0}";
        string outTxt = "VarErr";
        const double nokPenThickness = 4.0;
        const double okPenThickness = 1.0;
        Pen nokPen = new Pen(Brushes.DarkGray, nokPenThickness);
        Pen okPen = new Pen(Brushes.Black, okPenThickness);
        Typeface font;

        // Public interfaces to dependency properties.
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
            (d as AnalogTextValue).nokPen = new Pen((Brush)e.NewValue, nokPenThickness);
        }

        public string Unit
        {
            set
            {
                SetValue(UnitProperty, value);
            }
            get
            {
                return (string)GetValue(UnitProperty);
            }
        }

        private static void OnUnitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null && e.NewValue.ToString() != "")
                (d as AnalogTextValue).format = "{0:F" + (d as AnalogTextValue).DecimalPlaces + "} " + e.NewValue.ToString();
            else
                (d as AnalogTextValue).format = "{0:F" + (d as AnalogTextValue).DecimalPlaces + "}";
            (d as AnalogTextValue).InvalidateVisual();
        }

        public int DecimalPlaces
        {
            set
            {
                SetValue(DecimalPlacesProperty, value);
            }
            get
            {
                return (int)GetValue(DecimalPlacesProperty);
            }
        }

        private static void OnDecimalPlacesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as AnalogTextValue).Unit != null && (d as AnalogTextValue).Unit != "")
                (d as AnalogTextValue).format = "{0:F" + e.NewValue.ToString() + "} " + (d as AnalogTextValue).Unit;
            else
                (d as AnalogTextValue).format = "{0:F" + e.NewValue.ToString() + "}";
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
            if ((d as AnalogTextValue).fs2channel != null)
            {
                (d as AnalogTextValue).fs2channel.ValueChanged -= new EventHandler((d as AnalogTextValue).OnChannelValueChanged);
            }
            if ((d as AnalogTextValue).ChannelName != null) (d as AnalogTextValue).fs2channel = Env.Current.CommunicationPlugins.GetChannel((d as AnalogTextValue).ChannelName);
            if ((d as AnalogTextValue).fs2channel != null)
                (d as AnalogTextValue).fs2channel.ValueChanged += new EventHandler((d as AnalogTextValue).OnChannelValueChanged);
            else
                (d as AnalogTextValue).outTxt = "VarErr";
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
        public AnalogTextValue()
        {
            Background = Brushes.WhiteSmoke;
            ChannelBadEdge = Brushes.DarkGray;
            FontFamily = new FontFamily("Arial");
            FontSize = 25;
            ToolTip = new ToolTip();
            (ToolTip as ToolTip).Content = "Channel name set to non-existent variable";
        }

        // Destructor
        ~AnalogTextValue()
        {
            if (fs2channel != null)
                fs2channel.ValueChanged -= new EventHandler(OnChannelValueChanged);
        }

        // Static constructor.
        static AnalogTextValue()
        {
            ChannelBadEdgeProperty =
            DependencyProperty.Register("ChannelBadEdge", typeof(Brush),
            typeof(AnalogTextValue), new FrameworkPropertyMetadata(Brushes.DarkGray,
            FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnChannelBadEdgeChanged)));
            UnitProperty =
            DependencyProperty.Register("Unit", typeof(string),
            typeof(AnalogTextValue), new FrameworkPropertyMetadata("",
            FrameworkPropertyMetadataOptions.AffectsMeasure, new PropertyChangedCallback(OnUnitChanged)));
            DecimalPlacesProperty =
            DependencyProperty.Register("DecimalPlaces", typeof(int),
            typeof(AnalogTextValue), new FrameworkPropertyMetadata(0,
            FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnDecimalPlacesChanged)));
            ChannelNameProperty =
            DependencyProperty.Register("ChannelName", typeof(string),
            typeof(AnalogTextValue), new FrameworkPropertyMetadata(null,
            FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnChannelNameChanged)));
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
            Pen actualPen = nokPen;

            if (fs2channel != null)
            {
                if (fs2channel.StatusFlags == ChannelStatusFlags.Good)
                {
                    actualPen = okPen;
                }
                if (fs2channel.Value != null)
                    outTxt = string.Format(format, fs2channel.Value);
                else
                    outTxt = "{null}";
            }
            // Adjust rendering size for width of Pen.
            size.Width = Math.Max(0, size.Width - actualPen.Thickness);
            size.Height = Math.Max(0, size.Height - actualPen.Thickness);

            // Draw the rectangle.
            dc.DrawRectangle(Background, actualPen, new Rect(actualPen.Thickness / 2, actualPen.Thickness / 2, size.Width, size.Height));
            FormattedText formtxt =
                new FormattedText(outTxt, CultureInfo.CurrentCulture, FlowDirection, font, FontSize, Foreground);
            Point textPt = new Point((RenderSize.Width - formtxt.Width) / 2, (RenderSize.Height - formtxt.Height) / 2);
            dc.DrawText(formtxt, textPt);
        }
    }
}
