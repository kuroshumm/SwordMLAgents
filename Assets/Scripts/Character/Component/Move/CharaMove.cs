using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Define.Client;

public class CharaMove : ComponentBase
{
    //---------------------------------
    // îÒåˆäJ ïœêî
    //---------------------------------
    private CharaAnimator _charaAnimator;
    private CharaInput _charaInput;

    private NavMeshAgent _agent;
    private float _moveSpeed;
    private float _moveTurnSpeed;
    private float _turnSpeed;
    private Vector3 _lastPos;

    //---------------------------------
    // CharaComponentBase ä÷êî
    //---------------------------------
    public override void Execute(float deltaTime)
    {
        Move();
    }

    public override void Enter(IUnityActor owner)
    {
        _owner = owner;
        if(_agent == null)
        {
            _agent = _owner.Transform.gameObject.AddComponent<NavMeshAgent>();
        }
        _moveSpeed = 2.0f;
        _moveTurnSpeed = 0.1f;
        _turnSpeed = 180;

        _agent.speed = _moveSpeed;
        //_agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

        _charaAnimator = _owner.GetIComponent<CharaAnimator>();
        _charaInput = _owner.GetIComponent<CharaInput>();
    }

    public override void Exit()
    {
        _agent.speed = 0f;
    }

    //---------------------------------
    // îÒåˆäJ ä÷êî
    //---------------------------------
    private void Move()
    {
        switch (_charaAnimator.currentActionID)
        {
            case ActionID.ATTACK:
            //case ActionID.DEFENCE:
            //case ActionID.DEFENCE_END:
                return;
        }

        Vector3 moveForward = _charaInput.moveDir;
        Vector3 move = moveForward.normalized * _moveSpeed * Time.deltaTime;

        Vector3 turnForward = _charaInput.turnDir;
        Vector3 turn = turnForward.normalized * _turnSpeed * Time.deltaTime;

        if (move != Vector3.zero)
        {
            _charaAnimator.PlayMove(true);
            RotateWithMove();
            //_owner.transform.rotation = Quaternion.LookRotation(moveForward, Vector3.up);
        }
        else
        {
            _charaAnimator.PlayMove(false);
            //_owner.CharaAnimator.Play(ActionID.IDLE);
        }

        _lastPos = transform.position;
        _agent.Move(move);

        if(turn != Vector3.zero)
        {
            //_owner.transform.RotateAround(_owner.transform.position, Vector3.up, turn.x);
            _owner.Transform.Rotate(Vector3.up * turn.x);
        }
    }

    void RotateWithMove()
    {
        Vector3 differenceDis = new Vector3(_owner.Transform.position.x, 0, _owner.Transform.position.z) - new Vector3(_lastPos.x, 0, _lastPos.z);
        _lastPos = transform.position;
        if (Mathf.Abs(differenceDis.x) > 0.001f || Mathf.Abs(differenceDis.z) > 0.001f)
        {
            Quaternion rot = Quaternion.LookRotation(differenceDis);
            rot = Quaternion.Slerp(_owner.Transform.rotation, rot, _moveTurnSpeed);
            _owner.Transform.rotation = rot;
        }
    }
}
