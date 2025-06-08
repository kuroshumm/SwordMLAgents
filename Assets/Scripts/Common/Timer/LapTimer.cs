using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapTimer
{
    //---------------------------------
    // ����J �ϐ�
    //---------------------------------
    private List<LapTimeAction> _lapTimeActionList;
    private int _lapCount;
    private float _timer;

    private bool _isUpdate;


    //---------------------------------
    // ���J �֐�
    //---------------------------------
    public LapTimer(List<LapTimeAction> actionList)
    {
        _lapTimeActionList = actionList;

        ClearTimer();
    }

    public LapTimer(LapTimeAction action = null)
    {
        if(action != null)
        {
            _lapTimeActionList = new List<LapTimeAction>() { action };
        }
        ClearTimer();
    }

    public void StartTimer(List<LapTimeAction> actionList)
    {
        _isUpdate = true;
        _timer = 0;
        _lapCount = 0;

        if (actionList == null) return;

        _lapTimeActionList = actionList;
    }

    public void StartTimer(LapTimeAction action = null)
    {
        _isUpdate = true;
        _timer = 0;
        _lapCount = 0;

        if (action == null) return;

        _lapTimeActionList = new List<LapTimeAction>() { action };
    }

    public void ClearTimer()
    {
        _isUpdate = false;
        _timer = 0;
        _lapCount = 0;
    }

    public bool UpdateTimer(float deltaTime)
    {
        if (!_isUpdate)
        {
            return true;
        }

        _timer += deltaTime;
        if (ExecuteCallback(_timer))
        {
            ClearTimer();
            return true;
        }    

        return false;
    }

    bool ExecuteCallback(float timer)
    {
        // �^�C�}�[���ݒ肳��ĂȂ��ꍇ�͉������Ȃ�
        if (_lapTimeActionList == null) return false;
        if (_lapTimeActionList.Count == 0) return false;

        // �ݒ肵���S�Ẵ^�C�}�[���I�������ꍇ�͊����ŕԂ�
        if (_lapTimeActionList.Count <= _lapCount) return true;

        if (_lapTimeActionList[_lapCount].Execute(timer))
        {
            ++_lapCount;
        }

        return _lapTimeActionList.Count < _lapCount;
    }
}
