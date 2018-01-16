using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initEvidenceDropStates : MonoBehaviour {
    public GameObject evidenceSM;

    private void startup() {
        GameObject[] allEvidence = EvidenceManager.Instance.getAllEvidence();
        foreach (GameObject item in allEvidence) {
            item.transform.parent = evidenceSM.transform;
            item.transform.localPosition = new Vector3(0, 0, 0);
            item.transform.localScale = new Vector3(1, 1, 1);
        }
        evidenceSM.SetActive(true);
    }

	// Use this for initialization
	void Start () {
        EvidenceManager.Instance.runOnReady(new System.Action(startup));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
