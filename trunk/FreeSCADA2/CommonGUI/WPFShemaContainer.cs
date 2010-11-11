using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FreeSCADA.Common.Gestures;

namespace FreeSCADA.Common.Schema
{   public class WPFShemaContainer : System.Windows.Forms.Integration.ElementHost
	{
        FrameworkElement view;
        ScrollViewer scroll;
        public FrameworkElement View
		{
			get { return (Child as ScrollViewer).Content as FrameworkElement ; }
			set
			{
				
				(Child as ScrollViewer).Content =view= value;
        	 
    		}
            
		}
        
		public WPFShemaContainer()
		{
            this.Initialize();
		}

       
        private void Initialize()
        {
            Child = scroll = new ScrollViewer();
			System.Windows.Automation.AutomationProperties.SetAutomationId(Child, "SchemaCanvas");
			
			Child.SnapsToDevicePixels = true;
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            scroll.CanContentScroll = true;
            
        }

               
    }
}
