using System;
using FreeSCADA.Common;
namespace FreeSCADA.Communication.SimulatorPlug
{
	class RandomIntegerChannel : BaseChannel
	{
		static Random rnd = new Random();

		public RandomIntegerChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(int))
		{
		}

        public override void DoUpdate()
		{
			DoUpdate(rnd.Next());
		}
	}
}
