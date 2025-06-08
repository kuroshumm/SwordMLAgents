using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaHpGaugeView : MonoBehaviour
{
    //---------------------------------
    // Serialized Field
    //---------------------------------
    Slider _hpBar;

    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    //---------------------------------
    // åˆäJ ä÷êî
    //---------------------------------
    public void Init(float maxHp)
    {
        _hpBar = GetComponent<Slider>();
        _hpBar.maxValue = maxHp;
        _hpBar.value = maxHp;
    }
    public void UpdateHpGauge(float hp)
    {
        _hpBar.value = hp;
    }
}
