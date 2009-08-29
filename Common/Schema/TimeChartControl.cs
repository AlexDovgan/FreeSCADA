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

namespace FreeSCADA.Common.Schema
{
    public class TimeChartData
    {
        public DateTime Time { get; set; }
        public object Value { get; set; }
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





        protected Chart chart = new Chart();
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
                ((DateTimeAxis)tcc.chart.Axes[0]).Minimum = DateTime.Now.AddSeconds(-tcc.ChartPeriod / 2);
                ((DateTimeAxis)tcc.chart.Axes[0]).Maximum = DateTime.Now.AddSeconds(tcc.ChartPeriod / 2);
                //((DateTimeAxis)tcc.chart.Axes[0]).Interval=tcc.ChartPeriod/5;
                tcc.chart.Refresh();
            }
        }
        private static void OnScaleChange(DependencyObject o, DependencyPropertyChangedEventArgs ea)
        {
            TimeChartControl tcc = o as TimeChartControl;
            ((LinearAxis)tcc.chart.Axes[1]).Minimum = tcc.ChartScale.X;
            ((LinearAxis)tcc.chart.Axes[1]).Maximum = tcc.ChartScale.Y;
            ((LinearAxis)tcc.chart.Axes[1]).Interval = (tcc.ChartScale.Y - tcc.ChartScale.X) / 5;
            tcc.chart.Refresh();

        }

        private DispatcherTimer dispatcherTimer;


        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            foreach (TimeTrend trend in Trends)
            {
                trend.ChartData.Add(new TimeChartData { Time = DateTime.Now, Value = Env.Current.CommunicationPlugins.GetChannel(trend.Channel).Value });
                if (trend.ChartData.Count > ChartPeriod)
                    trend.ChartData.RemoveAt(0);
            }

        }


        public TimeChartControl()
        {
            SetValue(TimeChartControl.TrendsProperty, new ObservableCollection<TimeTrend>());

            Content = chart;
            DateTimeAxis dta = new DateTimeAxis();
            dta.ShowGridLines = true;
            dta.Orientation = AxisOrientation.X;
            chart.Axes.Add(dta);
            LinearAxis la = new LinearAxis();
            la.Orientation = AxisOrientation.Y;
            la.ShowGridLines = true;
            chart.Axes.Add(la);
            ChartPeriod = 60;
            ChartScale = new Point(-1, 1);
            System.Windows.Data.Binding b=new System.Windows.Data.Binding("ChartName");
            b.Source=this;
        
            chart.SetBinding(Chart.TitleProperty, b);
            Loaded += new RoutedEventHandler(TimeChartControl_Loaded);
        
        }

        void TimeChartControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Env.Current.Mode == EnvironmentMode.Runtime)
            {
                foreach (TimeTrend trend in Trends)
                {
                    LineSeries ls = new LineSeries();
                    ls.ItemsSource = trend.ChartData;
                    ls.IndependentValueBinding = new System.Windows.Data.Binding("Time");
                    ls.DependentValueBinding = new System.Windows.Data.Binding("Value");
                    //ls.DataPointStyle = new Style();
                    //ls.DataPointStyle.Setters.Add(new Setter(DataPoint.VisibilityProperty, Visibility.Hidden));
                    Style st = new Style();
                    st.TargetType = typeof(Control);
                    st.Setters.Add(new Setter(Control.BackgroundProperty, trend.Brush));
                    chart.StylePalette.Clear();
                    chart.StylePalette.Add(st);
                    //ls.Background = trend.Brush;
                    if (trend.Name == String.Empty)
                        ls.Title = trend.Channel;
                    else ls.Title = trend.Name;
                    chart.Series.Add(ls);
                }
                dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            
                dispatcherTimer.Start();
            }
        }

    }
}
