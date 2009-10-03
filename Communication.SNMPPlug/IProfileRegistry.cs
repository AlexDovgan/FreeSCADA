using System;
using System.Collections.Generic;
using System.Net;

namespace FreeSCADA.Communication.SNMPPlug
{
    internal interface IProfileRegistry
    {
        SNMPAgent DefaultProfile { get; set; }
        IEnumerable<SNMPAgent> Profiles { get; }
        void AddProfile(SNMPAgent profile);
        void DeleteProfile(IPEndPoint profile);
        void ReplaceProfile(SNMPAgent agentProfile);
        void LoadProfiles();
        void SaveProfiles();
        event EventHandler<EventArgs> OnChanged;
    }
}