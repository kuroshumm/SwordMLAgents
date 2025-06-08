using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    namespace Client
    {
        public enum ActionID
        {
            None,

            IDLE,
            WALK,
            ATTACK,
            DEFENCE,
            DEFENCE_IDLE,
            DEFENCE_END,
            DAMAGE,
        }

        public enum StateType
        {
            Search,
            Attack,
            Chase
        }
        public enum TriggerType
        {
            Search,
            Attack,
            Chase
        }

        public enum IDType 
        {
            Collision
        }

        public enum ObjType
        {
            None,

            Obstract,
            DefenceCollision,
            AttackCollision,
            Chara,
        }

        public enum CollisionShape
        {
            None,

            Sphere,
            Box,
        }

    }
}


