using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using Define.Client;
using Define.ML;

public class SwordManCpu : SwordManBase
{
    //---------------------------------------------
    // ”ñŒöŠJ •Ï”
    //---------------------------------------------
    private CharaInput _charaInput;
    private CharaAction _charaAction;

    //---------------------------------------------
    // CharaBase ŠÖ”
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
    // ”ñŒöŠJ ŠÖ”
    //---------------------------------------------



}
