/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit
{
    using Opsive.DeathmatchAIKit.Character.Abilities;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Game;
    using Opsive.UltimateCharacterController.Objects.CharacterAssist;
    using UnityEngine;

    /// <summary>
    /// The CoverPoint specifies the location that the agent can take cover.
    /// </summary>
    [RequireComponent(typeof(BoxCollider))]
    public class CoverPoint : MonoBehaviour
    {
        [Tooltip("Any other cover points which the agent can move between")]
        [SerializeField] protected CoverPoint[] m_LinkedCoverPoints;
        [Tooltip("The point at which the agent can attack from the cover point")]
        [SerializeField] protected Vector3 m_AttackOffset;
        [Tooltip("The maximim distance that the cover point can be away from the agent to be considered valid")]
        [SerializeField] protected float m_MaxDistance = 10f;
        [Tooltip("The minimum distance that the cover point can be away from the target to be considered valid")]
        [SerializeField] protected float m_MinTargetDistance = 2f;
        [Tooltip("The maximim distance that the cover point can be away from the target to be considered valid")]
        [SerializeField] protected float m_MaxTargetDistance = 10f;
        [Tooltip("A -1 to 1 threshold for when the cover point is looking at the target. A value of 1 indicates the cover point is looking directly at " +
                 "the target while a value of -1 indicates that the target is behind the cover look direction")]
        [SerializeField] protected float m_LookThreshold = 0.5f;


        public Vector3 AttackPosition { get { return m_Transform.TransformPoint(m_AttackOffset); } }
        public CoverPoint[] LinkedCoverPoints { get { return m_LinkedCoverPoints; } }

        private Transform m_Transform;
        private MoveTowardsLocation m_MoveTowardsLocation;
        private Transform m_Occupant;
        private RaycastHit m_RaycastHit;

        public MoveTowardsLocation MoveTowardsLocation { get { return m_MoveTowardsLocation; } }
        public Transform Occupant { get { return m_Occupant; } set { m_Occupant = value; } }

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Awake()
        {
            m_Transform = transform;
            m_MoveTowardsLocation = GetComponent<MoveTowardsLocation>();
        }

        /// <summary>
        /// An object has entered the trigger. Set the occupant if the collider is a character.
        /// </summary>
        /// <param name="other">The collider that entered the trigger.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (m_Occupant == null) {
                var characterLocomotion = other.gameObject.GetCachedComponent<UltimateCharacterLocomotion>();
                if (characterLocomotion == null) {
                    return;
                }

                var cover = characterLocomotion.GetAbility<Cover>();
                if (cover != null) {
                    m_Occupant = other.transform;
                }
            }
        }

        /// <summary>
        /// An object has exited the trigger. Reset the occupant if the collider matches the occupant.
        /// </summary>
        /// <param name="other">The collider that exited the trigger.</param>
        private void OnTriggerExit(Collider other)
        {
            if (m_Occupant != null && m_Occupant == other.transform) {
                m_Occupant = null;
            }
        }

        /// <summary>
        /// Can the cover point be used by the agent?
        /// </summary>
        /// <param name="agent">The agent which is searching for cover.</param>
        /// <param name="target">The object that the agent is attacking.</param>
        /// <returns>True if the cover point can be used by the agent.</returns>
        public bool IsValidCoverPoint(Transform agent, Transform target)
        {
            // The cover point is invalid if one of the following occurs:

            // There an occupant.
            if (m_Occupant != null) {
                return false;
            }

            Vector3 direction;
            if (target != null) {
                // The cover position is too close or far away from the target.
                direction = target.position - m_Transform.position;
                var distance = direction.magnitude;
                if (distance < m_MinTargetDistance || distance > m_MaxTargetDistance) {
                    return false;
                }

                // The cover position isn't facing the target.
                direction.y = 0;
                if (Vector3.Dot(m_Transform.forward, direction.normalized) < m_LookThreshold) {
                    return false;
                }

                // The cover position can't see the target.
                var attackPosition = AttackPosition;
                if (Physics.Linecast(target.position, attackPosition)) {
                    return false;
                }
            }

            // The cover position is too far away from the agent.
            direction = agent.position - m_Transform.position;
            if (direction.magnitude > m_MaxDistance) {
                return false;
            }

            // There is another agent occupied in the linked cover point.
            for (int i = 0; i < m_LinkedCoverPoints.Length; ++i) {
                if (m_LinkedCoverPoints[i].Occupant != null && m_LinkedCoverPoints[i].Occupant != agent) {
                    return false;
                }
            }

            // The cover point is valid.
            return true;
        }

        /// <summary>
        /// Draw a visual representation of the cover point.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            // Draw a cube representing the player on the ground.
            var valid = false;
            RaycastHit hit;
            var cubeHeight = 2f;
            var cubePosition = transform.position;
            var solidObjectLayers = ~((1 << LayerManager.IgnoreRaycast) | (1 << LayerManager.Water) | (1 << LayerManager.UI) | (1 << LayerManager.VisualEffect) | (1 << LayerManager.Overlay) | (1 << LayerManager.SubCharacter));
            if (Physics.Raycast(transform.position, -transform.up, out hit, m_MaxDistance, solidObjectLayers, QueryTriggerInteraction.Ignore)) {
                // Determine if the cover is standing or crouching cover. Note that this will not be completely accurate because the characters
                // have different heights and different cover settings.
                cubePosition = hit.point;
                if (!Physics.Raycast(cubePosition + Vector3.up * cubeHeight, transform.forward, 2, solidObjectLayers, QueryTriggerInteraction.Ignore)) {
                    cubeHeight /= 2;
                }
                cubePosition.y += cubeHeight / 2;

                // The cover point has to be near the object that the character is taking cover on.
                valid = Physics.Raycast(cubePosition, transform.forward, out hit, 0.3f, solidObjectLayers, QueryTriggerInteraction.Ignore);
                if (valid) {
                    // The cover point can be too close.
                    valid = hit.distance > 0.15f;
                }
            }
            var color = valid ? Color.green : Color.red;
            color.a = 0.75f;
            Gizmos.color = color;
            Gizmos.DrawCube(cubePosition, new Vector3(0.5f, cubeHeight, 0.5f));
            Gizmos.color = Color.green;
            var gizmoMatrix = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(transform.TransformPoint(m_AttackOffset), transform.rotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, 45, 5, 0.1f, 1);
            Gizmos.matrix = gizmoMatrix;
        }
    }
}