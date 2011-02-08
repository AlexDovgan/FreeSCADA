using FreeSCADA.Interfaces;

namespace FreeSCADA.Common.Schema
{
    ///TDOD: substitute this solution on custom dtata provider 
    ///this code is need for complicated with old schemas
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
