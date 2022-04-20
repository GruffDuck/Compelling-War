/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Character.Abilities
{
    using Opsive.Shared.Events;
    using Opsive.Shared.Game;
    using Opsive.Shared.StateSystem;
    using Opsive.Shared.Utility;
    using Opsive.UltimateCharacterController.Character;
    using Opsive.UltimateCharacterController.Character.Abilities;
    using Opsive.UltimateCharacterController.Character.Abilities.Items;
    using Opsive.UltimateCharacterController.Objects.CharacterAssist;
    using Opsive.UltimateCharacterController.Utility;
    using UnityEngine;

    /// <summary>
    /// The Cover ability allows the character to take standing and crouching cover. When the character aims they will pop out of cover and aim in the direction of the crosshairs.
    /// </summary>
    [DefaultAbilityIndex(401)]
    [DefaultStartType(AbilityStartType.ButtonDown)]
    [DefaultStopType(AbilityStopType.ButtonToggle)]
    [DefaultUseGravity(AbilityBoolOverride.False)]
    [DefaultDetectHorizontalCollisions(AbilityBoolOverride.False)]
    [DefaultDetectVerticalCollisions(AbilityBoolOverride.False)]
    [DefaultState("Cover")]
    public class Cover : DetectObjectAbilityBase
    {
        /// <summary>
        /// Specifies the current state of the Cover ability.
        /// </summary>
        public enum CoverState
        {
            TakeCoverLookLeft,  // The character is moving into cover position looking left.
            TakeCoverLookRight, // The character is moving into cover position looking right.
            Strafe,             // The character is strafing while in cover.
            AimLeft,            // The character is in cover while aiming left.
            AimRight,           // The character is in cover while aiming right.
            AimCenterLeft,      // The character is in cover while aiming from the center while looking left.
            AimCenterRight,     // The character is in cover while aiming from the center while looking right.
            None
        }

#if UNITY_EDITOR
        [Tooltip("Should a debug ray be drawn for the exposed distance?")]
        [SerializeField] protected bool m_DebugExposedDistanceRay;
#endif
        [Tooltip("The speed at which the character moves towards the cover location.")]
        [SerializeField] protected float m_MoveSpeed = 0.1f;
        [Tooltip("The offset between the cover object and the character.")]
        [SerializeField] protected float m_DepthOffset = 0.3f;
        [Tooltip("The distance when determining which side to aim from.")]
        [SerializeField] protected Vector3 m_ExposedDistance = new Vector3(0.6f, 0, 1);
        [Tooltip("Can the character take crouching cover?")]
        [SerializeField] protected bool m_CanTakeCrouchingCover = true;
        [Tooltip("The value to set the Height Animator parameter value to when crouching.")]
        [SerializeField] protected int m_CrouchHeightParameter = 1;
        [Tooltip("The minimum duration that the character can automatically switch between crouching cover and standing cover based on the cover object.")]
        [SerializeField] protected float m_MinAutoCoverSwitchDuration = 0.5f;
        [Tooltip("The name of the state when the character is aiming while exposed from the cover position.")]
        [SerializeField] protected string m_ExposedStateName = "ExposedCover";

        public float MoveSpeed { get { return m_MoveSpeed; } set { m_MoveSpeed = value; } }
        public float DepthOffset { get { return m_DepthOffset; } set { m_DepthOffset = value; } }
        public Vector3 ExposedDistance { get { return m_ExposedDistance; } set { m_ExposedDistance = value; } }
        public bool CanTakeCrouchingCover { get { return m_CanTakeCrouchingCover; } set { m_CanTakeCrouchingCover = value; } }
        public int CrouchHeightParameter { get { return m_CrouchHeightParameter; } set { m_CrouchHeightParameter = value; } }
        public float MinAutoCoverSwitchDuration { get { return m_MinAutoCoverSwitchDuration; } set { m_MinAutoCoverSwitchDuration = value; } }
        public string ExposedStateName { get { return m_ExposedStateName; } set { m_ExposedStateName = value; } }

        private MoveTowardsLocation[] m_PredeterminedMoveTowardsLocation = new MoveTowardsLocation[1];
        private CoverState m_CoverState = CoverState.None;
        private bool m_InPosition;
        private bool m_LookRight;
        private bool m_AIAgent;
        private bool m_Standing;
        private bool m_AtEdge;
        private float m_LastHorizontalInput;
        private float m_LastStandingSwitch;
        private bool m_Aiming;
        private bool m_RestrictPosition;

        public MoveTowardsLocation PredeterminedMoveTowardsLocation { set { m_PredeterminedMoveTowardsLocation[0] = value; } }
        public CoverState CurrentCoverState { get { return m_CoverState; } }
        public override int AbilityIntData { get => (int)m_CoverState; }
        public override float AbilityFloatData { get => m_LookRight ? 1 : 0; }

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            // AIAgents will have the LocalLookSource.
            m_AIAgent = m_GameObject.GetCachedComponent<LocalLookSource>();
        }

        /// <summary>
        /// Returns the possible MoveTowardsLocations that the character can move towards.
        /// </summary>
        /// <returns>The possible MoveTowardsLocations that the character can move towards.</returns>
        public override MoveTowardsLocation[] GetMoveTowardsLocations()
        {
            // If the Move Towards Location has been specified then an AI is moving to a specific cover location.
            if (m_PredeterminedMoveTowardsLocation[0] != null) {
                return m_PredeterminedMoveTowardsLocation;
            }
            return m_DetectedObject.GetCachedComponents<MoveTowardsLocation>();
        }

        /// <summary>
        /// The ability has started.
        /// </summary>
        protected override void AbilityStarted()
        {
            base.AbilityStarted();

            // Use the MoveTowardsLocation to determine which direction to look.
            var moveTowardsLocation = m_PredeterminedMoveTowardsLocation[0];
            if (m_DetectedObject != null) {
                moveTowardsLocation = m_DetectedObject.GetCachedComponent<MoveTowardsLocation>();
            }
            m_PredeterminedMoveTowardsLocation[0] = null;

            var collisionLayerEnabled = m_CharacterLocomotion.CollisionLayerEnabled;
            m_CharacterLocomotion.EnableColliderCollisionLayer(false);
            var direction = moveTowardsLocation != null ? (moveTowardsLocation.TargetRotation * Vector3.forward) : m_Transform.forward;
            var radius = m_CharacterLocomotion.Radius / 2;
            var collision = Physics.SphereCast(m_Transform.position + m_CharacterLocomotion.Center - direction * radius, radius,
                direction, out m_RaycastResult, radius + m_ExposedDistance.z, m_CharacterLayerManager.SolidObjectLayers, QueryTriggerInteraction.Ignore);
            m_CharacterLocomotion.EnableColliderCollisionLayer(collisionLayerEnabled);
            if (!collision) {
                Debug.Log("Unable to find cover.");
                StopAbility();
                return;
            }

            // Determine if the character should face left or right.
            // Ensure the raycast is clear between the character position and the raycast point. If an object is hit then the character should look to the right.
            var characterPosition = m_Transform.position + m_CharacterLocomotion.Up * m_CharacterLocomotion.Height;
            var position = m_Transform.position + m_CharacterLocomotion.Up * m_CharacterLocomotion.Height + Quaternion.LookRotation(-m_RaycastResult.normal) * new Vector3(m_ExposedDistance.x, 0, 0);
            if (Physics.Raycast(new Ray(characterPosition, position - characterPosition), (position - characterPosition).magnitude, m_DetectLayers, QueryTriggerInteraction.Ignore)) {
                m_LookRight = true;
            } else {
                // No object hit - the characater is free to perform a normal raycast.
                m_LookRight = Physics.Raycast(new Ray(position, -m_RaycastResult.normal), m_ExposedDistance.z * 2, m_DetectLayers, QueryTriggerInteraction.Ignore);
            }

            m_CoverState = m_LookRight ? CoverState.TakeCoverLookRight : CoverState.TakeCoverLookLeft;
            m_InPosition = false;

            SetAbilityIntDataParameter((int)m_CoverState);
            SetAbilityFloatDataParameter(AbilityFloatData);

            // The character should take crouching cover if the cover object isn't tall enough for the character to take standing cover.
            if (m_CanTakeCrouchingCover && !m_CharacterLocomotion.IsAbilityTypeActive<HeightChange>()) {
                position = m_Transform.position + m_CharacterLocomotion.Up * m_CharacterLocomotion.Height;
                var standingCover = Physics.Raycast(new Ray(position, -m_RaycastResult.normal), m_ExposedDistance.z, m_DetectLayers, QueryTriggerInteraction.Ignore);
                SetHeightParameter(standingCover ? 0: m_CrouchHeightParameter);
            }

            EventHandler.RegisterEvent<bool, bool>(m_GameObject, "OnAimAbilityStart", OnAim);
            EventHandler.RegisterEvent(m_GameObject, "OnAnimatorInCoverPosition", OnInPosition);
            EventHandler.RegisterEvent(m_GameObject, "OnAnimatorInExposedCoverAimPosition", OnInExposedAimPosition);
        }

        /// <summary>
        /// The Cover animations are in position.
        /// </summary>
        private void OnInPosition()
        {
            // The ability may toggle quickly between aiming and not aiming.
            if (m_Aiming || m_InPosition) {
                return;
            }
            // The character may be at an edge when the ability starts.
            if (m_CoverState < CoverState.Strafe) {
                CheckForEdge(0);
            }
            m_CoverState = CoverState.Strafe;
            m_InPosition = true;
            SetAbilityIntDataParameter(AbilityIntData);
            m_CharacterLocomotion.ForceRootMotionPosition = m_CharacterLocomotion.ForceRootMotionRotation = false;
            m_CharacterLocomotion.AllowRootMotionPosition = true;
        }

        /// <summary>
        /// Called when another ability is attempting to start and the current ability is active.
        /// Returns true or false depending on if the new ability should be blocked from starting.
        /// </summary>
        /// <param name="startingAbility">The ability that is starting.</param>
        /// <returns>True if the ability should be blocked.</returns>
        public override bool ShouldBlockAbilityStart(Ability startingAbility)
        {
            // The item should not be able to be used when the character is not exposed.
            if ((startingAbility is Use) && m_CoverState <= CoverState.Strafe && (!m_CharacterLocomotion.FirstPersonPerspective || m_AIAgent)) {
                return true;
            }
            // The character should not aim when there is nowhere to aim.
            if ((startingAbility is Aim) && !m_AtEdge) {
                // The character can aim above the cover position while crouching.
                if (!m_CharacterLocomotion.IsAbilityTypeActive<HeightChange>() && !m_Standing) {
                    var position = m_Transform.position + m_CharacterLocomotion.Up * m_CharacterLocomotion.Height;
                    if (Physics.Raycast(new Ray(position, -m_RaycastResult.normal), m_ExposedDistance.z, m_DetectLayers, QueryTriggerInteraction.Ignore)) {
                        return true;
                    }
                } else {
                    // There is nowhere to aim while standing and not at the edge.
                    return true;
                }
            }
            if ((startingAbility is HeightChange) && !m_CanTakeCrouchingCover) {
                return false;
            }
            return base.ShouldBlockAbilityStart(startingAbility);
        }

        /// <summary>
        /// Called when the current ability is attempting to start and another ability is active.
        /// Returns true or false depending on if the active ability should be stopped.
        /// </summary>
        /// <param name="activeAbility">The ability that is currently active.</param>
        /// <returns>True if the ability should be stopped.</returns>
        public override bool ShouldStopActiveAbility(Ability activeAbility)
        {
            if ((activeAbility is HeightChange) && !m_CanTakeCrouchingCover) {
                return true;
            }
            return base.ShouldStopActiveAbility(activeAbility);
        }

        /// <summary>
        /// Updates the ability.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (!m_InPosition) {
                return;
            }

            // Stop cover if there is no more cover object.
            var collisionLayerEnabled = m_CharacterLocomotion.CollisionLayerEnabled;
            m_CharacterLocomotion.EnableColliderCollisionLayer(false);
            var radius = m_CharacterLocomotion.Radius / 2;
            RaycastHit raycastHit;
            var collision = Physics.SphereCast(m_Transform.position + m_CharacterLocomotion.Center + m_RaycastResult.normal * radius,
                                        radius, -m_RaycastResult.normal, out raycastHit, radius + m_ExposedDistance.z, m_DetectLayers, QueryTriggerInteraction.Ignore);
            m_CharacterLocomotion.EnableColliderCollisionLayer(collisionLayerEnabled);

