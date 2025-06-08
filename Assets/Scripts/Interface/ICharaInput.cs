using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharaInput
{
    void Enter();
    InputData Execute(float deltaTime);
    void Exit();

    int GetCurrentState();
}
