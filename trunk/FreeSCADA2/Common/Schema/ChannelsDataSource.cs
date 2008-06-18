using System;
using System.Windows;

using System.Collections.Generic;
using System.Text;
using System.Windows.Data;

namespace FreeSCADA.Common.Schema
{
    class ChannelsDataSource: DataSourceProvider
    {
        string channelName;

        /// <summary>
        /// Gets/sets the path of a resource file 
        /// from which to get text.
        /// </summary>
        public string ChannelName
        {
            get { return this.channelName; }
            set
            {
                if (value == this.channelName)
                    return;

                this.channelName = value;

                //base.OnPropertyChanged(new PropertyChangedEventArgs("ChannelName"));
            }
        }

        protected override void BeginQuery()
        {
            string text = null;

            // Load text out of the resource file.
           
            base.OnQueryFinished(text);
        }

    }
}
