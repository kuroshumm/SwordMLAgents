using Define.Client;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents.Actuators;
using Unity.Sentis;
using UnityEngine;

public class SentisManager : MonoBehaviour
{
    //---------------------------------------------
    // Serialized Field
    //---------------------------------------------
    [SerializeField] private ModelAsset _modelSearchAsset;
    [SerializeField] private ModelAsset _modelChaceAsset;
    [SerializeField] private ModelAsset _modelAttackAsset;

    [SerializeField] private bool _isSentis;
    [SerializeField] private bool _isMLAgent;

    //---------------------------------------------
    // 非公開 変数
    //---------------------------------------------
    private IWorker _searchEngine;
    private IWorker _chaceEngine;
    private IWorker _attackEngine;

    private IUnityActor _mine;
    private IUnityActor _enemy;

    //---------------------------------------------
    // プロパティ
    //---------------------------------------------
    public bool isSentis => _isSentis;
    public bool isMLAgent => _isMLAgent;

    //---------------------------------------------
    // 公開 関数
    //---------------------------------------------
    public void Setup()
    {
        if (_isSentis)
        {
            Model searchModel = ModelLoader.Load(_modelSearchAsset);
            _searchEngine = WorkerFactory.CreateWorker(BackendType.GPUCompute, searchModel);

            Model chaceModel = ModelLoader.Load(_modelChaceAsset);
            _chaceEngine = WorkerFactory.CreateWorker(BackendType.GPUCompute, chaceModel);

            Model attackModel = ModelLoader.Load(_modelAttackAsset);
            _attackEngine = WorkerFactory.CreateWorker(BackendType.GPUCompute, attackModel);
        }
    }

    public void SetMine(CharaBase mine)
    {
        _mine = mine;
    }
    public void SetEnemy(CharaBase enemy)
    {
        _enemy = enemy;
    }

    public ActionBuffers SelectActionBuffers(ActionBuffers actions)
    {
        if (!_isSentis)
        {
            return actions;
        }

        var inputTensor = CalcInputTensor();

        CharaInput charaInput = _mine.GetIComponent<CharaInput>();
        TensorFloat outputTensor = null;
        switch ((StateType)charaInput.GetCurrentState())
        {
            case StateType.Search:
                _searchEngine.Execute(inputTensor);
                outputTensor = _searchEngine.PeekOutput() as TensorFloat;
                break;
            case StateType.Attack:
                _attackEngine.Execute(inputTensor);
                outputTensor = _attackEngine.PeekOutput() as TensorFloat;
                break;
            case StateType.Chase:
                _chaceEngine.Execute(inputTensor);
                outputTensor = _chaceEngine.PeekOutput() as TensorFloat;
                break;

        }

        outputTensor.MakeReadable();
        float[] output = outputTensor.ToReadOnlyArray();
        var action = output.ToList().IndexOf(output.Max());
        
        return new ActionBuffers(null, new int[] { action });
    }

    Dictionary<string, Tensor> CalcInputTensor()
    {
        float[] inputData = CalcInputData();

        TensorFloat inputTensor = new TensorFloat(new TensorShape(15), inputData);
        Dictionary<string, Tensor> inputTensors = new Dictionary<string, Tensor>() {
                    { "input", inputTensor}
                };

        return inputTensors;
    }

    public float[] CalcInputData()
    {
        float[] inputData = new float[15];

        // エージェントと敵の相対的な座標（2つ）
        Vector3 diffPos = Vector3.zero;
        if (_enemy != null)
        {
            diffPos = _enemy.Transform.position - this.transform.position;
        }
        inputData[0] = diffPos.x;
        inputData[1] = diffPos.z;

        // エージェントと敵の相対的な角度（1つ）
        Vector3 axis = Vector3.Cross(transform.forward, diffPos);
        float angle = Vector3.Angle(transform.forward, diffPos) * (axis.y < 0 ? -1 : 1);
        angle = (angle + 180) / 360;

        inputData[2] = angle;

        // 敵のステート情報（2つ）
        int[] state = new int[2];
        CharaInput charaInput = _enemy.GetIComponent<CharaInput>();
        int enemyState = charaInput.GetCurrentState();
        if (enemyState != -1)
        {
            state = BattleUtil.Instance.StateToBinary(enemyState);
        }
        TensorFloat[] tensorState = new TensorFloat[2];
        for (int i = 0, max = state.Length; i < max; i++)
        {
            inputData[i + 2] = state[i];
        }

        // 敵は攻撃モーション中か（一つ）
        CharaAnimator charaAnimator = _enemy.GetIComponent<CharaAnimator>();
        inputData[5] = charaAnimator.currentActionID == Define.Client.ActionID.ATTACK ? 1 : 0;

        // 3本のRaycastは敵に当たっているかどうか（9つ）
        CharaSensor charaSensor = _mine.GetIComponent<CharaSensor>();
        int[] isHitRaycast = BattleUtil.Instance.ConvertToOnehot(charaSensor.isHitRaycast, 3);
        TensorFloat[] tensorRaycast = new TensorFloat[isHitRaycast.Length];
        for (int i = 0, max = isHitRaycast.Length; i < max; i++)
        {
            //inputData[i + 7] = isHitRaycast[i] ? 1 : 0;
            inputData[i + 6] = isHitRaycast[i];
        }

        return inputData;
    }
}
