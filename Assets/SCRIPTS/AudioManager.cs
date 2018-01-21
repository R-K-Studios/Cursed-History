using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using System;

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

    internal void popupStartSound() {
        //throw new NotImplementedException();
    }

    internal void popupSound(bool showPopup) {
        //throw new NotImplementedException();
    }
}
