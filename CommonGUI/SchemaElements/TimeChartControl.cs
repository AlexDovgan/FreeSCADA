using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using FreeSCADA.Interfaces;
using System.ComponentModel;
using System.Windows.Markup;
using System.Windows.Controls.Primitives;
using Microsoft.Windows.Controls;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts;

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
        public Collection<TimeChartData> ChartData
        {
            get;
            protected set;

        }

        [Browsable(false)]
        public EnumerableDataSource<TimeChartData> DataSource
        {
            get;
            private set;
        }
        [Browsable(false)]
        public Func<DateTime, double> ConvertToDouble 
        {
            get;
            set;
        }
        public TimeTrend()
        {

            ChartData = new Collection<TimeChartData>();
            DataSource = new EnumerableDataSource<TimeChartData>(ChartData);
            DataSource.SetXMapping(x => ConvertToDouble(x._time));
            DataSource.SetYMapping(y =>Double.Parse(y._value.ToString()));
                 
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
        protected ChartPlotter _chart = new ChartPlotter();
        protected DatePicker _from;
        protected DatePicker _to;

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
                //((DateTimeAxis)tcc._chart.Axes[0]).Minimum = DateTime.Now.AddSeconds(-tcc.ChartPeriod / 2);
                //((DateTimeAxis)tcc._chart.Axes[0]).Maximum = DateTime.Now.AddSeconds(tcc.ChartPeriod / 2);
                //((DateTimeAxis)tcc._chart.Axes[0]).Interval=tcc.ChartPeriod/5;
                //(tcc._chart.HorizontalAxis as HorizontalDateTimeAxis).LabelProvider.LabelStringFormat
                tcc._chart.FitToView();
            }
        }
        private static void OnScaleChange(DependencyObject o, DependencyPropertyChangedEventArgs ea)
        {
            TimeChartControl tcc = o as TimeChartControl;/*
            ((LinearAxis)tcc._chart.Axes[1]).Minimum = tcc.ChartScale.X;
            ((LinearAxis)tcc._chart.Axes[1]).Maximum = tcc.ChartScale.Y;
            ((LinearAxis)tcc._chart.Axes[1]).Interval = (tcc.ChartScale.Y - tcc.ChartScale.X) / 5;
            tcc._chart.Refresh();*/
            

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
                    trend.DataSource.RaiseDataChanged();
                }
            
        }


        public TimeChartControl()
        {
            SetValue(TimeChartControl.TrendsProperty, new ObservableCollection<TimeTrend>());
            StackPanel sp = new StackPanel();
            StackPanel sp1 = new StackPanel();
            Button bt = new Button();
            bt.Content = "Show";
            bt.Click += new RoutedEventHandler(bt_Click);
            _mode = new CheckBox();
            _from = new DatePicker();
            _to = new DatePicker();


            _mode.Content = "History Mode";
            _mode.Checked += new RoutedEventHandler(_mode_Checked);
            _mode.Unchecked += new RoutedEventHandler(_mode_Unchecked);
            sp1.Orientation = Orientation.Horizontal;
            sp1.Children.Add(_mode);
            sp1.Children.Add(_from);
            sp1.Children.Add(_to);
            sp1.Children.Add(bt);
            sp.Children.Add(sp1);
            
            
            //_chart.VerticalAlignment = VerticalAlignment.Stretch;
            _chart.Children.Add(new Header());
            _chart.HorizontalAxis = new HorizontalDateTimeAxis();

            
            sp.Children.Add(_chart);
            sp.Orientation = Orientation.Vertical;
            sp.VerticalAlignment = VerticalAlignment.Stretch;
            Viewbox vb=new Viewbox();
            vb.Child=sp;
            Content =vb;

            ChartPeriod = 60;
            ChartScale = new Point(-1, 1);
            System.Windows.Data.Binding b = new System.Windows.Data.Binding("ChartName");
            b.Source = this;
            _chart.SetBinding(Plotter.NameProperty, b);
            Loaded += new RoutedEventHandler(TimeChartControl_Loaded);
            
            System.Windows.Data.Binding bind = new System.Windows.Data.Binding("IsChecked");
            bind.Source = _mode;
            _from.SetBinding(DatePicker.IsEnabledProperty, bind);
            _to.SetBinding(DatePicker.IsEnabledProperty, bind);
            bt.SetBinding(DatePicker.IsEnabledProperty, bind);
            if (Env.Current.Mode != EnvironmentMode.Runtime)
                _mode.IsEnabled = false;
            
        }

        void bt_Click(object sender, RoutedEventArgs e)
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
                System.Data.DataTable dt = Archiver.ArchiverMain.Current.GetChannelData(_from.SelectedDate.Value, _to.SelectedDate.Value, channels);
                //System.Data.DataTable dt = new System.Data.DataTable();
                trend.ChartData.Clear();
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    DateTime date = new DateTime();
                    double val = double.NaN;
                    DateTime.TryParse(row["Time"].ToString(), out date);
                    double.TryParse(row["Value"].ToString(), out val);

                    trend.ChartData.Add(
                        new TimeChartData
                        {
                            _time = date,
                            _value = val
                        });

                }
                trend.DataSource.RaiseDataChanged();
            }
            _chart.FitToView();
        }

        void _mode_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (TimeTrend trend in Trends)
            {
                trend.ChartData.Clear();
            }
            
        }

        void _mode_Checked(object sender, RoutedEventArgs e)
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
            _from.BlackoutDates.Clear();
            _to.BlackoutDates.Clear();

            DateTime dt = Archiver.ArchiverMain.Current.GetChannelsOlderDate(channels);
            _from.BlackoutDates.Add(new CalendarDateRange(DateTime.MinValue, dt.AddDays(-1)));
            _to.BlackoutDates.Add(new CalendarDateRange(DateTime.Now.AddDays(1),DateTime.MaxValue));
            _from.SelectedDate = dt;
            _to.SelectedDate = DateTime.Now;
           
        }

        void TimeChartControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Env.Current.Mode == EnvironmentMode.Runtime)
            {
                foreach (TimeTrend trend in Trends)
                {
                    trend.ConvertToDouble = (_chart.HorizontalAxis as DateTimeAxis).ConvertToDouble;
                    _chart.AddLineGraph(trend.DataSource,
                        (trend.Brush as System.Windows.Media.SolidColorBrush).Color,
                        1,trend.Name);
                    
                }
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

                dispatcherTimer.Start();
            }
        }

    }
}
