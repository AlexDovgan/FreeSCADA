using System;

namespace FreeSCADA.Communication.SimulatorPlug
{
	class SawIntegerChannel : ChannelBase
	{
		static int val = 0;
        static bool up = true;

		public SawIntegerChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(int))
		{
		}

		public override void DoUpdate()
		{
            if (up)
                val++;
            else
                val--;
            if (val == 100)
                up = false;
            if (val == -100)
                up = true;
            InternalSetValue(val);
		}
	}
}
