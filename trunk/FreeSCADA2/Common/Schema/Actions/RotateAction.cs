using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;


namespace FreeSCADA.Common.Schema.Actions
{
    public class RotateAction : BaseAction
    {
        double minAngle = 0;
        double maxAngle = 360;
        double minChannelValue = 0;
        double maxChannelValue = 100;
        RotateTransform rotate;

        public double MinAngle
        {
            get { return minAngle; }
            set { minAngle = value; }
        }
        public double MaxAngle
        {
            get { return maxAngle; }
            set { maxAngle = value; }
        }
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
            if (isLinked)
            {
                if (isLinked)
                {
                    rotate = (RotateTransform)((TransformGroup)actionedObject.RenderTransform).Children[3];

                }

            }
        }

        delegate void RotateDelegate();
        void Rotate()
        {
			double val = Convert.ToDouble(actionChannel.Value);
			double a = (val - minChannelValue) * (maxAngle - minAngle) / (maxChannelValue - minChannelValue) + minAngle;
            rotate.Angle=a;
        }

        protected override void Execute(object sender, EventArgs e)
        {
            base.Execute(sender, e);
            if (isLinked)
            {
                actionedObject.Dispatcher.Invoke(DispatcherPriority.Normal,new RotateDelegate(Rotate));
            }
        }
    }
}
