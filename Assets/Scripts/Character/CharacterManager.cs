using Define.ML;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// キャラクター管理を担当するマネージャークラス
/// </summary>
public class CharacterManager : MonoBehaviour
{
    [SerializeField] List<CharaID> _charaIDList;
    
    private static readonly string SWORDMAN_PREFAB = "Prefabs/SwordMan";
    private static readonly string ML_SWORDMAN_PREFAB = "Prefabs/MLSwordMan";
    private static readonly Vector3 _enemyPos = new Vector3(-6, 1, -6);
    private static readonly Vector3 _playerPos = new Vector3(7, 1, 7);

    private List<Vector3> _spawnDefaultPosList;
    private List<CharaBase> _charaList;
    private SentisManager _sentisManager;

    public List<CharaBase> CharaList => _charaList;

    private void Awake()
    {
        _spawnDefaultPosList = new List<Vector3>()
        {
            _playerPos,
            _enemyPos,
            _playerPos
        };

        _sentisManager = GetComponent<SentisManager>();
    }

    public void Initialize()
    {
        _sentisManager.Setup();
        CreateAllCharacters();
    }

    public void UpdateCharacters(float deltaTime)
    {
        if (_charaList == null) return;

        for (int i = 0, max = _charaList.Count; i < max; i++)
        {
            CharaBase chara = _charaList[i];
            if (chara is SwordManML) continue;
            chara.Execute(deltaTime);
        }
    }

    public void ResetAllCharacters()
    {
        if (_charaList == null) return;

        for (int i = 0, max = _charaList.Count; i < max; i++)
        {
            CharaBase chara = _charaList[i];
            chara.Exit();

            BattleUtil.Instance.RandomPoint(_spawnDefaultPosList[i], 1f, out Vector3 spawnPos);
            chara.SetActiveObj(true);
            chara.Setup(spawnPos, chara.charaid);
        }
    }

    public void DestroyAllCharacters()
    {
        if (_charaList == null) return;

        for (int i = 0, max = _charaList.Count; i < max; i++)
        {
            Destroy(_charaList[i].gameObject);
        }
        _charaList = null;
    }

    private void CreateAllCharacters()
    {
        _charaList = new List<CharaBase>();

        for (int i = 0, max = _charaIDList.Count; i < max; i++)
        {
            CharaBase chara = CreateCharacter(_charaIDList[i]);
            if (chara == null) continue;

            BattleUtil.Instance.RandomPoint(_spawnDefaultPosList[i], 1f, out Vector3 spawnPos);
            chara.Setup(spawnPos, _charaIDList[i]);
            _charaList.Add(chara);
        }
    }

    private CharaBase CreateCharacter(CharaID charaId)
    {
        CharaBase chara = null;
        switch (charaId)
        {
            case CharaID.Player:
                chara = CharaGenerator.Instance.CreateCharaWithAttach<SwordManPlayer>(SWORDMAN_PREFAB);
                _sentisManager.SetEnemy(chara);
                break;
            case CharaID.CPU:
                chara = CharaGenerator.Instance.CreateCharaWithAttach<SwordManCpu>(SWORDMAN_PREFAB);
                _sentisManager.SetEnemy(chara);
                break;
            case CharaID.ML:
                chara = CharaGenerator.Instance.CreateCharaWithAttach<SwordManML>(ML_SWORDMAN_PREFAB);
                _sentisManager.SetMine(chara);
                (chara as SwordManML).SetSentisManager(_sentisManager);
                break;
        }
        return chara;
    }
} 