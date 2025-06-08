using Define.ML;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaParametor : ComponentBase
{
    //---------------------------------
    // 非公開 変数
    //---------------------------------
    private CharaUI _charaUI;

    private int _hp;
    private bool _isDead;

    //---------------------------------
    // 非公開 定数
    //---------------------------------
    private static readonly string LOAD_PARAM_PATH = "Data/Param/";
    private static readonly int HP_MAX = 3;

    //---------------------------------
    // プロパティ
    //---------------------------------
    public int hp => _hp;
    public bool isDead => _isDead;

    //---------------------------------
    // CharaComponent 関数
    //---------------------------------
    public override void Enter(IUnityActor owner)
    {
        _owner = owner;
        _charaUI = _owner.GetIComponent<CharaUI>();
    }

    //---------------------------------
    // 公開 関数
    //---------------------------------]
    public void Init(CharaID charaID)
    {
        string path = LOAD_PARAM_PATH + charaID.ToString();
        Parametor param = AssetManager.Instance.LoadScriptableObj<Parametor>(path);

        _hp = param.HP;
        _charaUI.Init(_hp);
    }

    public void AddHp(int hp)
    {
        _hp += hp;
        _owner.AddReward(-1f);
        _charaUI.UpdateHpGauge(_hp);

        if (_hp <= 0)
        {
            Dead();
        }
    }

    //---------------------------------
    // 非公開 関数
    //---------------------------------
    void Dead()
    {
        _isDead = true;
        _owner.SetActiveObj(false);
        GameManager.SetReInit();

        _owner.EndEpisode();
    }
}
