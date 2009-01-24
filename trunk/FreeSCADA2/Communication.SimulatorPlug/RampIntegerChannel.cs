using FreeSCADA.Common;
namespace FreeSCADA.Communication.SimulatorPlug
{
	class RampIntegerChannel : BaseChannel
	{
		int val = 0;

		public RampIntegerChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(int))
		{
		}

		public override void Reset()
		{
			val = 0;
			base.Reset();
		}
        public override  void DoUpdate()
		{
            val++;
            if (val == 101)
                val = 0;
            DoUpdate(val);
		}
	}
}
