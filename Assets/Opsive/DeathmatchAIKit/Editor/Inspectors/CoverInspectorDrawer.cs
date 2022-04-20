/// ---------------------------------------------
/// Deathmatch AI Kit
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateCharacterController.Editor.Inspectors.Character.Abilities
{
	using Opsive.UltimateCharacterController.Editor.Utility;
	using UnityEditor;
	using UnityEditor.Animations;
	using UnityEngine;

	/// <summary>
	/// Draws a custom inspector for the Cover Ability.
	/// </summary>
	[Shared.Editor.Inspectors.InspectorDrawer(typeof(Opsive.DeathmatchAIKit.Character.Abilities.Cover))]
	public class CoverInspectorDrawer : DetectObjectAbilityBaseInspectorDrawer
	{
		// ------------------------------------------- Start Generated Code -------------------------------------------
		// ------- Do NOT make any changes below. Changes will be removed when the animator is generated again. -------
		// ------------------------------------------------------------------------------------------------------------

		/// <summary>
		/// Returns true if the ability can build to the animator.
		/// </summary>
		public override bool CanBuildAnimator { get { return true; } }

		/// <summary>
		/// An editor only method which can add the abilities states/transitions to the animator.
		/// </summary>
		/// <param name="animatorController">The Animator Controller to add the states to.</param>
		/// <param name="firstPersonAnimatorController">The first person Animator Controller to add the states to.</param>
		public override void BuildAnimator(AnimatorController animatorController, AnimatorController firstPersonAnimatorController)
		{
			var baseStateMachine1824871762 = animatorController.layers[0].stateMachine;

			// The state machine should start fresh.
			for (int i = 0; i < animatorController.layers.Length; ++i) {
				for (int j = 0; j < baseStateMachine1824871762.stateMachines.Length; ++j) {
					if (baseStateMachine1824871762.stateMachines[j].stateMachine.name == "Cover") {
						baseStateMachine1824871762.RemoveStateMachine(baseStateMachine1824871762.stateMachines[j].stateMachine);
						break;
					}
				}
			}

			// AnimationClip references.
			var crouchingCoverCenterLeftAimAnimationClip7278Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
			var crouchingCoverCenterLeftAimAnimationClip7278 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterLeftAimAnimationClip7278Path, "CrouchingCoverCenterLeftAim");
			var crouchingCoverCenterLeftAimReturnAnimationClip7282Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
			var crouchingCoverCenterLeftAimReturnAnimationClip7282 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterLeftAimReturnAnimationClip7282Path, "CrouchingCoverCenterLeftAimReturn");
			var crouchingCoverAimLeftHoldAnimationClip18946Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
			var crouchingCoverAimLeftHoldAnimationClip18946 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimLeftHoldAnimationClip18946Path, "CrouchingCoverAimLeftHold");
			var crouchingCoverAimRightAnimationClip18950Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
			var crouchingCoverAimRightAnimationClip18950 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimRightAnimationClip18950Path, "CrouchingCoverAimRight");
			var crouchingCoverAimRightHoldAnimationClip18952Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
			var crouchingCoverAimRightHoldAnimationClip18952 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimRightHoldAnimationClip18952Path, "CrouchingCoverAimRightHold");
			var crouchingCoverAimRightReturnAnimationClip18954Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
			var crouchingCoverAimRightReturnAnimationClip18954 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimRightReturnAnimationClip18954Path, "CrouchingCoverAimRightReturn");
			var crouchingCoverIdleLeftAnimationClip9640Path = AssetDatabase.GUIDToAssetPath("e9ee80e39ac40244b9ace63bd92ee356"); 
			var crouchingCoverIdleLeftAnimationClip9640 = AnimatorBuilder.GetAnimationClip(crouchingCoverIdleLeftAnimationClip9640Path, "CrouchingCoverIdleLeft");
			var crouchingCoverCenterRightAimHoldAnimationClip7286Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
			var crouchingCoverCenterRightAimHoldAnimationClip7286 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterRightAimHoldAnimationClip7286Path, "CrouchingCoverCenterRightAimHold");
			var crouchingCoverCenterRightAimReturnAnimationClip7288Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
			var crouchingCoverCenterRightAimReturnAnimationClip7288 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterRightAimReturnAnimationClip7288Path, "CrouchingCoverCenterRightAimReturn");
			var crouchingCoverCenterRightAimAnimationClip7284Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
			var crouchingCoverCenterRightAimAnimationClip7284 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterRightAimAnimationClip7284Path, "CrouchingCoverCenterRightAim");
			var crouchingCoverIdleRightAnimationClip9642Path = AssetDatabase.GUIDToAssetPath("e9ee80e39ac40244b9ace63bd92ee356"); 
			var crouchingCoverIdleRightAnimationClip9642 = AnimatorBuilder.GetAnimationClip(crouchingCoverIdleRightAnimationClip9642Path, "CrouchingCoverIdleRight");
			var crouchingCoverStartAnimationClip8470Path = AssetDatabase.GUIDToAssetPath("5f2facb2ef9e50c429ae4a517a926abb"); 
			var crouchingCoverStartAnimationClip8470 = AnimatorBuilder.GetAnimationClip(crouchingCoverStartAnimationClip8470Path, "CrouchingCoverStart");
			var crouchingCoverStrafeLeftAnimationClip14126Path = AssetDatabase.GUIDToAssetPath("856ef158bb445f74b8997062574876dd"); 
			var crouchingCoverStrafeLeftAnimationClip14126 = AnimatorBuilder.GetAnimationClip(crouchingCoverStrafeLeftAnimationClip14126Path, "CrouchingCoverStrafeLeft");
			var crouchingCoverStrafeRightAnimationClip14128Path = AssetDatabase.GUIDToAssetPath("856ef158bb445f74b8997062574876dd"); 
			var crouchingCoverStrafeRightAnimationClip14128 = AnimatorBuilder.GetAnimationClip(crouchingCoverStrafeRightAnimationClip14128Path, "CrouchingCoverStrafeRight");
			var crouchingCoverAimLeftAnimationClip18944Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
			var crouchingCoverAimLeftAnimationClip18944 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimLeftAnimationClip18944Path, "CrouchingCoverAimLeft");
			var crouchingCoverCenterLeftAimHoldAnimationClip7280Path = AssetDatabase.GUIDToAssetPath("791c6a81c01a0b045b6e2dcc6e0e9ff4"); 
			var crouchingCoverCenterLeftAimHoldAnimationClip7280 = AnimatorBuilder.GetAnimationClip(crouchingCoverCenterLeftAimHoldAnimationClip7280Path, "CrouchingCoverCenterLeftAimHold");
			var crouchingCoverAimLeftReturnAnimationClip18948Path = AssetDatabase.GUIDToAssetPath("cf8a597d4a1446f43b772efd9cbcdaaa"); 
			var crouchingCoverAimLeftReturnAnimationClip18948 = AnimatorBuilder.GetAnimationClip(crouchingCoverAimLeftReturnAnimationClip18948Path, "CrouchingCoverAimLeftReturn");
			var standingCoverAimRightReturnAnimationClip8242Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
			var standingCoverAimRightReturnAnimationClip8242 = AnimatorBuilder.GetAnimationClip(standingCoverAimRightReturnAnimationClip8242Path, "StandingCoverAimRightReturn");
			var standingCoverAimRightHoldAnimationClip8240Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
			var standingCoverAimRightHoldAnimationClip8240 = AnimatorBuilder.GetAnimationClip(standingCoverAimRightHoldAnimationClip8240Path, "StandingCoverAimRightHold");
			var standingCoverAimRightAnimationClip8238Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
			var standingCoverAimRightAnimationClip8238 = AnimatorBuilder.GetAnimationClip(standingCoverAimRightAnimationClip8238Path, "StandingCoverAimRight");
			var standingCoverAimLeftHoldAnimationClip8234Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
			var standingCoverAimLeftHoldAnimationClip8234 = AnimatorBuilder.GetAnimationClip(standingCoverAimLeftHoldAnimationClip8234Path, "StandingCoverAimLeftHold");
			var standingCoverAimLeftReturnAnimationClip8236Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
			var standingCoverAimLeftReturnAnimationClip8236 = AnimatorBuilder.GetAnimationClip(standingCoverAimLeftReturnAnimationClip8236Path, "StandingCoverAimLeftReturn");
			var standingCoverAimLeftAnimationClip8232Path = AssetDatabase.GUIDToAssetPath("ffbc1b8223c72b0408df1fd3d16e87fd"); 
			var standingCoverAimLeftAnimationClip8232 = AnimatorBuilder.GetAnimationClip(standingCoverAimLeftAnimationClip8232Path, "StandingCoverAimLeft");
			var standingCoverStrafeLeftAnimationClip18528Path = AssetDatabase.GUIDToAssetPath("5a16b70d64433284abc4081f9536e72b"); 
			var standingCoverStrafeLeftAnimationClip18528 = AnimatorBuilder.GetAnimationClip(standingCoverStrafeLeftAnimationClip18528Path, "StandingCoverStrafeLeft");
			var standingCoverIdleLeftAnimationClip21332Path = AssetDatabase.GUIDToAssetPath("242453afc20bc2b4cb84137b8059d653"); 
			var standingCoverIdleLeftAnimationClip21332 = AnimatorBuilder.GetAnimationClip(standingCoverIdleLeftAnimationClip21332Path, "StandingCoverIdleLeft");
			var standingCoverIdleRightAnimationClip21334Path = AssetDatabase.GUIDToAssetPath("242453afc20bc2b4cb84137b8059d653"); 
			var standingCoverIdleRightAnimationClip21334 = AnimatorBuilder.GetAnimationClip(standingCoverIdleRightAnimationClip21334Path, "StandingCoverIdleRight");
			var standingCoverStrafeRightAnimationClip18530Path = AssetDatabase.GUIDToAssetPath("5a16b70d64433284abc4081f9536e72b"); 
			var standingCoverStrafeRightAnimationClip18530 = AnimatorBuilder.GetAnimationClip(standingCoverStrafeRightAnimationClip18530Path, "StandingCoverStrafeRight");
			var standingCoverStartAnimationClip12372Path = AssetDatabase.GUIDToAssetPath("c5894cc6de53e9a4c89e74f1f19864b3"); 
			var standingCoverStartAnimationClip12372 = AnimatorBuilder.GetAnimationClip(standingCoverStartAnimationClip12372Path, "StandingCoverStart");

			// State Machine.
			var coverAnimatorStateMachine68418 = baseStateMachine1824871762.AddStateMachine("Cover", new Vector3(624f, 60f, 0f));

			// State Machine.
			var crouchingCoverAnimatorStateMachine68420 = coverAnimatorStateMachine68418.AddStateMachine("Crouching Cover", new Vector3(24f, -204f, 0f));

			// States.
			var coverCenterAimLeftAimAnimatorState68396 = crouchingCoverAnimatorStateMachine68420.AddState("Cover Center Aim Left Aim", new Vector3(-96f, 288f, 0f));
			coverCenterAimLeftAimAnimatorState68396.motion = crouchingCoverCenterLeftAimAnimationClip7278;
			coverCenterAimLeftAimAnimatorState68396.cycleOffset = 0f;
			coverCenterAimLeftAimAnimatorState68396.cycleOffsetParameterActive = false;
			coverCenterAimLeftAimAnimatorState68396.iKOnFeet = true;
			coverCenterAimLeftAimAnimatorState68396.mirror = false;
			coverCenterAimLeftAimAnimatorState68396.mirrorParameterActive = false;
			coverCenterAimLeftAimAnimatorState68396.speed = 1.5f;
			coverCenterAimLeftAimAnimatorState68396.speedParameterActive = false;
			coverCenterAimLeftAimAnimatorState68396.writeDefaultValues = true;

			var coverCenterAimLeftReturnAnimatorState68408 = crouchingCoverAnimatorStateMachine68420.AddState("Cover Center Aim Left Return", new Vector3(-336f, 288f, 0f));
			coverCenterAimLeftReturnAnimatorState68408.motion = crouchingCoverCenterLeftAimReturnAnimationClip7282;
			coverCenterAimLeftReturnAnimatorState68408.cycleOffset = 0f;
			coverCenterAimLeftReturnAnimatorState68408.cycleOffsetParameterActive = false;
			coverCenterAimLeftReturnAnimatorState68408.iKOnFeet = true;
			coverCenterAimLeftReturnAnimatorState68408.mirror = false;
			coverCenterAimLeftReturnAnimatorState68408.mirrorParameterActive = false;
			coverCenterAimLeftReturnAnimatorState68408.speed = 1.8f;
			coverCenterAimLeftReturnAnimatorState68408.speedParameterActive = false;
			coverCenterAimLeftReturnAnimatorState68408.writeDefaultValues = true;

			var crouchingCoverAimLeftHoldAnimatorState68382 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Aim Left Hold", new Vector3(-576f, 96f, 0f));
			crouchingCoverAimLeftHoldAnimatorState68382.motion = crouchingCoverAimLeftHoldAnimationClip18946;
			crouchingCoverAimLeftHoldAnimatorState68382.cycleOffset = 0f;
			crouchingCoverAimLeftHoldAnimatorState68382.cycleOffsetParameterActive = false;
			crouchingCoverAimLeftHoldAnimatorState68382.iKOnFeet = true;
			crouchingCoverAimLeftHoldAnimatorState68382.mirror = false;
			crouchingCoverAimLeftHoldAnimatorState68382.mirrorParameterActive = false;
			crouchingCoverAimLeftHoldAnimatorState68382.speed = 1f;
			crouchingCoverAimLeftHoldAnimatorState68382.speedParameterActive = false;
			crouchingCoverAimLeftHoldAnimatorState68382.writeDefaultValues = true;

			var crouchingCoverAimRightAnimatorState68384 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Aim Right", new Vector3(384f, 156f, 0f));
			crouchingCoverAimRightAnimatorState68384.motion = crouchingCoverAimRightAnimationClip18950;
			crouchingCoverAimRightAnimatorState68384.cycleOffset = 0f;
			crouchingCoverAimRightAnimatorState68384.cycleOffsetParameterActive = false;
			crouchingCoverAimRightAnimatorState68384.iKOnFeet = true;
			crouchingCoverAimRightAnimatorState68384.mirror = false;
			crouchingCoverAimRightAnimatorState68384.mirrorParameterActive = false;
			crouchingCoverAimRightAnimatorState68384.speed = 1.5f;
			crouchingCoverAimRightAnimatorState68384.speedParameterActive = false;
			crouchingCoverAimRightAnimatorState68384.writeDefaultValues = true;

			var crouchingCoverAimRightHoldAnimatorState68414 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Aim Right Hold", new Vector3(636f, 96f, 0f));
			crouchingCoverAimRightHoldAnimatorState68414.motion = crouchingCoverAimRightHoldAnimationClip18952;
			crouchingCoverAimRightHoldAnimatorState68414.cycleOffset = 0f;
			crouchingCoverAimRightHoldAnimatorState68414.cycleOffsetParameterActive = false;
			crouchingCoverAimRightHoldAnimatorState68414.iKOnFeet = true;
			crouchingCoverAimRightHoldAnimatorState68414.mirror = false;
			crouchingCoverAimRightHoldAnimatorState68414.mirrorParameterActive = false;
			crouchingCoverAimRightHoldAnimatorState68414.speed = 1f;
			crouchingCoverAimRightHoldAnimatorState68414.speedParameterActive = false;
			crouchingCoverAimRightHoldAnimatorState68414.writeDefaultValues = true;

			var crouchingCoverAimRightReturnAnimatorState68406 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Aim Right Return", new Vector3(384f, 36f, 0f));
			crouchingCoverAimRightReturnAnimatorState68406.motion = crouchingCoverAimRightReturnAnimationClip18954;
			crouchingCoverAimRightReturnAnimatorState68406.cycleOffset = 0f;
			crouchingCoverAimRightReturnAnimatorState68406.cycleOffsetParameterActive = false;
			crouchingCoverAimRightReturnAnimatorState68406.iKOnFeet = true;
			crouchingCoverAimRightReturnAnimatorState68406.mirror = false;
			crouchingCoverAimRightReturnAnimatorState68406.mirrorParameterActive = false;
			crouchingCoverAimRightReturnAnimatorState68406.speed = 1.8f;
			crouchingCoverAimRightReturnAnimatorState68406.speedParameterActive = false;
			crouchingCoverAimRightReturnAnimatorState68406.writeDefaultValues = true;

			var crouchingCoverIdleLeftAnimatorState68412 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Idle Left", new Vector3(-156f, -84f, 0f));
			crouchingCoverIdleLeftAnimatorState68412.motion = crouchingCoverIdleLeftAnimationClip9640;
			crouchingCoverIdleLeftAnimatorState68412.cycleOffset = 0f;
			crouchingCoverIdleLeftAnimatorState68412.cycleOffsetParameterActive = false;
			crouchingCoverIdleLeftAnimatorState68412.iKOnFeet = false;
			crouchingCoverIdleLeftAnimatorState68412.mirror = false;
			crouchingCoverIdleLeftAnimatorState68412.mirrorParameterActive = false;
			crouchingCoverIdleLeftAnimatorState68412.speed = 1f;
			crouchingCoverIdleLeftAnimatorState68412.speedParameterActive = false;
			crouchingCoverIdleLeftAnimatorState68412.writeDefaultValues = true;

			var coverCenterAimRightHoldAnimatorState68400 = crouchingCoverAnimatorStateMachine68420.AddState("Cover Center Aim Right Hold", new Vector3(264f, 396f, 0f));
			coverCenterAimRightHoldAnimatorState68400.motion = crouchingCoverCenterRightAimHoldAnimationClip7286;
			coverCenterAimRightHoldAnimatorState68400.cycleOffset = 0f;
			coverCenterAimRightHoldAnimatorState68400.cycleOffsetParameterActive = false;
			coverCenterAimRightHoldAnimatorState68400.iKOnFeet = true;
			coverCenterAimRightHoldAnimatorState68400.mirror = false;
			coverCenterAimRightHoldAnimatorState68400.mirrorParameterActive = false;
			coverCenterAimRightHoldAnimatorState68400.speed = 1f;
			coverCenterAimRightHoldAnimatorState68400.speedParameterActive = false;
			coverCenterAimRightHoldAnimatorState68400.writeDefaultValues = true;

			var coverCenterAimRightReturnAnimatorState68390 = crouchingCoverAnimatorStateMachine68420.AddState("Cover Center Aim Right Return", new Vector3(384f, 288f, 0f));
			coverCenterAimRightReturnAnimatorState68390.motion = crouchingCoverCenterRightAimReturnAnimationClip7288;
			coverCenterAimRightReturnAnimatorState68390.cycleOffset = 0f;
			coverCenterAimRightReturnAnimatorState68390.cycleOffsetParameterActive = false;
			coverCenterAimRightReturnAnimatorState68390.iKOnFeet = true;
			coverCenterAimRightReturnAnimatorState68390.mirror = false;
			coverCenterAimRightReturnAnimatorState68390.mirrorParameterActive = false;
			coverCenterAimRightReturnAnimatorState68390.speed = 1.8f;
			coverCenterAimRightReturnAnimatorState68390.speedParameterActive = false;
			coverCenterAimRightReturnAnimatorState68390.writeDefaultValues = true;

			var coverCenterAimRightAimAnimatorState68378 = crouchingCoverAnimatorStateMachine68420.AddState("Cover Center Aim Right Aim", new Vector3(144f, 288f, 0f));
			coverCenterAimRightAimAnimatorState68378.motion = crouchingCoverCenterRightAimAnimationClip7284;
			coverCenterAimRightAimAnimatorState68378.cycleOffset = 0f;
			coverCenterAimRightAimAnimatorState68378.cycleOffsetParameterActive = false;
			coverCenterAimRightAimAnimatorState68378.iKOnFeet = true;
			coverCenterAimRightAimAnimatorState68378.mirror = false;
			coverCenterAimRightAimAnimatorState68378.mirrorParameterActive = false;
			coverCenterAimRightAimAnimatorState68378.speed = 1.5f;
			coverCenterAimRightAimAnimatorState68378.speedParameterActive = false;
			coverCenterAimRightAimAnimatorState68378.writeDefaultValues = true;

			var crouchingCoverIdleRightAnimatorState68394 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Idle Right", new Vector3(204f, -84f, 0f));
			crouchingCoverIdleRightAnimatorState68394.motion = crouchingCoverIdleRightAnimationClip9642;
			crouchingCoverIdleRightAnimatorState68394.cycleOffset = 0f;
			crouchingCoverIdleRightAnimatorState68394.cycleOffsetParameterActive = false;
			crouchingCoverIdleRightAnimatorState68394.iKOnFeet = false;
			crouchingCoverIdleRightAnimatorState68394.mirror = false;
			crouchingCoverIdleRightAnimatorState68394.mirrorParameterActive = false;
			crouchingCoverIdleRightAnimatorState68394.speed = 1f;
			crouchingCoverIdleRightAnimatorState68394.speedParameterActive = false;
			crouchingCoverIdleRightAnimatorState68394.writeDefaultValues = true;

			var takeCrouchingCoverAnimatorState68370 = crouchingCoverAnimatorStateMachine68420.AddState("Take Crouching Cover", new Vector3(24f, -264f, 0f));
			takeCrouchingCoverAnimatorState68370.motion = crouchingCoverStartAnimationClip8470;
			takeCrouchingCoverAnimatorState68370.cycleOffset = 0f;
			takeCrouchingCoverAnimatorState68370.cycleOffsetParameterActive = false;
			takeCrouchingCoverAnimatorState68370.iKOnFeet = true;
			takeCrouchingCoverAnimatorState68370.mirror = false;
			takeCrouchingCoverAnimatorState68370.mirrorParameterActive = false;
			takeCrouchingCoverAnimatorState68370.speed = 2f;
			takeCrouchingCoverAnimatorState68370.speedParameterActive = false;
			takeCrouchingCoverAnimatorState68370.writeDefaultValues = true;

			var crouchingCoverStrafeAnimatorState68374 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Strafe", new Vector3(24f, 96f, 0f));
			var crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196 = new BlendTree();
			AssetDatabase.AddObjectToAsset(crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196, animatorController);
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.hideFlags = HideFlags.HideInHierarchy;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.blendParameter = "HorizontalMovement";
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.blendParameterY = "AbilityFloatData";
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.blendType = BlendTreeType.FreeformCartesian2D;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.maxThreshold = 3f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.minThreshold = 0f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.name = "Blend Tree";
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.useAutomaticThresholds = false;
			var crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0 =  new ChildMotion();
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0.motion = crouchingCoverStrafeLeftAnimationClip14126;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0.cycleOffset = 0f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0.directBlendParameter = "HorizontalMovement";
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0.mirror = false;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0.position = new Vector2(-1f, 0f);
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0.threshold = 0f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0.timeScale = 1f;
			var crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1 =  new ChildMotion();
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1.motion = crouchingCoverIdleLeftAnimationClip9640;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1.cycleOffset = 0f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1.directBlendParameter = "HorizontalMovement";
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1.mirror = false;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1.position = new Vector2(0f, 0f);
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1.threshold = 1f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1.timeScale = 1f;
			var crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2 =  new ChildMotion();
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2.motion = crouchingCoverIdleRightAnimationClip9642;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2.cycleOffset = 0f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2.directBlendParameter = "HorizontalMovement";
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2.mirror = false;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2.position = new Vector2(0f, 1f);
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2.threshold = 2f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2.timeScale = 1f;
			var crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3 =  new ChildMotion();
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3.motion = crouchingCoverStrafeRightAnimationClip14128;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3.cycleOffset = 0f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3.directBlendParameter = "HorizontalMovement";
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3.mirror = false;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3.position = new Vector2(1f, 1f);
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3.threshold = 3f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3.timeScale = 1f;
			crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196.children = new ChildMotion[] {
				crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child0,
				crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child1,
				crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child2,
				crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196Child3
			};
			crouchingCoverStrafeAnimatorState68374.motion = crouchingCoverStrafeAnimatorState68374blendTreeBlendTree68196;
			crouchingCoverStrafeAnimatorState68374.cycleOffset = 0f;
			crouchingCoverStrafeAnimatorState68374.cycleOffsetParameterActive = false;
			crouchingCoverStrafeAnimatorState68374.iKOnFeet = true;
			crouchingCoverStrafeAnimatorState68374.mirror = false;
			crouchingCoverStrafeAnimatorState68374.mirrorParameterActive = false;
			crouchingCoverStrafeAnimatorState68374.speed = 1f;
			crouchingCoverStrafeAnimatorState68374.speedParameterActive = false;
			crouchingCoverStrafeAnimatorState68374.writeDefaultValues = true;

			var crouchingCoverAimLeftAnimatorState68366 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Aim Left", new Vector3(-336f, 156f, 0f));
			crouchingCoverAimLeftAnimatorState68366.motion = crouchingCoverAimLeftAnimationClip18944;
			crouchingCoverAimLeftAnimatorState68366.cycleOffset = 0f;
			crouchingCoverAimLeftAnimatorState68366.cycleOffsetParameterActive = false;
			crouchingCoverAimLeftAnimatorState68366.iKOnFeet = true;
			crouchingCoverAimLeftAnimatorState68366.mirror = false;
			crouchingCoverAimLeftAnimatorState68366.mirrorParameterActive = false;
			crouchingCoverAimLeftAnimatorState68366.speed = 1.5f;
			crouchingCoverAimLeftAnimatorState68366.speedParameterActive = false;
			crouchingCoverAimLeftAnimatorState68366.writeDefaultValues = true;

			var coverCenterAimLeftHoldAnimatorState68368 = crouchingCoverAnimatorStateMachine68420.AddState("Cover Center Aim Left Hold", new Vector3(-216f, 396f, 0f));
			coverCenterAimLeftHoldAnimatorState68368.motion = crouchingCoverCenterLeftAimHoldAnimationClip7280;
			coverCenterAimLeftHoldAnimatorState68368.cycleOffset = 0f;
			coverCenterAimLeftHoldAnimatorState68368.cycleOffsetParameterActive = false;
			coverCenterAimLeftHoldAnimatorState68368.iKOnFeet = true;
			coverCenterAimLeftHoldAnimatorState68368.mirror = false;
			coverCenterAimLeftHoldAnimatorState68368.mirrorParameterActive = false;
			coverCenterAimLeftHoldAnimatorState68368.speed = 1f;
			coverCenterAimLeftHoldAnimatorState68368.speedParameterActive = false;
			coverCenterAimLeftHoldAnimatorState68368.writeDefaultValues = true;

			var crouchingCoverAimLeftReturnAnimatorState68392 = crouchingCoverAnimatorStateMachine68420.AddState("Crouching Cover Aim Left Return", new Vector3(-336f, 36f, 0f));
			crouchingCoverAimLeftReturnAnimatorState68392.motion = crouchingCoverAimLeftReturnAnimationClip18948;
			crouchingCoverAimLeftReturnAnimatorState68392.cycleOffset = 0f;
			crouchingCoverAimLeftReturnAnimatorState68392.cycleOffsetParameterActive = false;
			crouchingCoverAimLeftReturnAnimatorState68392.iKOnFeet = true;
			crouchingCoverAimLeftReturnAnimatorState68392.mirror = false;
			crouchingCoverAimLeftReturnAnimatorState68392.mirrorParameterActive = false;
			crouchingCoverAimLeftReturnAnimatorState68392.speed = 1.8f;
			crouchingCoverAimLeftReturnAnimatorState68392.speedParameterActive = false;
			crouchingCoverAimLeftReturnAnimatorState68392.writeDefaultValues = true;

			// State Machine Defaults.
			crouchingCoverAnimatorStateMachine68420.anyStatePosition = new Vector3(-564f, -252f, 0f);
			crouchingCoverAnimatorStateMachine68420.defaultState = takeCrouchingCoverAnimatorState68370;
			crouchingCoverAnimatorStateMachine68420.entryPosition = new Vector3(-564f, -300f, 0f);
			crouchingCoverAnimatorStateMachine68420.exitPosition = new Vector3(768f, -72f, 0f);
			crouchingCoverAnimatorStateMachine68420.parentStateMachinePosition = new Vector3(384f, -264f, 0f);

			// State Machine.
			var standingCoverAnimatorStateMachine68422 = coverAnimatorStateMachine68418.AddStateMachine("Standing Cover", new Vector3(24f, -312f, 0f));

			// States.
			var standingAimRightReturnAnimatorState68388 = standingCoverAnimatorStateMachine68422.AddState("Standing Aim Right Return", new Vector3(624f, 96f, 0f));
			standingAimRightReturnAnimatorState68388.motion = standingCoverAimRightReturnAnimationClip8242;
			standingAimRightReturnAnimatorState68388.cycleOffset = 0f;
			standingAimRightReturnAnimatorState68388.cycleOffsetParameterActive = false;
			standingAimRightReturnAnimatorState68388.iKOnFeet = true;
			standingAimRightReturnAnimatorState68388.mirror = false;
			standingAimRightReturnAnimatorState68388.mirrorParameterActive = false;
			standingAimRightReturnAnimatorState68388.speed = 1.8f;
			standingAimRightReturnAnimatorState68388.speedParameterActive = false;
			standingAimRightReturnAnimatorState68388.writeDefaultValues = true;

			var standingCoverAimRightHoldAnimatorState68410 = standingCoverAnimatorStateMachine68422.AddState("Standing Cover Aim Right Hold", new Vector3(864f, 36f, 0f));
			standingCoverAimRightHoldAnimatorState68410.motion = standingCoverAimRightHoldAnimationClip8240;
			standingCoverAimRightHoldAnimatorState68410.cycleOffset = 0f;
			standingCoverAimRightHoldAnimatorState68410.cycleOffsetParameterActive = false;
			standingCoverAimRightHoldAnimatorState68410.iKOnFeet = true;
			standingCoverAimRightHoldAnimatorState68410.mirror = false;
			standingCoverAimRightHoldAnimatorState68410.mirrorParameterActive = false;
			standingCoverAimRightHoldAnimatorState68410.speed = 1f;
			standingCoverAimRightHoldAnimatorState68410.speedParameterActive = false;
			standingCoverAimRightHoldAnimatorState68410.writeDefaultValues = true;

			var standingCoverAimRightAnimatorState68404 = standingCoverAnimatorStateMachine68422.AddState("Standing Cover Aim Right", new Vector3(624f, -24f, 0f));
			standingCoverAimRightAnimatorState68404.motion = standingCoverAimRightAnimationClip8238;
			standingCoverAimRightAnimatorState68404.cycleOffset = 0f;
			standingCoverAimRightAnimatorState68404.cycleOffsetParameterActive = false;
			standingCoverAimRightAnimatorState68404.iKOnFeet = true;
			standingCoverAimRightAnimatorState68404.mirror = false;
			standingCoverAimRightAnimatorState68404.mirrorParameterActive = false;
			standingCoverAimRightAnimatorState68404.speed = 1.5f;
			standingCoverAimRightAnimatorState68404.speedParameterActive = false;
			standingCoverAimRightAnimatorState68404.writeDefaultValues = true;

			var standingCoverAimLeftHoldAnimatorState68416 = standingCoverAnimatorStateMachine68422.AddState("Standing Cover Aim Left Hold", new Vector3(-348f, 36f, 0f));
			standingCoverAimLeftHoldAnimatorState68416.motion = standingCoverAimLeftHoldAnimationClip8234;
			standingCoverAimLeftHoldAnimatorState68416.cycleOffset = 0f;
			standingCoverAimLeftHoldAnimatorState68416.cycleOffsetParameterActive = false;
			standingCoverAimLeftHoldAnimatorState68416.iKOnFeet = true;
			standingCoverAimLeftHoldAnimatorState68416.mirror = false;
			standingCoverAimLeftHoldAnimatorState68416.mirrorParameterActive = false;
			standingCoverAimLeftHoldAnimatorState68416.speed = 1f;
			standingCoverAimLeftHoldAnimatorState68416.speedParameterActive = false;
			standingCoverAimLeftHoldAnimatorState68416.writeDefaultValues = true;

			var standingCoverAimLeftReturnAnimatorState68380 = standingCoverAnimatorStateMachine68422.AddState("Standing Cover Aim Left Return", new Vector3(-96f, 96f, 0f));
			standingCoverAimLeftReturnAnimatorState68380.motion = standingCoverAimLeftReturnAnimationClip8236;
			standingCoverAimLeftReturnAnimatorState68380.cycleOffset = 0f;
			standingCoverAimLeftReturnAnimatorState68380.cycleOffsetParameterActive = false;
			standingCoverAimLeftReturnAnimatorState68380.iKOnFeet = true;
			standingCoverAimLeftReturnAnimatorState68380.mirror = false;
			standingCoverAimLeftReturnAnimatorState68380.mirrorParameterActive = false;
			standingCoverAimLeftReturnAnimatorState68380.speed = 1.8f;
			standingCoverAimLeftReturnAnimatorState68380.speedParameterActive = false;
			standingCoverAimLeftReturnAnimatorState68380.writeDefaultValues = true;

			var standingCoverAimLeftAnimatorState68386 = standingCoverAnimatorStateMachine68422.AddState("Standing Cover Aim Left", new Vector3(-96f, -24f, 0f));
			standingCoverAimLeftAnimatorState68386.motion = standingCoverAimLeftAnimationClip8232;
			standingCoverAimLeftAnimatorState68386.cycleOffset = 0f;
			standingCoverAimLeftAnimatorState68386.cycleOffsetParameterActive = false;
			standingCoverAimLeftAnimatorState68386.iKOnFeet = true;
			standingCoverAimLeftAnimatorState68386.mirror = false;
			standingCoverAimLeftAnimatorState68386.mirrorParameterActive = false;
			standingCoverAimLeftAnimatorState68386.speed = 1.5f;
			standingCoverAimLeftAnimatorState68386.speedParameterActive = false;
			standingCoverAimLeftAnimatorState68386.writeDefaultValues = true;

			var standingCoverStrafeAnimatorState68376 = standingCoverAnimatorStateMachine68422.AddState("Standing Cover Strafe", new Vector3(264f, 36f, 0f));
			var standingCoverStrafeAnimatorState68376blendTreeBlendTree68194 = new BlendTree();
			AssetDatabase.AddObjectToAsset(standingCoverStrafeAnimatorState68376blendTreeBlendTree68194, animatorController);
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.hideFlags = HideFlags.HideInHierarchy;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.blendParameter = "HorizontalMovement";
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.blendParameterY = "AbilityFloatData";
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.blendType = BlendTreeType.FreeformCartesian2D;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.maxThreshold = 3f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.minThreshold = 0f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.name = "Blend Tree";
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.useAutomaticThresholds = false;
			var standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0 =  new ChildMotion();
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0.motion = standingCoverStrafeLeftAnimationClip18528;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0.cycleOffset = 0f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0.directBlendParameter = "HorizontalMovement";
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0.mirror = false;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0.position = new Vector2(-1f, 0f);
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0.threshold = 0f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0.timeScale = 1f;
			var standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1 =  new ChildMotion();
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1.motion = standingCoverIdleLeftAnimationClip21332;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1.cycleOffset = 0f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1.directBlendParameter = "HorizontalMovement";
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1.mirror = false;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1.position = new Vector2(0f, 0f);
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1.threshold = 1f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1.timeScale = 1f;
			var standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2 =  new ChildMotion();
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2.motion = standingCoverIdleRightAnimationClip21334;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2.cycleOffset = 0f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2.directBlendParameter = "HorizontalMovement";
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2.mirror = false;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2.position = new Vector2(0f, 1f);
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2.threshold = 2f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2.timeScale = 1f;
			var standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3 =  new ChildMotion();
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3.motion = standingCoverStrafeRightAnimationClip18530;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3.cycleOffset = 0f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3.directBlendParameter = "HorizontalMovement";
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3.mirror = false;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3.position = new Vector2(1f, 1f);
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3.threshold = 3f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3.timeScale = 1f;
			standingCoverStrafeAnimatorState68376blendTreeBlendTree68194.children = new ChildMotion[] {
				standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child0,
				standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child1,
				standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child2,
				standingCoverStrafeAnimatorState68376blendTreeBlendTree68194Child3
			};
			standingCoverStrafeAnimatorState68376.motion = standingCoverStrafeAnimatorState68376blendTreeBlendTree68194;
			standingCoverStrafeAnimatorState68376.cycleOffset = 0f;
			standingCoverStrafeAnimatorState68376.cycleOffsetParameterActive = false;
			standingCoverStrafeAnimatorState68376.iKOnFeet = true;
			standingCoverStrafeAnimatorState68376.mirror = false;
			standingCoverStrafeAnimatorState68376.mirrorParameterActive = false;
			standingCoverStrafeAnimatorState68376.speed = 1f;
			standingCoverStrafeAnimatorState68376.speedParameterActive = false;
			standingCoverStrafeAnimatorState68376.writeDefaultValues = true;

			var takeStandingCoverAnimatorState68398 = standingCoverAnimatorStateMachine68422.AddState("Take Standing Cover", new Vector3(264f, -324f, 0f));
			takeStandingCoverAnimatorState68398.motion = standingCoverStartAnimationClip12372;
			takeStandingCoverAnimatorState68398.cycleOffset = 0f;
			takeStandingCoverAnimatorState68398.cycleOffsetParameterActive = false;
			takeStandingCoverAnimatorState68398.iKOnFeet = true;
			takeStandingCoverAnimatorState68398.mirror = false;
			takeStandingCoverAnimatorState68398.mirrorParameterActive = false;
			takeStandingCoverAnimatorState68398.speed = 2f;
			takeStandingCoverAnimatorState68398.speedParameterActive = false;
			takeStandingCoverAnimatorState68398.writeDefaultValues = true;

			var standingCoverIdleRightAnimatorState68402 = standingCoverAnimatorStateMachine68422.AddState("Standing Cover Idle Right", new Vector3(444f, -204f, 0f));
			standingCoverIdleRightAnimatorState68402.motion = standingCoverIdleRightAnimationClip21334;
			standingCoverIdleRightAnimatorState68402.cycleOffset = 0f;
			standingCoverIdleRightAnimatorState68402.cycleOffsetParameterActive = false;
			standingCoverIdleRightAnimatorState68402.iKOnFeet = false;
			standingCoverIdleRightAnimatorState68402.mirror = false;
			standingCoverIdleRightAnimatorState68402.mirrorParameterActive = false;
			standingCoverIdleRightAnimatorState68402.speed = 1f;
			standingCoverIdleRightAnimatorState68402.speedParameterActive = false;
			standingCoverIdleRightAnimatorState68402.writeDefaultValues = true;

			var standingCoverIdleLeftAnimatorState68372 = standingCoverAnimatorStateMachine68422.AddState("Standing Cover Idle Left", new Vector3(96f, -204f, 0f));
			standingCoverIdleLeftAnimatorState68372.motion = standingCoverIdleLeftAnimationClip21332;
			standingCoverIdleLeftAnimatorState68372.cycleOffset = 0f;
			standingCoverIdleLeftAnimatorState68372.cycleOffsetParameterActive = false;
			standingCoverIdleLeftAnimatorState68372.iKOnFeet = false;
			standingCoverIdleLeftAnimatorState68372.mirror = false;
			standingCoverIdleLeftAnimatorState68372.mirrorParameterActive = false;
			standingCoverIdleLeftAnimatorState68372.speed = 1f;
			standingCoverIdleLeftAnimatorState68372.speedParameterActive = false;
			standingCoverIdleLeftAnimatorState68372.writeDefaultValues = true;

			// State Machine Defaults.
			standingCoverAnimatorStateMachine68422.anyStatePosition = new Vector3(-444f, -324f, 0f);
			standingCoverAnimatorStateMachine68422.defaultState = takeStandingCoverAnimatorState68398;
			standingCoverAnimatorStateMachine68422.entryPosition = new Vector3(-444f, -372f, 0f);
			standingCoverAnimatorStateMachine68422.exitPosition = new Vector3(1008f, -84f, 0f);
			standingCoverAnimatorStateMachine68422.parentStateMachinePosition = new Vector3(744f, -324f, 0f);

			// State Machine Defaults.
			coverAnimatorStateMachine68418.anyStatePosition = new Vector3(-312f, -288f, 0f);
			coverAnimatorStateMachine68418.defaultState = takeStandingCoverAnimatorState68398;
			coverAnimatorStateMachine68418.entryPosition = new Vector3(-312f, -336f, 0f);
			coverAnimatorStateMachine68418.exitPosition = new Vector3(408f, -192f, 0f);
			coverAnimatorStateMachine68418.parentStateMachinePosition = new Vector3(396f, -288f, 0f);

			// State Transitions.
			var animatorStateTransition68360 = coverCenterAimLeftAimAnimatorState68396.AddTransition(coverCenterAimLeftHoldAnimatorState68368);
			animatorStateTransition68360.canTransitionToSelf = true;
			animatorStateTransition68360.duration = 0.01f;
			animatorStateTransition68360.exitTime = 1f;
			animatorStateTransition68360.hasExitTime = true;
			animatorStateTransition68360.hasFixedDuration = false;
			animatorStateTransition68360.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68360.offset = 0f;
			animatorStateTransition68360.orderedInterruption = true;
			animatorStateTransition68360.isExit = false;
			animatorStateTransition68360.mute = false;
			animatorStateTransition68360.solo = false;
			animatorStateTransition68360.AddCondition(AnimatorConditionMode.Equals, 5f, "AbilityIntData");

			var animatorStateTransition68344 = coverCenterAimLeftAimAnimatorState68396.AddTransition(crouchingCoverIdleLeftAnimatorState68412);
			animatorStateTransition68344.canTransitionToSelf = true;
			animatorStateTransition68344.duration = 0.01f;
			animatorStateTransition68344.exitTime = 0.625f;
			animatorStateTransition68344.hasExitTime = false;
			animatorStateTransition68344.hasFixedDuration = true;
			animatorStateTransition68344.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68344.offset = 0f;
			animatorStateTransition68344.orderedInterruption = true;
			animatorStateTransition68344.isExit = false;
			animatorStateTransition68344.mute = false;
			animatorStateTransition68344.solo = false;
			animatorStateTransition68344.AddCondition(AnimatorConditionMode.NotEqual, 5f, "AbilityIntData");

			var animatorStateTransition68312 = coverCenterAimLeftReturnAnimatorState68408.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68312.canTransitionToSelf = true;
			animatorStateTransition68312.duration = 0.01f;
			animatorStateTransition68312.exitTime = 0.92f;
			animatorStateTransition68312.hasExitTime = true;
			animatorStateTransition68312.hasFixedDuration = false;
			animatorStateTransition68312.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68312.offset = 0f;
			animatorStateTransition68312.orderedInterruption = true;
			animatorStateTransition68312.isExit = false;
			animatorStateTransition68312.mute = false;
			animatorStateTransition68312.solo = false;
			animatorStateTransition68312.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68336 = coverCenterAimLeftReturnAnimatorState68408.AddTransition(crouchingCoverIdleLeftAnimatorState68412);
			animatorStateTransition68336.canTransitionToSelf = true;
			animatorStateTransition68336.duration = 0.01f;
			animatorStateTransition68336.exitTime = 0.92f;
			animatorStateTransition68336.hasExitTime = true;
			animatorStateTransition68336.hasFixedDuration = false;
			animatorStateTransition68336.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68336.offset = 0f;
			animatorStateTransition68336.orderedInterruption = true;
			animatorStateTransition68336.isExit = false;
			animatorStateTransition68336.mute = false;
			animatorStateTransition68336.solo = false;
			animatorStateTransition68336.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68336.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68234 = coverCenterAimLeftReturnAnimatorState68408.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68234.canTransitionToSelf = true;
			animatorStateTransition68234.duration = 0.01f;
			animatorStateTransition68234.exitTime = 0.92f;
			animatorStateTransition68234.hasExitTime = true;
			animatorStateTransition68234.hasFixedDuration = false;
			animatorStateTransition68234.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68234.offset = 0f;
			animatorStateTransition68234.orderedInterruption = true;
			animatorStateTransition68234.isExit = false;
			animatorStateTransition68234.mute = false;
			animatorStateTransition68234.solo = false;
			animatorStateTransition68234.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68274 = crouchingCoverAimLeftHoldAnimatorState68382.AddTransition(crouchingCoverAimLeftReturnAnimatorState68392);
			animatorStateTransition68274.canTransitionToSelf = true;
			animatorStateTransition68274.duration = 0.01f;
			animatorStateTransition68274.exitTime = 0.9f;
			animatorStateTransition68274.hasExitTime = false;
			animatorStateTransition68274.hasFixedDuration = false;
			animatorStateTransition68274.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68274.offset = 0f;
			animatorStateTransition68274.orderedInterruption = true;
			animatorStateTransition68274.isExit = false;
			animatorStateTransition68274.mute = false;
			animatorStateTransition68274.solo = false;
			animatorStateTransition68274.AddCondition(AnimatorConditionMode.NotEqual, 3f, "AbilityIntData");

			var animatorStateTransition68232 = crouchingCoverAimLeftHoldAnimatorState68382.AddTransition(standingCoverAimLeftHoldAnimatorState68416);
			animatorStateTransition68232.canTransitionToSelf = true;
			animatorStateTransition68232.duration = 0.1f;
			animatorStateTransition68232.exitTime = 0f;
			animatorStateTransition68232.hasExitTime = false;
			animatorStateTransition68232.hasFixedDuration = true;
			animatorStateTransition68232.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68232.offset = 0f;
			animatorStateTransition68232.orderedInterruption = true;
			animatorStateTransition68232.isExit = false;
			animatorStateTransition68232.mute = false;
			animatorStateTransition68232.solo = false;
			animatorStateTransition68232.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

			var animatorStateTransition68226 = crouchingCoverAimRightAnimatorState68384.AddTransition(crouchingCoverAimRightHoldAnimatorState68414);
			animatorStateTransition68226.canTransitionToSelf = true;
			animatorStateTransition68226.duration = 0.01f;
			animatorStateTransition68226.exitTime = 1f;
			animatorStateTransition68226.hasExitTime = true;
			animatorStateTransition68226.hasFixedDuration = false;
			animatorStateTransition68226.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68226.offset = 0f;
			animatorStateTransition68226.orderedInterruption = true;
			animatorStateTransition68226.isExit = false;
			animatorStateTransition68226.mute = false;
			animatorStateTransition68226.solo = false;
			animatorStateTransition68226.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

			var animatorStateTransition68230 = crouchingCoverAimRightAnimatorState68384.AddTransition(crouchingCoverIdleRightAnimatorState68394);
			animatorStateTransition68230.canTransitionToSelf = true;
			animatorStateTransition68230.duration = 0.01f;
			animatorStateTransition68230.exitTime = 0.625f;
			animatorStateTransition68230.hasExitTime = false;
			animatorStateTransition68230.hasFixedDuration = true;
			animatorStateTransition68230.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68230.offset = 0f;
			animatorStateTransition68230.orderedInterruption = true;
			animatorStateTransition68230.isExit = false;
			animatorStateTransition68230.mute = false;
			animatorStateTransition68230.solo = false;
			animatorStateTransition68230.AddCondition(AnimatorConditionMode.NotEqual, 4f, "AbilityIntData");

			var animatorStateTransition68200 = crouchingCoverAimRightHoldAnimatorState68414.AddTransition(crouchingCoverAimRightReturnAnimatorState68406);
			animatorStateTransition68200.canTransitionToSelf = true;
			animatorStateTransition68200.duration = 0.01f;
			animatorStateTransition68200.exitTime = 0.9f;
			animatorStateTransition68200.hasExitTime = false;
			animatorStateTransition68200.hasFixedDuration = false;
			animatorStateTransition68200.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68200.offset = 0f;
			animatorStateTransition68200.orderedInterruption = true;
			animatorStateTransition68200.isExit = false;
			animatorStateTransition68200.mute = false;
			animatorStateTransition68200.solo = false;
			animatorStateTransition68200.AddCondition(AnimatorConditionMode.NotEqual, 4f, "AbilityIntData");

			var animatorStateTransition68270 = crouchingCoverAimRightHoldAnimatorState68414.AddTransition(standingCoverAimRightHoldAnimatorState68410);
			animatorStateTransition68270.canTransitionToSelf = true;
			animatorStateTransition68270.duration = 0.1f;
			animatorStateTransition68270.exitTime = 0f;
			animatorStateTransition68270.hasExitTime = false;
			animatorStateTransition68270.hasFixedDuration = true;
			animatorStateTransition68270.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68270.offset = 0f;
			animatorStateTransition68270.orderedInterruption = true;
			animatorStateTransition68270.isExit = false;
			animatorStateTransition68270.mute = false;
			animatorStateTransition68270.solo = false;
			animatorStateTransition68270.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

			var animatorStateTransition68314 = crouchingCoverAimRightReturnAnimatorState68406.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68314.canTransitionToSelf = true;
			animatorStateTransition68314.duration = 0.01f;
			animatorStateTransition68314.exitTime = 0.92f;
			animatorStateTransition68314.hasExitTime = true;
			animatorStateTransition68314.hasFixedDuration = false;
			animatorStateTransition68314.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68314.offset = 0f;
			animatorStateTransition68314.orderedInterruption = true;
			animatorStateTransition68314.isExit = false;
			animatorStateTransition68314.mute = false;
			animatorStateTransition68314.solo = false;
			animatorStateTransition68314.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68204 = crouchingCoverAimRightReturnAnimatorState68406.AddTransition(crouchingCoverIdleRightAnimatorState68394);
			animatorStateTransition68204.canTransitionToSelf = true;
			animatorStateTransition68204.duration = 0.01f;
			animatorStateTransition68204.exitTime = 0.92f;
			animatorStateTransition68204.hasExitTime = true;
			animatorStateTransition68204.hasFixedDuration = false;
			animatorStateTransition68204.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68204.offset = 0f;
			animatorStateTransition68204.orderedInterruption = true;
			animatorStateTransition68204.isExit = false;
			animatorStateTransition68204.mute = false;
			animatorStateTransition68204.solo = false;
			animatorStateTransition68204.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68204.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68224 = crouchingCoverAimRightReturnAnimatorState68406.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68224.canTransitionToSelf = true;
			animatorStateTransition68224.duration = 0.01f;
			animatorStateTransition68224.exitTime = 0.92f;
			animatorStateTransition68224.hasExitTime = true;
			animatorStateTransition68224.hasFixedDuration = false;
			animatorStateTransition68224.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68224.offset = 0f;
			animatorStateTransition68224.orderedInterruption = true;
			animatorStateTransition68224.isExit = false;
			animatorStateTransition68224.mute = false;
			animatorStateTransition68224.solo = false;
			animatorStateTransition68224.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68208 = crouchingCoverIdleLeftAnimatorState68412.AddTransition(coverCenterAimLeftAimAnimatorState68396);
			animatorStateTransition68208.canTransitionToSelf = true;
			animatorStateTransition68208.duration = 0.01f;
			animatorStateTransition68208.exitTime = 0.95f;
			animatorStateTransition68208.hasExitTime = false;
			animatorStateTransition68208.hasFixedDuration = false;
			animatorStateTransition68208.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68208.offset = 0f;
			animatorStateTransition68208.orderedInterruption = true;
			animatorStateTransition68208.isExit = false;
			animatorStateTransition68208.mute = false;
			animatorStateTransition68208.solo = false;
			animatorStateTransition68208.AddCondition(AnimatorConditionMode.Equals, 5f, "AbilityIntData");

			var animatorStateTransition68358 = crouchingCoverIdleLeftAnimatorState68412.AddTransition(crouchingCoverAimLeftAnimatorState68366);
			animatorStateTransition68358.canTransitionToSelf = true;
			animatorStateTransition68358.duration = 0.01f;
			animatorStateTransition68358.exitTime = 0.95f;
			animatorStateTransition68358.hasExitTime = false;
			animatorStateTransition68358.hasFixedDuration = false;
			animatorStateTransition68358.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68358.offset = 0f;
			animatorStateTransition68358.orderedInterruption = true;
			animatorStateTransition68358.isExit = false;
			animatorStateTransition68358.mute = false;
			animatorStateTransition68358.solo = false;
			animatorStateTransition68358.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

			var animatorStateTransition68340 = crouchingCoverIdleLeftAnimatorState68412.AddTransition(crouchingCoverIdleRightAnimatorState68394);
			animatorStateTransition68340.canTransitionToSelf = true;
			animatorStateTransition68340.duration = 0.1f;
			animatorStateTransition68340.exitTime = 0.95f;
			animatorStateTransition68340.hasExitTime = false;
			animatorStateTransition68340.hasFixedDuration = true;
			animatorStateTransition68340.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68340.offset = 0f;
			animatorStateTransition68340.orderedInterruption = true;
			animatorStateTransition68340.isExit = false;
			animatorStateTransition68340.mute = false;
			animatorStateTransition68340.solo = false;
			animatorStateTransition68340.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");
			animatorStateTransition68340.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68340.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68288 = crouchingCoverIdleLeftAnimatorState68412.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68288.canTransitionToSelf = true;
			animatorStateTransition68288.duration = 0.15f;
			animatorStateTransition68288.exitTime = 0.95f;
			animatorStateTransition68288.hasExitTime = false;
			animatorStateTransition68288.hasFixedDuration = true;
			animatorStateTransition68288.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68288.offset = 0f;
			animatorStateTransition68288.orderedInterruption = true;
			animatorStateTransition68288.isExit = false;
			animatorStateTransition68288.mute = false;
			animatorStateTransition68288.solo = false;
			animatorStateTransition68288.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68308 = crouchingCoverIdleLeftAnimatorState68412.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68308.canTransitionToSelf = true;
			animatorStateTransition68308.duration = 0.15f;
			animatorStateTransition68308.exitTime = 0.95f;
			animatorStateTransition68308.hasExitTime = false;
			animatorStateTransition68308.hasFixedDuration = true;
			animatorStateTransition68308.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68308.offset = 0f;
			animatorStateTransition68308.orderedInterruption = true;
			animatorStateTransition68308.isExit = false;
			animatorStateTransition68308.mute = false;
			animatorStateTransition68308.solo = false;
			animatorStateTransition68308.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68346 = crouchingCoverIdleLeftAnimatorState68412.AddTransition(standingCoverIdleLeftAnimatorState68372);
			animatorStateTransition68346.canTransitionToSelf = true;
			animatorStateTransition68346.duration = 0.1f;
			animatorStateTransition68346.exitTime = 0.95f;
			animatorStateTransition68346.hasExitTime = false;
			animatorStateTransition68346.hasFixedDuration = true;
			animatorStateTransition68346.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68346.offset = 0f;
			animatorStateTransition68346.orderedInterruption = true;
			animatorStateTransition68346.isExit = false;
			animatorStateTransition68346.mute = false;
			animatorStateTransition68346.solo = false;
			animatorStateTransition68346.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

			var animatorStateTransition68212 = coverCenterAimRightHoldAnimatorState68400.AddTransition(coverCenterAimRightReturnAnimatorState68390);
			animatorStateTransition68212.canTransitionToSelf = true;
			animatorStateTransition68212.duration = 0.01f;
			animatorStateTransition68212.exitTime = 0.9f;
			animatorStateTransition68212.hasExitTime = false;
			animatorStateTransition68212.hasFixedDuration = false;
			animatorStateTransition68212.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68212.offset = 0f;
			animatorStateTransition68212.orderedInterruption = true;
			animatorStateTransition68212.isExit = false;
			animatorStateTransition68212.mute = false;
			animatorStateTransition68212.solo = false;
			animatorStateTransition68212.AddCondition(AnimatorConditionMode.NotEqual, 6f, "AbilityIntData");

			var animatorStateTransition68236 = coverCenterAimRightReturnAnimatorState68390.AddTransition(crouchingCoverIdleRightAnimatorState68394);
			animatorStateTransition68236.canTransitionToSelf = true;
			animatorStateTransition68236.duration = 0.01f;
			animatorStateTransition68236.exitTime = 0.92f;
			animatorStateTransition68236.hasExitTime = true;
			animatorStateTransition68236.hasFixedDuration = false;
			animatorStateTransition68236.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68236.offset = 0f;
			animatorStateTransition68236.orderedInterruption = true;
			animatorStateTransition68236.isExit = false;
			animatorStateTransition68236.mute = false;
			animatorStateTransition68236.solo = false;
			animatorStateTransition68236.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68236.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68302 = coverCenterAimRightReturnAnimatorState68390.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68302.canTransitionToSelf = true;
			animatorStateTransition68302.duration = 0.01f;
			animatorStateTransition68302.exitTime = 0.92f;
			animatorStateTransition68302.hasExitTime = true;
			animatorStateTransition68302.hasFixedDuration = false;
			animatorStateTransition68302.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68302.offset = 0f;
			animatorStateTransition68302.orderedInterruption = true;
			animatorStateTransition68302.isExit = false;
			animatorStateTransition68302.mute = false;
			animatorStateTransition68302.solo = false;
			animatorStateTransition68302.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68228 = coverCenterAimRightReturnAnimatorState68390.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68228.canTransitionToSelf = true;
			animatorStateTransition68228.duration = 0.01f;
			animatorStateTransition68228.exitTime = 0.92f;
			animatorStateTransition68228.hasExitTime = true;
			animatorStateTransition68228.hasFixedDuration = false;
			animatorStateTransition68228.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68228.offset = 0f;
			animatorStateTransition68228.orderedInterruption = true;
			animatorStateTransition68228.isExit = false;
			animatorStateTransition68228.mute = false;
			animatorStateTransition68228.solo = false;
			animatorStateTransition68228.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68320 = coverCenterAimRightAimAnimatorState68378.AddTransition(coverCenterAimRightHoldAnimatorState68400);
			animatorStateTransition68320.canTransitionToSelf = true;
			animatorStateTransition68320.duration = 0.01f;
			animatorStateTransition68320.exitTime = 1f;
			animatorStateTransition68320.hasExitTime = true;
			animatorStateTransition68320.hasFixedDuration = false;
			animatorStateTransition68320.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68320.offset = 0f;
			animatorStateTransition68320.orderedInterruption = true;
			animatorStateTransition68320.isExit = false;
			animatorStateTransition68320.mute = false;
			animatorStateTransition68320.solo = false;
			animatorStateTransition68320.AddCondition(AnimatorConditionMode.Equals, 6f, "AbilityIntData");

			var animatorStateTransition68250 = coverCenterAimRightAimAnimatorState68378.AddTransition(crouchingCoverIdleRightAnimatorState68394);
			animatorStateTransition68250.canTransitionToSelf = true;
			animatorStateTransition68250.duration = 0.01f;
			animatorStateTransition68250.exitTime = 0.625f;
			animatorStateTransition68250.hasExitTime = false;
			animatorStateTransition68250.hasFixedDuration = true;
			animatorStateTransition68250.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68250.offset = 0f;
			animatorStateTransition68250.orderedInterruption = true;
			animatorStateTransition68250.isExit = false;
			animatorStateTransition68250.mute = false;
			animatorStateTransition68250.solo = false;
			animatorStateTransition68250.AddCondition(AnimatorConditionMode.NotEqual, 6f, "AbilityIntData");

			var animatorStateTransition68310 = crouchingCoverIdleRightAnimatorState68394.AddTransition(coverCenterAimRightAimAnimatorState68378);
			animatorStateTransition68310.canTransitionToSelf = true;
			animatorStateTransition68310.duration = 0.01f;
			animatorStateTransition68310.exitTime = 0.95f;
			animatorStateTransition68310.hasExitTime = false;
			animatorStateTransition68310.hasFixedDuration = false;
			animatorStateTransition68310.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68310.offset = 0f;
			animatorStateTransition68310.orderedInterruption = true;
			animatorStateTransition68310.isExit = false;
			animatorStateTransition68310.mute = false;
			animatorStateTransition68310.solo = false;
			animatorStateTransition68310.AddCondition(AnimatorConditionMode.Equals, 6f, "AbilityIntData");

			var animatorStateTransition68252 = crouchingCoverIdleRightAnimatorState68394.AddTransition(crouchingCoverAimRightAnimatorState68384);
			animatorStateTransition68252.canTransitionToSelf = true;
			animatorStateTransition68252.duration = 0.01f;
			animatorStateTransition68252.exitTime = 0.95f;
			animatorStateTransition68252.hasExitTime = false;
			animatorStateTransition68252.hasFixedDuration = false;
			animatorStateTransition68252.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68252.offset = 0f;
			animatorStateTransition68252.orderedInterruption = true;
			animatorStateTransition68252.isExit = false;
			animatorStateTransition68252.mute = false;
			animatorStateTransition68252.solo = false;
			animatorStateTransition68252.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

			var animatorStateTransition68248 = crouchingCoverIdleRightAnimatorState68394.AddTransition(crouchingCoverIdleLeftAnimatorState68412);
			animatorStateTransition68248.canTransitionToSelf = true;
			animatorStateTransition68248.duration = 0.1f;
			animatorStateTransition68248.exitTime = 0.95f;
			animatorStateTransition68248.hasExitTime = false;
			animatorStateTransition68248.hasFixedDuration = true;
			animatorStateTransition68248.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68248.offset = 0f;
			animatorStateTransition68248.orderedInterruption = true;
			animatorStateTransition68248.isExit = false;
			animatorStateTransition68248.mute = false;
			animatorStateTransition68248.solo = false;
			animatorStateTransition68248.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");
			animatorStateTransition68248.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68248.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68238 = crouchingCoverIdleRightAnimatorState68394.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68238.canTransitionToSelf = true;
			animatorStateTransition68238.duration = 0.15f;
			animatorStateTransition68238.exitTime = 0.95f;
			animatorStateTransition68238.hasExitTime = false;
			animatorStateTransition68238.hasFixedDuration = true;
			animatorStateTransition68238.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68238.offset = 0f;
			animatorStateTransition68238.orderedInterruption = true;
			animatorStateTransition68238.isExit = false;
			animatorStateTransition68238.mute = false;
			animatorStateTransition68238.solo = false;
			animatorStateTransition68238.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68276 = crouchingCoverIdleRightAnimatorState68394.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68276.canTransitionToSelf = true;
			animatorStateTransition68276.duration = 0.15f;
			animatorStateTransition68276.exitTime = 0.95f;
			animatorStateTransition68276.hasExitTime = false;
			animatorStateTransition68276.hasFixedDuration = true;
			animatorStateTransition68276.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68276.offset = 0f;
			animatorStateTransition68276.orderedInterruption = true;
			animatorStateTransition68276.isExit = false;
			animatorStateTransition68276.mute = false;
			animatorStateTransition68276.solo = false;
			animatorStateTransition68276.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68356 = crouchingCoverIdleRightAnimatorState68394.AddTransition(standingCoverIdleRightAnimatorState68402);
			animatorStateTransition68356.canTransitionToSelf = true;
			animatorStateTransition68356.duration = 0.1f;
			animatorStateTransition68356.exitTime = 0.95f;
			animatorStateTransition68356.hasExitTime = false;
			animatorStateTransition68356.hasFixedDuration = true;
			animatorStateTransition68356.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68356.offset = 0f;
			animatorStateTransition68356.orderedInterruption = true;
			animatorStateTransition68356.isExit = false;
			animatorStateTransition68356.mute = false;
			animatorStateTransition68356.solo = false;
			animatorStateTransition68356.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

			var animatorStateTransition68280 = takeCrouchingCoverAnimatorState68370.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68280.canTransitionToSelf = true;
			animatorStateTransition68280.duration = 1f;
			animatorStateTransition68280.exitTime = 1f;
			animatorStateTransition68280.hasExitTime = true;
			animatorStateTransition68280.hasFixedDuration = true;
			animatorStateTransition68280.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68280.offset = 0f;
			animatorStateTransition68280.orderedInterruption = true;
			animatorStateTransition68280.isExit = false;
			animatorStateTransition68280.mute = false;
			animatorStateTransition68280.solo = false;
			animatorStateTransition68280.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68324 = takeCrouchingCoverAnimatorState68370.AddTransition(crouchingCoverIdleRightAnimatorState68394);
			animatorStateTransition68324.canTransitionToSelf = true;
			animatorStateTransition68324.duration = 0.3f;
			animatorStateTransition68324.exitTime = 1f;
			animatorStateTransition68324.hasExitTime = true;
			animatorStateTransition68324.hasFixedDuration = true;
			animatorStateTransition68324.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68324.offset = 0f;
			animatorStateTransition68324.orderedInterruption = true;
			animatorStateTransition68324.isExit = false;
			animatorStateTransition68324.mute = false;
			animatorStateTransition68324.solo = false;
			animatorStateTransition68324.AddCondition(AnimatorConditionMode.Greater, 0f, "AbilityIntData");
			animatorStateTransition68324.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68324.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68268 = takeCrouchingCoverAnimatorState68370.AddTransition(crouchingCoverIdleLeftAnimatorState68412);
			animatorStateTransition68268.canTransitionToSelf = true;
			animatorStateTransition68268.duration = 0.3f;
			animatorStateTransition68268.exitTime = 1f;
			animatorStateTransition68268.hasExitTime = true;
			animatorStateTransition68268.hasFixedDuration = true;
			animatorStateTransition68268.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68268.offset = 0f;
			animatorStateTransition68268.orderedInterruption = true;
			animatorStateTransition68268.isExit = false;
			animatorStateTransition68268.mute = false;
			animatorStateTransition68268.solo = false;
			animatorStateTransition68268.AddCondition(AnimatorConditionMode.Less, 0f, "AbilityIntData");
			animatorStateTransition68268.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68268.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68326 = takeCrouchingCoverAnimatorState68370.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68326.canTransitionToSelf = true;
			animatorStateTransition68326.duration = 1f;
			animatorStateTransition68326.exitTime = 1f;
			animatorStateTransition68326.hasExitTime = true;
			animatorStateTransition68326.hasFixedDuration = false;
			animatorStateTransition68326.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68326.offset = 0f;
			animatorStateTransition68326.orderedInterruption = true;
			animatorStateTransition68326.isExit = false;
			animatorStateTransition68326.mute = false;
			animatorStateTransition68326.solo = false;
			animatorStateTransition68326.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68266 = crouchingCoverStrafeAnimatorState68374.AddTransition(coverCenterAimLeftAimAnimatorState68396);
			animatorStateTransition68266.canTransitionToSelf = true;
			animatorStateTransition68266.duration = 0.01f;
			animatorStateTransition68266.exitTime = 0.9210526f;
			animatorStateTransition68266.hasExitTime = false;
			animatorStateTransition68266.hasFixedDuration = false;
			animatorStateTransition68266.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68266.offset = 0f;
			animatorStateTransition68266.orderedInterruption = true;
			animatorStateTransition68266.isExit = false;
			animatorStateTransition68266.mute = false;
			animatorStateTransition68266.solo = false;
			animatorStateTransition68266.AddCondition(AnimatorConditionMode.Equals, 5f, "AbilityIntData");

			var animatorStateTransition68258 = crouchingCoverStrafeAnimatorState68374.AddTransition(crouchingCoverIdleLeftAnimatorState68412);
			animatorStateTransition68258.canTransitionToSelf = true;
			animatorStateTransition68258.duration = 0.15f;
			animatorStateTransition68258.exitTime = 0.9210526f;
			animatorStateTransition68258.hasExitTime = false;
			animatorStateTransition68258.hasFixedDuration = true;
			animatorStateTransition68258.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68258.offset = 0f;
			animatorStateTransition68258.orderedInterruption = true;
			animatorStateTransition68258.isExit = false;
			animatorStateTransition68258.mute = false;
			animatorStateTransition68258.solo = false;
			animatorStateTransition68258.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");
			animatorStateTransition68258.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68258.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");

			var animatorStateTransition68198 = crouchingCoverStrafeAnimatorState68374.AddTransition(crouchingCoverIdleRightAnimatorState68394);
			animatorStateTransition68198.canTransitionToSelf = true;
			animatorStateTransition68198.duration = 0.15f;
			animatorStateTransition68198.exitTime = 0.9210526f;
			animatorStateTransition68198.hasExitTime = false;
			animatorStateTransition68198.hasFixedDuration = true;
			animatorStateTransition68198.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68198.offset = 0f;
			animatorStateTransition68198.orderedInterruption = true;
			animatorStateTransition68198.isExit = false;
			animatorStateTransition68198.mute = false;
			animatorStateTransition68198.solo = false;
			animatorStateTransition68198.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");
			animatorStateTransition68198.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68198.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");

			var animatorStateTransition68292 = crouchingCoverStrafeAnimatorState68374.AddTransition(coverCenterAimRightAimAnimatorState68378);
			animatorStateTransition68292.canTransitionToSelf = true;
			animatorStateTransition68292.duration = 0.01f;
			animatorStateTransition68292.exitTime = 0.9210526f;
			animatorStateTransition68292.hasExitTime = false;
			animatorStateTransition68292.hasFixedDuration = false;
			animatorStateTransition68292.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68292.offset = 0f;
			animatorStateTransition68292.orderedInterruption = true;
			animatorStateTransition68292.isExit = false;
			animatorStateTransition68292.mute = false;
			animatorStateTransition68292.solo = false;
			animatorStateTransition68292.AddCondition(AnimatorConditionMode.Equals, 6f, "AbilityIntData");

			var animatorStateTransition68214 = crouchingCoverStrafeAnimatorState68374.AddTransition(crouchingCoverAimLeftAnimatorState68366);
			animatorStateTransition68214.canTransitionToSelf = true;
			animatorStateTransition68214.duration = 0.01f;
			animatorStateTransition68214.exitTime = 0.9f;
			animatorStateTransition68214.hasExitTime = false;
			animatorStateTransition68214.hasFixedDuration = false;
			animatorStateTransition68214.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68214.offset = 0f;
			animatorStateTransition68214.orderedInterruption = true;
			animatorStateTransition68214.isExit = false;
			animatorStateTransition68214.mute = false;
			animatorStateTransition68214.solo = false;
			animatorStateTransition68214.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

			var animatorStateTransition68316 = crouchingCoverStrafeAnimatorState68374.AddTransition(crouchingCoverAimRightAnimatorState68384);
			animatorStateTransition68316.canTransitionToSelf = true;
			animatorStateTransition68316.duration = 0.01f;
			animatorStateTransition68316.exitTime = 0.9f;
			animatorStateTransition68316.hasExitTime = false;
			animatorStateTransition68316.hasFixedDuration = false;
			animatorStateTransition68316.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68316.offset = 0f;
			animatorStateTransition68316.orderedInterruption = true;
			animatorStateTransition68316.isExit = false;
			animatorStateTransition68316.mute = false;
			animatorStateTransition68316.solo = false;
			animatorStateTransition68316.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

			var animatorStateTransition68244 = crouchingCoverStrafeAnimatorState68374.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68244.canTransitionToSelf = true;
			animatorStateTransition68244.duration = 0.1f;
			animatorStateTransition68244.exitTime = 0.9210526f;
			animatorStateTransition68244.hasExitTime = false;
			animatorStateTransition68244.hasFixedDuration = true;
			animatorStateTransition68244.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68244.offset = 0f;
			animatorStateTransition68244.orderedInterruption = true;
			animatorStateTransition68244.isExit = false;
			animatorStateTransition68244.mute = false;
			animatorStateTransition68244.solo = false;
			animatorStateTransition68244.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");

			var animatorStateTransition68298 = crouchingCoverAimLeftAnimatorState68366.AddTransition(crouchingCoverAimLeftHoldAnimatorState68382);
			animatorStateTransition68298.canTransitionToSelf = true;
			animatorStateTransition68298.duration = 0.01f;
			animatorStateTransition68298.exitTime = 1f;
			animatorStateTransition68298.hasExitTime = true;
			animatorStateTransition68298.hasFixedDuration = false;
			animatorStateTransition68298.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68298.offset = 0f;
			animatorStateTransition68298.orderedInterruption = true;
			animatorStateTransition68298.isExit = false;
			animatorStateTransition68298.mute = false;
			animatorStateTransition68298.solo = false;
			animatorStateTransition68298.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

			var animatorStateTransition68362 = crouchingCoverAimLeftAnimatorState68366.AddTransition(crouchingCoverIdleLeftAnimatorState68412);
			animatorStateTransition68362.canTransitionToSelf = true;
			animatorStateTransition68362.duration = 0.01f;
			animatorStateTransition68362.exitTime = 0.625f;
			animatorStateTransition68362.hasExitTime = false;
			animatorStateTransition68362.hasFixedDuration = false;
			animatorStateTransition68362.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68362.offset = 0f;
			animatorStateTransition68362.orderedInterruption = true;
			animatorStateTransition68362.isExit = false;
			animatorStateTransition68362.mute = false;
			animatorStateTransition68362.solo = false;
			animatorStateTransition68362.AddCondition(AnimatorConditionMode.NotEqual, 3f, "AbilityIntData");

			var animatorStateTransition68290 = coverCenterAimLeftHoldAnimatorState68368.AddTransition(coverCenterAimLeftReturnAnimatorState68408);
			animatorStateTransition68290.canTransitionToSelf = true;
			animatorStateTransition68290.duration = 0.01f;
			animatorStateTransition68290.exitTime = 0.9f;
			animatorStateTransition68290.hasExitTime = false;
			animatorStateTransition68290.hasFixedDuration = false;
			animatorStateTransition68290.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68290.offset = 0f;
			animatorStateTransition68290.orderedInterruption = true;
			animatorStateTransition68290.isExit = false;
			animatorStateTransition68290.mute = false;
			animatorStateTransition68290.solo = false;
			animatorStateTransition68290.AddCondition(AnimatorConditionMode.NotEqual, 5f, "AbilityIntData");

			var animatorStateTransition68296 = crouchingCoverAimLeftReturnAnimatorState68392.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68296.canTransitionToSelf = true;
			animatorStateTransition68296.duration = 0.01f;
			animatorStateTransition68296.exitTime = 0.92f;
			animatorStateTransition68296.hasExitTime = true;
			animatorStateTransition68296.hasFixedDuration = false;
			animatorStateTransition68296.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68296.offset = 0f;
			animatorStateTransition68296.orderedInterruption = true;
			animatorStateTransition68296.isExit = false;
			animatorStateTransition68296.mute = false;
			animatorStateTransition68296.solo = false;
			animatorStateTransition68296.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68330 = crouchingCoverAimLeftReturnAnimatorState68392.AddTransition(crouchingCoverIdleLeftAnimatorState68412);
			animatorStateTransition68330.canTransitionToSelf = true;
			animatorStateTransition68330.duration = 0.01f;
			animatorStateTransition68330.exitTime = 0.92f;
			animatorStateTransition68330.hasExitTime = true;
			animatorStateTransition68330.hasFixedDuration = false;
			animatorStateTransition68330.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68330.offset = 0f;
			animatorStateTransition68330.orderedInterruption = true;
			animatorStateTransition68330.isExit = false;
			animatorStateTransition68330.mute = false;
			animatorStateTransition68330.solo = false;
			animatorStateTransition68330.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68330.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68246 = crouchingCoverAimLeftReturnAnimatorState68392.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68246.canTransitionToSelf = true;
			animatorStateTransition68246.duration = 0.01f;
			animatorStateTransition68246.exitTime = 0.92f;
			animatorStateTransition68246.hasExitTime = true;
			animatorStateTransition68246.hasFixedDuration = false;
			animatorStateTransition68246.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68246.offset = 0f;
			animatorStateTransition68246.orderedInterruption = true;
			animatorStateTransition68246.isExit = false;
			animatorStateTransition68246.mute = false;
			animatorStateTransition68246.solo = false;
			animatorStateTransition68246.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			// State Transitions.
			var animatorStateTransition68334 = standingAimRightReturnAnimatorState68388.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68334.canTransitionToSelf = true;
			animatorStateTransition68334.duration = 0.01f;
			animatorStateTransition68334.exitTime = 0.92f;
			animatorStateTransition68334.hasExitTime = true;
			animatorStateTransition68334.hasFixedDuration = false;
			animatorStateTransition68334.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68334.offset = 0f;
			animatorStateTransition68334.orderedInterruption = true;
			animatorStateTransition68334.isExit = false;
			animatorStateTransition68334.mute = false;
			animatorStateTransition68334.solo = false;
			animatorStateTransition68334.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68364 = standingAimRightReturnAnimatorState68388.AddTransition(standingCoverIdleRightAnimatorState68402);
			animatorStateTransition68364.canTransitionToSelf = true;
			animatorStateTransition68364.duration = 0.01f;
			animatorStateTransition68364.exitTime = 0.92f;
			animatorStateTransition68364.hasExitTime = true;
			animatorStateTransition68364.hasFixedDuration = false;
			animatorStateTransition68364.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68364.offset = 0f;
			animatorStateTransition68364.orderedInterruption = true;
			animatorStateTransition68364.isExit = false;
			animatorStateTransition68364.mute = false;
			animatorStateTransition68364.solo = false;
			animatorStateTransition68364.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68364.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68348 = standingAimRightReturnAnimatorState68388.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68348.canTransitionToSelf = true;
			animatorStateTransition68348.duration = 0.01f;
			animatorStateTransition68348.exitTime = 0.92f;
			animatorStateTransition68348.hasExitTime = true;
			animatorStateTransition68348.hasFixedDuration = false;
			animatorStateTransition68348.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68348.offset = 0f;
			animatorStateTransition68348.orderedInterruption = true;
			animatorStateTransition68348.isExit = false;
			animatorStateTransition68348.mute = false;
			animatorStateTransition68348.solo = false;
			animatorStateTransition68348.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68294 = standingCoverAimRightHoldAnimatorState68410.AddTransition(standingAimRightReturnAnimatorState68388);
			animatorStateTransition68294.canTransitionToSelf = true;
			animatorStateTransition68294.duration = 0.01f;
			animatorStateTransition68294.exitTime = 0.9f;
			animatorStateTransition68294.hasExitTime = false;
			animatorStateTransition68294.hasFixedDuration = false;
			animatorStateTransition68294.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68294.offset = 0f;
			animatorStateTransition68294.orderedInterruption = true;
			animatorStateTransition68294.isExit = false;
			animatorStateTransition68294.mute = false;
			animatorStateTransition68294.solo = false;
			animatorStateTransition68294.AddCondition(AnimatorConditionMode.NotEqual, 4f, "AbilityIntData");

			var animatorStateTransition68282 = standingCoverAimRightHoldAnimatorState68410.AddTransition(crouchingCoverAimRightHoldAnimatorState68414);
			animatorStateTransition68282.canTransitionToSelf = true;
			animatorStateTransition68282.duration = 0.1f;
			animatorStateTransition68282.exitTime = 0f;
			animatorStateTransition68282.hasExitTime = false;
			animatorStateTransition68282.hasFixedDuration = true;
			animatorStateTransition68282.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68282.offset = 0f;
			animatorStateTransition68282.orderedInterruption = true;
			animatorStateTransition68282.isExit = false;
			animatorStateTransition68282.mute = false;
			animatorStateTransition68282.solo = false;
			animatorStateTransition68282.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

			var animatorStateTransition68260 = standingCoverAimRightAnimatorState68404.AddTransition(standingCoverAimRightHoldAnimatorState68410);
			animatorStateTransition68260.canTransitionToSelf = true;
			animatorStateTransition68260.duration = 0.01f;
			animatorStateTransition68260.exitTime = 1f;
			animatorStateTransition68260.hasExitTime = true;
			animatorStateTransition68260.hasFixedDuration = false;
			animatorStateTransition68260.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68260.offset = 0f;
			animatorStateTransition68260.orderedInterruption = true;
			animatorStateTransition68260.isExit = false;
			animatorStateTransition68260.mute = false;
			animatorStateTransition68260.solo = false;
			animatorStateTransition68260.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

			var animatorStateTransition68306 = standingCoverAimRightAnimatorState68404.AddTransition(standingCoverIdleRightAnimatorState68402);
			animatorStateTransition68306.canTransitionToSelf = true;
			animatorStateTransition68306.duration = 0.01f;
			animatorStateTransition68306.exitTime = 0.625f;
			animatorStateTransition68306.hasExitTime = false;
			animatorStateTransition68306.hasFixedDuration = true;
			animatorStateTransition68306.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68306.offset = 0f;
			animatorStateTransition68306.orderedInterruption = true;
			animatorStateTransition68306.isExit = false;
			animatorStateTransition68306.mute = false;
			animatorStateTransition68306.solo = false;
			animatorStateTransition68306.AddCondition(AnimatorConditionMode.NotEqual, 4f, "AbilityIntData");

			var animatorStateTransition68242 = standingCoverAimLeftHoldAnimatorState68416.AddTransition(standingCoverAimLeftReturnAnimatorState68380);
			animatorStateTransition68242.canTransitionToSelf = true;
			animatorStateTransition68242.duration = 0.01f;
			animatorStateTransition68242.exitTime = 0.9f;
			animatorStateTransition68242.hasExitTime = false;
			animatorStateTransition68242.hasFixedDuration = false;
			animatorStateTransition68242.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68242.offset = 0f;
			animatorStateTransition68242.orderedInterruption = true;
			animatorStateTransition68242.isExit = false;
			animatorStateTransition68242.mute = false;
			animatorStateTransition68242.solo = false;
			animatorStateTransition68242.AddCondition(AnimatorConditionMode.NotEqual, 3f, "AbilityIntData");

			var animatorStateTransition68202 = standingCoverAimLeftHoldAnimatorState68416.AddTransition(crouchingCoverAimLeftHoldAnimatorState68382);
			animatorStateTransition68202.canTransitionToSelf = true;
			animatorStateTransition68202.duration = 0.1f;
			animatorStateTransition68202.exitTime = 0f;
			animatorStateTransition68202.hasExitTime = false;
			animatorStateTransition68202.hasFixedDuration = true;
			animatorStateTransition68202.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68202.offset = 0f;
			animatorStateTransition68202.orderedInterruption = true;
			animatorStateTransition68202.isExit = false;
			animatorStateTransition68202.mute = false;
			animatorStateTransition68202.solo = false;
			animatorStateTransition68202.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

			var animatorStateTransition68338 = standingCoverAimLeftReturnAnimatorState68380.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68338.canTransitionToSelf = true;
			animatorStateTransition68338.duration = 0.01f;
			animatorStateTransition68338.exitTime = 0.92f;
			animatorStateTransition68338.hasExitTime = true;
			animatorStateTransition68338.hasFixedDuration = false;
			animatorStateTransition68338.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68338.offset = 0f;
			animatorStateTransition68338.orderedInterruption = true;
			animatorStateTransition68338.isExit = false;
			animatorStateTransition68338.mute = false;
			animatorStateTransition68338.solo = false;
			animatorStateTransition68338.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68254 = standingCoverAimLeftReturnAnimatorState68380.AddTransition(standingCoverIdleLeftAnimatorState68372);
			animatorStateTransition68254.canTransitionToSelf = true;
			animatorStateTransition68254.duration = 0.01f;
			animatorStateTransition68254.exitTime = 0.92f;
			animatorStateTransition68254.hasExitTime = true;
			animatorStateTransition68254.hasFixedDuration = false;
			animatorStateTransition68254.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68254.offset = 0f;
			animatorStateTransition68254.orderedInterruption = true;
			animatorStateTransition68254.isExit = false;
			animatorStateTransition68254.mute = false;
			animatorStateTransition68254.solo = false;
			animatorStateTransition68254.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68254.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68354 = standingCoverAimLeftReturnAnimatorState68380.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68354.canTransitionToSelf = true;
			animatorStateTransition68354.duration = 0.01f;
			animatorStateTransition68354.exitTime = 0.92f;
			animatorStateTransition68354.hasExitTime = true;
			animatorStateTransition68354.hasFixedDuration = false;
			animatorStateTransition68354.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68354.offset = 0f;
			animatorStateTransition68354.orderedInterruption = true;
			animatorStateTransition68354.isExit = false;
			animatorStateTransition68354.mute = false;
			animatorStateTransition68354.solo = false;
			animatorStateTransition68354.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68216 = standingCoverAimLeftAnimatorState68386.AddTransition(standingCoverAimLeftHoldAnimatorState68416);
			animatorStateTransition68216.canTransitionToSelf = true;
			animatorStateTransition68216.duration = 0.01f;
			animatorStateTransition68216.exitTime = 1f;
			animatorStateTransition68216.hasExitTime = true;
			animatorStateTransition68216.hasFixedDuration = false;
			animatorStateTransition68216.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68216.offset = 0f;
			animatorStateTransition68216.orderedInterruption = true;
			animatorStateTransition68216.isExit = false;
			animatorStateTransition68216.mute = false;
			animatorStateTransition68216.solo = false;
			animatorStateTransition68216.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

			var animatorStateTransition68278 = standingCoverAimLeftAnimatorState68386.AddTransition(standingCoverIdleLeftAnimatorState68372);
			animatorStateTransition68278.canTransitionToSelf = true;
			animatorStateTransition68278.duration = 0.01f;
			animatorStateTransition68278.exitTime = 0.625f;
			animatorStateTransition68278.hasExitTime = false;
			animatorStateTransition68278.hasFixedDuration = true;
			animatorStateTransition68278.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68278.offset = 0f;
			animatorStateTransition68278.orderedInterruption = true;
			animatorStateTransition68278.isExit = false;
			animatorStateTransition68278.mute = false;
			animatorStateTransition68278.solo = false;
			animatorStateTransition68278.AddCondition(AnimatorConditionMode.NotEqual, 3f, "AbilityIntData");

			var animatorStateTransition68322 = standingCoverStrafeAnimatorState68376.AddTransition(standingCoverAimRightAnimatorState68404);
			animatorStateTransition68322.canTransitionToSelf = true;
			animatorStateTransition68322.duration = 0.01f;
			animatorStateTransition68322.exitTime = 0.9f;
			animatorStateTransition68322.hasExitTime = false;
			animatorStateTransition68322.hasFixedDuration = false;
			animatorStateTransition68322.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68322.offset = 0f;
			animatorStateTransition68322.orderedInterruption = true;
			animatorStateTransition68322.isExit = false;
			animatorStateTransition68322.mute = false;
			animatorStateTransition68322.solo = false;
			animatorStateTransition68322.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

			var animatorStateTransition68350 = standingCoverStrafeAnimatorState68376.AddTransition(standingCoverAimLeftAnimatorState68386);
			animatorStateTransition68350.canTransitionToSelf = true;
			animatorStateTransition68350.duration = 0.01f;
			animatorStateTransition68350.exitTime = 0.9f;
			animatorStateTransition68350.hasExitTime = false;
			animatorStateTransition68350.hasFixedDuration = false;
			animatorStateTransition68350.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68350.offset = 0f;
			animatorStateTransition68350.orderedInterruption = true;
			animatorStateTransition68350.isExit = false;
			animatorStateTransition68350.mute = false;
			animatorStateTransition68350.solo = false;
			animatorStateTransition68350.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

			var animatorStateTransition68342 = standingCoverStrafeAnimatorState68376.AddTransition(standingCoverIdleRightAnimatorState68402);
			animatorStateTransition68342.canTransitionToSelf = true;
			animatorStateTransition68342.duration = 0.15f;
			animatorStateTransition68342.exitTime = 0.9210526f;
			animatorStateTransition68342.hasExitTime = false;
			animatorStateTransition68342.hasFixedDuration = true;
			animatorStateTransition68342.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68342.offset = 0f;
			animatorStateTransition68342.orderedInterruption = true;
			animatorStateTransition68342.isExit = false;
			animatorStateTransition68342.mute = false;
			animatorStateTransition68342.solo = false;
			animatorStateTransition68342.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");
			animatorStateTransition68342.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68342.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");

			var animatorStateTransition68262 = standingCoverStrafeAnimatorState68376.AddTransition(standingCoverIdleLeftAnimatorState68372);
			animatorStateTransition68262.canTransitionToSelf = true;
			animatorStateTransition68262.duration = 0.15f;
			animatorStateTransition68262.exitTime = 0.9210526f;
			animatorStateTransition68262.hasExitTime = false;
			animatorStateTransition68262.hasFixedDuration = true;
			animatorStateTransition68262.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68262.offset = 0f;
			animatorStateTransition68262.orderedInterruption = true;
			animatorStateTransition68262.isExit = false;
			animatorStateTransition68262.mute = false;
			animatorStateTransition68262.solo = false;
			animatorStateTransition68262.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");
			animatorStateTransition68262.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68262.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");

			var animatorStateTransition68272 = standingCoverStrafeAnimatorState68376.AddTransition(crouchingCoverStrafeAnimatorState68374);
			animatorStateTransition68272.canTransitionToSelf = true;
			animatorStateTransition68272.duration = 0.1f;
			animatorStateTransition68272.exitTime = 0.9210526f;
			animatorStateTransition68272.hasExitTime = false;
			animatorStateTransition68272.hasFixedDuration = true;
			animatorStateTransition68272.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68272.offset = 0f;
			animatorStateTransition68272.orderedInterruption = true;
			animatorStateTransition68272.isExit = false;
			animatorStateTransition68272.mute = false;
			animatorStateTransition68272.solo = false;
			animatorStateTransition68272.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

			var animatorStateTransition68328 = takeStandingCoverAnimatorState68398.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68328.canTransitionToSelf = true;
			animatorStateTransition68328.duration = 1f;
			animatorStateTransition68328.exitTime = 1f;
			animatorStateTransition68328.hasExitTime = true;
			animatorStateTransition68328.hasFixedDuration = false;
			animatorStateTransition68328.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68328.offset = 0f;
			animatorStateTransition68328.orderedInterruption = true;
			animatorStateTransition68328.isExit = false;
			animatorStateTransition68328.mute = false;
			animatorStateTransition68328.solo = false;
			animatorStateTransition68328.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68332 = takeStandingCoverAnimatorState68398.AddTransition(standingCoverIdleLeftAnimatorState68372);
			animatorStateTransition68332.canTransitionToSelf = true;
			animatorStateTransition68332.duration = 0.3f;
			animatorStateTransition68332.exitTime = 1f;
			animatorStateTransition68332.hasExitTime = true;
			animatorStateTransition68332.hasFixedDuration = true;
			animatorStateTransition68332.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68332.offset = 0f;
			animatorStateTransition68332.orderedInterruption = true;
			animatorStateTransition68332.isExit = false;
			animatorStateTransition68332.mute = false;
			animatorStateTransition68332.solo = false;
			animatorStateTransition68332.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");
			animatorStateTransition68332.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68332.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68206 = takeStandingCoverAnimatorState68398.AddTransition(standingCoverIdleRightAnimatorState68402);
			animatorStateTransition68206.canTransitionToSelf = true;
			animatorStateTransition68206.duration = 0.3f;
			animatorStateTransition68206.exitTime = 1f;
			animatorStateTransition68206.hasExitTime = true;
			animatorStateTransition68206.hasFixedDuration = true;
			animatorStateTransition68206.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68206.offset = 0f;
			animatorStateTransition68206.orderedInterruption = true;
			animatorStateTransition68206.isExit = false;
			animatorStateTransition68206.mute = false;
			animatorStateTransition68206.solo = false;
			animatorStateTransition68206.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");
			animatorStateTransition68206.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68206.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68218 = takeStandingCoverAnimatorState68398.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68218.canTransitionToSelf = true;
			animatorStateTransition68218.duration = 1f;
			animatorStateTransition68218.exitTime = 1f;
			animatorStateTransition68218.hasExitTime = true;
			animatorStateTransition68218.hasFixedDuration = false;
			animatorStateTransition68218.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68218.offset = 0f;
			animatorStateTransition68218.orderedInterruption = true;
			animatorStateTransition68218.isExit = false;
			animatorStateTransition68218.mute = false;
			animatorStateTransition68218.solo = false;
			animatorStateTransition68218.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68284 = standingCoverIdleRightAnimatorState68402.AddTransition(standingCoverIdleLeftAnimatorState68372);
			animatorStateTransition68284.canTransitionToSelf = true;
			animatorStateTransition68284.duration = 0.1f;
			animatorStateTransition68284.exitTime = 0.95f;
			animatorStateTransition68284.hasExitTime = false;
			animatorStateTransition68284.hasFixedDuration = true;
			animatorStateTransition68284.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68284.offset = 0f;
			animatorStateTransition68284.orderedInterruption = true;
			animatorStateTransition68284.isExit = false;
			animatorStateTransition68284.mute = false;
			animatorStateTransition68284.solo = false;
			animatorStateTransition68284.AddCondition(AnimatorConditionMode.Less, 0.5f, "AbilityFloatData");
			animatorStateTransition68284.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68284.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68352 = standingCoverIdleRightAnimatorState68402.AddTransition(standingCoverAimRightAnimatorState68404);
			animatorStateTransition68352.canTransitionToSelf = true;
			animatorStateTransition68352.duration = 0.01f;
			animatorStateTransition68352.exitTime = 0.95f;
			animatorStateTransition68352.hasExitTime = false;
			animatorStateTransition68352.hasFixedDuration = false;
			animatorStateTransition68352.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68352.offset = 0f;
			animatorStateTransition68352.orderedInterruption = true;
			animatorStateTransition68352.isExit = false;
			animatorStateTransition68352.mute = false;
			animatorStateTransition68352.solo = false;
			animatorStateTransition68352.AddCondition(AnimatorConditionMode.Equals, 4f, "AbilityIntData");

			var animatorStateTransition68264 = standingCoverIdleRightAnimatorState68402.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68264.canTransitionToSelf = true;
			animatorStateTransition68264.duration = 0.15f;
			animatorStateTransition68264.exitTime = 0.95f;
			animatorStateTransition68264.hasExitTime = false;
			animatorStateTransition68264.hasFixedDuration = true;
			animatorStateTransition68264.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68264.offset = 0f;
			animatorStateTransition68264.orderedInterruption = true;
			animatorStateTransition68264.isExit = false;
			animatorStateTransition68264.mute = false;
			animatorStateTransition68264.solo = false;
			animatorStateTransition68264.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68210 = standingCoverIdleRightAnimatorState68402.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68210.canTransitionToSelf = true;
			animatorStateTransition68210.duration = 0.15f;
			animatorStateTransition68210.exitTime = 0.95f;
			animatorStateTransition68210.hasExitTime = false;
			animatorStateTransition68210.hasFixedDuration = true;
			animatorStateTransition68210.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68210.offset = 0f;
			animatorStateTransition68210.orderedInterruption = true;
			animatorStateTransition68210.isExit = false;
			animatorStateTransition68210.mute = false;
			animatorStateTransition68210.solo = false;
			animatorStateTransition68210.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68256 = standingCoverIdleRightAnimatorState68402.AddTransition(crouchingCoverIdleRightAnimatorState68394);
			animatorStateTransition68256.canTransitionToSelf = true;
			animatorStateTransition68256.duration = 0.1f;
			animatorStateTransition68256.exitTime = 0.95f;
			animatorStateTransition68256.hasExitTime = false;
			animatorStateTransition68256.hasFixedDuration = true;
			animatorStateTransition68256.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68256.offset = 0f;
			animatorStateTransition68256.orderedInterruption = true;
			animatorStateTransition68256.isExit = false;
			animatorStateTransition68256.mute = false;
			animatorStateTransition68256.solo = false;
			animatorStateTransition68256.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

			var animatorStateTransition68240 = standingCoverIdleLeftAnimatorState68372.AddTransition(standingCoverIdleRightAnimatorState68402);
			animatorStateTransition68240.canTransitionToSelf = true;
			animatorStateTransition68240.duration = 0.1f;
			animatorStateTransition68240.exitTime = 0.95f;
			animatorStateTransition68240.hasExitTime = false;
			animatorStateTransition68240.hasFixedDuration = true;
			animatorStateTransition68240.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68240.offset = 0f;
			animatorStateTransition68240.orderedInterruption = true;
			animatorStateTransition68240.isExit = false;
			animatorStateTransition68240.mute = false;
			animatorStateTransition68240.solo = false;
			animatorStateTransition68240.AddCondition(AnimatorConditionMode.Greater, 0.5f, "AbilityFloatData");
			animatorStateTransition68240.AddCondition(AnimatorConditionMode.Greater, -0.1f, "HorizontalMovement");
			animatorStateTransition68240.AddCondition(AnimatorConditionMode.Less, 0.1f, "HorizontalMovement");

			var animatorStateTransition68220 = standingCoverIdleLeftAnimatorState68372.AddTransition(standingCoverAimLeftAnimatorState68386);
			animatorStateTransition68220.canTransitionToSelf = true;
			animatorStateTransition68220.duration = 0.01f;
			animatorStateTransition68220.exitTime = 0.95f;
			animatorStateTransition68220.hasExitTime = false;
			animatorStateTransition68220.hasFixedDuration = false;
			animatorStateTransition68220.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68220.offset = 0f;
			animatorStateTransition68220.orderedInterruption = true;
			animatorStateTransition68220.isExit = false;
			animatorStateTransition68220.mute = false;
			animatorStateTransition68220.solo = false;
			animatorStateTransition68220.AddCondition(AnimatorConditionMode.Equals, 3f, "AbilityIntData");

			var animatorStateTransition68318 = standingCoverIdleLeftAnimatorState68372.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68318.canTransitionToSelf = true;
			animatorStateTransition68318.duration = 0.15f;
			animatorStateTransition68318.exitTime = 0.95f;
			animatorStateTransition68318.hasExitTime = false;
			animatorStateTransition68318.hasFixedDuration = true;
			animatorStateTransition68318.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68318.offset = 0f;
			animatorStateTransition68318.orderedInterruption = true;
			animatorStateTransition68318.isExit = false;
			animatorStateTransition68318.mute = false;
			animatorStateTransition68318.solo = false;
			animatorStateTransition68318.AddCondition(AnimatorConditionMode.Greater, 0.1f, "HorizontalMovement");

			var animatorStateTransition68286 = standingCoverIdleLeftAnimatorState68372.AddTransition(standingCoverStrafeAnimatorState68376);
			animatorStateTransition68286.canTransitionToSelf = true;
			animatorStateTransition68286.duration = 0.15f;
			animatorStateTransition68286.exitTime = 0.95f;
			animatorStateTransition68286.hasExitTime = false;
			animatorStateTransition68286.hasFixedDuration = true;
			animatorStateTransition68286.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68286.offset = 0f;
			animatorStateTransition68286.orderedInterruption = true;
			animatorStateTransition68286.isExit = false;
			animatorStateTransition68286.mute = false;
			animatorStateTransition68286.solo = false;
			animatorStateTransition68286.AddCondition(AnimatorConditionMode.Less, -0.1f, "HorizontalMovement");

			var animatorStateTransition68304 = standingCoverIdleLeftAnimatorState68372.AddTransition(crouchingCoverIdleLeftAnimatorState68412);
			animatorStateTransition68304.canTransitionToSelf = true;
			animatorStateTransition68304.duration = 0.1f;
			animatorStateTransition68304.exitTime = 0.95f;
			animatorStateTransition68304.hasExitTime = false;
			animatorStateTransition68304.hasFixedDuration = true;
			animatorStateTransition68304.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68304.offset = 0f;
			animatorStateTransition68304.orderedInterruption = true;
			animatorStateTransition68304.isExit = false;
			animatorStateTransition68304.mute = false;
			animatorStateTransition68304.solo = false;
			animatorStateTransition68304.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");


			// State Machine Transitions.
			var animatorStateTransition68222 = baseStateMachine1824871762.AddAnyStateTransition(takeCrouchingCoverAnimatorState68370);
			animatorStateTransition68222.canTransitionToSelf = true;
			animatorStateTransition68222.duration = 0.25f;
			animatorStateTransition68222.exitTime = 0.75f;
			animatorStateTransition68222.hasExitTime = false;
			animatorStateTransition68222.hasFixedDuration = true;
			animatorStateTransition68222.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68222.offset = 0f;
			animatorStateTransition68222.orderedInterruption = true;
			animatorStateTransition68222.isExit = false;
			animatorStateTransition68222.mute = false;
			animatorStateTransition68222.solo = false;
			animatorStateTransition68222.AddCondition(AnimatorConditionMode.If, 0f, "AbilityChange");
			animatorStateTransition68222.AddCondition(AnimatorConditionMode.Equals, 401f, "AbilityIndex");
			animatorStateTransition68222.AddCondition(AnimatorConditionMode.Greater, 0.5f, "Height");

			var animatorStateTransition68300 = baseStateMachine1824871762.AddAnyStateTransition(takeStandingCoverAnimatorState68398);
			animatorStateTransition68300.canTransitionToSelf = true;
			animatorStateTransition68300.duration = 0.25f;
			animatorStateTransition68300.exitTime = 0.75f;
			animatorStateTransition68300.hasExitTime = false;
			animatorStateTransition68300.hasFixedDuration = true;
			animatorStateTransition68300.interruptionSource = TransitionInterruptionSource.None;
			animatorStateTransition68300.offset = 0f;
			animatorStateTransition68300.orderedInterruption = true;
			animatorStateTransition68300.isExit = false;
			animatorStateTransition68300.mute = false;
			animatorStateTransition68300.solo = false;
			animatorStateTransition68300.AddCondition(AnimatorConditionMode.If, 0f, "AbilityChange");
			animatorStateTransition68300.AddCondition(AnimatorConditionMode.Equals, 401f, "AbilityIndex");
			animatorStateTransition68300.AddCondition(AnimatorConditionMode.Less, 0.5f, "Height");
		}
	}
}
