using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Fluctuate : MonoBehaviour {

    public float alpha;
    public Gradient alphaGradient;
    Image myImage;
	// Use this for initialization
	void Start () {
        myImage = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        
        myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, alphaGradient.Evaluate(Time.deltaTime).a);
	}
}
