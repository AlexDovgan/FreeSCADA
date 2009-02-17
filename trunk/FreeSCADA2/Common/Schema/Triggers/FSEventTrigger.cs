using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;

namespace FreeSCADA.Common.Schema.Triggers
{
    public class FSEventTrigger:EventTrigger
    {
        public FSEventTrigger()
        {
        }
        protected override void AddChild(object value)
        {
            base.AddChild(value);
        }
        
    }
    public class ChannelSetterAction
    {
        ChannelSetterAction()
        {
        }
    }
}
