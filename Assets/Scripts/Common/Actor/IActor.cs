using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActor : IObject, IComponent
{
    int ID => -1;

    T AddIComponent<T>()
        where T : MonoBehaviour, IComponent;
    
    T GetIComponent<T>()
        where T : IComponent;
}
