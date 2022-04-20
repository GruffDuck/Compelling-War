using System.Collections.Generic;
using UnityEngine;

// This class is created for the example scene. There is no support for this script.
public class InteractiveSwitch : MonoBehaviour
{
	public List<TargetHealth> targets;
	public bool startVisible;
	public InteractiveSwitch nextStage;
	public bool levelEnd;
	public bool timeTrial;
	public AudioClip activateSound;

	private GameObject player;
	private TargetHealth boss;
	private int minionsDead = 0;
	private State currentState;

	private TimeTrialManager timer;

	private enum State
	{
		DISABLED,
		MINIONS,
		BOSS,
		END
	}

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		this.ToggleState(false, startVisible);
		timer = GameObject.Find("Timer").GetComponent<TimeTrialManager>();

		if (levelEnd)
		{
			currentState = State.END;
		}
		else
			currentState = State.DISABLED;
	}

	void Update()
	{
		switch (currentState)
		{
			case State.MINIONS:
				minionsDead = 0;
				foreach (TargetHealth target in targets)
				{
					if (!target.boss && target.IsDead)
					{
						minionsDead++;
					}
				}
				if (minionsDead == targets.Count - 1)
				{
					boss.Revive();
					currentState = State.BOSS;
				}
				break;
			case State.BOSS:
				if(boss.IsDead)
				{
					this.ToggleState(false, false);
					if(nextStage)
					{
						nextStage.ToggleState(false, true);
					}
				}
				break;
		}
	}

	public void ToggleState(bool active, bool visible)
	{
		if (active)
			currentState = State.MINIONS;
		else
			currentState = State.DISABLED;
		this.GetComponent<BoxCollider>().enabled = visible;
		this.GetComponent<MeshRenderer>().enabled = visible;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == player)
		{
			if (levelEnd)
			{
				timer.EndTimer();

				ToggleState(false, false);

				if (nextStage)
				{
					nextStage.ToggleState(false, true);
				}
			}
			else
			{
				if(timeTrial && !timer.IsRunning)
				{
					timer.StartTimer();
				}
				ToggleState(true, false);
				foreach (TargetHealth target in targets)
				{
					if (!target.boss)
					{
						target.Revive();
					}
					else
					{
						boss = target;
						boss.Kill();
					}
				}
			}
			AudioSource.PlayClipAtPoint(activateSound, transform.position + Vector3.up);
		}
	}
}

