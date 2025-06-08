using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define.Client;

public class CollisionBase : MonoBehaviour, IObject
{
    protected Action<IObject, int> _callback;
    protected int _ownerId;
    protected ObjType _objType;
    protected CollisionShape _shape;

    public int ownerId => _ownerId;
    public ObjType objType => _objType;
    public CollisionShape shape => _shape;

    public void Init(Action<IObject, int> callback, int ownerId, ObjType objType, CollisionShape shape)
    {
        _callback = callback;
        _ownerId = ownerId;
        _objType = objType;
        _shape = shape;
    }

    public void SetActiveObj(bool _isActive)
    {
        throw new NotImplementedException();
    }
}
