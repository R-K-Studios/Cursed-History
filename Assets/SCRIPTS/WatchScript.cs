using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WatchScript : MonoBehaviour {

    public Image HourHand;
    public Image MinuteHand;
    public Image SecondHand;

    public float Hour;
    public float Minute;
    public float Second;
    public float speed;
    


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Minute += Time.deltaTime * speed;

        //Hour = Mathf.Floor(Hour) + (Minute / 60);

        Second -= Time.deltaTime * speed * 2;


        if(Minute < 0)
        {
            Minute += 60;
        }
        if(Hour < 0)
        {
            Hour += 12;
        }
        if(Second < 0)
        {
            Second += 60;
        }

        Minute = Minute % 60;
        Hour = Hour % 12;
        Second = Second % 60;

        HourHand.rectTransform.localRotation = Quaternion.Euler(0, 0, -GetAngleFromInterval(Hour, 12));
        MinuteHand.rectTransform.localRotation = Quaternion.Euler(0, 0, -GetAngleFromInterval(Minute, 60));
        SecondHand.rectTransform.localRotation = Quaternion.Euler(0, 0, -GetAngleFromInterval(Mathf.Floor(Second), 60));
    }

    public float GetAngleFromInterval(float interval, int maxIntervals)
    {
        float angle = 0;
        
        angle = (360 / maxIntervals) * interval;
        
        return angle;
    }


}
