using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActorBase : MonoBehaviour, IUnityActor
{
    //---------------------------------
    // îÒåˆäJ ïœêî
    //---------------------------------
    protected Dictionary<Type, IComponent> _componentDict = new Dictionary<Type, IComponent>();
    protected Transform _pelvis;
    protected int _id;

    //---------------------------------
    // IUnityActor ä÷êî
    //---------------------------------
    public int ID => _id;
    public Transform Transform => transform;
    public Transform Pelvis => _pelvis;

    public virtual void AddReward(float reward)
    {
    }
    public virtual void SetActiveObj(bool _isActive)
    {
    }
    public virtual void EndEpisode()
    {
    }

    //---------------------------------
    // IActor ä÷êî
    //---------------------------------
    public T AddIComponent<T>() where T : MonoBehaviour, IComponent
    {
        T component;

        Type type = typeof(T);
        if (_componentDict.ContainsKey(type))
        {
            component = (T)_componentDict[type];
            component.Enter(this);
            return component;
        }

        component = gameObject.AddComponent<T>();
        component.Enter(this);
        _componentDict[type] = component;

        return component;
    }

    public T GetIComponent<T>() where T : IComponent
    {
        if (_componentDict.TryGetValue(typeof(T), out IComponent component))
        {
            return (T)component;
        }

        throw new KeyNotFoundException($"{typeof(T).Name} ÇÕìoò^Ç≥ÇÍÇƒÇ¢Ç‹ÇπÇÒÅB");
    }

    //---------------------------------
    // IComponent ä÷êî
    //---------------------------------
    public void Enter(IUnityActor owner)
    {
        _pelvis = transform.Find("mixamorig:Hips");
    }
    public virtual void Execute(float deltaTime)
    {
        foreach (var component in _componentDict.Values)
        {
            component.Execute(deltaTime);
        }
    }
    public virtual void Exit()
    {
        foreach (var component in _componentDict.Values)
        {
            component.Exit();
        }
    }
}
