/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using System.Collections.Generic;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;
    
    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Traverses the map looking for a new target.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class SearchForTarget : SpeedChangeMovement
    {
        // Specifies how to traverse between waypoints.
        protected enum PatrolType
        { 
            Sequence,   // Traverse in order of the list.
            Random,     // Traverse in a random order.
            Directional // Traverse to the next waypoint most in front of the current Transform.
        }
        
        [Tooltip("Specifies how to traverse")]
        [SerializeField] protected PatrolType m_PatrolType = PatrolType.Sequence;
        [Tooltip("A list of waypoints to traverse")]
        [SerializeField] protected SharedGameObjectList m_Waypoints;
        [Tooltip("Allow the agent to randomly break out of the directional movement type")]
        [SerializeField] protected SharedFloat m_RandomWaypointProbability = 0.1f;
        [Tooltip("As the agent is traversing to the next waypoint they can give up early starting at this distance away from the destination")]
        [SerializeField] protected SharedFloat m_StartGiveUpDistance = 5;
        [Tooltip("As the agent is traversing to the next waypoint they can give up early based on the specified curve")]
        [SerializeField] protected AnimationCurve m_GiveUpCurve = new AnimationCurve(new Keyframe[2] { new Keyframe(0, 0), new Keyframe(1, 1) });

        private int m_WaypointIndex = -1;
        private Vector3 m_EndPosition;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            base.OnAwake();

            m_EndPosition = -transform.position + Vector3.one;
        }

        /// <summary>
        /// Set the first waypoint destination.
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();
            
            // If a TeamManager exists then the game is a team game and the agent should register itself as the leader of the team.
            if (TeamManager.IsInstantiated) {
                TeamManager.SetLeader(gameObject, true);
            }

            if (m_Waypoints.Value.Count > 0) {
                // Do not get a new waypoint position if the task is starting again immediately after it was stopped.
                if (m_WaypointIndex == -1 || (m_EndPosition - transform.position).sqrMagnitude > 0.5f) {
                    if (m_PatrolType == PatrolType.Random) {
                        m_WaypointIndex = Random.Range(0, m_Waypoints.Value.Count);
                    } else { // Sequence or Directional. Move towards the closest waypoint.
                        var distance = Mathf.Infinity;
                        float localDistance;
                        for (int i = 0; i < m_Waypoints.Value.Count; ++i) {
                            if ((localDistance = Vector3.Magnitude(transform.position - m_Waypoints.Value[i].transform.position)) < distance) {
                                distance = localDistance;
                                m_WaypointIndex = i;
                            }
                        }
                    }
                }

                SetDestination(m_Waypoints.Value[m_WaypointIndex].transform.position);
            }
        }

        public override TaskStatus OnUpdate()
        {
            // Stay idle if the agent does not have any waypoints.
            if (m_Waypoints.Value.Count == 0) {
                return TaskStatus.Running;
            }

            // Move to the next waypoint if either of the following occurs:
            // - The agent arrives at the destination.
            // - The agent gives up and moves onto the next waypoint. The closer the agent gets to the destination the higher the chances are they will give up. This prevents the agent
            // from always moving into a room when there is a higher likelyhood that nobody is there the closer they get to the middle of the room.
            if (HasArrived()) {
                SetDestination(NextWaypoint());
            } else {
                var distance = (transform.position - m_Waypoints.Value[m_WaypointIndex].transform.position).magnitude;
                if ((distance < m_StartGiveUpDistance.Value && Random.value < m_GiveUpCurve.Evaluate(1 - (distance / m_StartGiveUpDistance.Value)))) {
                    SetDestination(NextWaypoint());
                }
            }
            
            return TaskStatus.Running;
        }

        private Vector3 NextWaypoint()
        {
            if (m_PatrolType == PatrolType.Random) {
                m_WaypointIndex = Random.Range(0, m_Waypoints.Value.Count);
            } else if (m_PatrolType == PatrolType.Sequence) {
                m_WaypointIndex = (m_WaypointIndex + 1) % m_Waypoints.Value.Count;
            } else {
                // Pick a waypoint that is most facing the same direction that the character is already facing.
                var bestIndex = m_WaypointIndex;
                var bestDotProduct = float.NegativeInfinity;
                float dotProduct;
                for (int i = 0; i < m_Waypoints.Value.Count; ++i) {
                    var direction = (m_Waypoints.Value[i].transform.position - transform.position);
                    direction.y = 0;
                    
                    // The higher the dot product the more in front of the current Transform the waypoint is.
                    if ((dotProduct = Vector3.Dot(transform.forward, direction.normalized)) > bestDotProduct) {
                        bestIndex = i;
                        bestDotProduct = dotProduct;
                    }

                    // Break out of the loop if the waypoint is randonmly chosen. This will give the AI some variety.
                    if (Random.value < m_RandomWaypointProbability.Value) {
                        bestIndex = i;
                        break;
                    }
                }
                m_WaypointIndex = bestIndex;
            }
            return m_Waypoints.Value[m_WaypointIndex].transform.position;
        }

        /// <summary>
        /// The task has ended.
        /// </summary>
        public override void OnEnd()
        {
            base.OnEnd();

            // Remember the end position in case the task immediately starts again. In this case a new waypoint should not be chosen.
            m_EndPosition = transform.position;

            // If a TeamManager exists then the game is a team game and the agent should unregister itself as being the leader of the team.
            if (TeamManager.IsInstantiated) {
                TeamManager.SetLeader(gameObject, false);
            }
        }

        /// <summary>
        /// Editor function which will draw all waypoints within the scene view.
        /// </summary>
        public override void OnDrawGizmos()
        {
            if (m_Waypoints.Value == null) {
                return;
            }

            for (int i = 0; i < m_Waypoints.Value.Count; ++i) {
                if (m_Waypoints.Value[i] == null) {
                    continue;
                }
                if (Application.isPlaying && i == m_WaypointIndex) {
                    Gizmos.color = Color.green;
                } else {
                    Gizmos.color = Color.yellow;
                }
                Gizmos.DrawSphere(m_Waypoints.Value[i].transform.position, 0.5f);
            }
        }

    }
}