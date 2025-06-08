using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents.Actuators;
using Unity.Sentis;
using UnityEngine;

using Define.ML;
using Unity.MLAgents.Sensors;

public class SwordManML : SwordManBase
{
    //---------------------------------------------
    // 非公開 変数
    //---------------------------------------------
    private CharaInput _charaInput;
    private CharaAction _charaAction;
    private CharaParametor _charaParametor;

    private bool _init;
    private float _deathTimer;
    private MLActionID _mlActionId;
    private ActionBuffers _actionBuffers;

    private MLAgentComponent _mlAgent;
    private SentisManager _sentisManager;
    public void SetSentisManager(SentisManager sentis)
    {
        _sentisManager = sentis;
    }

    //---------------------------------------------
    // プロパティ
    //---------------------------------------------
    public ActionBuffers actionBuffers => _actionBuffers;
    public MLAgentComponent mlAgent => _mlAgent;
    public bool isMLAgents => _sentisManager.isMLAgent;

    //---------------------------------------------
    // ML-Agents
    //---------------------------------------------
    void OnActionReceived(ActionBuffers actions)
    {
        if (!_init) return;

        _actionBuffers = _sentisManager.SelectActionBuffers(actions);

        Execute(Time.deltaTime);
    }

    void CollectObservations(VectorSensor sensor)
    {
        if (!_init) return;

        float[] inputData = _sentisManager.CalcInputData();

        for (int i = 0, max = inputData.Length; i < max; i++)
        {
            sensor.AddObservation(inputData[i]);
        }
    }

    public MLActionID GetCurrentMLActionID()
    {
        if (_actionBuffers.IsEmpty())
        {
            return MLActionID.None;
        }

        _mlActionId = (MLActionID)_actionBuffers.DiscreteActions[0];
        _actionBuffers.Clear();

        return _mlActionId;
    }

    public void ClearActionBuffers()
    {
        _actionBuffers.Clear();
    }

    //---------------------------------------------
    // CharaBase 関数
    //---------------------------------------------
    public override void Setup(Vector3 pos, CharaID charaID)
    {
        base.Setup(pos, charaID);

        if(_mlAgent == null)
        {
            _mlAgent = GetComponent<MLAgentComponent>();
        }
        _mlAgent.SetCallback(
            OnActionReceived,
            CollectObservations
        );

        AddIComponent<CharaSensor>();

        if (_charaInput == null)
        {
            _charaInput = GetIComponent<CharaInput>();
            _charaInput.SetCharaInput(new MLCharaInput(this));
        }
        if (_charaAction == null)
        {
            _charaAction = GetIComponent<CharaAction>();
            _charaAction.SetAction(new MLCharaAction(this));
        }

        _charaParametor = GetIComponent<CharaParametor>();

        _deathTimer = 0f;
        _init = true;
    }

    public override void Execute(float deltaTime)
    {
        base.Execute(deltaTime);

        _charaInput.ClearInput();
        TimeDeath();
    }

    public override void Exit()
    {
        base.Exit();

        _init = false;
    }

    public override void AddReward(float reward)
    {
        _mlAgent.AddReward(reward);
    }

    public override void EndEpisode()
    {
        _mlAgent.EndEpisode();
    }

    void TimeDeath()
    {
        _deathTimer += Time.deltaTime / 100;
        if(_deathTimer > 10)
        {
            _charaParametor.AddHp(-100);
        }
    }
}
