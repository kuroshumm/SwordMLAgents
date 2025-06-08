using Define.ML;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class CharaBase : ActorBase
{
    //---------------------------------
    // ����J �ϐ�
    //---------------------------------
    protected Vector3 _initPos;
    protected CharaID _charaId = CharaID.None;

    //---------------------------------
    // �v���p�e�B
    //---------------------------------
    public CharaID charaid => _charaId;
    public Vector3 initPos => _initPos;

    //---------------------------------
    // ���J �֐�
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
