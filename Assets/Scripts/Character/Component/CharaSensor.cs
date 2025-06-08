using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharaSensor : ComponentBase
{
    //---------------------------------
    // ����J �ϐ�
    //---------------------------------
    int[] _isHitRaycast;
    int[] _rayDirectionArray;
    int _rayDirectionNum;

    IUnityActor _targetChara;

    //---------------------------------
    // �v���p�e�B
    //---------------------------------
    public IUnityActor targetChara => _targetChara;
    public int[] isHitRaycast => _isHitRaycast;

    //---------------------------------
    // CharaComponentBase �֐�
    //---------------------------------
    public override void Enter(IUnityActor owner)
    {
        _owner = owner;

        _rayDirectionArray = new int[]
        {
                0,
                30,
                -30
        };
        _rayDirectionNum = _rayDirectionArray.Length;
        _isHitRaycast = new int[_rayDirectionNum];
    }

    //---------------------------------
    // ���J �֐�
    //---------------------------------
    public void Raycast(Action callback)
    {
        // ���C���΂��ē���������ǌ��X�e�[�g�Ɉڍs
        for (int i = 0; i < _rayDirectionNum; ++i)
        {
            _isHitRaycast[i] = 0;

            var dir = _rayDirectionArray[i];
            var rayDir = Quaternion.Euler(0, dir, 0) * transform.forward;

            //Debug.DrawRay(_owner.pelvis.position, rayDir * 3, Color.red, 1);

            RaycastHit hit;
            if (Physics.Raycast(_owner.Pelvis.position, rayDir, out hit, 3f))
            {
                int hitInstanceId = hit.collider.gameObject.GetInstanceID();
                IObject obj = AssetManager.Instance.GetIObject(hitInstanceId, null);
                if (obj == null)
                {
                    _isHitRaycast[i] = 2;
                    continue;
                }
                if (hitInstanceId == gameObject.GetInstanceID()) continue;

                if(obj is IUnityActor)
                {
                    callback?.Invoke();
                    _targetChara = obj as IUnityActor;
                    _isHitRaycast[i] = 1;
                    continue;
                }
            }
        }
    }
}
