/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.Game
{
    using Opsive.DeathmatchAIKit.Demo.Character.Abilities;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Traits;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Applies a vertical and forward force to the character similar to the object being on an air lift.
    /// </summary>
    public class AirLift : MonoBehaviour
    {
        [Tooltip("The upwards force to apply to each character")]
        [SerializeField] protected float m_UpwardsForce = 5;
        [Tooltip("The forwards force to apply to each character, in the direction of the airlift's forward transform")]
        [SerializeField] protected float m_ForwardsForce = 0.5f;

        private Transform m_Transform;
        private AudioSource m_AudioSource;
        private List<UltimateCharacterLocomotion> m_Characters = new List<UltimateCharacterLocomotion>();
        private Dictionary<GameObject, UseAirLift> m_UseAirLiftByCharacter = new Dictionary<GameObject, UseAirLift>();
        private float m_CapsuleColliderTop;

        /// <summary>
        /// Cache the component references and initialize the default values.
        /// </summary>
        private void Awake()
        {
            m_Transform = transform;
            m_AudioSource = GetComponent<AudioSource>();
            var capsuleCollider = GetComponent<CapsuleCollider>();
            m_CapsuleColliderTop = transform.position.y + capsuleCollider.center.y + capsuleCollider.height / 2;

            // The component does not need to be active until there is an object within the trigger.
            enabled = false;
        }

        /// <summary>
        /// Apply a force to all of the characters.
        /// </summary>
        private void FixedUpdate()
        {
            // There may be multiple objects on the lift in which case the force should apply to all of them.
            if (m_Characters.Count > 0) {
                for (int i = m_Characters.Count - 1; i > -1; --i) {
                    // Remove the character if it is above the top of the capsule collider. OnTriggerExit isn't always reliable.
                    if (m_Characters[i].transform.position.y > m_CapsuleColliderTop) {
                        // Add a final forward force to push the character out of the trigger.
                        m_Characters[i].AddForce(m_Transform.forward * m_ForwardsForce);
                        var liftAbility = m_UseAirLiftByCharacter[m_Characters[i].gameObject];
                        liftAbility.StopAbility();
                    } else {
                        // Apply a forwards force to any character within the trigger.
                        m_Characters[i].AddForce(Vector3.up * m_UpwardsForce);
                    }
                }
                
                // The component no longer needs to update if there are no more characters to apply a force to.
                if (m_Characters.Count == 0) {
                    enabled = false;
                }
            }
        }

        /// <summary>
        /// An object has entered the trigger.
        /// </summary>
        /// <param name="other">The object that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            var characterLocomotion = other.gameObject.GetCachedParentComponent<UltimateCharacterLocomotion>();
            // Do not add the character if it doesn't exist or has already been added.
            if (characterLocomotion == null || m_Characters.Contains(characterLocomotion)) {
                return;
            }

            var health = other.gameObject.GetCachedParentComponent<Health>();
            // Do not add the character to the lift if they are dead.
            if (health != null && health.Value == 0) {
                return;
            }
            
            enabled = true;
            m_Characters.Add(characterLocomotion);
            m_AudioSource.Play();

            // If the object that entered the trigger is an UltimateCharacterController character then start the air lift ability.
            UseAirLift liftAbility;
            if (characterLocomotion != null && (liftAbility = characterLocomotion.GetAbility<UseAirLift>()) != null) {
                if (characterLocomotion.TryStartAbility(liftAbility, true)) {
                    liftAbility.AirLift = this;
                    m_UseAirLiftByCharacter.Add(characterLocomotion.gameObject, liftAbility);
                }
            }
        }

        /// <summary>
        /// An object has exited the trigger.
        /// </summary>
        /// <param name="other">The object that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            RemoveCharacter(other.gameObject);
        }

        /// <summary>
        /// The character's Air Lift ability has ended. Remove the character from the lift.
        /// </summary>
        /// <param name="character">The character that should be removed from the air lift.</param>
        public void RemoveCharacter(GameObject character)
        {
            var characterLocomotion = character.GetCachedComponent<UltimateCharacterLocomotion>();
            if (characterLocomotion != null && m_Characters.Contains(characterLocomotion)) {
                characterLocomotion.TryStopAbility(m_UseAirLiftByCharacter[characterLocomotion.gameObject]);
                m_Characters.Remove(characterLocomotion);
                m_UseAirLiftByCharacter.Remove(characterLocomotion.gameObject);
            }

            // The component no longer needs to update if there are no more characters to apply a force to.
            if (m_Characters.Count == 0) {
                enabled = false;
            }
        }
    }
}