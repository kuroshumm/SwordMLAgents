using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Define
{
    namespace ML
    {
        public enum MLActionID
        {
            None = -1,
            Right,
            Left,
            Up,
            Down,
            RightRot,
            LeftRot,
            Attack,
            Defence,
        }

        public enum CharaID 
        {
            None,
            Player,
            CPU,
            ML,
        }

    }

}