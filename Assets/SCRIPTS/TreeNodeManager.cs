using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using Pixelplacement;
using System.Linq;


public class TreeNodeManager : Singleton<TreeNodeManager> {

    public GameObject errorNode;
    public GameObject nodePrefab;
    public Sprite[] nodeSprites;
    public List<System.Action> doOnStart = new List<System.Action>();
    public Dictionary<string, GameObject> nodes = new Dictionary<string, GameObject>();
    public GameObject[] rootNodes;
    public WatchScript watch;
    public int ready = 0;
    public const int prepActions = 2;
    public bool win = false;
    public bool lose = false;
    public Yarn.Unity.DialogueRunner dr;
    public TransitionManager tm;

    private void Start() {
        print("On Tree Node Manager Start...");
        // Make a key/sprite dict
        Dictionary<string, Sprite> spriteRef = new Dictionary<string, Sprite>();
        foreach (Sprite cur in nodeSprites) {
            //print(cur.name);
            spriteRef.Add(cur.name, cur);
        }
        List<GameObject> rootNodeList = new List<GameObject>();
        XmlSerializer xmlSer = new XmlSerializer(typeof(FamilyTreeNode));
        string path = "DATA/TreeNodes";
        print(Resources.LoadAll<TextAsset>(path).Length);
        foreach(TextAsset xml in Resources.LoadAll<TextAsset>(path)) {
            print(xml.text);
            try {
                MemoryStream xmlStream = new MemoryStream(xml.bytes);
                //XmlReader reader = XmlReader.Create(xml.text);
                //FileStream evidenceItem = new FileStream(fileName, FileMode.Open);
                //print(fileName);
                //FamilyTreeNode newNode = (FamilyTreeNode)xmlSer.Deserialize(xml.text);
                //FamilyTreeNode newNode = (FamilyTreeNode)xmlSer.Deserialize(reader);
                FamilyTreeNode newNode = (FamilyTreeNode)xmlSer.Deserialize(xmlStream);
                // Build the gameobject based on the new evidence base
                GameObject newGO = Instantiate(nodePrefab);
                newGO.name = newNode.ID;
                Sprite nodeSprite;
                if (!spriteRef.TryGetValue(newNode.Icon, out nodeSprite)) {
                    nodeSprite = null;
                }
                TreeNode script = newGO.GetComponent<TreeNode>();
                script.SetFromBaseData(newNode);
                script.PrepNode(nodeSprite);
                if (string.IsNullOrEmpty(script.ParentName)) {
                    rootNodeList.Add(newGO);
                }
                newGO.SetActive(false);
                nodes.Add(newNode.ID, newGO);
            } finally {
                //print(fileName + " attempted.");
            }
            
        }
        rootNodes = rootNodeList.ToArray();
        PrepActionDone();
    }

    public GameObject GetNode(string nodeName) {
        if (nodes.ContainsKey(nodeName)) {
            return nodes[nodeName];
        } else {
            return Instantiate(errorNode);
        }
    }

    public void Update()
    {
        if(win || lose)
            if (!dr.isDialogueRunning)
            {
                tm.TitleScreen();
            }
    }

    private GameObject AttachAllChildren(GameObject node) {
        TreeNode nodeData = node.GetComponent<TreeNode>();
        string[] childNames = nodeData.GetChildNames();
        if (childNames == null) {
            return node;
        }
        GameObject[] kids = new GameObject[childNames.Length];
        for (int i = 0; i < childNames.Length; i++) {
            string curChildName = childNames[i];
            GameObject curChild;
            if (nodes.ContainsKey(curChildName)) {
                curChild = nodes[curChildName];
            } else {
                curChild = errorNode;
            }
            kids[i] = AttachAllChildren((curChild));
        }
        nodeData.ChildNodes = kids;
        return node;
    }

    public GameObject[] GetNodeTree() {
        GameObject[] nodeTreeRoot = new GameObject[rootNodes.Length];
        for (var i = 0; i < rootNodes.Length; i++) {
            nodeTreeRoot[i] = AttachAllChildren((rootNodes[i]));
        }
        return nodeTreeRoot;
    }

    public GameObject[] GetAllNodes() {
        if (nodes.Count > 0) {
            GameObject[] baseVersions = nodes.Values.ToArray();
            GameObject[] clones = new GameObject[nodes.Count];
            int i = 0;
            foreach (GameObject cur in baseVersions) {
                clones[i] = Instantiate(cur);
                clones[i].name = cur.name;
                clones[i].SetActive(true);
                i++;
            }
            return clones;
        } else {
            return new GameObject[1] { Instantiate(errorNode) };
        }
    }

    public void RunOnReady(System.Action toDo) {
        if (ready == prepActions) {
            toDo();
        } else {
            doOnStart.Add(toDo);
        }
    }

    private void PrepActionDone() {
        ready++;
        if (ready == prepActions) {
            print("TreeNode Manager Prep Done");
            foreach (System.Action waiting in doOnStart) {
                waiting();
            }
        }
    }

    protected override void OnRegistration() {
        base.OnRegistration();
        PrepActionDone();
    }

    public void CheckCurrentNodeCorrectness() {
        foreach(GameObject cur in nodes.Values) {
            TreeNode curNode = cur.GetComponent<TreeNode>();
            bool isOneNull = curNode.currentEvidence == null || curNode.CorrectItem == null;
            bool bothNull = isOneNull && (curNode.currentEvidence == null && curNode.CorrectItem == null);
            bool match = !isOneNull && curNode.currentEvidence.GetComponent<EvidenceBase>().ID == curNode.CorrectItem;
            if (!bothNull && !match) {
                Incorrect();
                print(curNode.name);
                if (isOneNull && curNode.currentEvidence == null) {
                    print(null);
                } else {
                    print(curNode.currentEvidence.GetComponent<EvidenceBase>().ID);
                }
                print(curNode.CorrectItem);
                return;
            }
        }
        Correct();
    }

    private void Incorrect() {
        
        watch.Hour-= 1;
        if (watch.Hour == 0)
        {
            GameObject.Find("YarnSpinnerHolder").GetComponent<Yarn.Unity.DialogueRunner>().StartDialogue("TotalFailure");
            lose = true;
        }
        else
        {
            GameObject.Find("YarnSpinnerHolder").GetComponent<Yarn.Unity.DialogueRunner>().StartDialogue("Failure");
        }
        print("WRONG");
    }

    private void Correct() {
        print("RIGHT");
        GameObject.Find("YarnSpinnerHolder").GetComponent<Yarn.Unity.DialogueRunner>().StartDialogue("Success");
        win = true;
    }
}
