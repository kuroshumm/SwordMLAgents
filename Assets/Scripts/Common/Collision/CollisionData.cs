using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define.Client;
using System;

[Serializable]
public class CollisionData
{
    public int uniqueId;
    public int ownerId;
    public ObjType objType;
    public CollisionShape shape;
    public Vector3 offset;
    public CollisionSizeBase size;

    public CollisionData(
        int ownerId,
        ObjType objType,
        CollisionShape shape,
        Vector3 offset,
        CollisionSizeBase size)
    {
        uniqueId = IDGenerator.Instance.Generate(IDType.Collision);

        this.ownerId = ownerId;
        this.objType = objType;
        this.shape = shape;
        this.offset = offset;
        this.size = size;
    }
}