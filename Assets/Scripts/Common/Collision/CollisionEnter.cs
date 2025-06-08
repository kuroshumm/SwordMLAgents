using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEnter : CollisionBase
{
    void OnTriggerEnter(Collider other)
    {
        IObject obj = AssetManager.Instance.GetIObject(other.GetInstanceID(), other.gameObject);
        //var chara = obj as CharaBase;
        //if(chara == null) return;

        _callback?.Invoke(obj, _ownerId);
    }
}
