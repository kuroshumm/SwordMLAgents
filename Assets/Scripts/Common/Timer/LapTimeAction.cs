using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LapTimeAction
{
    public float lapTime;
    public Action callback;

    public LapTimeAction(float lapTime, Action callback)
    {
        this.lapTime = lapTime;
        this.callback = callback;
    }

    public bool Execute(float timer)
    {
        if (timer < lapTime) return false;

        callback?.Invoke();
        return true;
    }
}