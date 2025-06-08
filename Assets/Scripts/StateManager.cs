using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateManager<TStateType, UTrigger> 
    where TStateType : Enum
    where UTrigger : Enum
{
    //=============================================
    // プライベートクラス・構造体
    //=============================================

    public class StateTransition 
    {
        public TStateType ToState;
        public UTrigger TriggerType;
    }

    //=============================================
    // パブリック変数
    //=============================================

    public Action<TStateType> changeStateCallback;

    //=============================================
    // プライベート変数
    //=============================================

    IState _currentState;
    TStateType _currentStateType;

    Dictionary<TStateType, IState> _stateDict;
    Dictionary<TStateType, List<StateTransition>> _transitionDict;

    //=============================================
    // パブリック関数
    //=============================================

    public StateManager()
    {
        _stateDict = new Dictionary<TStateType, IState>();
        _transitionDict = new Dictionary<TStateType, List<StateTransition>>();
    }

    public void InitState(TStateType stateType)
    {
        changeStateCallback?.Invoke(stateType);

        _currentStateType = stateType;
        _currentState = _stateDict[_currentStateType];
        _currentState.OnEnter();
    }

    /// <summary>
    /// トリガーを実行し、状態遷移を試みます
    /// </summary>
    /// <param name="trigger">実行するトリガー</param>
    public void ExecuteTrigger(UTrigger trigger)
    {
        var transitionList = _transitionDict[_currentStateType];
        foreach (var transition in transitionList)
        {
            if (transition.TriggerType.Equals(trigger))
            {
                ChangeState(transition.ToState);
                return;
            }
        }
    }

    /// <summary>
    /// 遷移条件を登録
    /// </summary>
    /// <param name="_fromState">遷移元の状態</param>
    /// <param name="_toState">遷移先の状態</param>
    /// <param name="_trigger">トリガー</param>
    public void RegistTrans(TStateType _fromState, TStateType _toState, UTrigger _trigger)
    {
        if (!_transitionDict.ContainsKey(_fromState))
        {
            _transitionDict.Add(_fromState, new List<StateTransition>());
        }

        List<StateTransition> stateTransitionList = _transitionDict[_fromState];
        StateTransition stateTransition = stateTransitionList.FirstOrDefault(trans => trans.ToState.Equals(_toState));
        
        if (stateTransition != null) 
        {
            stateTransition.ToState = _toState;
            stateTransition.TriggerType = _trigger;
            return;
        }

        stateTransitionList.Add(new StateTransition { ToState = _toState, TriggerType = _trigger });
    }

    public void RegistState(TStateType _stateType, IState _state)
    {
        if (!_stateDict.ContainsKey(_stateType))
        {
            _stateDict.Add(_stateType, _state);
            return;
        }

        _stateDict[_stateType] = _state;
    }

    /// <summary>
    /// 現在のステートの更新関数を実行する
    /// </summary>
    /// <param name="_deltaTime">デルタタイム</param>
    public void Update(float _deltaTime)
    {
        _currentState.OnUpdate(_deltaTime);
    }

    public TStateType GetCurrentStateType()
    {
        return _currentStateType;
    }

    //=============================================
    // プライベート関数
    //=============================================

    /// <summary>
    /// ステートを遷移
    /// </summary>
    /// <param name="_stateType">遷移先のステート</param>
    private void ChangeState(TStateType _stateType)
    {
        if (_currentState != null) _currentState.OnExit();

        changeStateCallback?.Invoke(_stateType);

        _currentStateType = _stateType;
        _currentState = _stateDict[_currentStateType];
        _currentState.OnEnter();
    }
}

public interface IState
{
    public void OnEnter();

    public void OnUpdate(float _deltaTime);

    public void OnExit();
}



