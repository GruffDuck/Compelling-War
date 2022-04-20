/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI
{
    using UnityEngine;
    using BehaviorDesigner.Runtime;
    using System.Collections.Generic;

    [System.Serializable]
    public class SharedGameObjectSet : SharedVariable<HashSet<GameObject>>
    {
        public SharedGameObjectSet() { mValue = new HashSet<GameObject>(); }
        public static implicit operator SharedGameObjectSet(HashSet<GameObject> value) { return new SharedGameObjectSet { mValue = value }; }
    }
}