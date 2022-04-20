/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.AI
{
    using BehaviorDesigner.Runtime;
    using BehaviorDesigner.Runtime.UltimateCharacterController;
    using Opsive.DeathmatchAIKit.Character.Abilities;
    using Opsive.Shared.Game;
    using Opsive.Shared.Inventory;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Inventory;
    using Opsive.UltimateCharacterController.Items;
    using Opsive.UltimateCharacterController.Items.Actions;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.AI;

    /// <summary>
    /// Extends AIAgent by allowing the agent to detect any targets and keeps the list of possible weapons.
    /// </summary>
    public class DeathmatchAgent : BehaviorTreeAgent
    {
        /// <summary>
        /// Specifies how a Ultimate Character Controller ItemDefinition can be used by the agent.
        /// </summary>
        [System.Serializable]
        public class WeaponStat
        {
            // Specifies the type of weapon.
            public enum WeaponClass { Power, Primary, Secondary, Melee, Grenade }

            [Tooltip("The Ultimate Character Controller ItemDefinition.")]
            [SerializeField] protected ItemDefinitionBase m_ItemDefinition;
            [Tooltip("Specifies the type of weapon.")]
            [SerializeField] protected WeaponClass m_Class = WeaponClass.Primary;
            [Tooltip("A curve describing how likely an item can be used at any distance. The higher the value the more likely that item will be used.")]
            [SerializeField] protected AnimationCurve m_UseLikelihood = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
            [Tooltip("The minimum distance that the item can be used.")]
            [SerializeField] protected float m_MinUseDistance = 5;
            [Tooltip("The maximum distance that the item can be used.")]
            [SerializeField] protected float m_MaxUseDistance = 15;
            [Tooltip("Can the weapon damage multiple targets in one hit?")]
            [SerializeField] protected bool m_GroupDamage;

            public ItemDefinitionBase ItemDefinition { get { return m_ItemDefinition; } set { m_ItemDefinition = value; } }
            public WeaponClass Class { get { return m_Class; } set { m_Class = value; } }
            public float MinUseDistance { get { return m_MinUseDistance; } }
            public float MaxUseDistance { get { return m_MaxUseDistance; } }
            public bool GroupDamage { get { return m_GroupDamage; } }

            private Item m_Item;
            private IUsableItem m_UsableItem;
            private ItemDefinitionBase m_UsableItemDefinition;
            private InventoryBase m_Inventory;
            private int m_ItemSetIndex;
            private float m_PrevTargetDistance = -1;
            private float m_PrevUseLikelihood;

            public Item Item { get { return m_Item; } }
            public IUsableItem UsableItem { get { return m_UsableItem; } }
            public ItemDefinitionBase UsableItemDefinition { get { return m_UsableItemDefinition; } }
            public int ItemSetIndex { get { return m_ItemSetIndex; } }

            /// <summary>
            /// Initializes the WeaponStat to the specified GameObject.
            /// </summary>
            /// <param name="gameObject">The GameObject to initialize the WeaponStat to.</param>
            public void Initialize(GameObject gameObject)
            {
                m_Inventory = gameObject.GetCachedComponent<InventoryBase>();
                var allItems = m_Inventory.GetAllItems();
                for (int i = 0; i < allItems.Count; ++i) {
                    var item = allItems[i];
                    if (item.ItemDefinition != m_ItemDefinition) {
                        continue;
                    }

                    m_Item = item;
                    m_UsableItem = GetUsableItem();
                    if (m_UsableItem != null) {
                        m_UsableItemDefinition = m_UsableItem.GetConsumableItemIdentifier().GetItemDefinition();
                    }
                    break;
                }

                // Determine the ItemSet index so the item can be equipped.
                var itemSetMangaer = gameObject.GetCachedComponent<ItemSetManager>();
                var categoryIndex = -1;
                for (int i = 0; i < itemSetMangaer.CategoryItemSets.Length; ++i) {
                    if (itemSetMangaer.CategoryItemSets[i].ItemCategory == m_Item.ItemDefinition.GetItemCategory()) {
                        categoryIndex = i;
                    }
                }

                m_ItemSetIndex = itemSetMangaer.GetItemSetIndex(m_Item, categoryIndex, false);
            }

            /// <summary>
            /// Returns the first valid IUsableItem attached to the specified item.
            /// </summary>
            /// <returns>The first valid IUsableItem.</returns>
            private IUsableItem GetUsableItem()
            {
                var usableItems = m_Item.GetComponents<IUsableItem>();
                if (usableItems == null) {
                    return null;
                }

                for (int i = 0; i < usableItems.Length; ++i) {
                    // A valid IUsableItem has an Item Identifier.
                    if (usableItems[i].GetConsumableItemIdentifier() != null) {
                        return usableItems[i];
                    }
                }

                return null;
            }

            /// <summary>
            /// Returns the use likelihood based off of the target distance.
            /// </summary>
            /// <param name="targetDistance">The distance that the weapon is being evaluated at.</param>
            /// <returns>The use likelhood based off of the target distance.</returns>
            public float GetUseLikelihood(float targetDistance)
            {
                // Cache the distance to prevent the agent from having to reevaluate the curve every tick.
                if (m_PrevTargetDistance == targetDistance) {
                    return m_PrevUseLikelihood;
                }
                m_PrevTargetDistance = targetDistance;

                // The weapon cannot be used if the distance is out of range.
                if (targetDistance > m_MaxUseDistance || targetDistance < m_MinUseDistance) {
                    m_PrevUseLikelihood = 0;
                } else {
                    // Normalize the target distance based off of the min and max use distance.
                    var normalizedTargetDistance = (targetDistance - m_MinUseDistance) / (m_MaxUseDistance - m_MinUseDistance);
                    m_PrevUseLikelihood = m_UseLikelihood.Evaluate(normalizedTargetDistance);
                }

                return m_PrevUseLikelihood;
            }

            /// <summary>
            /// Returns the total ammo of the usable Item Identifier.
            /// </summary>
            /// <returns>The total ammo of the usable Item Identifier.</returns>
            public int GetTotalAmmo()
            {
                if (m_UsableItem == null) {
                    return 0;
                }
                return m_Inventory.GetItemIdentifierAmount(m_UsableItem.GetConsumableItemIdentifier()) + m_UsableItem.GetConsumableItemIdentifierAmount();
            }
        }

        [Tooltip("Should the DeathmatchAgent add the agent to a team?")]
        [SerializeField] protected bool m_AddToTeam;
        [Tooltip("If AddToTeam is enabled, specifies the team that the agent should be added to.")]
        [SerializeField] protected int m_TeamIndex;
        [Tooltip("The maximum number of colliders that the character can detect.")]
        [SerializeField] protected int m_MaxColliderCount = 16;
        [Tooltip("Optionally specify a transform to determine where to check the line of sight from.")]
        [SerializeField] protected Transform m_LookTransform;
        [Tooltip("If the sight check locates a humanoid, specify the bone that the agent should look at.")]
        [SerializeField] protected HumanBodyBones m_TargetBone = HumanBodyBones.Head;
        [Tooltip("The amount of weight to apply to the distance when determining which target to attack score. The higher the value the more influence the distance has.")]
        [SerializeField] protected float m_DistanceScoreWeight = 2;
        [Tooltip("The amount of weight to apply to the angle when determining which target to attack score. The higher the value the more influence the angle has.")]
        [SerializeField] protected float m_AngleScoreWeight = 1;
        [Tooltip("All possible weapons that the agent can carry.")]
        [SerializeField] protected WeaponStat[] m_AvailableWeapons;

        public bool AddToTeam { set { m_AddToTeam = value; } }
        [Shared.Utility.NonSerialized] public Transform LookTransform { get { return m_LookTransform; } set { m_LookTransform = value; } }
        public HumanBodyBones TargetBone { get { return m_TargetBone; } set { m_TargetBone = value; } }
        public float DistanceScoreWeight { get { return m_DistanceScoreWeight; } set { m_DistanceScoreWeight = value; } }
        public float AngleScoreWeight { get { return m_AngleScoreWeight; } set { m_AngleScoreWeight = value; } }
        [Shared.Utility.NonSerialized] public WeaponStat[] AvailableWeapons { get { return m_AvailableWeapons; } set { m_AvailableWeapons = value; } }

#if UNITY_EDITOR
        // Used by the editor to keep the available weapon selection.
        [SerializeField] protected int m_SelectedAvailableWeapon = -1;
        public int SelectedAvailableWeapon { get { return m_SelectedAvailableWeapon; } set { m_SelectedAvailableWeapon = value; } }
#endif

        private Transform m_Transform;
        private CapsuleCollider m_CapsuleCollider;
        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private CharacterLayerManager m_CharacterLayerManager;
        private Cover m_Cover;
        private CoverPoint m_CoverPoint;
        private Dictionary<ItemDefinitionBase, WeaponStat> m_AvailableWeaponMap;
        private Dictionary<Transform, Transform> m_TransformBoneMap = new Dictionary<Transform, Transform>();
        private Collider[] m_HitColliders;

        [Shared.Utility.NonSerialized] public CoverPoint CoverPoint { get { return m_CoverPoint; } set { m_CoverPoint = value; } }
        private Vector3 LookPosition { get { return (m_CoverPoint == null || m_Cover.CurrentCoverState > Cover.CoverState.Strafe) ? m_LookTransform.position : m_CoverPoint.AttackPosition; } }

        /// <summary>
        /// Initialize default values.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            
            m_Transform = transform;
            m_CharacterLocomotion = gameObject.GetCachedComponent<UltimateCharacterLocomotion>();
            m_CharacterLayerManager = gameObject.GetCachedComponent<CharacterLayerManager>();
            
            if (m_LookTransform == null) {
                m_LookTransform = gameObject.GetCachedComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);
            }

            var capsuleColliderPositioner = GetComponentInChildren<CapsuleColliderPositioner>(true);
            if (capsuleColliderPositioner == null) {
                Debug.LogError($"No CapsuleCollider could be found for {gameObject}. Ensure {gameObject} has a CapsuleColliderPositioner with a CapsuleCollider.");
            }
            m_CapsuleCollider = capsuleColliderPositioner.GetComponent<CapsuleCollider>();
            m_HitColliders = new Collider[m_MaxColliderCount];
        }

        /// <summary>
        /// Finishes initialization.
        /// </summary>
        private void Start()
        {
            // Assign the Cover ability after the abilities have been deserialized.
            var characterLocomotion = gameObject.GetCachedComponent<UltimateCharacterLocomotion>();
            m_Cover = characterLocomotion.GetAbility<Cover>();

            // Initialize the WeaponStat after the weapons have been added.
            m_AvailableWeaponMap = new Dictionary<ItemDefinitionBase, WeaponStat>();
            for (int i = 0; i < m_AvailableWeapons.Length; ++i) {
                m_AvailableWeapons[i].Initialize(gameObject);
                m_AvailableWeaponMap.Add(m_AvailableWeapons[i].ItemDefinition, m_AvailableWeapons[i]);
            }

            if (m_AddToTeam) {
                TeamManager.AddTeamMember(gameObject, m_TeamIndex);
            }

            // Start the behavior tree.
            var behaviorTree = gameObject.GetComponent<BehaviorTree>();
            behaviorTree.EnableBehavior();
        }

        /// <summary>
        /// Returns any targets in sight. If multiple targets are in sight the target most directly in front of the agent will be returned.
        /// </summary>
        /// <param name="fieldOfView">The maximum field of view that the agent can see other targets (in degrees).</param>
        /// <param name="maxDistance">The maximum distance that the agent can see other targets.</param>
        /// <param name="ignoreTargets">The targets that can be ignored.</param>
        /// <returns>A reference to the target in sight. Can be null if no targets are in sight.</returns>
        public GameObject TargetInSight(float fieldOfView, float maxDistance, HashSet<GameObject> ignoreTargets)
        {
            GameObject foundTarget = null;
            // Find all of the nearby objects with the specified layer mask.
            var hitCount = Physics.OverlapSphereNonAlloc(LookPosition, maxDistance, m_HitColliders, m_CharacterLayerManager.EnemyLayers, QueryTriggerInteraction.Ignore);
            if (hitCount > 0) {
                // The angle and distance are normalized to get a 0 - 2 value. The target with the highest value will be returned.
                var highestScore = Mathf.NegativeInfinity;
                var maxDistanceSqr = maxDistance * maxDistance;
                var fieldOfViewHalf = fieldOfView * 0.5f;
                for (int i = 0; i < hitCount; ++i) {
                    var target = m_HitColliders[i].gameObject.GetCachedComponent<CapsuleColliderPositioner>() != null ? m_HitColliders[i].transform.parent.parent : m_HitColliders[i].transform;
                    // The agent cannot see themself.
                    if (target == m_Transform) {
                        continue;
                    }

                    // Don't return any targets that are within the ignore list.
                    if (ignoreTargets.Contains(target.gameObject)) {
                        continue;
                    }
                    
                    float angle;
                    // The position should only be checked when cover is not active. The cover attack point will always be valid so no check is necessary.
                    if (TargetInSight(target, m_CoverPoint == null, fieldOfView, maxDistanceSqr, out angle)) {
                        // The object is in sight, determine the score to determine which is the best target.
                        var score = ((1 - ((target.position - LookPosition).magnitude / maxDistance)) * m_DistanceScoreWeight) + 
                                    ((1 - (angle / fieldOfViewHalf)) * m_AngleScoreWeight);
                        if (score > highestScore) {
                            highestScore = score;
                            foundTarget = target.gameObject;
                        }
                    }
                }
            }
            return foundTarget;
        }

        /// <summary>
        /// Is the specified target in sight?
        /// </summary>
        /// <param name="target">The target to determine if it is in sight.</param>
        /// <returns>True if the target is in sight.</returns>
        public bool TargetInSight(Transform target, float fieldOfView)
        {
            float angle;
            return TargetInSight(target, false, fieldOfView, float.MaxValue, out angle);
        }

        /// <summary>
        /// Is the specified target in sight?
        /// </summary>
        /// <param name="target">The target to determine if it is in sight.</param>
        /// <param name="checkPosition">Should the pathfinding position be checked?</param>
        /// <param name="fieldOfView">The maximum field of view that the agent can see other targets (in degrees).</param>
        /// <param name="maxDistanceSqr">The maximum distance squared that the agent can see other targets.</param>
        /// <param name="angle">If a target is within sight, specifies the angle difference between the target and the current agent.</param>
        /// <returns>True if the target is in sight.</returns>
        private bool TargetInSight(Transform target, bool checkPosition, float fieldOfView, float maxDistanceSqr, out float angle)
        {
            // The target has to exist.
            if (target == null) {
                angle = 0;
                return false;
            }

            // The target has to be within distance.
            var direction = target.position - LookPosition;
            if (direction.sqrMagnitude > maxDistanceSqr) {
                angle = 0;
                return false;
            }

            // The target has to be in the field of view.
            direction.y = 0;
            angle = Vector3.Angle(direction.normalized, m_CoverPoint == null ? m_Transform.forward : m_CoverPoint.transform.forward);
            if (angle > fieldOfView * 0.5f) {
                return false;
            }

            // The target has to be in line of sight.
            return LineOfSight(target, checkPosition) != null;
        }

        /// <summary>
        /// Is the specified target within line of sight?
        /// </summary>
        /// <param name="target">The target to determine if it is within sight.</param>
        /// <param name="checkPosition">Should the pathfinding position be checked?</param>
        /// <returns>True if the target is within line of sight.</returns>
        public Transform LineOfSight(Transform target, bool checkPosition)
        {
            return LineOfSight(m_CoverPoint == null ? m_Transform.position : m_CoverPoint.AttackPosition, target, checkPosition);
        }

        /// <summary>
        /// Is the position within within line of sight of hte target?
        /// </summary>
        /// <param name="position">The position to check against.</param>
        /// <param name="target">The target to determine if it is within sight.</param>
        /// <param name="checkPosition">Should the pathfinding position be checked?</param>
        /// <returns>True if the target is within line of sight.</returns>
        public Transform LineOfSight(Vector3 position, Transform target, bool checkPosition)
        {
            if (position == target.transform.position) {
                return target;
            }

            // Temporarily set the character to a layer not being checked. This will prevent the line of sight from hitting the character.
            m_CharacterLocomotion.EnableColliderCollisionLayer(false);

            // Set the local z offset to 0 to prevent the line of sight check from intersecting with a wall immediately in front of the character.
            var offset = Vector3.zero;
            if (m_CoverPoint == null) {
                offset = m_Transform.InverseTransformPoint(m_LookTransform.position);
                offset.z = 0;
                offset = m_Transform.TransformPoint(offset) - m_Transform.position;
            }

            // Sample a pathfinding position to ensure the position will be valid. Do this instead of immediately firing the raycast because the position
            // may be in an invalid pathfinding location.
            var bonePosition = GetTargetBoneTransform(target).position;
            RaycastHit hit;
            var hitTarget = false;
            if (checkPosition) {
                NavMeshHit navmeshHit;
                if (NavMesh.SamplePosition(position, out navmeshHit, m_CapsuleCollider.height * 2, NavMesh.AllAreas)) {
                    position = navmeshHit.position;
                    if (Physics.Linecast(position + offset, bonePosition, out hit, m_CharacterLayerManager.IgnoreInvisibleCharacterLayers, QueryTriggerInteraction.Ignore)) {
                        hitTarget = hit.transform.IsChildOf(target);
                    } else {
                        hitTarget = true;
                    }
                    Debug.DrawLine(position + offset, bonePosition, hitTarget ? Color.green : Color.red);
                }
            } else {
                if (Physics.Linecast(position + offset, bonePosition, out hit, m_CharacterLayerManager.IgnoreInvisibleCharacterLayers, QueryTriggerInteraction.Ignore)) {
                    hitTarget = hit.transform.IsChildOf(target);
                } else {
                    hitTarget = true;
                }
                Debug.DrawLine(position + offset, bonePosition, hitTarget ? Color.green : Color.red);
            }
            m_CharacterLocomotion.EnableColliderCollisionLayer(true);
            return hitTarget ? target : null;
        }

        /// <summary>
        /// Returns the bone transform for the specified Transform.
        /// </summary>
        /// <param name="target">The Transform to return the bone transform of.</param>
        /// <returns>The bone transform for the specified Transform. If the Transform does not have an Animator the Transform will be returned.</returns>
        public Transform GetTargetBoneTransform(Transform target)
        {
            Transform bone;
            if (!m_TransformBoneMap.TryGetValue(target, out bone)) {
                var targetAnimator = target.gameObject.GetCachedParentComponent<Animator>();
                if (targetAnimator != null) {
                    bone = targetAnimator.GetBoneTransform(m_TargetBone);
                    // The bone may be null for generic characters.
                    if (bone == null) {
                        bone = target;
                    }
                    m_TransformBoneMap.Add(target, bone);
                } else {
                    // If no Animator exists then the current target is considered the target bone.
                    m_TransformBoneMap.Add(target, target);
                    return target;
                }
            }
            return bone;
        }

        /// <summary>
        /// Returns the WeaponStat for the specified ItemDefinition.
        /// </summary>
        /// <param name="itemType">The ItemDefinition to get the WeaponStat of.</param>
        /// <returns>The WeaponStat for the specified ItemDefinition.</returns>
        public WeaponStat WeaponStatForItemDefinition(ItemDefinitionBase itemDefinition)
        {
            if (itemDefinition == null) {
                return null;
            }
            WeaponStat weaponStat;
            if (!m_AvailableWeaponMap.TryGetValue(itemDefinition, out weaponStat)) {
                return null;
            }
            return weaponStat;
        }

        /// <summary>
        /// Returns the WeaponStat for the specified WeaponClass.
        /// </summary>
        /// <param name="weaponClass">The WeaponClass to get the WeaponStat of.</param>
        /// <returns>The WeaponStat for the specified WeaponClass.</returns>
        public WeaponStat WeaponStatForClass(WeaponStat.WeaponClass weaponClass)
        {
            for (int i = 0; i < m_AvailableWeaponMap.Count; ++i) {
                if (m_AvailableWeapons[i].Class == weaponClass) {
                    return m_AvailableWeapons[i];
                }
            }
            return null;
        }
    }
}