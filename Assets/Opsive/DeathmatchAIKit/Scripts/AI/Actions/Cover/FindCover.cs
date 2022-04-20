/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Finds an unoccupied cover point.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class FindCover : Action
    {
        [Tooltip("The GameObject to attack.")]
        [SerializeField] protected SharedGameObject m_Target;
        [Tooltip("The found CoverPoint.")]
        [SerializeField] protected SharedCoverPoint m_CoverPoint;

        private CoverPoint[] m_CoverPoints;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            base.OnAwake();

            m_CoverPoints = GameObject.FindObjectsOfType<CoverPoint>();
        }

        /// <summary>
        /// Starts to find a cover point.
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            if (m_CoverPoint.Value != null) {
                m_CoverPoint.Value.Occupant = null;
            }
        }

        /// <summary>
        /// Find an unoccupied cover point. Returns success if a cover point was found.
        /// </summary>
        /// <returns>True if a cover point was found.</returns>
        public override TaskStatus OnUpdate()
        {
            var distance = float.MaxValue;
            float localDistance;

            CoverPoint[] coverPoints;
            // If the agent is going into cover for the first time then find the closest unoccupied cover point. Otherwise, find the closest linked cover point.
            if (m_CoverPoint.Value == null || m_CoverPoint.Value.LinkedCoverPoints.Length == 0) {
                coverPoints = m_CoverPoints;
            } else {
                coverPoints = m_CoverPoint.Value.LinkedCoverPoints;
            }
            for (int i = 0; i < coverPoints.Length; ++i) {
                if (coverPoints[i].gameObject.activeInHierarchy && coverPoints[i].IsValidCoverPoint(transform, m_Target.Value == null ? null : m_Target.Value.transform)) {
                    if ((localDistance = (transform.position - coverPoints[i].transform.position).sqrMagnitude) < distance) {
                        distance = localDistance;
                        m_CoverPoint.Value = coverPoints[i];
                    }
                }
            }
            // If the distance is still the max value then a new cover point wasn't found.
            if (distance == float.MaxValue) {
                m_CoverPoint.Value = null;
                return TaskStatus.Failure;
            }

            // A cover point was found. Return success.
            m_CoverPoint.Value.Occupant = transform;
            return TaskStatus.Success;
        }
    }
}