/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using BehaviorDesigner.Runtime.Tasks.Movement;
    using Opsive.Shared.Events;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Character.Abilities.AI;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Create a V formation around the leader.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class VFormation : SpeedChangeMovement
    {
        [Tooltip("The leader to follow")]
        [SerializeField] protected SharedGameObject m_Leader;
        [Tooltip("The separation between agents")]
        [SerializeField] protected SharedVector2 m_Separation = new Vector2(1, -1);
        [Tooltip("Start moving into formation if the leader has moved more than the specified distance")]
        [SerializeField] protected SharedFloat m_LeaderMoveDistance = 2;
        
        private GameObject m_PrevLeader;
        private BehaviorTree m_LeaderTree;
        private Transform m_LeaderTransform;
        private PathfindingMovement m_PatfindingMovement;

        private Vector3 m_LeaderPosition;
        private bool m_SendListenerEvent;
        private TaskStatus m_RunStatus;
        private int m_Index;
        private bool m_HasMoved;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            base.OnAwake();

            var characterLocomotion = gameObject.GetCachedComponent<UltimateCharacterLocomotion>();
            m_PatfindingMovement = characterLocomotion.GetAbility<PathfindingMovement>();

            // Force MoveIntoPosition to execute on the first run.
            m_LeaderPosition = Vector3.one * float.MaxValue;

            EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnDeath);
        }

        /// <summary>
        /// Initializes the default values and registers for any interested events.
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            if (m_Leader.Value != m_PrevLeader) {
                m_PrevLeader = m_Leader.Value;
                m_LeaderTree = m_Leader.Value.GetComponent<BehaviorTree>();
                m_LeaderTransform = m_Leader.Value.transform;
            }

            if (m_LeaderTree != null) {
                EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(m_Leader.Value, "OnDeath", OnLeaderDeath);
                m_SendListenerEvent = true;
            }
            m_RunStatus = TaskStatus.Running;

            Owner.RegisterEvent<int>("FormationUpdated", FormationUpdated);
            m_Index = TeamManager.AddToFormation(m_Leader.Value, Owner);
        }

        /// <summary>
        /// Move in a V formation behind the leader.
        /// </summary>
        /// <returns>Running until the leader indicates that the orders have been finished.</returns>
        public override TaskStatus OnUpdate()
        {
            // Send within OnUpdate to ensure the at least one leader behavior tree is active. If registered within OnStart there is a chance that the behavior tree
            // isn't active yet and will never receive the event.
            if (m_SendListenerEvent) {
                if (m_LeaderTree.ExecutionStatus == TaskStatus.Running) {
                    m_LeaderTree.SendEvent<GameObject>("StartListeningForOrders", gameObject);
                    m_SendListenerEvent = false;
                }
            }

            // Form a V behind the leader.
            MoveIntoPosition();
            if (HasArrived()) {
                if (m_HasMoved) {
                    UpdateRotation(false);
                    m_LeaderPosition = m_LeaderTransform.position;
                    m_HasMoved = false;
                }
            }

            return m_RunStatus;
        }

        /// <summary>
        /// Moves the agent into position.
        /// </summary>
        private void MoveIntoPosition()
        {
            if ((m_LeaderTransform.position - m_LeaderPosition).magnitude > m_LeaderMoveDistance.Value) {
                var roundedIndex = Mathf.CeilToInt((float)(m_Index - 1) / 2) + 1;
                var position = m_LeaderTransform.TransformPoint(m_Separation.Value.x * (m_Index % 2 == 0 ? -1 : 1) * roundedIndex, 0,
                                                                m_Separation.Value.y * roundedIndex);
                SetDestination(position);
                UpdateRotation(true);
                m_PatfindingMovement.SetDestinationRotation(m_LeaderTransform.rotation);

                // Remember the leader position and rotation. This value won't be correct any time the leader moves within the leader move distance but
                // the agent should not continuously adjust to every little move.
                m_LeaderPosition = m_LeaderTransform.position;
                m_HasMoved = true;
            }
        }

        /// <summary>
        /// A formation member agent left the formation. Update the index to stay in a V-formation.
        /// <param name="index">The new formation index.</param>
        /// </summary>
        private void FormationUpdated(int index)
        {
            m_Index = index;
        }

        /// <summary>
        /// The task has ended. Notify the leader that the current agent is no longer listening.
        /// </summary>
        public override void OnEnd()
        {
            if (m_LeaderTree != null) {
                m_LeaderTree.SendEvent<GameObject>("StopListeningToOrders", gameObject);
                EventHandler.UnregisterEvent<Vector3, Vector3, GameObject>(m_Leader.Value, "OnDeath", OnLeaderDeath);
            }
            Owner.UnregisterEvent<int>("FormationUpdated", FormationUpdated);
            TeamManager.RemoveFromFormation(m_Leader.Value, Owner);
            UpdateRotation(true);
        }

        /// <summary>
        /// The character has died.
        /// </summary>
        /// <param name="position">The position of the force.</param>
        /// <param name="force">The amount of force which killed the character.</param>
        /// <param name="attacker">The GameObject that killed the character.</param>
        private void OnDeath(Vector3 position, Vector3 force, GameObject attacker)
        {
            // Force MoveIntoPosition to execute again.
            m_LeaderPosition = Vector3.one * float.MaxValue;
        }

        /// <summary>
        /// The leader has died.
        /// </summary>
        /// <param name="position">The position of the force.</param>
        /// <param name="force">The amount of force which killed the character.</param>
        /// <param name="attacker">The GameObject that killed the character.</param>
        private void OnLeaderDeath(Vector3 position, Vector3 force, GameObject attacker)
        {
            m_RunStatus = TaskStatus.Failure;
        }

        /// <summary>
        /// The behavior tree has completed.
        /// </summary>
        public override void OnBehaviorComplete()
        {
            base.OnBehaviorComplete();

            EventHandler.UnregisterEvent<Vector3, Vector3, GameObject>(gameObject, "OnDeath", OnDeath);
        }

        /// <summary>
        /// Reset the Behavior Designer variables.
        /// </summary>
        public override void OnReset()
        {
            m_Leader = null;
        }
    }
}