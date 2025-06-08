using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColBoxSize : CollisionSizeBase
{
    public Vector3 Center;
    public Vector3 Size;

    public ColBoxSize(Vector3 center, Vector3 size)
    {
        Center = center;
        Size = size;
    }
}
