
namespace FreeSCADA.Communication.MODBUSPlug
{
    public interface IModbusStation
    {
        string Name { get; }
        void Stop();
        int Start();
        void ClearChannels();
        void AddChannel(ModbusChannelImp channel);
        int CycleTimeout { get; set; }
        int RetryTimeout { get; set; }
        int RetryCount { get; set; }
   }
}
