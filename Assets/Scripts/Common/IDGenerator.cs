using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define.Client;

public class IDGenerator : Singleton<IDGenerator>
{
    Dictionary<IDType, int> _idDict = new Dictionary<IDType, int>();

    public int Generate(IDType type)
    {
        if (!_idDict.ContainsKey(type))
        {
            _idDict.Add(type, 0);
        }

        int id = _idDict[type];
        _idDict[type]++;

        return id;
    }
}
