using System;
using System.Timers;

namespace FreeSCADA.Communication.Timers
{
    class RelativeTimerChannel : FreeSCADA.Common.BaseChannel
	{
        Timer timer;

        public double Interval
        {
            get { return timer.Interval; }
        }

		public RelativeTimerChannel(string name, Plugin plugin, double interval)
			: base(name, true, plugin, typeof(DateTime))
		{
            timer = new Timer();
            timer.Enabled = false;
            timer.AutoReset = true;
            timer.Elapsed +=new ElapsedEventHandler(timer_Elapsed);
            timer.Interval = interval;
		}

        void  timer_Elapsed(object sender, ElapsedEventArgs e)
        {
 	        DoUpdate();
        }

        public override void DoUpdate()
		{
            DoUpdate(DateTime.Now);
		}

        public bool Enable {
            get { return timer.Enabled; }
            set { timer.Enabled = value; }
        }
	}
}
