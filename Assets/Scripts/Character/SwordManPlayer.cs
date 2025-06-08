using Define.ML;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordManPlayer : SwordManBase
{
    //---------------------------------------------
    // ����J �ϐ�
    //---------------------------------------------
    private CharaInput _charaInput;
    private CharaAction _charaAction;

    //---------------------------------
    // �v���p�e�B
    //---------------------------------

    //---------------------------------------------
    // CharaBase �֐�
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
