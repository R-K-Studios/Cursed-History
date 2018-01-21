using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using Pixelplacement;

public class FamilyTreeNode {
    public string Name { get; set; }
    public string ID { get; set; }
    public string Icon { get; set; }
    public string CorrectItem { get; set; }
    public string Parent { get; set; }

    [XmlArray("Children")]
    [XmlArrayItem("Child")]
    public string[] Chidren { get; set; }
    public string Bio { get; set; }
}

public class TreeNode : MonoBehaviour {

    public GameObject Main;
    public GameObject portraitSpot;
    public GameObject evidenceItemSpot;
    public GameObject spline;
    public string NodeName;
    public string ID;
    public string Icon;
    public string CorrectItem;
    public string ParentName;
    public GameObject ParentNode;
    public GameObject[] ChildNodes;
    public string Bio;

    public GameObject currentEvidence;
    public StateMachine myStateMachine;
    public Vector3 initLocalScale;
    public string[] childNames;
    public bool objectOver = false;

    // Use this for initialization
    void Start() {
        myStateMachine = evidenceItemSpot.GetComponent<StateMachine>();
    }

    public void SetFromBaseData(FamilyTreeNode setFrom) {
        print("Setting the node base...");
        NodeName = (string)setFrom.Name.Clone();
        ID = (string)setFrom.ID.Clone();
        Icon = (string)setFrom.Icon.Clone();
        CorrectItem = (string)setFrom.CorrectItem.Clone();
        ParentName = setFrom.Parent;
        if(setFrom.Chidren == null) {
            childNames = null;
            ChildNodes = null;
        } else {
            childNames = setFrom.Chidren;
            ChildNodes = new GameObject[childNames.Length];

        }
        Bio = (string)setFrom.Bio.Clone();
    }

    public void PrepNode(Sprite sprite) {
        if (sprite != null) {
            SpriteRenderer charPortrait = portraitSpot.GetComponent<SpriteRenderer>();
            charPortrait.enabled = true;
            charPortrait.sprite = sprite;
            portraitSpot.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Getter
    public string[] GetChildNames() {
        return childNames;
    }

    // Code for collision behaviors
    private void OnTriggerEnter2D(Collider2D collision) {
        objectOver = true;
        initLocalScale = Main.transform.localScale;
        Main.transform.localScale = initLocalScale * 1.5f;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        objectOver = false;
        Main.transform.localScale = initLocalScale;
    }

    public void HandleDrop(GameObject DroppedEvidence) {
        currentEvidence = DroppedEvidence;
        string evidenceID = currentEvidence.GetComponent<EvidenceBase>().ID;
        print(evidenceID);
        myStateMachine.ChangeState(evidenceID);
    }
}
