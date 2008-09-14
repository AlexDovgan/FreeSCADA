using System;

namespace FreeSCADA.Communication.SimulatorPlug
{
    class SinusDoubleChannel : ChannelBase
	{
        static double angle = 0;
        static double delta = 2*Math.PI/360.0;

		public SinusDoubleChannel(string name, Plugin plugin)
			: base(name, true, plugin, typeof(double))
		{
		}

		public override void DoUpdate()
		{
            angle += delta;
            InternalSetValue(Math.Sin(angle));
		}
	}
}
