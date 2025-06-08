using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define.ML;

/// <summary>
/// ゲーム全体の管理を担当するマネージャークラス
/// </summary>
public class GameManager : MonoBehaviour
{
    //---------------------------------------------
    // Serialized Field
    //---------------------------------------------
    [SerializeField] Transform _stageRoot;

    //---------------------------------------------
    // プライベート変数
    //---------------------------------------------
    private static bool _isReInit;
    
    private CharacterManager _characterManager;
    private CameraManager _cameraManager;

    //---------------------------------------------
    // プロパティ
    //---------------------------------------------

    //---------------------------------------------
    // Monobehaviour 関数
    //---------------------------------------------
    private void Awake()
    {
        _characterManager = GetComponent<CharacterManager>();
        _cameraManager = new CameraManager();
        _cameraManager.Init(GetComponent<PlayerCameraManager>());
    }

    private void Start()
    {
        _isReInit = false;
        InitializeGame();
        StartCoroutine(GameLoop());
    }

    //---------------------------------------------
    // メソッド
    //---------------------------------------------
    public static void SetReInit()
    {
        _isReInit = true;
    }

    //---------------------------------------------
    // メソッド
    //---------------------------------------------
    private void InitializeGame()
    {
        _characterManager.Initialize();
        
        // プレイヤーキャラクターをカメラの対象に設定
        var characters = _characterManager.CharaList;
        if (characters != null && characters.Count > 0)
        {
            _cameraManager.SetTargetPlayer(characters[0] as SwordManBase);
        }
    }

    private IEnumerator GameLoop()
    {
        while (true)
        {
            if (_isReInit)
            {
                ReInitializeGame();
            }

            float deltaTime = Time.deltaTime;
            _characterManager.UpdateCharacters(deltaTime);

            yield return null;
        }
    }

    private void ReInitializeGame()
    {
        _characterManager.ResetAllCharacters();
        _cameraManager.ResetCamera();
        _isReInit = false;
    }

    private void OnDestroy()
    {
        _characterManager.DestroyAllCharacters();
    }
}
