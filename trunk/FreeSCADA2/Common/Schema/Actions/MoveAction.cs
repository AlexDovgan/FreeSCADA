using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;


namespace FreeSCADA.Common.Schema.Actions
{
    public class MoveAction:BaseAction
    {
        double minChannelValue = 0;
        double maxChannelValue = 100;
        TranslateTransform move;
        
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

                move = (TranslateTransform)((TransformGroup)actionedObject.RenderTransform).Children[4];
            }
        }

        delegate void MoveDelegate();
        void Move()
        {
            if (HelperObject is PathGeometry)
            {
                PathGeometry path = HelperObject as PathGeometry;
                double val = Math.Max(MinChannelValue, Convert.ToDouble(actionChannel.Value));
                val=Math.Min(Convert.ToDouble(actionChannel.Value),MaxChannelValue);
                double progress=(val-MinChannelValue)/(MaxChannelValue-MinChannelValue);
                Point p,t;
                path.GetPointAtFractionLength(progress, out p, out t);

                move.X = p.X - Canvas.GetLeft(actionedObject);
                move.Y = p.Y - Canvas.GetTop(actionedObject); 
 
            }
    
        }

        protected override void Execute(object sender, EventArgs e)
        {
            base.Execute(sender, e);
            if (isLinked)
            {
                actionedObject.Dispatcher.Invoke(DispatcherPriority.Normal, new MoveDelegate(Move));
            }
        }
    }
}
