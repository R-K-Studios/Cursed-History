using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour {

    public CanvasGroup MyGroup;
    float activeYPosition;

    //Bool for if the Menu is on screen
    bool active = false;
    //Bool for if the Menu is transitioning
    bool moving = false;

    // Use this for initialization
    public void Start () {
        //Get the active position
        activeYPosition = transform.localPosition.y;
        //Move it out of the way
        transform.Translate(new Vector3(0, -1, 0));
        //Make it invisible
        MyGroup.alpha = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //Is the menu currently on screen?
    public bool IsActive()
    {
        return active;
    }




    //Coroutine to fade in and out the menu and move it between positions
    IEnumerator MovePanel()
    {
        moving = true;
        if (active && transform.localPosition.y < activeYPosition)
        {
            while (transform.localPosition.y < activeYPosition)
            {
                transform.Translate(0, .1f, 0);
                MyGroup.alpha += .2f;
                yield return null;
            }
            transform.localPosition.Set(transform.localPosition.x, activeYPosition, 0);
            yield return null;
        }
        else if (!active && MyGroup.alpha > 0)
        {
            while (MyGroup.alpha > 0)
            {
                MyGroup.alpha -= .1f;
                transform.Translate(0, -.1f, 0);
                yield return null;
            }
            yield return null;
        }
        moving = false;
        yield return null;
    }

    //Call up the menu
    public void SetActive()
    {
        if (!moving)
        {
            MyGroup.blocksRaycasts = true;
            active = true;
            StartCoroutine(MovePanel());
        }
    }

    //Dismiss the menue
    public void SetInactive()
    {
        if (!moving)
        {
            MyGroup.blocksRaycasts = false;
            active = false;
            StartCoroutine(MovePanel());
        }
    }

}
