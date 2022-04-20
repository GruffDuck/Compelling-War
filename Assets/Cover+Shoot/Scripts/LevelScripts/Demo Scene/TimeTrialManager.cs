using UnityEngine;
using UnityEngine.UI;

// This class is created for the example scene. There is no support for this script.
public class TimeTrialManager : MonoBehaviour
{
	public Vector3 playerPosition;
	private float bestTime, totalTime, startTime = 0;
	private Text bestTimeLabel, currentTimeLabel;
	private bool isTimerRunning = false;

	private GameObject player;

	public bool IsRunning { get { return isTimerRunning; } }

	void Awake()
	{
		currentTimeLabel = this.transform.Find("Current").GetComponent<Text>();
		bestTimeLabel = this.transform.Find("Best").GetComponent<Text>();

		if (PlayerPrefs.HasKey("bestTime"))
		{
			bestTime = PlayerPrefs.GetFloat("bestTime");
			bestTimeLabel.text = bestTime.ToString("n2");
		}
		else
		{
			bestTimeLabel.text = "";
		}
		currentTimeLabel.text = "0.00";
		currentTimeLabel.gameObject.SetActive(false);
		bestTimeLabel.gameObject.SetActive(false);
	}

	private void Update()
	{
		if(isTimerRunning)
		{
			currentTimeLabel.text = (Time.time - startTime).ToString("n2");
		}
	}

	public void StartTimer()
	{
		isTimerRunning = true;
		startTime = Time.time;
		currentTimeLabel.gameObject.SetActive(true);
		bestTimeLabel.gameObject.SetActive(true);
	}

	public void EndTimer()
	{
		totalTime = Time.time - startTime;
		isTimerRunning = false;
		startTime = 0;

		if (bestTime == 0 || (bestTime > 0 && totalTime < bestTime))
		{
			bestTime = totalTime;
			currentTimeLabel.text = bestTimeLabel.text = bestTime.ToString("n2");
			PlayerPrefs.SetFloat("bestTime", bestTime);
		}
	}
}
