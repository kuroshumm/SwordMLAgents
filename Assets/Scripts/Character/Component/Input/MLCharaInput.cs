using Define.Client;
using Define.ML;
using Unity.MLAgents.SideChannels;
using UnityEngine;

public class MLCharaInput : ICharaInput
{
    //---------------------------------
    // 非公開 変数
    //---------------------------------
    private SwordManML _ownerML;
    private CharaSensor _charaSensor;

    protected StateManager<StateType, TriggerType> _stateManager;
    protected AgentSideChannel _sideChannel;
    private InputData _inputData;

    private Vector3 _targetPos;

    public MLCharaInput(IUnityActor owner)
    {
        _ownerML = owner as SwordManML;
        _charaSensor = owner.GetIComponent<CharaSensor>();

        _inputData = new InputData();
    }

    //---------------------------------
    // CharaComponentBase 関数
    //---------------------------------
    public void Enter()
    {
        if (_sideChannel == null)
        {
            _sideChannel = new AgentSideChannel();
            SideChannelManager.RegisterSideChannel(_sideChannel);
        }
        InitStateMachine();
    }

    public InputData Execute(float deltaTime)
    {
        _inputData.Clear();

        _stateManager.Update(deltaTime);

        return _inputData;
    }

    public void Exit()
    {
        _stateManager.InitState(StateType.Search);
    }

    public int GetCurrentState()
    {
        return (int)_stateManager.GetCurrentStateType();
    }

    //---------------------------------
    // ����J �֐�
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

        _stateManager.changeStateCallback = (stateType) => _sideChannel.Send((int)stateType);

        _stateManager.InitState(StateType.Search);
    }
    void SearchEnter()
    {
        _targetPos = Vector3.zero;
    }

    void SearchUpdate(float deltaTime)
    {
        MLActionID mlActionID = _ownerML.GetCurrentMLActionID();
        var initValue = CalcInitMoveValue(mlActionID);

        _inputData.moveDir = initValue.move;
        _inputData.turnDir = initValue.turn;
        if (_ownerML.isMLAgents)
        {
            _inputData.actionID = CalcInitActionValue(mlActionID);
        }

        _charaSensor.Raycast(
            () =>
            {
                _stateManager.ExecuteTrigger(TriggerType.Chase);
                _ownerML.AddReward(1f);
            }
        );
    }

    void AttackEnter()
    {
    }
    void AttackUpdate(float deltaTime)
    {
        MLActionID mlActionID = _ownerML.GetCurrentMLActionID();
        var initValue = CalcInitMoveValue(mlActionID);

        _inputData.moveDir = initValue.move;
        _inputData.turnDir = initValue.turn;
        _inputData.actionID = CalcInitActionValue(mlActionID);

        _charaSensor.Raycast(null);

        _targetPos = _charaSensor.targetChara.Transform.position;
        if (BattleUtil.Instance.CalcDistanceXZ(_ownerML.transform.position, _targetPos) > 3.0f)
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
    }
    void ChaceUpdate(float deltaTime)
    {
        MLActionID mlActionID = _ownerML.GetCurrentMLActionID();
        var initValue = CalcInitMoveValue(mlActionID);

        _inputData.moveDir = initValue.move;
        _inputData.turnDir = initValue.turn;
        if (_ownerML.isMLAgents)
        {
            _inputData.actionID = CalcInitActionValue(mlActionID);
        }

        _charaSensor.Raycast(null);
        
        _targetPos = _charaSensor.targetChara.Transform.position;
        if (BattleUtil.Instance.CalcDistanceXZ(_ownerML.transform.position, _targetPos) < 3.0f)
        {
            _stateManager.ExecuteTrigger(TriggerType.Attack);
            _ownerML.AddReward(1f);
        }
    }

    (Vector3 move, Vector3 turn) CalcInitMoveValue(MLActionID mlActionID)
    {
        float moveVertical = 0;
        float moveHorizontal = 0;
        float turnHorizontal = 0;
        switch (mlActionID)
        {
            case MLActionID.Left:
                moveHorizontal = -1;
                break;
            case MLActionID.Right:
                moveHorizontal = 1;
                break;
            case MLActionID.Up:
                moveVertical = 1;
                break;
            case MLActionID.Down:
                moveVertical = -1;
                break;
            case MLActionID.RightRot:
                turnHorizontal = 1;
                break;
            case MLActionID.LeftRot:
                turnHorizontal = -1;
                break;
        }
        Vector3 moveForward = _ownerML.transform.forward * moveVertical + _ownerML.transform.right * moveHorizontal;

        Vector3 turnFoward = new Vector3(turnHorizontal, 0f, 0f);

        return (moveForward, turnFoward);
    }

    ActionID CalcInitActionValue(MLActionID mLActionID)
    {
        ActionID actionID = ActionID.None;
        switch (mLActionID)
        {
            case MLActionID.Attack:
                actionID = ActionID.ATTACK;
                break;
            case MLActionID.Defence:
                actionID = ActionID.DEFENCE;
                break;
        }

        return actionID;
    }
}
