using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class AudioManager : Singleton<AudioManager> {

    // Use this for initialization
    void Awake () {
        AudioManager instance = FindObjectOfType<AudioManager>();
        if(instance != this)
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
