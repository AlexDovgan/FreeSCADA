using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSCADA.Communication.SimulatorPlug
{
	class RandomIntegerChannel : ChannelBase
	{
		static Random rnd = new Random();

		public RandomIntegerChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(int), InternalChannelType.RandomInteger)
		{
		}

		public override void DoUpdate()
		{
			InternalSetValue(rnd.Next());
		}
	}
}
