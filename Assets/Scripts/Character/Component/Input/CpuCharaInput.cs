using Define.Client;
using Google.Protobuf;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.MLAgents;
using UnityEngine;
using UnityEngine.AI;

public class CpuCharaInput : ICharaInput
{
    //---------------------------------
    // îÒåˆäJ íËêî
    //---------------------------------
    static readonly float WALK_WAIT_TIME = 1f;

    //---------------------------------
    // îÒåˆäJ ïœêî
    //---------------------------------
    IUnityActor _owner;
    private CharaSensor _charaSensor;

    protected StateManager<StateType, TriggerType> _stateManager;
    private LapTimer _walkWaitTimer;
    private InputData _inputData;

    private Vector3 _targetPos;
    private NavMeshPath _path;
    private Vector3 _targetPathPos;
    private int _pathIndex;

    public CpuCharaInput(IUnityActor owner)
    {
        _owner = owner;
        _charaSensor = _owner.GetIComponent<CharaSensor>();

        _inputData = new InputData();
    }

    //---------------------------------
    // ICharaInput ä÷êî
    //---------------------------------
    public void Enter()
    {
        InitStateMachine();
        _walkWaitTimer = new LapTimer(new LapTimeAction(WALK_WAIT_TIME, null));
    }

    public InputData Execute(float deltaTime)
    {
        _inputData.Clear();

        _stateManager.Update(deltaTime);
     
        return _inputData;
    }

    public void Exit()
    {
        _inputData.Clear();
        _stateManager.InitState(StateType.Search);
    }

    public int GetCurrentState()
    {
        return (int)_stateManager.GetCurrentStateType();
    }

    //---------------------------------
    // îÒåˆäJ ä÷êî
    //---------------------------------
    void InitStateMachine()
    {
        _stateManager = new StateManager<StateType, TriggerType>();
        SwordManState searchState = new SwordManState(
                enter: SearchEnter,
                exit: null,
                update: SearchUpdate
            );
        SwordManState attackState = new SwordManState(
                enter: AttackEnter,
                exit: AttackExit,
                update: AttackUpdate
            );
        SwordManState chaceState = new SwordManState(
                enter: ChaceEnter,
                exit: null,
                update: ChaceUpdate
            );
        _stateManager.RegistState(StateType.Search, searchState);
        _stateManager.RegistState(StateType.Attack, attackState);
        _stateManager.RegistState(StateType.Chase, chaceState);

        _stateManager.RegistTrans(StateType.Search, StateType.Chase, TriggerType.Chase);
        _stateManager.RegistTrans(StateType.Chase, StateType.Attack, TriggerType.Attack);
        _stateManager.RegistTrans(StateType.Attack, StateType.Chase, TriggerType.Chase);
        _stateManager.RegistTrans(StateType.Attack, StateType.Search, TriggerType.Search);

        _stateManager.InitState(StateType.Search);
    }
    void SearchEnter()
    {
        _path = new NavMeshPath();
        _pathIndex = 0;
        _targetPos = Vector3.zero;
    }

    void SearchUpdate(float deltaTime)
    {
        Explore(deltaTime);
        _charaSensor.Raycast(
            ()=>
            {
                _stateManager.ExecuteTrigger(TriggerType.Chase);
            }
        );
    }

    void AttackEnter()
    {
        _targetPos = Vector3.zero;
        _path = null;
        _pathIndex = 0;
    }
    void AttackUpdate(float deltaTime)
    {
        _charaSensor.Raycast(null);

        _inputData.actionID = ActionID.ATTACK;
        if(_owner.GetIComponent<CharaAnimator>().currentActionID == ActionID.DEFENCE)
        {
            _inputData.actionID = ActionID.DEFENCE_END;
        }

        CharaAnimator targetAnimator = _charaSensor.targetChara.GetIComponent<CharaAnimator>();
        if (targetAnimator.currentActionID == ActionID.ATTACK)
        {
            _inputData.actionID = ActionID.DEFENCE;
        }

        _targetPos = _charaSensor.targetChara.Transform.position;

        Vector3 moveDir = (_targetPos - _owner.Transform.position).normalized;
        _owner.Transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);

        if (BattleUtil.Instance.CalcDistanceXZ(_owner.Transform.position, _targetPos) > 3.0f)
        {
            _stateManager.ExecuteTrigger(TriggerType.Chase);
        }
    }
    void AttackExit()
    {
    }

    void ChaceEnter()
    {
        _targetPos = Vector3.zero;
        _path = null;
        _pathIndex = 0;
    }
    void ChaceUpdate(float deltaTime)
    {
        MoveToTarget();
        _charaSensor.Raycast(null);

        if (BattleUtil.Instance.CalcDistanceXZ(_owner.Transform.position, _targetPos) < 1.0f)
        {
            _stateManager.ExecuteTrigger(TriggerType.Attack);
        }
        if (BattleUtil.Instance.CalcDistanceXZ(_owner.Transform.position, _targetPos) > 3.0f)
        {
            //_stateManager.ExecuteTrigger(TriggerType.Search);
        }
    }

    void Explore(float deltaTime)
    {
        if (_targetPos == Vector3.zero)
        {
            if (!_walkWaitTimer.UpdateTimer(deltaTime))
            {
                return;
            }

            _path = new NavMeshPath();
            _pathIndex = 0;

            Vector3 initPos = (_owner as CharaBase).initPos;
            BattleUtil.Instance.RandomPoint(initPos, 10f, out _targetPos);
            bool isFinidh = NavMesh.CalculatePath(_owner.Transform.position, _targetPos, NavMesh.AllAreas, _path);
        }

        MoveToPath();
    }
    
    void MoveToTarget()
    {
        if (_targetPos == Vector3.zero)
        {
            _path = new NavMeshPath();
            _pathIndex = 0;

            _targetPos = _charaSensor.targetChara.Transform.position;
            NavMesh.CalculatePath(_owner.Transform.position, _targetPos, NavMesh.AllAreas, _path);
        }

        MoveToPath();
    }

    void MoveToPath()
    {
        if (_path.corners.Length > 0)
        {
            Vector3 targetPath = _path.corners[_pathIndex];
            _inputData.moveDir = (targetPath - _owner.Transform.position).normalized;
            if (BattleUtil.Instance.CalcDistanceXZ(_owner.Transform.position, targetPath) < 0.5f)
            {
                _pathIndex++;
            }
        }

        if (BattleUtil.Instance.CalcDistanceXZ(_owner.Transform.position, _targetPos) < 0.5f)
        {
            _path = new NavMeshPath();
            _pathIndex = 0;
            _targetPos = Vector3.zero;

            _walkWaitTimer.StartTimer();
        }
    }
}
