using System;
using FreeSCADA.Common;
namespace FreeSCADA.Communication.SimulatorPlug
{
    class SinusDoubleChannel : BaseChannel
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
            DoUpdate(Math.Sin(angle));
		}
	}
}
