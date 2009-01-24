using FreeSCADA.Common;
namespace FreeSCADA.Communication.SimulatorPlug
{
	class SawIntegerChannel : BaseChannel
	{
		int val = 0;
        bool up = true;

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
            DoUpdate(val);
		}
	}
}
