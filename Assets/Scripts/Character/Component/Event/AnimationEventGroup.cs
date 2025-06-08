using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventGroup
{
    public string StateName { get; }
    public List<(EventTiming Timing, float Progress, TimelineEventHandler Handler, object Context)> Events { get; }

    public AnimationEventGroup(string stateName)
    {
        StateName = stateName;
        Events = new List<(EventTiming, float, TimelineEventHandler, object)>();
    }

    public void AddEvent(EventTiming timing, float progress, TimelineEventHandler handler, object context = null)
    {
        Events.Add((timing, progress, handler, context));
    }
}