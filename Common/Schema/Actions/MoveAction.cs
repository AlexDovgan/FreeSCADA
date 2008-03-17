using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSCADA.Common.Schema.Actions
{
    public class MoveAction:BaseAction
    {
        double minXmove = 0;
        double maxXmove = 360;
        double minXChannelValue = 0;
        double maxXChannelValue = 100;


        public double MinXMove
        {
            get { return minXmove; }
            set { minXmove = value; }
        }
        public double MaxXMove
        {
            get { return maxXmove; }
            set { maxXmove = value; }
        }
        public double MinXChannelValue
        {
            get { return minXChannelValue; }
            set { minXChannelValue = value; }
        }
        public double MaxXChannelValue
        {
            get { return maxXChannelValue; }
            set { maxXChannelValue = value; }
        }

   
        protected override void PrepareExecute()
        {
            base.PrepareExecute();
        }
        protected override void Execute(object sender, EventArgs e)
        {
            base.Execute(sender, e);

        }

    }
}
