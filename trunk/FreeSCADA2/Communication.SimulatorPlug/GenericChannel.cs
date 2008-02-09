
namespace FreeSCADA.Communication.SimulatorPlug
{
	class GenericChannel<T> :ChannelBase
	{
		public GenericChannel(string name, bool readOnly, Plugin plugin)
			:base(name,readOnly,plugin, typeof(T), InternalChannelType.SimpleGeneric)
		{
		}
	}
}
