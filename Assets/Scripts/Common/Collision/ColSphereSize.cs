using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColSphereSize : CollisionSizeBase
{
    public float Radius;

    public ColSphereSize(float radius)
    {
        Radius = radius;
    }
}
