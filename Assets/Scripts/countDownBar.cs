using UnityEngine;
using System.Collections;

public class countDownBar : MonoBehaviour {

	public UISlider slider;
	bool running = false;
	float leftTime;
	float counttime;

	public void Run(float secs)
	{
		counttime = leftTime = secs;
		slider.sliderValue = 1;
		running = true;
	}

	// Use this for initialization
	void Start () {
		//slider = this.gameObject.GetComponent<UISlider>();
	}
	
	// Update is called once per frame
	void Update () {
		if(running)
		{
			leftTime -= Time.deltaTime;
			slider.sliderValue = leftTime/counttime;
			if(leftTime <= 0)
				running = false;
		}
	}
}
