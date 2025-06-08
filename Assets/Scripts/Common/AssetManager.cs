using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AssetManager : Singleton<AssetManager>
{
    private Dictionary<int, IObject> mAssetDict;
    
    private const string mBasePath = "ScriptableObj/";


    public override void Awake()
    {
        base.Awake();
        mAssetDict = new Dictionary<int, IObject>();
    }

    public IObject GetIObject(int _instanceID, GameObject _gameObj)
    {
        IObject obj;
        if (!mAssetDict.TryGetValue(_instanceID, out obj))
        {
            if(_gameObj != null && !_gameObj.TryGetComponent<IObject>(out obj))
            {
                return null;
            }
            mAssetDict.Add(_instanceID, obj);
        }

        return obj;
    }

    public T InstantiateUI<T>(string _name, Transform _parant)
    {
        if (mAssetDict == null)
            mAssetDict = new Dictionary<int, IObject>();

        GameObject prefab = (GameObject)Resources.Load(_name);
        GameObject uiAsset = Instantiate(prefab, _parant);

        return uiAsset.GetComponent<T>();
    }

    public T Instantiate<T>(string _name, Transform _parent = null) where T : IObject
    {
        if (mAssetDict == null)
            mAssetDict = new Dictionary<int, IObject>();

        GameObject gameObj = InstantiateAsset(null, _name, Vector3.zero, Quaternion.identity);

        if(gameObj == null)
        {
            Debug.LogError("オブジェクト生成に失敗：" + _name);
        }

        if(_parent != null) gameObj.transform.parent = _parent;

        return (T)GetIObject(gameObj.GetInstanceID(), gameObj);
    }

    public T InstantiateWithAttach<T>(string _name, Transform _parent = null) where T : MonoBehaviour, IObject
    {
        if (mAssetDict == null)
            mAssetDict = new Dictionary<int, IObject>();

        GameObject gameObj = InstantiateAsset(null, _name, Vector3.zero, Quaternion.identity);
        var g = gameObj.AddComponent<T>();
        
        if (gameObj == null)
        {
            Debug.LogError("オブジェクト生成に失敗：" + _name);
        }

        if (_parent != null) gameObj.transform.parent = _parent;

        return (T)GetIObject(gameObj.GetInstanceID(), gameObj);
    }

    GameObject InstantiateAsset(IObject _object, string _path, Vector3 _pos, Quaternion _rot)
    {
        GameObject prefab = (GameObject)Resources.Load(_path);
        GameObject asset = Instantiate(prefab, _pos, _rot);
        //mAssetDict.Add(asset.GetInstanceID(), _object);
        return asset;
    }

    public T[] LoadAllScriptableObj<T>(string path) where T : ScriptableObject
    {
        var loadObjs = Resources.LoadAll<T>(path);
        T[] instatiateObjs = new T[loadObjs.Length];
        for(int i = 0, max = loadObjs.Length; i < max; i++)
        {
            instatiateObjs[i] = Instantiate(loadObjs[i]);
        }
        return instatiateObjs;
    }

    public T LoadScriptableObj<T>(string path) where T : ScriptableObject
    {
        var loadObj = Resources.Load<T>(path);
        T instatiateObjs = Instantiate(loadObj);
        return instatiateObjs;
    }

    public T[] LoadAllResouce<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    public GameObject InstantiateObj(string _name, Transform _parent = null)
    {
        var obj = InstantiateAsset(null, _name, Vector3.zero, Quaternion.identity);
        if(_parent != null) obj.transform.parent = _parent;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        return obj;
    }

    public void Clear()
    {
        mAssetDict = null;
    }

}
