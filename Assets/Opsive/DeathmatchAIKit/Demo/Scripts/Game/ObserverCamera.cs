/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.DeathmatchAIKit.Demo.Game
{
    using Opsive.UltimateCharacterController.Game;
    using UnityEngine;

    /// <summary>
    /// Allows the camera to free fly while observering the deathmatch game.
    /// </summary>
    public class ObserverCamera : MonoBehaviour
    {
        [Tooltip("The name of the horizontal movement input.")]
        [SerializeField] protected string m_HorizontalInputName = "Horizontal";
        [Tooltip("The name of the veritcal movement input.")]
        [SerializeField] protected string m_ForwardInputName = "Vertical";
        [Tooltip("The name of the yaw input.")]
        [SerializeField] protected string m_YawInputName = "Mouse X";
        [Tooltip("The name of the pitch input.")]
        [SerializeField] protected string m_PitchInputName = "Mouse Y";
        [Tooltip("The amount of smoothing to apply to the movement. Can be zero.")]
        [SerializeField] protected float m_MoveSmoothing = 0.1f;
        [Tooltip("The amount of smoothing to apply to the pitch and yaw. Can be zero.")]
        [SerializeField] protected float m_TurnSmoothing = 0.05f;
        [Tooltip("The speed at which the camera turns.")]
        [SerializeField] protected float m_TurnSpeed = 1.5f;
        [Tooltip("The radius of the camera's collision sphere to prevent it from clipping with other objects.")]
        [SerializeField] protected float m_CollisionRadius = 0.01f;
        [Tooltip("Specifies the layers that can obstruct the camera's view.")]
        [SerializeField] protected LayerMask m_ObstructionMask = ~((1 << LayerManager.TransparentFX) | (1 << LayerManager.IgnoreRaycast) | (1 << LayerManager.UI) | 
                                                                   (1 << LayerManager.VisualEffect) | (1 << LayerManager.Overlay));

        private Transform m_Transform;

        private float m_Yaw;
        private float m_Pitch;

        private float m_SmoothX;
        private float m_SmoothY;
        private float m_SmoothXVelocity;
        private float m_SmoothYVelocity;
        private float m_SmoothHorizontal;
        private float m_SmoothForward;
        private float m_SmoothHorizontalVelocity;
        private float m_SmoothForwardVelocity;

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Awake()
        {
            m_Transform = transform;
        }

        /// <summary>
        /// Performs the camera rotation and movement.
        /// </summary>
        /// <param name="horizontalMovement">The horizontal movement delta.</param>
        /// <param name="forwardMovement">The forward movement delta.</param>
        /// <param name="pitch">The pitch delta.</param>
        /// <param name="yaw">The yaw delta.</param>
        private void Update()
        {
            if (Time.deltaTime == 0) {
                return;
            }

            m_SmoothHorizontal = Mathf.SmoothDamp(m_SmoothHorizontal, Input.GetAxis(m_HorizontalInputName), ref m_SmoothHorizontalVelocity, m_MoveSmoothing);
            m_SmoothForward = Mathf.SmoothDamp(m_SmoothForward, Input.GetAxis(m_ForwardInputName), ref m_SmoothForwardVelocity, m_MoveSmoothing);
            m_SmoothX = Mathf.SmoothDamp(m_SmoothX, Input.GetAxis(m_YawInputName), ref m_SmoothXVelocity, m_TurnSmoothing);
            m_SmoothY = Mathf.SmoothDamp(m_SmoothY, Input.GetAxis(m_PitchInputName), ref m_SmoothYVelocity, m_TurnSmoothing);

            UpdateRotation();

            UpdateMovement();
        }

        /// <summary>
        /// Rotate the camera.
        /// </summary>
        private void UpdateRotation()
        {
            // The rotation can only happen so fast.
            m_Pitch += m_SmoothY * m_TurnSpeed * -1;
            m_Yaw += m_SmoothX * m_TurnSpeed;
            m_Transform.rotation = Quaternion.Euler(m_Pitch, m_Yaw, 0);
        }

        /// <summary>
        /// Move the camera.
        /// </summary>
        private void UpdateMovement()
        {
            var targetPosition = m_Transform.position + m_Transform.TransformDirection(m_SmoothHorizontal, 0, m_SmoothForward);
            m_Transform.position = ValidateMovement(targetPosition);
        }

        /// <summary>
        /// Ensure the move direction is valid - don't allow the camera to run into a wall.
        /// </summary>
        /// <param name="moveDirection">The target move direction.</param>
        /// <returns>The valid position.</returns>
        private Vector3 ValidateMovement(Vector3 targetPosition)
        {
            var direction = targetPosition - m_Transform.position;
            RaycastHit hit;
            if (Physics.SphereCast(m_Transform.position - direction.normalized * m_CollisionRadius, m_CollisionRadius, direction.normalized, out hit, m_CollisionRadius + direction.magnitude, m_ObstructionMask)) {
                targetPosition = hit.point + hit.normal * m_CollisionRadius;
            }
            return targetPosition;
        }
    }
}