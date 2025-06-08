using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define.Client;
using System;

public class CharaAnimator : ComponentBase
{
    //---------------------------------
    // 変数
    //---------------------------------
    private AnimationEventManager _animEventManager;

    private Animator _animator;
    private ActionID _prevActionID;
    private ActionID _currentActionID;
    private string _currentStateName;

    //---------------------------------
    // プロパティ
    //---------------------------------
    public ActionID currentActionID => _currentActionID;
    public string currentStateName => _currentStateName;
    public float normalizedTime => _animator?.GetCurrentAnimatorStateInfo(0).normalizedTime ?? 0f;

    //---------------------------------
    // override 関数
    //---------------------------------
    public override void Enter(IUnityActor owner)
    {
        _prevActionID = ActionID.None;
        _currentActionID = ActionID.None;

        _owner = owner;
        _animator = _owner.Transform.GetComponentInChildren<Animator>();
        _animEventManager = _owner.GetIComponent<AnimationEventManager>();
    }

    public override void Exit()
    {
    }

    //---------------------------------
    // 関数
    //---------------------------------
    public bool PlayAction(ActionID actionID)
    {
        if (_animator == null) return false;
        
        var stateName = actionID.ToString().ToLower();
        if(!CheckIsPlaySameAnimation(actionID, stateName)) return false;

        if (_currentActionID == ActionID.ATTACK && actionID == ActionID.DEFENCE) return false;
        if (_currentActionID == ActionID.ATTACK && actionID == ActionID.DEFENCE_END) return false;
        if (_currentActionID == ActionID.DEFENCE && actionID == ActionID.ATTACK) return false;
        if (_currentActionID == ActionID.DEFENCE_END && actionID == ActionID.ATTACK) return false;

        _prevActionID = _currentActionID;
        _currentActionID = actionID;

        _currentStateName = stateName;
        _animator.CrossFade(_currentStateName, 0f);

        // アニメーション状態の変更を通知
        _animEventManager.SetCurrentAnimation(_currentStateName);

        return true;
    }

    public void PlayMove(bool isWalk, float runBlend = 0f)
    {
        if (_animator == null) return;

        _animator.SetBool("isWalk", isWalk);
        if (isWalk)
        {
            if(_currentActionID != ActionID.IDLE && _currentActionID != ActionID.WALK)
            {
                PlayAction(ActionID.IDLE);
            }
            _prevActionID = _currentActionID;
            _currentActionID = ActionID.WALK;
            _animator.SetFloat("run", runBlend);
        }
    }

    //---------------------------------
    // 非公開 関数
    //---------------------------------
    bool CheckIsPlaySameAnimation(ActionID actionID, string stateName)
    {
        if (_currentActionID == actionID) return false;

        var state = _animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsName(stateName)) return false;

        return true;
    }
}
