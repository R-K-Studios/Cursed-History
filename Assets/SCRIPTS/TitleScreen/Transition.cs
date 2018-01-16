using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour {

    CanvasGroup MyGroup;
    float destinationAlpha = 0;

	// Use this for initialization
	void Start () {
        MyGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () {

        if(Mathf.Abs(MyGroup.alpha - destinationAlpha) < .02f)
        {
            MyGroup.alpha = destinationAlpha;
        }

		if(MyGroup.alpha > destinationAlpha)
        {
            MyGroup.alpha -= .01f;
        }
        else if(MyGroup.alpha < destinationAlpha)
        {
            MyGroup.alpha += .01f;
        }

	}
}
