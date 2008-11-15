using FreeSCADA.Common;
namespace FreeSCADA.Communication.SimulatorPlug
{
	class GenericChannel<T> :BaseChannel
	{
		public GenericChannel(string name, bool readOnly, Plugin plugin)
			:base(name,readOnly,plugin, typeof(T))
		{
		}
        public override void DoUpdate()
        {
        }
	}
}
