using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define.Client;
using TMPro;

public class CharaInput : ComponentBase
{
    //---------------------------------
    // ����J �ϐ�
    //---------------------------------
    private ICharaInput _charaInput;

    protected Vector3 _moveDir;
    protected Vector3 _turnDir;
    protected ActionID _actionID;

    protected bool _isDisableInput;
    protected LapTimer _disableInputTimer;

    //---------------------------------
    // �v���p�e�B
    //---------------------------------
    public Vector3 moveDir => _moveDir;
    public Vector3 turnDir => _turnDir;
    public ActionID actionID => _actionID;

    public void SetCharaInput(ICharaInput charaInput)
    {
        _charaInput = charaInput;
        _charaInput.Enter();
    }

    //---------------------------------
    // CharaComponentBase �֐�
    //---------------------------------
    public override void Enter(IUnityActor owner)
    {
        _owner = owner;

        _disableInputTimer = new LapTimer();
    }

    public override void Execute(float deltaTime)
    {
        if (!UpdateDisableInput(deltaTime)) return;

        InputData inputData = _charaInput.Execute(deltaTime);
        _moveDir = inputData.moveDir;
        _turnDir = inputData.turnDir;
        _actionID = inputData.actionID;
    }

    public override void Exit()
    {
        _charaInput.Exit();
    }

    //---------------------------------
    // CharaInput �֐�
    //---------------------------------
    protected bool UpdateDisableInput(float deltaTime)
    {
        bool isFinish = _disableInputTimer.UpdateTimer(deltaTime);

        return isFinish;
    }

    public virtual void SetAction(ActionID actionID)
    {
        _actionID = actionID;
    }

    public int GetCurrentState()
    {
        return _charaInput.GetCurrentState();
    }

    //---------------------------------
    // ���J �֐�
    //---------------------------------
    public void ClearInput()
    {
        ClearInputMove();
        ClearInputAction();
    }

    public void ClearInputMove()
    {
        _moveDir = Vector3.zero;
        _turnDir = Vector3.zero;
    }

    public void ClearInputAction()
    {
        _actionID = ActionID.None;
    }

    public void StartDisableInput(float time)
    {
        ClearInputMove();
        _isDisableInput = true;
        _disableInputTimer.StartTimer(new LapTimeAction(time, FinishDisableInput));
    }

    public void FinishDisableInput()
    {
        _isDisableInput = false;
    }
}

public class InputData
{
    public Vector3 moveDir;
    public Vector3 turnDir;
    public ActionID actionID;

    public InputData()
    {
        Clear();
    }

    public void Clear()
    {
        moveDir = Vector3.zero;
        turnDir = Vector3.zero;
        actionID = ActionID.None;
    }
}