/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.Game
{
    using UnityEngine;
    using Opsive.UltimateCharacterController.Traits;
    using Opsive.UltimateCharacterController.Traits.Damage;

    /// <summary>
    /// Extends the Health component by notifying the Scoreboard of a death.
    /// </summary>
    public class DeathmatchHealth : CharacterHealth
    {
        /// <summary>
        /// The object has taken been damaged.
        /// </summary>
        /// <param name="damageData">The data associated with the damage.</param>
        public override void OnDamage(DamageData damageData)
        {
            // Do not allow friendly fire.
            if (DeathmatchManager.TeamGame && TeamManager.IsTeammate(m_GameObject, damageData.DamageOriginator.OriginatingGameObject)) {
                return;
            }

            base.OnDamage(damageData);
        }

        /// <summary>
        /// The character has died. Report the death to interested components.
        /// </summary>
        /// <param name="force">The amount of force which killed the character.</param>
        /// <param name="position">The position of the force.</param>
        /// <param name="attacker">The GameObject that killed the character.</param>
        public override void Die(Vector3 position, Vector3 force, GameObject attacker)
        {
            Scoreboard.ReportDeath(attacker, gameObject);
            if (TeamManager.IsInstantiated) {
                TeamManager.CancelBackupRequest(gameObject);
            }

            base.Die(position, force, attacker);
        }
    }
}