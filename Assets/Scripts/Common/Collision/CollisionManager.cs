using System;
using UnityEngine;

using Define.Client;

public class CollisionManager
{
    public IUnityActor owner;
    public CollisionData data;
    public GameObject colObj;

    private Action<IObject, int> _onStart;

    public CollisionManager(
        IUnityActor owner,
        CollisionData data,
        Action<IObject, int> onStart)
    {
        this.owner = owner;
        this.data = data;
        _onStart = onStart;
    }

    public void Generate<T>() where T : CollisionBase
    {
        T collision = CollisionGenerator.Instance.Generate<T>(data, _onStart);
        colObj = collision.gameObject;

        CollisionGenerator.Instance.Setup(colObj, data, owner.Transform);
    }

    public void Clear()
    {
        if (colObj != null)
        {
            BattleUtil.Instance.DestroyObj(colObj);
            colObj = null;
        }
    }
}