#if UNITY_EDITOR
            if (m_DebugExposedDistanceRay) {
                var raycastRotation = Quaternion.LookRotation(m_RaycastResult.normal, m_CharacterLocomotion.Up);
                var debugPosition = m_Transform.position + m_CharacterLocomotion.Center + m_RaycastResult.normal * m_RaycastResult.distance +
                                    MathUtility.TransformDirection(Vector3.right * m_ExposedDistance.x * (m_LookRight ? 1 : -1), raycastRotation);
                Debug.DrawRay(debugPosition, -m_RaycastResult.normal * (radius + m_ExposedDistance.z), Color.yellow);
            }
#endif
            if (!collision) {
                StopAbility();
                return;
            }

            // If the hit object has changed then the object should be validated again. If the hit object is not valid then the ability should stop.
            if (m_RaycastResult.collider != raycastHit.collider) {
                if (!ValidateObject(m_RaycastResult.collider.gameObject, raycastHit)) {
                    StopAbility();
                    return;
                }
            }
            m_RaycastResult = raycastHit;

            // Cover does not use any y input.
            var inputVector = m_CharacterLocomotion.InputVector;
            inputVector.y = 0;
            if (inputVector.x != 0) {
                m_RaycastResult = raycastHit;
                // While in third person mode the left input should move the character to the right. This is done because the camera will be looking at the character from
                // the front rather than behind.
                if (!m_CharacterLocomotion.FirstPersonPerspective) {
                    inputVector.x = -inputVector.x;
                }

                var lookRight = inputVector.x > 0;
                if (lookRight != m_LookRight) {
                    m_LookRight = lookRight;
                    SetAbilityFloatDataParameter(AbilityFloatData);
                }

                // Do not continue to check if the character is near an edge if the input direction does not change and the character is already at an edge.
                if (!m_AtEdge || m_LastHorizontalInput == 0 || Mathf.Sign(inputVector.x) != Mathf.Sign(m_LastHorizontalInput)) {
                    CheckForEdge(inputVector.x);
                }

                m_LastHorizontalInput = inputVector.x;
                m_RestrictPosition = true;
                if (m_AtEdge) {
                    inputVector = Vector2.zero;
                }
            } else {
                m_LastHorizontalInput = 0;
                m_RestrictPosition = false;
                if (!m_AtEdge) {
                    CheckForEdge(0);
                }
            }
            m_CharacterLocomotion.InputVector = inputVector;

            // The character should take crouching cover if the cover object isn't tall enough for the character to take standing cover.
            // Do not allow the character to quickly switch back and forth between standing and cover. This will prevent root motion from throwing
            // the raycast off.
            if (m_CanTakeCrouchingCover && !m_CharacterLocomotion.IsAbilityTypeActive<HeightChange>() && m_LastStandingSwitch + m_MinAutoCoverSwitchDuration < Time.time) {
                var position = m_Transform.position + m_CharacterLocomotion.Up * m_CharacterLocomotion.Height + m_RaycastResult.normal * radius;
                var standing = Physics.SphereCast(new Ray(position, -m_RaycastResult.normal), radius, radius + m_ExposedDistance.z, m_DetectLayers, QueryTriggerInteraction.Ignore);
                if (standing != m_Standing) {
                    m_Standing = standing;
                    m_LastStandingSwitch = Time.time;
                    SetHeightParameter(m_Standing ? 0 : m_CrouchHeightParameter);
                }
            }
        }

        /// <summary>
        /// Determines if the character is at an edge.
        /// </summary>
        /// <param name="horizontalMovement">The horizontal movement input value.</param>
        private void CheckForEdge(float horizontalMovement)
        {
            // If horizontal movement is 0 then the edge is being checked when the ability starts. Use the look direction instead of the input value.
            if (horizontalMovement == 0) {
                horizontalMovement = m_LookRight ? 1 : -1;
            }

            // The character should not continue to move left or right if the cover object no longer exists.
            var raycastRotation = Quaternion.LookRotation(m_RaycastResult.normal, m_CharacterLocomotion.Up);
            var raycastPosition = m_Transform.position + m_CharacterLocomotion.Center + m_RaycastResult.normal * m_RaycastResult.distance +
                                    MathUtility.TransformDirection(Vector3.right * m_ExposedDistance.x * (horizontalMovement < 0 ? -1 : 1), raycastRotation);
            m_AtEdge = !Physics.Raycast(raycastPosition + m_CharacterLocomotion.Up * (m_Standing ? (m_CharacterLocomotion.Height / 4) : 0),
                    -m_RaycastResult.normal, m_ExposedDistance.z + m_RaycastResult.distance + 0.01f, m_DetectLayers, QueryTriggerInteraction.Ignore);

            // The character may not be at the edge if the character can take crouching cover.
            if (m_Standing && m_AtEdge && m_CanTakeCrouchingCover) {
                m_AtEdge = !Physics.Raycast(raycastPosition, -m_RaycastResult.normal, m_ExposedDistance.z + m_RaycastResult.distance + 0.01f, m_DetectLayers, QueryTriggerInteraction.Ignore);
            }
        }

        /// <summary>
        /// Update the controller's rotation values.
        /// </summary>
        public override void UpdateRotation()
        {
            base.UpdateRotation();

            if (m_Aiming || m_CoverState < CoverState.Strafe) {
                return;
            }

            // The character should always face away from the cover object.
            var raycastNormal = Vector3.ProjectOnPlane(m_RaycastResult.normal, m_CharacterLocomotion.Up).normalized;
            var localLookDirection = m_Transform.InverseTransformDirection(raycastNormal);
            localLookDirection.y = 0;
            var deltaRotation = m_CharacterLocomotion.DeltaRotation;
            deltaRotation.y = MathUtility.ClampInnerAngle(Quaternion.LookRotation(localLookDirection.normalized, m_CharacterLocomotion.Up).eulerAngles.y);
            m_CharacterLocomotion.DeltaRotation = deltaRotation;
        }

        /// <summary>
        /// Update the controller's position values.
        /// </summary>
        public override void UpdatePosition()
        {
            base.UpdatePosition();

            if (m_Aiming || m_CoverState < CoverState.Strafe) {
                return;
            }

            // Position the character so their back is against the wall while in the strafe state.
            var raycastNormal = Vector3.ProjectOnPlane(m_RaycastResult.normal, m_CharacterLocomotion.Up).normalized;
            var targetPosition = MathUtility.TransformPoint(m_RaycastResult.point, Quaternion.LookRotation(-raycastNormal, m_CharacterLocomotion.Up), new Vector3(0, 0, -m_DepthOffset));
            var localTargetPosition = m_Transform.InverseTransformPoint(targetPosition);
            localTargetPosition.y = 0;
            targetPosition = m_Transform.TransformPoint(localTargetPosition);

            Debug.DrawLine(targetPosition, m_Transform.position);
            var deltaPosition = Vector3.MoveTowards(m_Transform.position, targetPosition, m_MoveSpeed) - m_Transform.position;
            m_CharacterLocomotion.AbilityMotor = deltaPosition / (m_CharacterLocomotion.TimeScaleSquared * Time.timeScale * TimeUtility.FramerateDeltaTime);
            m_CharacterLocomotion.MotorThrottle = Vector3.zero;
        }

        /// <summary>
        /// Verify the position values. Called immediately before the position is applied.
        /// </summary>
        public override void ApplyPosition()
        {
            base.ApplyPosition();

            // Prevent the character from moving if they are moving outside the edge.
            if (m_AtEdge && m_RestrictPosition) {
                var localMoveDirection = m_Transform.InverseTransformDirection(m_CharacterLocomotion.MoveDirection);
                if ((m_LookRight && localMoveDirection.x > 0) || (!m_LookRight && localMoveDirection.x < 0)) {
                    localMoveDirection.x = 0;
                    m_CharacterLocomotion.MoveDirection = m_Transform.TransformDirection(localMoveDirection);
                }
            }

            // Prevent the character from moving in front of the cover position.
            var localCoverDirection = MathUtility.InverseTransformPoint(m_RaycastResult.point, Quaternion.LookRotation(m_RaycastResult.normal, m_CharacterLocomotion.Up), m_Transform.position + m_CharacterLocomotion.MoveDirection);
            if (localCoverDirection.z < 0) {
                var localMoveDirection = m_Transform.InverseTransformDirection(m_CharacterLocomotion.MoveDirection);
                localMoveDirection.z = 0;
                m_CharacterLocomotion.MoveDirection = m_Transform.TransformDirection(localMoveDirection);
            }
        }

        /// <summary>
        /// The Aim ability has started or stopped.
        /// </summary>
        /// <param name="start">Has the Aim ability started?</param>
        /// <param name="inputStart">Was the ability started from input?</param>
        private void OnAim(bool aim, bool inputStart)
        {
            if (!m_AIAgent && !inputStart) {
                return;
            }

            if (aim) {
                m_InPosition = false;
                // If the character is at the edge of a low cover object then the regular right/left aiming animation should play.
                // If the character is not at the edge then the character should aim over the cover object.
                if (!m_AtEdge && (m_CharacterLocomotion.IsAbilityTypeActive<HeightChange>() || !m_Standing)) {
                    m_CoverState = m_LookRight ? CoverState.AimCenterRight : CoverState.AimCenterLeft;
                } else {
                    m_CoverState = m_LookRight ? CoverState.AimRight : CoverState.AimLeft;
                }
            } else {
                m_CoverState = CoverState.Strafe;
                m_CharacterLocomotion.AllowRootMotionPosition = false;
                // Ensure the use ability is not active.
                for (int i = m_CharacterLocomotion.ActiveItemAbilities.Length - 1; i >= 0; --i) {
                    if (m_CharacterLocomotion.ActiveItemAbilities[i] is Use) {
                        m_CharacterLocomotion.TryStopAbility(m_CharacterLocomotion.ActiveItemAbilities[i], true);
                    }
                }
                if (!string.IsNullOrWhiteSpace(m_ExposedStateName)) {
                    StateManager.SetState(m_GameObject, m_ExposedStateName, false);
                }
            }
            SetAbilityIntDataParameter(AbilityIntData);
            m_Aiming = aim;
            m_CharacterLocomotion.ForceRootMotionPosition = m_CharacterLocomotion.ForceRootMotionRotation = m_Aiming;
            m_CharacterLocomotion.AbilityMotor = Vector3.zero;
        }

        /// <summary>
        /// The Cover animations are in aim position.
        /// </summary>
        private void OnInExposedAimPosition()
        {
            // The ability may toggle quickly between aiming and not aiming.
            if (!m_Aiming) {
                return;
            }

            // Set the state so other GameObjects can respond to the aim state.
            if (!string.IsNullOrWhiteSpace(m_ExposedStateName)) {
                StateManager.SetState(m_GameObject, m_ExposedStateName, true);
            }
        }

        /// <summary>
        /// The ability has stopped running.
        /// </summary>
        /// <param name="force">Was the ability force stopped?</param>
        protected override void AbilityStopped(bool force)
        {
            base.AbilityStopped(force);

            if (!m_CharacterLocomotion.IsAbilityTypeActive<HeightChange>()) {
                SetHeightParameter(0);
            }
            m_CharacterLocomotion.AbilityMotor = Vector3.zero;
            m_CharacterLocomotion.ForceRootMotionPosition = m_CharacterLocomotion.ForceRootMotionRotation = false;
            EventHandler.UnregisterEvent<bool, bool>(m_GameObject, "OnAimAbilityStart", OnAim);
            EventHandler.UnregisterEvent(m_GameObject, "OnAnimatorInCoverPosition", OnInPosition);
            EventHandler.UnregisterEvent(m_GameObject, "OnAnimatorInExposedCoverAimPosition", OnInExposedAimPosition);
        }
    }
}