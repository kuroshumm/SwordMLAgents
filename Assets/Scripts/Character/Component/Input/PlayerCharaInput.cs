using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Define.Client;

public class PlayerCharaInput : ICharaInput
{
    //---------------------------------
    // ����J �ϐ�
    //---------------------------------
    IUnityActor _owner;
    private CharaSensor _charaSensor;

    InputData _inputData;
    private float _inputHorizontal;
    private float _inputVertical;

    public PlayerCharaInput(IUnityActor owner)
    {
        _owner = owner;
        _charaSensor = _owner.GetIComponent<CharaSensor>();

        _inputData = new InputData();
    }

    //---------------------------------
    // CharaComponentBase �֐�
    //---------------------------------
    public void Enter()
    {
           
    }
    public InputData Execute(float deltaTime)
    {
        _inputData.Clear();

        _charaSensor.Raycast(null);
        var array = _charaSensor.isHitRaycast;

        _inputData.actionID = InputAction();
        if(_inputData.actionID == ActionID.None)
        {
            _inputData.moveDir = InputMove();
        }

        return _inputData;
    }

    public void Exit()
    {
    }

    public int GetCurrentState()
    {
        return -1;
    }

    //---------------------------------
    // ����J �֐�
    //---------------------------------
    Vector3 InputMove()
    {
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        // �J�����̕�������AX-Z���ʂ̒P�ʃx�N�g�����擾
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �����L�[�̓��͒l�ƃJ�����̌�������A�ړ�����������
        Vector3 moveForward = cameraForward * _inputVertical + Camera.main.transform.right * _inputHorizontal;

        return moveForward;
    }

    ActionID InputAction()
    {
        ActionID actionID = ActionID.None;
        
        if (Input.GetMouseButton(1))
        {
            actionID = ActionID.DEFENCE;
        }
        if (Input.GetMouseButtonUp(1))
        {
            actionID = ActionID.DEFENCE_END;
        }
        if (Input.GetMouseButtonDown(0))
        {
            actionID = ActionID.ATTACK;
        }
        return actionID;
    }
}
