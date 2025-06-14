using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharaUI : ComponentBase
{
    //---------------------------------
    // 非公開 定数
    //---------------------------------
    static readonly string HP_GAUGE_PREFAB = "UI/HPGauge";

    //---------------------------------
    // 非公開 変数
    //---------------------------------
    Canvas _canvas;
    CharaHpGaugeView _hpGauge;

    //---------------------------------
    // CharaComponent 関数
    //---------------------------------
    public override void Enter(IUnityActor owner)
    {
        _owner = owner;

        _canvas = GetComponentInChildren<Canvas>();
        _canvas.worldCamera = Camera.main;

        _hpGauge = AssetManager.Instance.InstantiateUI<CharaHpGaugeView>(HP_GAUGE_PREFAB, _canvas.transform);
    }

    //---------------------------------
    // 公開 関数
    //---------------------------------
    public void Init(float maxHp)
    {
        _hpGauge.Init(maxHp);
    }

    public void UpdateHpGauge(float hp)
    {
        _hpGauge.UpdateHpGauge(hp);
    }
}
