﻿using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0xb, serviceName: "bnet.protocol.presence.PresenceService")]
    public class PresenceService : bnet.protocol.presence.PresenceService,IServerService
    {
        protected static readonly Logger Logger = LogManager.CreateLogger();
        public IClient Client { get; set; }

        public override void Subscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.SubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Subscribe()");
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Unsubscribe(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.UnsubscribeRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Unsubscribe()");
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Update(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.UpdateRequest request, System.Action<bnet.protocol.NoData> done)
        {
            Logger.Trace("Update()");
            var builder = bnet.protocol.NoData.CreateBuilder();
            done(builder.Build());
        }

        public override void Query(Google.ProtocolBuffers.IRpcController controller, bnet.protocol.presence.QueryRequest request, System.Action<bnet.protocol.presence.QueryResponse> done)
        {
            throw new System.NotImplementedException();
        }               
    }
}
