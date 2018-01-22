using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using Pixelplacement;
using System.Linq;

public class EvidenceManager : Singleton<EvidenceManager> {

    public GameObject table;
    private List<System.Action> doOnStart = new List<System.Action>();
    private Dictionary<string, GameObject> evidence = new Dictionary<string, GameObject>();
    private int ready = 0;
    private const int prepActions = 2;
    public GameObject errorEvidence;
    public GameObject evidencePrefab;
    public Sprite[] EvidenceSprites;

    private void Start() {
        // Make a key/sprite dict
        Dictionary<string, Sprite> spriteRef = new Dictionary<string, Sprite>();
        foreach (Sprite cur in EvidenceSprites) {
            //print(cur.name);
            spriteRef.Add(cur.name, cur);
        }
        XmlSerializer xmlSer = new XmlSerializer(typeof(EvidenceBaseData));
        string path = Path.Combine("DATA", "ItemXML");
        foreach (TextAsset xml in Resources.LoadAll<TextAsset>(path)) {
            print(xml.text);
            try {
                MemoryStream xmlStream = new MemoryStream(xml.bytes);
                EvidenceBaseData newItem = (EvidenceBaseData)xmlSer.Deserialize(xmlStream);
                // Build the gameobject based on the new evidence base
                GameObject newGO = Instantiate(evidencePrefab);
                newGO.name = newItem.ID;
                newGO.GetComponent<SpriteRenderer>().sprite = spriteRef[newItem.Icon];
                newGO.GetComponent<EvidenceBase>().setFromBaseData(newItem);
                newGO.SetActive(false);
                evidence.Add(newItem.ID, newGO);
            } catch {
                // Do nothing
            }
        }
        prepActionDone();
    }

    public GameObject getEvidence(string evidenceName) {
        if(evidence.ContainsKey(evidenceName)) {
            return evidence[evidenceName];
        } else {
            return Instantiate(errorEvidence);
        }
    }

    public GameObject[] getAllEvidence() {
        if (evidence.Count > 0) {
            GameObject[] baseVersions = evidence.Values.ToArray();
            GameObject[] clones = new GameObject[evidence.Count];
            int i = 0;
            foreach (GameObject cur in baseVersions) {
                clones[i] = Instantiate(cur);
                clones[i].name = cur.name;
                clones[i].SetActive(true);
                i++;
            }
            return clones;
        } else {
            return new GameObject[1] { Instantiate(errorEvidence) };
        }
    }

    public void runOnReady(System.Action toDo) {
        if (ready == prepActions) {
            toDo();
        } else {
            doOnStart.Add(toDo);
        }
    }

    private void prepActionDone() {
        ready++;
        if (ready == prepActions) {
            //print("EvidenceManager Prep Done");
            foreach(System.Action waiting in doOnStart) {
                waiting();
            }
            table.SetActive(true);
        }
    }

    protected override void OnRegistration() {
        base.OnRegistration();
        prepActionDone();
    }

}
