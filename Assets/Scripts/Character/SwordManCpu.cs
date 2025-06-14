using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Define.Client;
using Define.ML;

public class SwordManCpu : SwordManBase
{
    //---------------------------------------------
    // 非公開 変数
    //---------------------------------------------
    private CharaInput _charaInput;
    private CharaAction _charaAction;

    //---------------------------------------------
    // CharaBase 関数
    //---------------------------------------------
    public override void Setup(Vector3 pos, CharaID charaID)
    {
        base.Setup(pos, charaID);

        AddIComponent<CharaSensor>();
        
        if(_charaInput == null)
        {
            _charaInput = GetIComponent<CharaInput>();
            _charaInput.SetCharaInput(new CpuCharaInput(this));
        }
        if(_charaAction == null)
        {
            _charaAction = GetIComponent<CharaAction>();
            _charaAction.SetAction(new CpuCharaAction(this));
        }
    }

    public override void Execute(float deltaTime)
    {
        base.Execute(deltaTime);

        _charaInput.ClearInput();
    }

    //---------------------------------------------
    // 非公開 関数
    //---------------------------------------------



}
