/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.UltimateCharacterController.Objects.CharacterAssist;
    using UnityEngine;
    using UnityEngine.AI;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Searches for a health pickup.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class SearchForHealth : SpeedChangeMovement
    {
        private Transform[] m_HealthPickups;
        private Vector3 m_TargetPosition;
        private NavMeshPath m_NavMeshPath = new NavMeshPath();
        private bool m_PathFound;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            base.OnAwake();

            // Cache all of the HealthPickup locations to prevent having to use FindObjects every frame.
            var healthPickups = GameObject.FindObjectsOfType<HealthPickup>();
            m_HealthPickups = new Transform[healthPickups.Length];
            for (int i = 0; i < healthPickups.Length; ++i) {
                m_HealthPickups[i] = healthPickups[i].transform;
            }
        }

        /// <summary>
        /// Determine the closest health position.
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            // Move to the closest ItemPickup.
            var closestDistance = Mathf.Infinity;
            float distance;
            m_PathFound = false;
            for (int i = 0; i < m_HealthPickups.Length; ++i) {
                // Use the NavMesh to determine the closest position - just because the item is physically the closest it doesn't mean that the path distance is the closest.
                NavMesh.CalculatePath(transform.position, m_HealthPickups[i].position, NavMesh.AllAreas, m_NavMeshPath);
                if (m_NavMeshPath.corners.Length > 0) {
                    distance = 0;
                    var prevCorner = m_NavMeshPath.corners[0];
                    for (int j = 1; j < m_NavMeshPath.corners.Length; ++j) {
                        distance += Vector3.Distance(m_NavMeshPath.corners[j], prevCorner);
                        prevCorner = m_NavMeshPath.corners[j];
                    }
                    // Go to the position that has the least distance.
                    if (distance < closestDistance) {
                        closestDistance = distance;
                        m_TargetPosition = m_HealthPickups[i].position;
                    }
                    m_PathFound = true;
                }
            }

            if (m_PathFound) {
                SetDestination(m_TargetPosition);
            }
        }

        /// <summary>
        /// Return Success when the agent has arrived at the health position.
        /// </summary>
        /// <returns>Success when the agent has arrived at the health position, otherwise Running.</returns>
        public override TaskStatus OnUpdate()
        {
            if (!m_PathFound) {
                return TaskStatus.Failure;
            }
            if (HasArrived()) {
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }
    }
}