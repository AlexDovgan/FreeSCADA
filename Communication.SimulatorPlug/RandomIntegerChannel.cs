using System;

namespace FreeSCADA.Communication.SimulatorPlug
{
	class RandomIntegerChannel : ChannelBase
	{
		static Random rnd = new Random();

		public RandomIntegerChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(int))
		{
		}

		public override void DoUpdate()
		{
			InternalSetValue(rnd.Next());
		}
	}
}
