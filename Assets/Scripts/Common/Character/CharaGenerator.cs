using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CharaGenerator : Singleton<CharaGenerator>
{
    //---------------------------------
    // ”ñŒöŠJ ’è”
    //---------------------------------
    private int _charaId = 0;
    Dictionary<int, CharaBase> _charaDict = new Dictionary<int, CharaBase>();

    public Dictionary<int, CharaBase> charaDict => _charaDict;

    public T CreateCharaWithAttach<T>(string name) where T : CharaBase
    {
        var chara = AssetManager.Instance.InstantiateWithAttach<T>(name);
        if (chara == null)
            return chara;

        _charaDict.Add(_charaId, chara);
        chara.SetId(_charaId);
        _charaId++;

        return chara;
    }

    public T CreateChara<T>(string name) where T : CharaBase
    {
        var chara = AssetManager.Instance.Instantiate<T>(name);
        if (chara == null)
            return chara;

        _charaDict.Add(_charaId, chara);
        chara.SetId(_charaId);
        _charaId++;

        return chara;
    }
}
