using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManState : IState
{
    Action _enter;
    Action _exit;
    Action<float> _update;

    public SwordManState(Action enter, Action exit, Action<float> update) 
    {
        _enter = enter;
        _exit = exit;
        _update = update;
    }

    public void OnEnter()
    {
        _enter?.Invoke();
    }

    public void OnExit()
    {
        _exit?.Invoke();
    }

    public void OnUpdate(float _deltaTime)
    {
        _update?.Invoke(_deltaTime);
    }
}
