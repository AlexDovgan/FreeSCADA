using System;

namespace FreeSCADA.Communication.SimulatorPlug
{
    class CurrentTimeChannel : FreeSCADA.Common.BaseChannel
	{
		public CurrentTimeChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(DateTime))
		{
		}

        public override void DoUpdate()
		{
            DoUpdate(DateTime.Now);
		}
	}
}
