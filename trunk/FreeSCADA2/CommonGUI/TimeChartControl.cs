using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Threading;
using FreeSCADA.Interfaces;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;
namespace FreeSCADA.Common.Schema
{
    public class TimeChartData
    {
        public DateTime _time { get; set; }
        public object _value { get; set; }
    }
    public class TimeTrend
    {
        public String Name
        {
            get;
            set;
        }
        [Editor("FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors.ChannelSelectEditor, Designer",
        typeof(System.Drawing.Design.UITypeEditor))]
        public String Channel
        {
            get;
            set;
        }
        [Editor("FreeSCADA.Designer.SchemaEditor.PropertiesUtils.PropertyGridTypeEditors.BrushEditor, Designer",
        typeof(System.Drawing.Design.UITypeEditor))]
        public System.Windows.Media.Brush Brush { get; set; }
        [Browsable(false)]
        public ObservableCollection<TimeChartData> ChartData
        {
            get;
            private set;
        }
        public TimeTrend()
        {
            ChartData = new ObservableCollection<TimeChartData>();
        }
    }
    [ContentProperty("Trends")]
    public partial class TimeChartControl : UserControl
    {
        public override bool ShouldSerializeContent()
        {
            return false;
        }





        protected CheckBox _mode;
        protected ScrollBar _scroll;
        protected Chart _chart = new Chart();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ObservableCollection<TimeTrend> Trends
        {
            get
            {
                return (ObservableCollection<TimeTrend>)this.GetValue(TimeChartControl.TrendsProperty);
            }
        }

        public static readonly DependencyProperty TrendsProperty =
            DependencyProperty.Register(
                "Trends", typeof(ObservableCollection<TimeTrend>), typeof(TimeChartControl));//, new FrameworkPropertyMetadata(null));
        public String ChartName
        {
            get { return (String)GetValue(ChartNameProperty); }
            set { SetValue(ChartNameProperty, value); }
        }

        public static readonly DependencyProperty ChartNameProperty =
            DependencyProperty.Register(
                "ChartName", typeof(String), typeof(TimeChartControl));

        public Point ChartScale
        {
            get { return (Point)GetValue(ChartScaleProperty); }
            set { SetValue(ChartScaleProperty, value); }
        }

        public static readonly DependencyProperty ChartScaleProperty =
            DependencyProperty.Register(
                "ChartScale", typeof(Point), typeof(TimeChartControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnScaleChange)));

        public int ChartPeriod
        {
            get { return (int)GetValue(ChartPeriodProperty); }
            set { SetValue(ChartPeriodProperty, value); }
        }

        public static readonly DependencyProperty ChartPeriodProperty =
            DependencyProperty.Register(
                "ChartPeriod", typeof(int), typeof(TimeChartControl), new FrameworkPropertyMetadata(new PropertyChangedCallback(OnPeriodChange)));


        private static void OnPeriodChange(DependencyObject o, DependencyPropertyChangedEventArgs ea)
        {
            if (Env.Current.Mode != EnvironmentMode.Runtime)
            {
                TimeChartControl tcc = o as TimeChartControl;
                ((DateTimeAxis)tcc._chart.Axes[0]).Minimum = DateTime.Now.AddSeconds(-tcc.ChartPeriod / 2);
                ((DateTimeAxis)tcc._chart.Axes[0]).Maximum = DateTime.Now.AddSeconds(tcc.ChartPeriod / 2);
                //((DateTimeAxis)tcc._chart.Axes[0]).Interval=tcc.ChartPeriod/5;
                tcc._chart.Refresh();
            }
        }
        private static void OnScaleChange(DependencyObject o, DependencyPropertyChangedEventArgs ea)
        {
            TimeChartControl tcc = o as TimeChartControl;
            ((LinearAxis)tcc._chart.Axes[1]).Minimum = tcc.ChartScale.X;
            ((LinearAxis)tcc._chart.Axes[1]).Maximum = tcc.ChartScale.Y;
            ((LinearAxis)tcc._chart.Axes[1]).Interval = (tcc.ChartScale.Y - tcc.ChartScale.X) / 5;
            tcc._chart.Refresh();

        }

        private DispatcherTimer dispatcherTimer;


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {

            if (_mode.IsChecked == false)
                foreach (TimeTrend trend in Trends)
                {
                    trend.ChartData.Add(new TimeChartData { _time = DateTime.Now, _value = Env.Current.CommunicationPlugins.GetChannel(trend.Channel).Value });
                    if (trend.ChartData.Count > ChartPeriod)
                        trend.ChartData.RemoveAt(0);
                }

        }


