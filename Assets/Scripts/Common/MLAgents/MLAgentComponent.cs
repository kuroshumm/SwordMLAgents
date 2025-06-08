using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System;
using Unity.MLAgents.Sensors;

public class MLAgentComponent : Agent
{
    Action<ActionBuffers> _onActionReceive;
    Action<VectorSensor> _collectObservations;

    public void SetCallback(Action<ActionBuffers> onActionReceive, Action<VectorSensor> collectObservations)
    {
        _onActionReceive = onActionReceive;
        _collectObservations = collectObservations;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        _onActionReceive?.Invoke(actions);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        _collectObservations?.Invoke(sensor);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }
}
