/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI
{
    using BehaviorDesigner.Runtime;

    [System.Serializable]
    public class SharedCoverPoint : SharedVariable<CoverPoint>
    {
        public static implicit operator SharedCoverPoint(CoverPoint value) { return new SharedCoverPoint { mValue = value }; }
    }
}