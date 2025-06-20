using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    //---------------------------------
    // 非公開 変数
    //---------------------------------
    private float _time;

    private float _timer;
    private bool _isUpdate;
    private bool _isFinish;

    public bool isFinish => _isFinish;

    //---------------------------------
    // 公開 関数
    //---------------------------------
    public Timer(float time)
    {
        _time = time;
        ClearTimer();
        _isFinish = true;
    }

    public void StartTimer()
    {
        _timer = 0;
        _isFinish = false;
        _isUpdate = true;
    }

    public void ClearTimer()
    {
        _timer = 0;
        _isUpdate = false;
    }

    public void UpdateTimer(float deltaTime)
    {
        if (!_isUpdate) return;

        _timer += deltaTime;
        if(_timer > _time)
        {
            ClearTimer();
            _isFinish = true;
        }
    }

}
