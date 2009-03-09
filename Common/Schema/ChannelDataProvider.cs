using System.Windows.Data;

namespace FreeSCADA.Common.Schema
{
    /// <summary>
    /// class for providing channel objects by channel name in process of xaml loading
    /// </summary>
    public class ChannelDataProvider : DataSourceProvider
    {
        string channelName;
        Interfaces.IChannel channel;
        public bool IsBindInDesign
        {
            get;
            set;
        }
        public Interfaces.IChannel Channel
        {
            get{Refresh();return channel;}
           
        }
        public string ChannelName
        {
            get { return channelName; }
            set
            {
                if (value == channelName)
                    return;
                channelName = value;
                base.OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("ChannelName"));
            }
        }
        public ChannelDataProvider()
        {

        }
        protected override void BeginQuery()
        {
            if (ChannelName.Equals(string.Empty))
                return;
            FreeSCADA.Interfaces.IChannel ch;
            ch=Env.Current.CommunicationPlugins.GetChannel(ChannelName);
            channel = ch;
            if (Env.Current.Mode == FreeSCADA.Interfaces.EnvironmentMode.Designer)
                ch=null;
            base.OnQueryFinished(ch);
        }
    }
}
