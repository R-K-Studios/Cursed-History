using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using Pixelplacement;
using UnityEngine.EventSystems;
using System;
using Yarn.Unity;

public class Parent {
    [XmlAttribute("ShowLink")]
    public string ShowLink { get; set; }
    [XmlText]
    public string Name { get; set; }

}

public class FamilyTreeNode {
    public string Name { get; set; }
    public string ID { get; set; }
    [XmlElement("Portrait")]
    public string Icon { get; set; }
    public string CorrectItem { get; set; }

    public int SiblingRank { get; set; }

    [XmlElement("Parent")]
    public Parent Parent { get; set; }

    [XmlArray("Children")]
    [XmlArrayItem("Child")]
    public string[] Chidren { get; set; }
    public string Bio { get; set; }
    [XmlElement("Yarn")]
    public string Yarn { get; set; }
}

public class TreeNode : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IComparable<TreeNode> {

    public GameObject Main;
    public GameObject portraitSpot;
    public GameObject evidenceItemSpot;
    public GameObject spline;
    public TMPro.TextMeshPro NamePlate;
    public int SiblingRank;
    public string NodeName;
    public string ID;
    public string Icon;
    public string CorrectItem;
    public string ParentName;
    public GameObject ParentNode;
    public bool ShowParentLink;
    public GameObject[] ChildNodes;
    public string Bio;

    public GameObject currentEvidence;
    public StateMachine myStateMachine;
    public Vector3 initLocalScale;
    public string[] childNames;
    public bool objectOver = false;

    public string YarnNodeTitle;


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
        SiblingRank = setFrom.SiblingRank;
        ParentName = setFrom.Parent.Name;
        ShowParentLink = (setFrom.Parent.ShowLink == "true" || setFrom.Parent.ShowLink == null);
        if (setFrom.Chidren == null) {
            childNames = null;
            ChildNodes = null;
        } else {
            childNames = setFrom.Chidren;
            ChildNodes = new GameObject[childNames.Length];

        }
        Bio = (string)setFrom.Bio.Clone();
        YarnNodeTitle = (string)setFrom.Yarn.Clone();
        NamePlate.text = ID;
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

    private bool ValidityCheck(PointerEventData eventData) {
        return (eventData.dragging && eventData.pointerDrag.CompareTag("Evidence"));
    }

    private void HandleDrop(GameObject DroppedEvidence) {
        EvidenceBase curEvidenceBase = DroppedEvidence.GetComponentInChildren<EvidenceBase>();
        currentEvidence = curEvidenceBase.gameObject;
        print(curEvidenceBase.ID);
        myStateMachine.ChangeState(curEvidenceBase.ID);
        objectOver = false;
        Main.transform.localScale = initLocalScale;
    }

    public void OnDrop(PointerEventData eventData) {
        if (!ValidityCheck(eventData)) {
            return;
        }
        HandleDrop(eventData.pointerDrag);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!ValidityCheck(eventData)) {
            return;
        }
        objectOver = true;
        initLocalScale = Main.transform.localScale;
        Main.transform.localScale = initLocalScale * 1.5f;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!ValidityCheck(eventData) || !objectOver) {
            return;
        }
        objectOver = false;
        Main.transform.localScale = initLocalScale;
    }

    public void OnMouseUp()
    {
        //Debug.Log(YarnNodeTitle);
        DialogueRunner dr = GameObject.Find("YarnSpinnerHolder").GetComponent<DialogueRunner>();
        if(!dr.isDialogueRunning)
            dr.StartDialogue(YarnNodeTitle);
    }

    public int CompareTo(TreeNode other) {
        return SiblingRank - other.SiblingRank;
    }
}