        public TimeChartControl()
        {
            SetValue(TimeChartControl.TrendsProperty, new ObservableCollection<TimeTrend>());
            StackPanel sp = new StackPanel();
            _chart.VerticalAlignment = VerticalAlignment.Stretch;

            sp.Children.Add(_chart);
            Content = sp;
            DateTimeAxis dta = new DateTimeAxis();
            dta.ShowGridLines = true;
            dta.Orientation = AxisOrientation.X;
            _chart.Axes.Add(dta);
            LinearAxis la = new LinearAxis();
            la.Orientation = AxisOrientation.Y;
            la.ShowGridLines = true;
            _chart.Axes.Add(la);
            ChartPeriod = 60;
            ChartScale = new Point(-1, 1);
            System.Windows.Data.Binding b = new System.Windows.Data.Binding("ChartName");
            b.Source = this;
            _chart.SetBinding(Chart.TitleProperty, b);
            Loaded += new RoutedEventHandler(TimeChartControl_Loaded);
            _mode = new CheckBox();
            sp.Children.Insert(0, _mode);
            _mode.Content = "Mode";
            _mode.Checked += new RoutedEventHandler(cb_Checked);
            _scroll = new ScrollBar();
            _scroll.Orientation = Orientation.Horizontal;
            _scroll.Scroll += new System.Windows.Controls.Primitives.ScrollEventHandler(sb_Scroll);
            sp.Children.Add(_scroll);
            System.Windows.Data.Binding bind = new System.Windows.Data.Binding("IsChecked");
            bind.Source = _mode;
            _scroll.SetBinding(ScrollBar.IsEnabledProperty, bind);
            //if (Env.Current.Mode != EnvironmentMode.Runtime)
            //cb.IsEnabled = false;Al

        }

        void cb_Checked(object sender, RoutedEventArgs e)
        {
            List<Archiver.ChannelInfo> channels = new List<FreeSCADA.Archiver.ChannelInfo>();
            foreach (TimeTrend trend in Trends)
            {
                Archiver.ChannelInfo ci = new Archiver.ChannelInfo();
                String[] strs = trend.Channel.Split('.');
                ci.PluginId = strs[0];
                ci.ChannelName = strs[1];
                channels.Add(ci);
            }

            DateTime dt = Archiver.ArchiverMain.Current.GetChannelsOlderDate(channels);
            _scroll.Minimum = -(DateTime.Now - dt).TotalSeconds;
            _scroll.Maximum = 0;

            _scroll.ViewportSize = ChartPeriod;
            _scroll.LargeChange = ChartPeriod;
            _scroll.Value = 0;

        }

        void sb_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            Dictionary<TimeTrend, System.Data.DataTable> tables = new Dictionary<TimeTrend, System.Data.DataTable>();

            foreach (TimeTrend trend in Trends)
            {
                Archiver.ChannelInfo ci = new Archiver.ChannelInfo();
                String[] strs = trend.Channel.Split('.');
                ci.PluginId = strs[0];
                ci.ChannelName = strs[1];
                List<Archiver.ChannelInfo> channels = new List<FreeSCADA.Archiver.ChannelInfo>();
                channels.Add(ci);
                //System.Data.DataTable dt = Archiver.ArchiverMain.Current.GetChannelData(DateTime.Now.AddSeconds(e.NewValue - ChartPeriod),
                  //  DateTime.Now.AddSeconds(e.NewValue), channels);
                System.Data.DataTable dt = new System.Data.DataTable();
                trend.ChartData.Clear();
                
                for (int i = 0; i < ChartPeriod; i++)
                {
                    double val = double.NaN;
                    if (dt.Rows.Count>0)
                    {
                        DateTime date = new DateTime();
                        DateTime.TryParse(dt.Rows[0]["Time"].ToString(), out date);

                        if (date <= DateTime.Now.AddSeconds(e.NewValue - ChartPeriod + i))
                        {

                            double.TryParse(dt.Rows[0]["Value"].ToString(), out val);
                            dt.Rows.RemoveAt(0);
                        }
                    }
                    trend.ChartData.Add(
                        new TimeChartData
                        {
                            _time = DateTime.Now.AddSeconds(e.NewValue - ChartPeriod + i),
                            _value = val
                        });

                }
            }



        }

        void TimeChartControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Env.Current.Mode == EnvironmentMode.Runtime)
            {
                foreach (TimeTrend trend in Trends)
                {
                    LineSeries ls = new LineSeries();
                    ls.ItemsSource = trend.ChartData;
                    ls.IndependentValueBinding = new System.Windows.Data.Binding("_time");
                    ls.DependentValueBinding = new System.Windows.Data.Binding("_value");
                    //ls.DataPointStyle = new Style();
                    //ls.DataPointStyle.Setters.Add(new Setter(DataPoint.VisibilityProperty, Visibility.Hidden));
                    Style st = new Style();
                    st.TargetType = typeof(Control);
                    st.Setters.Add(new Setter(Control.BackgroundProperty, trend.Brush));
                    _chart.StylePalette.Clear();
                    _chart.StylePalette.Add(st);
                    //ls.Background = trend.Brush;
                    if (trend.Name == String.Empty)
                        ls.Title = trend.Channel;
                    else ls.Title = trend.Name;
                    _chart.Series.Add(ls);
                }
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

                dispatcherTimer.Start();
            }
        }

    }
}
