using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class NodeItemHandle : MonoBehaviour {

    Vector3 initLocalScale;
    bool objectOver = false;
    public GameObject EvidenceBox;
    public StateMachine myStateMachine;

    // Use this for initialization
    void Start () {
        Transform t = transform.Find("AttachedEvidence");
        print(t);
        t = t.Find("EvidenceItem");
        print(t);
        myStateMachine = t.GetComponent<StateMachine>();
        print(myStateMachine);
    }
	
	// Update is called once per frame
	void Update () {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        objectOver = true;
        initLocalScale = EvidenceBox.transform.localScale;
        transform.localScale = initLocalScale * 1.5f;
        print("HEY");
    }

    private void OnTriggerExit2D(Collider2D collision) {
        objectOver = false;
        transform.localScale = initLocalScale;
        print("BYE");
    }

    public void HandleDrop(GameObject DroppedEvidence) {
        print(DroppedEvidence);
        string evidenceID = DroppedEvidence.GetComponent<EvidenceBase>().ID;
        print(evidenceID);
        myStateMachine.ChangeState(evidenceID);
    }

}
