/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.Character.Abilities
{
    using Opsive.DeathmatchAIKit.Demo.Game;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character.Abilities;
    using UnityEngine.AI;

    /// <summary>
    /// The Use Air Lift ability will allow the character to use a Deathmatch Kit air lift.
    /// </summary>
    [DefaultAbilityIndex(402)]
    [DefaultStartType(AbilityStartType.Manual)]
    [DefaultAllowPositionalInput(false)]
    [DefaultAllowRotationalInput(false)]
    [DefaultUseGravity(AbilityBoolOverride.False)]
    public class UseAirLift : Ability
    {
        private AirLift m_AirLift;

        public AirLift AirLift { set { m_AirLift = value; } }

        /// <summary>
        /// The ability has stopped running.
        /// </summary>
        /// <param name="force">Was the ability force stopped?</param>
        protected override void AbilityStopped(bool force)
        {
            m_AirLift.RemoveCharacter(m_GameObject);
            m_AirLift = null;

            // After the ability has stopped they should continue on from their current location.
            var navMeshAgent = m_GameObject.GetCachedComponent<NavMeshAgent>();
            if (navMeshAgent != null) {
                navMeshAgent.Warp(m_Transform.position);
            }

            base.AbilityStopped(force);
        }
    }
}