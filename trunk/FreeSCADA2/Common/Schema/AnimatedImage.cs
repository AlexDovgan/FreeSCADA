using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace FreeSCADA.Common.Schema
{
    
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfAnimatedControl"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:WpfAnimatedControl;assembly=WpfAnimatedControl"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    public class AnimatedImage : System.Windows.Controls.Image
    {
     
        private int _nCurrentFrame = 0;
        private Timer timer;

        private bool _bIsAnimating = false;

        private  BitmapDecoder animatedBitmap;

        public bool IsAnimating
        {
            get { return _bIsAnimating; }
        }

        public AnimatedImage()
        {
            Stretch = Stretch.Fill;
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(AnimatedImage), new FrameworkPropertyMetadata(typeof(AnimatedImage)));
        }
        public string ImageName
        {
            get { return (string)GetValue(ImageNameProperty); }
            set { StopAnimate(); SetValue(ImageNameProperty, value); }
        }
        public bool AnimatedControl
        {
            get { return (bool)GetValue(AnimatedControlProperty); }
            set { StopAnimate(); SetValue(AnimatedControlProperty, value); }
        }

        /// <summary>
        /// Identifies the Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ImageNameProperty =
            DependencyProperty.Register(
                "ImageName", typeof(string), typeof(AnimatedImage),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAnimatedBitmapChanged)));

        public static readonly DependencyProperty AnimatedControlProperty =
                  DependencyProperty.Register(
                      "AnimatedControl", typeof(bool), typeof(AnimatedImage),
                      new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnAnimatedControlChanged)));
      
        private static void OnAnimatedControlChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {

            AnimatedImage control = (AnimatedImage)obj;
            if (control.ImageName == null)
                return;

            if ((bool)args.NewValue == true)
                control.StartAnimate();
            if ((bool)args.NewValue == false)
                control.StopAnimate();
        }


        private static void OnAnimatedBitmapChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            AnimatedImage control = (AnimatedImage)obj;

            System.IO.Stream stream = Env.Current.Project.GetData(ProjectEntityType.Image,control.ImageName);
            control.animatedBitmap = BitmapDecoder.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            control.Source = control.animatedBitmap.Frames[0];

            RoutedPropertyChangedEventArgs<string> e = new RoutedPropertyChangedEventArgs<string>(
                (string)args.OldValue, (string)args.NewValue, AnimatedBitmapChangedEvent);
            control.OnAnimatedBitmapChanged(e);
        }

        /// <summary>
        /// Identifies the ValueChanged routed event.
        /// </summary>
        public static readonly RoutedEvent AnimatedBitmapChangedEvent = EventManager.RegisterRoutedEvent(
            "AnimatedBitmapChanged", RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<string>), typeof(AnimatedImage));

        /// <summary>
        /// Occurs when the Value property changes.
        /// </summary>
        public event RoutedPropertyChangedEventHandler<string> AnimatedBitmapChanged
        {
            add { AddHandler(AnimatedBitmapChangedEvent, value); }
            remove { RemoveHandler(AnimatedBitmapChangedEvent, value); }
        }

        /// <summary>
        /// Raises the ValueChanged event.
        /// </summary>
        /// <param name="args">Arguments associated with the ValueChanged event.</param>
        protected virtual void OnAnimatedBitmapChanged(RoutedPropertyChangedEventArgs<string> args)
        {
            RaiseEvent(args);
        }
     
        private delegate void VoidDelegate();

        private void OnFrameChanged(object o)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Render, new VoidDelegate(() => ChangeSource()));

        }
        void ChangeSource()
        {
            Source = animatedBitmap.Frames[_nCurrentFrame++];
            _nCurrentFrame = _nCurrentFrame % (animatedBitmap.Frames.Count);
            if (_nCurrentFrame == 0) _nCurrentFrame = 1;

        }

        public void StopAnimate()
        {
            if (_bIsAnimating)
            {
                timer.Dispose();
                //ImageAnimator.StopAnimate(AnimatedBitmap, new EventHandler(this.OnFrameChanged));
                _bIsAnimating = false;
            }
        }

        public void StartAnimate()
        {
            if (!_bIsAnimating)
            {
                if (animatedBitmap.Frames.Count > 1)
                {
                    timer = new Timer(new TimerCallback(OnFrameChanged));
                    timer.Change(0, 100);
                }
               //ImageAnimator.Animate(AnimatedBitmap, new EventHandler(this.OnFrameChanged));
                _bIsAnimating = true;
            }
        }
        protected override bool ShouldSerializeProperty(DependencyProperty dp)
        {
            if (dp == Image.SourceProperty)
                return false;
            else
                return base.ShouldSerializeProperty(dp);
        }

    }
}


