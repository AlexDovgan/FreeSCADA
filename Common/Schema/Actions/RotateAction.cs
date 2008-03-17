using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Threading;

namespace FreeSCADA.Common.Schema.Actions
{
    public class RotateAction : BaseAction
    {
        double minAngle = 0;
        double maxAngle = 360;
        double minChannelValue = 0;
        double maxChannelValue = 100;


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
                actionedObject.RenderTransform = new MatrixTransform(actionedObject.RenderTransform.Value);

            }
        }

        delegate void RotateDelegate();
        void Rotate()
        {

            //MatrixTransform m = (actionedObject.RenderTransform as MatrixTransform).Clone();
            //m.Matrix.Rotate(20);
            double a = (((double)actionChannel.Value) - minChannelValue) * (maxAngle - minAngle) / (maxChannelValue - minChannelValue) + minAngle;
            actionedObject.RenderTransform = new RotateTransform(a);

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
