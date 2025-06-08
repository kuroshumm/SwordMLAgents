using Define.ML;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class CharaBase : ActorBase
{
    //---------------------------------
    // 非公開 変数
    //---------------------------------
    protected Vector3 _initPos;
    protected CharaID _charaId = CharaID.None;

    //---------------------------------
    // プロパティ
    //---------------------------------
    public CharaID charaid => _charaId;
    public Vector3 initPos => _initPos;

    //---------------------------------
    // 公開 関数
    //---------------------------------
    public virtual void Setup(Vector3 pos, CharaID charaID)
    {
        _initPos = pos;
        transform.position = pos;
        _charaId = charaID;

        Enter(null);

        AddIComponent<AnimationEventManager>();
        AddIComponent<CharaAnimator>();
        AddIComponent<CharaCollision>();
        AddIComponent<CharaUI>();
        AddIComponent<CharaInput>();
        AddIComponent<CharaMove>();
        AddIComponent<CharaAction>();
        var charaParametor = AddIComponent<CharaParametor>();
        if(charaParametor != null)
        {
            charaParametor.Init(charaID);
        }
    }

    public void SetId(int id)
    {
        _id = id;
    }

    public override void SetActiveObj(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

}
