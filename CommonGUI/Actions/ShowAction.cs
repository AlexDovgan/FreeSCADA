using System;
using System.Windows.Threading;

namespace FreeSCADA.Common.Schema.Actions
{
    public class ShowAction : BaseAction
    {
        double minChannelValue = 0;
        double maxChannelValue = 100;


        public double MinChannelValue
        {
            get { return minChannelValue; }
            set { minChannelValue = value; }
        }
        public double MaxChannelValue
        {
            get { return maxChannelValue; }
            set { maxChannelValue = value; }
        }


        protected override void PrepareExecute()
        {
            base.PrepareExecute();
            
        }

        delegate void ShowDelegate();
        void Show()
        {

            double val = Math.Max(MinChannelValue, Convert.ToDouble(actionChannel.Value));
            val = Math.Min(Convert.ToDouble(actionChannel.Value), MaxChannelValue);
            double show = (val - MinChannelValue) / (MaxChannelValue - MinChannelValue);
            actionedObject.Opacity = show;
        }

        protected override void Execute(object sender, EventArgs e)
        {
            base.Execute(sender, e);
            if (isLinked)
            {
                actionedObject.Dispatcher.Invoke(DispatcherPriority.Normal, new ShowDelegate(Show));
            }
        }
    }

}
