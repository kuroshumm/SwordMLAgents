using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Define.Client;
using System;

public class CharaAction : ComponentBase
{
    //---------------------------------
    // 定数
    //---------------------------------
    static readonly float DISABLE_DAMAGE_TIME = 0.5f;
    static readonly float DISABLE_PARRY_TIME = 1.5f;

    //---------------------------------
    // 変数
    //---------------------------------
    private AnimationEventManager _animEventManager;
    private CharaAnimator _charaAnimator;
    private CharaInput _charaInput;
    private CharaCollision _charaCollision;

    int _attackColId;
    bool _isDefence;
    ICharaAction _action;

    //---------------------------------
    // プロパティ
    //---------------------------------
    public bool isDefence => _isDefence;
    public ActionID currentActionId => _charaAnimator.currentActionID;

    //---------------------------------
    // CharaComponentBase 関数
    //---------------------------------
    public override void Enter(IUnityActor owner)
    {
        _owner = owner;
        _charaAnimator = _owner.GetIComponent<CharaAnimator>();
        _charaInput = _owner.GetIComponent<CharaInput>();
        _charaCollision = _owner.GetIComponent<CharaCollision>();
        _animEventManager = _owner.GetIComponent<AnimationEventManager>();

        _isDefence = false;

        // アニメーションイベントの登録
        RegisterAnimationEvents();
    }

    private void RegisterAnimationEvents()
    {
        // 攻撃アニメーション
        var attackGroup = new AnimationEventGroup("attack");
        attackGroup.AddEvent(EventTiming.Progress, 0.3f, (sender, e) => {
                    if (e.Progress >= 0.3f)
                    {
                        ColSphereSize size = new ColSphereSize(0.5f);

                        // コリジョン生成
                        CollisionData colData = new CollisionData(
                                _owner.ID,
                                ObjType.AttackCollision,
                                CollisionShape.Sphere,
                                new Vector3(0.0f, 1.0f, 1.0f),
                                size
                            );
                        _charaCollision.CreateCollision<CollisionEnter>(colData, OnTriggerCollision);
                    }
                }
        );
        attackGroup.AddEvent(EventTiming.Progress, 0.6f, (sender, e) => {
            if (e.Progress >= 0.6f)
            {
                // コリジョン削除
                _charaCollision.ClearCollision();
            }
        });
        attackGroup.AddEvent(EventTiming.Complete, 1.0f, (sender, e) => {
            _charaAnimator.PlayAction(ActionID.IDLE);
        });


        // 防御アニメーション
        var defence = new AnimationEventGroup("defence");
        defence.AddEvent(EventTiming.Start, 0f, (sender, e) => {
            ColBoxSize size = new ColBoxSize(Vector3.zero, new Vector3(1.0f, 1.0f, 0.5f));

            // 防御コリジョン生成
            CollisionData colData = new CollisionData(
                    _owner.ID,
                    ObjType.DefenceCollision,
                    CollisionShape.Box,
                    new Vector3(0.0f, 1.0f, 0.5f),
                    size
                );
            _charaCollision.CreateCollision<CollisionStay>(colData);
        });
        defence.AddEvent(EventTiming.Complete, 1f, (sender, e) =>
        {
            // 防御コリジョン削除
            _charaCollision.ClearCollision();
            //_charaAnimator.PlayAction(ActionID.IDLE);
        });

        // 防御終了アニメーション
        var defenceEnd = new AnimationEventGroup("defence_end");
        defenceEnd.AddEvent(EventTiming.Complete, 1f, (sender, e) => {
            _charaAnimator.PlayAction(ActionID.IDLE);
        });

        // ダメージアニメーション
        var damage = new AnimationEventGroup("damage");
        damage.AddEvent(EventTiming.Complete, 1f, (sender, e) => {
            _charaAnimator.PlayAction(ActionID.IDLE);
        });

        _animEventManager.RegisterEventGroup(attackGroup);
        _animEventManager.RegisterEventGroup(defence);
        _animEventManager.RegisterEventGroup(defenceEnd);
        _animEventManager.RegisterEventGroup(damage);
    }

