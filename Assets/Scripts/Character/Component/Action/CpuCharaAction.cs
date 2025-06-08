using Define.Client;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CpuCharaAction : ICharaAction
{
    //---------------------------------
    // 非公開 定数
    //---------------------------------
    static readonly float ATTACK_WAIT_TIME = 2f;
    static readonly float DEFENCE_WAIT_TIME = 2f;

    //---------------------------------
    // 非公開 変数
    //---------------------------------
    IUnityActor _owner;
    private CharaAnimator _charaAnimator;
    private CharaInput _charaInput;
    private CharaCollision _charaCollision;

    private Timer _attackWaitTimer;
    private Timer _defenceWaitTimer;
    
    //---------------------------------
    // コンストラクタ
    //---------------------------------
    public CpuCharaAction(IUnityActor owner)
    {
        _owner = owner;
        _charaAnimator = _owner.GetIComponent<CharaAnimator>();
        _charaInput = _owner.GetIComponent<CharaInput>();
        _charaCollision = _owner.GetIComponent<CharaCollision>();

        _attackWaitTimer = new Timer(ATTACK_WAIT_TIME);
        _defenceWaitTimer = new Timer(DEFENCE_WAIT_TIME);
    }

    //---------------------------------
    // ICharaAction 関数
    //---------------------------------
    public void Execute(float deltaTime)
    {
        _attackWaitTimer.UpdateTimer(deltaTime);
        _defenceWaitTimer.UpdateTimer(deltaTime);
    }

    public void Attack(float deltaTime, Action<IObject, int> callback)
    {
        if (!_attackWaitTimer.isFinish)
        {
            return;
        }

        bool isSuccess = _charaAnimator.PlayAction(ActionID.ATTACK);
        if (isSuccess)
        {
            _attackWaitTimer.StartTimer();
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
        if (!_defenceWaitTimer.isFinish)
        {
            return;
        }

        bool isSuccess = _charaAnimator.PlayAction(ActionID.DEFENCE);
        if (isSuccess)
        {
            _defenceWaitTimer.StartTimer();
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
