using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PopUpImage : MonoBehaviour {

    public string YarnSummaryNode;
    BoxCollider2D myCollider;
    Yarn.Unity.DialogueRunner dr;
    GameObject yarnFrame;
    // Use this for initialization
    void Start () {
        myCollider = GetComponent<BoxCollider2D>();
        dr = GameObject.Find("YarnSpinnerHolder").GetComponent<Yarn.Unity.DialogueRunner>();
        yarnFrame = GameObject.Find("YarnFrame");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !yarnFrame.activeSelf)
        {
            Camera mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
            Vector2 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            
            if (!myCollider.OverlapPoint(mousePosition))
            {
                PopupManager pop = FindObjectOfType<PopupManager>();
                pop.SetPopup(false);
            }
        }
	}

    public void OnMouseDown()
    {
        
        if(dr.NodeExists(YarnSummaryNode) && !dr.isDialogueRunning)
            dr.StartDialogue(YarnSummaryNode);
        
    }

    
}
