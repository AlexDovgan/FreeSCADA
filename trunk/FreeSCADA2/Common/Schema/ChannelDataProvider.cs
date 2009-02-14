using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace FreeSCADA.Common.Schema
{
    /// <summary>
    /// class for providing channel objects by channel name in process of xaml loading
    /// </summary>
    public class ChannelDataProvider : DataSourceProvider
    {
        string channelName;
        public bool IsBindInDesign
        {
            get;
            set;
        }
        public Interfaces.IChannel Channel
        {
            get;
            protected set;
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
            Channel = ch;
            if (Env.Current.Mode == FreeSCADA.Interfaces.EnvironmentMode.Designer)
                ch=null;
            base.OnQueryFinished(ch);
        }
    }
}
