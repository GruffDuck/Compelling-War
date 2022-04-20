/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI.Actions
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.Tasks;
    using Opsive.Shared.Game;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Inventory;
    using UnityEngine;

    using Tooltip = BehaviorDesigner.Runtime.Tasks.TooltipAttribute;

    [TaskCategory("Deathmatch AI Kit")]
    [TaskDescription("Moves into attack position.")]
    [TaskIcon("Assets/Opsive/DeathmatchAIKit/Editor/Images/DeathmatchAIKitIcon.png")]
    public class AttackMovement : SpeedChangeMovement
    {
        [Tooltip("The GameObject that is being attacked.")]
        [SerializeField] protected SharedGameObject m_Target;
        [Tooltip("The ItemDefinition used to attack.")]
        [SerializeField] protected SharedFloat m_EvadeAngle = 15;
        [Tooltip("Specifies the minimum angle that the agent should move when determining a new destination.")]
        [SerializeField] protected SharedFloat m_MinMoveAngle = 10;
        [Tooltip("Specifies the maximum angle that the agent should move when determining a new destination.")]
        [SerializeField] protected SharedFloat m_MaxMoveAngle = 20;
        [Tooltip("When the agent looks for a new destination a new position is checked to determine if it's a valid position. If more than the threshold number of positions are checked " + 
                 "then angles which are closer to the agent's current position will be checked.")]
        [SerializeField] protected SharedInt m_SmallAngleCount = 10;
        [Tooltip("Specifies the distance that the new destination check should increase upon each interation. This is only used if the iteration count is greater than the SmallMovementThreshold.")]
        [SerializeField] protected SharedFloat m_DistanceStep = 0.01f;
        [Tooltip("Likelihood that the distance value will be randomly updated.")]
        [SerializeField] protected SharedFloat m_DistanceUpdateLikelihood = 0.2f;
        [Tooltip("Prevent the agent from getting stuck in the same position by multiplying the new destination location by the specified multiplier if the number of distance checks is greater " +
                 "than the evade threshold.")]
        [SerializeField] protected SharedFloat m_StuckIterationCount = 10;
        [Tooltip("Prevent the agent from getting stuck in the same position by multiplying the new destination location by the specified multiplier if the number of distance checks is greater " +
                 "than the evade threshold.")]
        [SerializeField] protected SharedFloat m_StuckMultiplier = 4;
        [Tooltip("Is the target within sight?")]
        [SharedRequired] [SerializeField] protected SharedBool m_TargetInSight;

        private DeathmatchAgent m_DeathmatchAgent;
        private ItemSetManagerBase m_ItemSetManager;
        private InventoryBase m_Inventory;
        private LocalLookSource m_LocalLookSource;
        private GameObject m_PrevTarget;
        private GameObject m_TargetParent;

        private bool m_PositiveAngle;
        private float m_Distance = float.MaxValue;
        private Vector3 m_Destination;
        private Vector3 m_LookDirection;
        private Vector3 m_EvadeDestination;
        private bool m_SmallAngles;
        private int m_EvadeCount;
        private float m_DistancePercent;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void OnAwake()
        {
            base.OnAwake();

            m_PositiveAngle = Random.value < 0.5f;

            m_DeathmatchAgent = gameObject.GetCachedComponent<DeathmatchAgent>();
            m_LocalLookSource = gameObject.GetCachedComponent<LocalLookSource>();
            m_ItemSetManager = gameObject.GetCachedComponent<ItemSetManagerBase>();
            m_Inventory = gameObject.GetCachedComponent<InventoryBase>();
        }

        /// <summary>
        /// Initialize the values which should be updated when the task starts.
        /// </summary>
        public override void OnStart()
        {
            base.OnStart();

            m_PrevTarget = m_Target.Value;
            m_TargetParent = m_PrevTarget.GetCachedParentComponent<UltimateCharacterLocomotion>().gameObject;
            m_Destination = m_Target.Value.transform.position;
        }

        /// <summary>
        /// Move into attack position.
        /// </summary>
        /// <returns>Returns Failure if a new destination cannot be determined.</returns>
        public override TaskStatus OnUpdate()
        {
            // The target may have updated since the last tick.
            if (m_Target.Value != m_PrevTarget) {
                m_PrevTarget = m_Target.Value;
                m_TargetParent = m_PrevTarget.GetCachedParentComponent<UltimateCharacterLocomotion>().gameObject;
            }

            // The WeaponStat is used to determine the min and max distance away from the target that the agent should move to.
            DeathmatchAgent.WeaponStat weaponStat = null;
            for (int i = 0; i < m_Inventory.SlotCount; ++i) {
                var itemIdetifier = m_ItemSetManager.GetEquipItemIdentifier(i);
                if (itemIdetifier == null) {
                    continue;
                }
                weaponStat = m_DeathmatchAgent.WeaponStatForItemDefinition(itemIdetifier.GetItemDefinition());
                break;
            }
            if (weaponStat == null) {
                return TaskStatus.Failure;
            }

            // The look direction should ignore the y offset.
            m_LookDirection = transform.position - m_TargetParent.transform.position;
            m_LookDirection.y = 0;
            var distance = Mathf.Clamp(m_LookDirection.magnitude, weaponStat.MinUseDistance, weaponStat.MaxUseDistance);
            // Only update the distance if it is less than the previous distance value. This allows the agent to readjust when the enemy moves away. 
            if (distance < m_Distance || Random.value < m_DistanceUpdateLikelihood.Value) {
                m_Distance = distance;
            }

            var setDestination = false;
            // Start to evade the target as soon as the target is looking at the agent.
            if (Quaternion.Angle(Quaternion.LookRotation(m_LookDirection), m_TargetParent.transform.rotation) < m_EvadeAngle.Value || !m_TargetInSight.Value) {
                // Keep moving to evade the target for as long as the target is looking at the agent.
                if (!HasPath()) {
                    if (!NextDestination(weaponStat, true)) {
                        return TaskStatus.Failure;
                    }
                    setDestination = true;
                }
            } else {
                m_EvadeCount = 0;
            }

            if (!setDestination) {
                // Determine a new destination if the destination hasn't been set yet, the destination is no longer within sight of the target, or the target is too far away.
                distance = (m_Destination - m_Target.Value.transform.position).magnitude;
                if (distance < 0.01f || distance > m_Distance || m_DeathmatchAgent.LineOfSight(m_Destination, m_TargetParent.transform, true) == null) {
                    if (!NextDestination(weaponStat, false)) {
                        return TaskStatus.Failure;
                    }
                    setDestination = true;
                }
            }

            // Keep looking at the target.
            m_LocalLookSource.Target = m_Target.Value.transform;

            // Move into position.
            if (setDestination) {
                UpdateRotation(false);
                SetDestination(m_Destination);
            }

            return TaskStatus.Running;
        }

        /// <summary>
        /// Determine a new destination which has the target within sight.
        /// </summary>
        /// <param name="weaponStat">The WeaponStat of the current item.</param>
        /// <param name="newAngle">Should a new angle be retrieved? Will be false on first run.</param>
        /// <returns>True if a new destination was retrieved. The destination will be stored in m_Destination.</returns>
        private bool NextDestination(DeathmatchAgent.WeaponStat weaponStat, bool newAngle)
        {
            m_DistancePercent = 1;
            m_SmallAngles = false;
            var count = 0;
            var direction = m_LookDirection;
            var angle = 0f;
            var flipAngle = false;
            // Keep iterating to find a position which has the target within sight.
            do {
                // Do not flip the angle the first time NextDestination is called so the character can continue to move in the direction that they
                // were previously moving in.
                if (flipAngle) {
                    m_PositiveAngle = !m_PositiveAngle;
                }

                if (newAngle) {
                    if (count > m_SmallAngleCount.Value) {
                        m_SmallAngles = true;
                        m_DistancePercent = Mathf.Clamp01(m_DistancePercent - m_DistanceStep.Value);
                    }
                    if (m_SmallAngles) {
                        angle = Random.Range(0, m_MinMoveAngle.Value);
                    } else if (m_EvadeCount > m_StuckIterationCount.Value) {
                        angle = Random.Range(m_MinMoveAngle.Value, m_MaxMoveAngle.Value) * m_StuckMultiplier.Value;
                    } else {
                        angle = Random.Range(m_MinMoveAngle.Value, m_MaxMoveAngle.Value);
                    }
                    angle *= (m_PositiveAngle ? 1 : -1);
                }

                // Add the random angle to the last direction.
                var lookRotation = Quaternion.LookRotation(direction).eulerAngles;
                lookRotation.y += angle;
                direction = Quaternion.Euler(lookRotation) * Vector3.forward;

                // Set the new destination based on the random direction.
                m_Destination = m_TargetParent.transform.position + direction.normalized * Mathf.Clamp(m_Distance * m_DistancePercent, weaponStat.MinUseDistance, weaponStat.MaxUseDistance);

                count++;
                flipAngle = true;
                newAngle = true;
            } while (m_DeathmatchAgent.LineOfSight(m_Destination, m_TargetParent.transform, true) == null && count < 100);

            // Prevent the agent from getting stuck in the same position by increasing the evade count. As soon as the evade count gets too high
            // the character will start choose a wider angle.
            if (m_EvadeCount == 0) {
                m_EvadeDestination = m_Destination;
                m_EvadeCount++;
            } else {
                if ((m_EvadeDestination - m_Destination).magnitude < 1) {
                    m_EvadeCount++;
                } else {
                    m_EvadeCount = 0;
                }
            }
            return count < 100;
        }

        /// <summary>
        /// The task has ended.
        /// </summary>
        public override void OnEnd()
        {
            base.OnEnd();

            UpdateRotation(true);
            m_Distance = float.MaxValue;
            m_EvadeCount = 0;
        }
    }
}