using Define.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class CollisionGenerator : Singleton<CollisionGenerator>
{
    public T Generate<T>(CollisionData data, Action<IObject, int> onStart)
        where T : CollisionBase
    {
        T col = AssetManager.Instance.InstantiateWithAttach<T>("Prefabs/Collision");
        col.Init(onStart, data.ownerId, data.objType, data.shape);

        return col;
    }

    public void Setup(GameObject colObj, CollisionData data, Transform parent) 
    {
        Collider collider = null;
        switch (data.shape)
        {
            case CollisionShape.Sphere:
                collider = CreateSphereCollider(colObj, data);
                break;
            case CollisionShape.Box:
                collider = CreateBoxCollider(colObj, data);
                break;
        }
        collider.isTrigger = true;

        Rigidbody rb = colObj.AddComponent<Rigidbody>();
        rb.useGravity = false;

        colObj.transform.SetParent(parent);
        colObj.transform.localRotation = Quaternion.identity;
        colObj.transform.localPosition = Vector3.zero;
        colObj.transform.localPosition += data.offset;
    }

    Collider CreateBoxCollider(GameObject colObj, CollisionData data)
    {
        var col = colObj.AddComponent<BoxCollider>();
        ColBoxSize size = data.size as ColBoxSize;
        col.center = size.Center;
        col.size = size.Size;

        return col;
    }

    Collider CreateSphereCollider(GameObject colObj, CollisionData data)
    {
        var col = colObj.AddComponent<SphereCollider>();
        ColSphereSize size = data.size as ColSphereSize;
        col.radius = size.Radius;

        return col;
    }
}
