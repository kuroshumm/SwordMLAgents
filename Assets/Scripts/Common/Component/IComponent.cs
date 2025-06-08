using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponent
{
    void Enter(IUnityActor owner);
    void Execute(float deltaTime);
    void Exit();
}
