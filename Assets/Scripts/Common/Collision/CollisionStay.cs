using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionStay : CollisionBase
{
    void OnTriggerStay(Collider other)
    {
        IObject obj = AssetManager.Instance.GetIObject(other.GetInstanceID(), other.gameObject);

        _callback?.Invoke(obj, _ownerId);
    }
}
