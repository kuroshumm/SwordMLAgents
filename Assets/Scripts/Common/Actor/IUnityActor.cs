using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnityActor : IActor
{
    Transform Transform => null;
    Transform Pelvis => null;

    void AddReward(float reward);
    void EndEpisode();
}
