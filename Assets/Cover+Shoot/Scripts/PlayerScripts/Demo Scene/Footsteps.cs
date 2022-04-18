using UnityEngine;

// This class is created for the example scene. There is no support for this script.
public class Footsteps : MonoBehaviour
{
	public AudioClip[] stepClips;

	private Animator anim;
	private int index;
	private Transform lFoot, rFoot;
	private float dist;
	private int groundedBool, coverBool, aimBool, crouchFloat;
	private bool grounded;
	private enum Foot
	{
		LEFT,
		RIGHT
	}
	private Foot step = Foot.LEFT;
	private float oldDist, maxDist = 0;

	void Awake()
	{
		anim = this.GetComponent<Animator>();
		lFoot = anim.GetBoneTransform(HumanBodyBones.LeftFoot);
		rFoot = anim.GetBoneTransform(HumanBodyBones.RightFoot);
		groundedBool = Animator.StringToHash("Grounded");
		coverBool = Animator.StringToHash("Cover");
		aimBool = Animator.StringToHash("Aim");
		crouchFloat = Animator.StringToHash("Crouch");
	}

	private void Update()
	{
		if (!grounded && anim.GetBool(groundedBool))
		{
			PlayFootStep();
		}
		grounded = anim.GetBool(groundedBool);

		float factor = 0.15f;
		if(anim.GetBool(coverBool) || anim.GetBool(aimBool))
		{
			if (anim.GetFloat(crouchFloat) < 0.5f && !anim.GetBool(aimBool))
				factor = 0.17f;
			else
				factor = 0.11f;
		}

		if(grounded && anim.velocity.magnitude > 1.6f)
		{
			oldDist = maxDist;
			switch(step)
			{
				case Foot.LEFT:
					dist = lFoot.position.y - transform.position.y;
					maxDist = dist > maxDist ? dist : maxDist;
					if (dist <= factor)
					{
						PlayFootStep();
						step = Foot.RIGHT;
					}
					break;
				case Foot.RIGHT:
					dist = rFoot.position.y - transform.position.y;
					maxDist = dist > maxDist ? dist : maxDist;
					if (dist <= factor)
					{
						PlayFootStep();
						step = Foot.LEFT;
					}
					break;
			}
		}
	}


	private void PlayFootStep()
	{
		// still stepping away
		if (oldDist < maxDist)
			return;

		oldDist = maxDist = 0;
		int oldIndex = index;
		while (oldIndex == index)
		{
			index = (int)Random.Range(0, stepClips.Length - 1);
		}
		AudioSource.PlayClipAtPoint(stepClips[index], transform.position, 0.2f);
	}
}
