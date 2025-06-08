using Define.Client;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharaCollision : ComponentBase
{
    //---------------------------------
    // 変数
    //---------------------------------
    Dictionary<int, CollisionManager> _colManageDict;
    Collider _bodyCollision;

    //---------------------------------
    // ComponentBase関数
    //---------------------------------
    public override void Enter(IUnityActor owner)
    {
        _owner = owner;
        _colManageDict = new Dictionary<int, CollisionManager>();
        _bodyCollision = _owner.Transform.GetComponent<CapsuleCollider>();
    }

    public override void Exit()
    {
        ClearCollision();
    }

    //---------------------------------
    // 公開関数
    //---------------------------------
    public void CreateCollision<T>(CollisionData colData, Action<IObject, int> callback = null) 
        where T : CollisionBase
    {
        CollisionManager colManager = new CollisionManager(_owner, colData, callback);
        if (!_colManageDict.ContainsKey(colData.uniqueId))
        {
            _colManageDict.Add(colData.uniqueId, colManager);
        }
        else
        {
            _colManageDict[colData.uniqueId] = colManager;
        }
        
        colManager.Generate<T>();
    }

    public void RemoveCollision(int uniqueId)
    {
        if (_colManageDict.TryGetValue(uniqueId, out var manager))
        {
            manager.Clear();
            _colManageDict.Remove(uniqueId);
        }
    }

    public void ClearCollision()
    {
        foreach (var col in _colManageDict.Values)
        {
            col.Clear();
        }
        _colManageDict.Clear();
    }

    public void SetEnableBodyCollision(bool isEnabled)
    {
        _bodyCollision.enabled = isEnabled;
    }
}