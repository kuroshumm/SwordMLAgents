using Define.ML;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaParametor : ComponentBase
{
    //---------------------------------
    // ����J �ϐ�
    //---------------------------------
    private CharaUI _charaUI;

    private int _hp;
    private bool _isDead;

    //---------------------------------
    // ����J �萔
    //---------------------------------
    private static readonly string LOAD_PARAM_PATH = "Data/Param/";
    private static readonly int HP_MAX = 3;

    //---------------------------------
    // �v���p�e�B
    //---------------------------------
    public int hp => _hp;
    public bool isDead => _isDead;

    //---------------------------------
    // CharaComponent �֐�
    //---------------------------------
    public override void Enter(IUnityActor owner)
    {
        _owner = owner;
        _charaUI = _owner.GetIComponent<CharaUI>();
    }

    //---------------------------------
    // ���J �֐�
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
    // ����J �֐�
    //---------------------------------
    void Dead()
    {
        _isDead = true;
        _owner.SetActiveObj(false);
        GameManager.SetReInit();

        _owner.EndEpisode();
    }
}
