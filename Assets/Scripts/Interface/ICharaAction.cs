using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ICharaAction
{
    void Execute(float deltaTime);
    void Attack(float deltaTime, Action<IObject, int> callback);
    void Defence(float deltaTime);
    void DefenceEnd(float deltaTime);
    void Damage(float deltaTime);
}
