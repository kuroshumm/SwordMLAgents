using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.SideChannels;
using UnityEngine;

public class AgentSideChannel : SideChannel
{
    public AgentSideChannel()
    {
        ChannelId = new System.Guid("621f0a70-4f87-11ea-a6bf-784f4387d1f7");
    }

    protected override void OnMessageReceived(IncomingMessage msg)
    {
        throw new System.NotImplementedException();
    }

    public void Send(int stateType)
    {
        using (var msgOut = new OutgoingMessage())
        {
            msgOut.WriteInt32(stateType);
            QueueMessageToSend(msgOut);
        }
    }
}
