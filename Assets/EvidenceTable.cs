using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceTable : MonoBehaviour {

    public GameObject TableItemPrefab;

    private void startup() {
        print("Hello!");
        GameObject[] allEvidence = EvidenceManager.Instance.getAllEvidence();
        int x = 0;
        foreach (GameObject item in allEvidence) {
            GameObject tableItem = Instantiate(TableItemPrefab);
            print(tableItem);
            print(item);
            item.transform.parent = tableItem.transform;
            item.transform.localPosition = new Vector3(0, 0, 0);
            item.transform.localScale = new Vector3(1, 1, 1);
            tableItem.transform.parent = this.transform;
            tableItem.transform.localPosition = new Vector3(x, 0, 0);
            tableItem.transform.localScale = new Vector3(1, 1, 1);
            x += 2;
        }
    }

    // Use this for initialization
    void Start() {
        EvidenceManager.Instance.runOnReady(new System.Action(startup));
    }

    // Update is called once per frame
    void Update () {
		
	}
}
