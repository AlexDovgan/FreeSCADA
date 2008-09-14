using System;

namespace FreeSCADA.Communication.SimulatorPlug
{
	class RampIntegerChannel : ChannelBase
	{
		static int val = 0;

		public RampIntegerChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(int))
		{
		}

		public override void DoUpdate()
		{
            val++;
            if (val == 101)
                val = 0;
            InternalSetValue(val);
		}
	}
}
