using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceTableSetup : MonoBehaviour {

    private void Startup() {
        //print("Hello!");
        GameObject[] allEvidence = EvidenceManager.Instance.getAllEvidence();
        int x = 0;
        foreach (GameObject item in allEvidence) {
            Transform evidenceReference = transform.Find(item.GetComponent<EvidenceBase>().ID);
            if (evidenceReference != null) {
                foreach (Transform child in evidenceReference) {
                    Destroy(child.gameObject);
                }
                GameObject tableSpot = evidenceReference.gameObject;
                item.transform.parent = tableSpot.transform;
                item.transform.localPosition = new Vector3(0, 0, 0);
                item.transform.localScale = new Vector3(1, 1, 1);
            }
            x += 2;
        }
    }

    // Use this for initialization
    void Start() {
        EvidenceManager.Instance.runOnReady(new System.Action(Startup));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
