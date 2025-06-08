using Define.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerCharaAction : ICharaAction
{
    //---------------------------------
    // ����J �萔
    //---------------------------------

    //---------------------------------
    // ����J �ϐ�
    //---------------------------------
    IUnityActor _owner;
    private CharaAnimator _charaAnimator;
    private CharaInput _charaInput;
    private CharaCollision _charaCollision;

    //---------------------------------
    // �R���X�g���N�^
    //---------------------------------
    public PlayerCharaAction(IUnityActor owner)
    {
        _owner = owner;

        _charaAnimator = _owner.GetIComponent<CharaAnimator>();
        _charaInput = _owner.GetIComponent<CharaInput>();
        _charaCollision = _owner.GetIComponent<CharaCollision>();
    }

    //---------------------------------
    // ICharaAction �֐�
    //---------------------------------
    public void Execute(float deltaTime)
    {
    }

    public void Attack(float deltaTime, Action<IObject, int> callback)
    {
        bool isSuccess = _charaAnimator.PlayAction(ActionID.ATTACK);
        if (isSuccess)
        {
           
        }
    }

    public void Damage(float deltaTime)
    {
        bool isSuccess = _charaAnimator.PlayAction(ActionID.DAMAGE);
        if (isSuccess)
        {
        }
    }

    public void Defence(float deltaTime)
    {
        bool isSuccess = _charaAnimator.PlayAction(ActionID.DEFENCE );
        if (isSuccess)
        {
        }
    }

    public void DefenceEnd(float deltaTime)
    {
        bool isSuccess = _charaAnimator.PlayAction(ActionID.DEFENCE_END);
        if (isSuccess)
        {

        }
    }
}
