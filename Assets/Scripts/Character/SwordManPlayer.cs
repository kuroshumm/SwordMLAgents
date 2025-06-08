using Define.ML;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordManPlayer : SwordManBase
{
    //---------------------------------------------
    // 非公開 変数
    //---------------------------------------------
    private CharaInput _charaInput;
    private CharaAction _charaAction;

    //---------------------------------
    // プロパティ
    //---------------------------------

    //---------------------------------------------
    // CharaBase 関数
    //---------------------------------------------
    public override void Setup(Vector3 pos, CharaID charaID)
    {
        base.Setup(pos, charaID);

        AddIComponent<CharaSensor>();

        if (_charaInput == null)
        {
            _charaInput = GetIComponent<CharaInput>();
            _charaInput.SetCharaInput(new PlayerCharaInput(this));
        }
        if (_charaAction == null)
        {
            _charaAction = GetIComponent<CharaAction>();
            _charaAction.SetAction(new PlayerCharaAction(this));
        }
    }
}
