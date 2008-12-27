using System;

namespace FreeSCADA.Communication.Timers
{
    class AbsoluteTimerChannel : FreeSCADA.Common.BaseChannel
	{
		public AbsoluteTimerChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(DateTime))
		{
		}

        public override void DoUpdate()
		{
            DoUpdate(DateTime.Now);
		}
	}
}
