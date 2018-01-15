﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPointer : MonoBehaviour {
    public float[] MenuAngles = new float[3];
    public int selection = 0;
    int LastSelection = -1;

    public Text[] menuOptions;
    public float[] menuStartX;

    float offset = 20;
    float destinationRotation;

    // Use this for initialization
	void Start () {
        destinationRotation = MenuAngles[0];
        menuStartX = new float[menuOptions.Length];
        for(int i=0; i < menuOptions.Length; i++)
        {
            menuStartX[i] = menuOptions[i].rectTransform.localPosition.x;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (selection != LastSelection)
        {
            //run coroutine

            LastSelection = selection;
            destinationRotation = MenuAngles[selection];
            StartCoroutine(RotateHand());
        }

		for(int i = 0; i < menuOptions.Length; i++)
        {
            if (i != selection)
            {
                menuOptions[i].rectTransform.localPosition = new Vector2(menuStartX[i], menuOptions[i].rectTransform.localPosition.y);
                menuOptions[i].color = Color.white;
            }
            else
            {
                menuOptions[i].rectTransform.localPosition = new Vector2(menuStartX[i]+offset, menuOptions[i].rectTransform.localPosition.y);
                menuOptions[i].color = Color.yellow;
            }
        }


        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selection++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selection--;
        }

        selection = (selection < 0) ? menuOptions.Length - 1 : (selection >= menuOptions.Length) ? 0 : selection;


    }

    IEnumerator RotateHand()
    {
        float moveSpeed = 1.5f;

        while (transform.localRotation.z != destinationRotation)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, destinationRotation), moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(0, 0, destinationRotation);
        yield return null;
    }
}