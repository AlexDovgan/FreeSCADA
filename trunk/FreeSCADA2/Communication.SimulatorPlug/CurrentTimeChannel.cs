using System;

namespace FreeSCADA.Communication.SimulatorPlug
{
	class CurrentTimeChannel : ChannelBase
	{
		public CurrentTimeChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(DateTime))
		{
		}

		public override void DoUpdate()
		{
			InternalSetValue(DateTime.Now);
		}
	}
}
