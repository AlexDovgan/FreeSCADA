using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSCADA.Common.Schema.Actions
{
    public class MoveAction:BaseAction
    {
        double minMove = 0;
        double maxMove = 100;
        double minChannelValue = 0;
        double maxChannelValue = 100;


        public double MinMove
        {
            get { return minMove; }
            set { minMove = value; }
        }
        public double MaxMove
        {
            get { return maxMove; }
            set { maxMove = value; }
        }
        public double MinChannelValue
        {
            get { return minChannelValue; }
            set { minChannelValue = value; }
        }
        public double MaxXChannelValue
        {
            get { return maxChannelValue; }
            set { maxChannelValue = value; }
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
