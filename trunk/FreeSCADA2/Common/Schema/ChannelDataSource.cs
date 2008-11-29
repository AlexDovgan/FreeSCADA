using FreeSCADA.ShellInterfaces;

namespace FreeSCADA.Common.Schema
{
    public class ChannelDataSource
    {
        
        public string ChannelName
        {
            get;
            set;
        }
        public ChannelDataSource()
        {
        }
        public IChannel GetChannel()
        {
            if (!ChannelName.Equals(string.Empty))
                return Env.Current.CommunicationPlugins.GetChannel(ChannelName);
            else
                return null;
        }

    }
   
}
