using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public abstract class ComponentBase : MonoBehaviour, IComponent
{
    protected IUnityActor _owner;

    //---------------------------------
    // IComponent ä÷êî
    //---------------------------------
    public virtual void Enter(IUnityActor owner)
    {
        _owner = owner;
    }
    public virtual void Execute(float deltaTime)
    {
    }
    public virtual void Exit()
    {
        _owner = null;
    }
    
}