    public override void Execute(float deltaTime)
    {
        _action.Execute(deltaTime);

        ActionID currentActionID = _charaInput.actionID;
        switch (currentActionID) 
        {
            case ActionID.ATTACK:
                Attack(deltaTime);
                break;
            case ActionID.DEFENCE:
                Defence(deltaTime);
                break;
            case ActionID.DEFENCE_END:
                DefenceEnd(deltaTime);
                break;
            case ActionID.DAMAGE:
                Damage(deltaTime);
                break;
        }
    }

    public void SetAction(ICharaAction action)
    {
        _action = action;
    }

    //---------------------------------
    // ICharaAction 関数
    //---------------------------------
    void Attack(float deltaTime)
    {
        if (currentActionId == ActionID.DAMAGE) return;

        _action?.Attack(deltaTime, OnTriggerCollision);
    }

    void Defence(float deltaTime)
    {
        if (currentActionId == ActionID.DAMAGE) return;

        _action?.Defence(deltaTime);
    }

    void DefenceEnd(float deltaTime)
    {
        if (currentActionId == ActionID.DAMAGE) return;

        _action?.DefenceEnd(deltaTime);
    }

    void Damage(float deltaTime)
    {
        _action?.Damage(deltaTime);
    }

    //---------------------------------
    // 関数
    //---------------------------------
    void OnTriggerCollision(IObject obj, int ownerId)
    {
        IUnityActor chara = obj as IUnityActor;
        if (chara == null) return;
        if (chara.ID == ownerId) return;

        ObjType objType = CheckTriggerObjType(chara);
        CharaAnimator charaAnimator = chara.GetIComponent<CharaAnimator>();
        if (charaAnimator.currentActionID == ActionID.DEFENCE)
        {
            if (objType == ObjType.DefenceCollision)
            {
                return;
            }
        }

        CharaParametor charaParametor = chara.GetIComponent<CharaParametor>();
        charaParametor.AddHp(-1);

        _owner.AddReward(1f);
    }
    
    /// <summary>
    /// 衝突判定
    /// </summary>
    /// <param name="chara"></param>
    /// <returns></returns>
    ObjType CheckTriggerObjType(IUnityActor chara)
    {
        // 衝突までの距離と方向を計算
        var distance = BattleUtil.Instance.CalcDistanceXZ(chara.Pelvis.position, _owner.Pelvis.position);
        var dir = (chara.Pelvis.position - _owner.Pelvis.position).normalized;

        Debug.DrawRay(_owner.Pelvis.position, dir * distance, Color.red, 1);

        // 衝突までの間にあるIオブジェクトを取得
        RaycastHit[] hitArray = Physics.RaycastAll(_owner.Pelvis.position, dir, distance);
        if (hitArray == null) return ObjType.None;
        if (hitArray.Length == 0) return ObjType.None;
        if (hitArray.Length > 1)
        {
            Array.Sort(hitArray, (x, y) => x.distance.CompareTo(y.distance));
        }

        // 衝突したオブジェクトの間にQオブジェクトがない場合はGオブジェクトに衝突したとみなす
        for (int i = 0, max = hitArray.Length; i < max; i++)
        {
            RaycastHit hit = hitArray[i];

            int hitInstanceId = hit.collider.gameObject.GetInstanceID();
            if (hitInstanceId == gameObject.GetInstanceID()) return ObjType.None;

            IObject hitObj = AssetManager.Instance.GetIObject(hitInstanceId, null);
            if (hitObj == null) return ObjType.Obstract;

            CollisionBase col = hitObj as CollisionBase;
            bool isCol = col != null;
            if (isCol && col.ownerId == _owner.ID) continue;
            if (isCol && col.objType == ObjType.AttackCollision) continue;
            
            if (isCol && col.objType == ObjType.DefenceCollision) return ObjType.DefenceCollision;
            if (hitObj as SwordManBase) return ObjType.Chara;
        }

        return ObjType.None;
    }
}
