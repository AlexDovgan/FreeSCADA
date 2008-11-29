using System;
using System.Windows;
using System.Windows.Threading;

namespace FreeSCADA.Common.Schema.Actions
{
    public class ValueAction : BaseAction
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

        delegate void ProgressDelegate();
        void Value()
        {
            System.Windows.Controls.Primitives.RangeBase pb=actionedObject as System.Windows.Controls.Primitives.RangeBase;
            double val = Math.Max(MinChannelValue, Convert.ToDouble(actionChannel.Value));
            val = Math.Min(Convert.ToDouble(actionChannel.Value), MaxChannelValue);
            pb.Value= (val - MinChannelValue) / (MaxChannelValue - MinChannelValue)*(pb.Maximum-pb.Minimum)+pb.Minimum;
            
         }

        protected override void Execute(object sender, EventArgs e)
        {
            base.Execute(sender, e);
            if (isLinked)
            {
                actionedObject.Dispatcher.Invoke(DispatcherPriority.Normal, new ProgressDelegate(Value));
            }
        }


        public override bool CheckActionFor(UIElement obj)
        {
            if (obj is System.Windows.Controls.Primitives.RangeBase)
                return true;

            return false;
        }
    }
}
