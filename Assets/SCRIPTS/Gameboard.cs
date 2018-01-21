using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour {

    public int rowSize = 5;
    public GameObject spots;

    private void SetupBoard() {
        print("+++Setting up the gameboard...");
        GameObject[] curLevel = TreeNodeManager.Instance.GetNodeTree();
        int y = 0;
        List<GameObject> nextLevel;
        GameObject parent = null;
        Transform row;
        do {
            int x = (rowSize - curLevel.Length) / 2;
            nextLevel = new List<GameObject>();
            row = spots.transform.Find(y.ToString());
            foreach (GameObject node in curLevel) {
                if (x == rowSize) {
                    break;
                }
                // We treat null as a null space, unless there's not enough room
                if (node == null) {
                    if (curLevel.Length < rowSize) {
                        x++;
                    }
                    print(x);
                    continue;
                }
                // If it's a valid node, insert it and make sure it's properly scaled/positioned
                Transform position = row.Find(x.ToString());
                node.transform.parent = position;
                node.transform.localPosition = new Vector3(0, 0, node.transform.localPosition.z);
                node.transform.localScale = new Vector3(1, 1, 1);
                GameObject[] children = node.GetComponent<TreeNode>().ChildNodes;
                if (children.Length > 0) {
                    foreach (GameObject kid in children) {
                        // Set the current node as this child's parent
                        kid.GetComponent<TreeNode>().ParentNode = node;
                    }
                    nextLevel.AddRange(children);
                    nextLevel.Add(null);
                }
                TreeNode nodeCode = node.GetComponent<TreeNode>();
                if (nodeCode.ParentNode != null) {
                    // Draw the line between the two icons
                    GameObject spline = nodeCode.spline;
                    Vector3 homeLocation = node.transform.position;
                    homeLocation -= new Vector3(0, 0, -5);
                    Vector3 parentLocation = nodeCode.ParentNode.transform.position;
                    parentLocation -= new Vector3(0, 0, -5);
                    spline.transform.Find("ParentAnchor").position = parentLocation;
                    Vector3 middleLocation = (parentLocation + homeLocation) / 2;
                    Transform middleAnchor = spline.transform.Find("MiddleAnchor");
                    middleAnchor.position = middleLocation;
                    middleAnchor.Find("InTangent").transform.position = new Vector3(homeLocation.x, middleLocation.y, middleLocation.z);
                    middleAnchor.Find("OutTangent").transform.localPosition = -1 * middleAnchor.Find("InTangent").transform.localPosition;
                    spline.GetComponent<LineRenderer>().enabled = true;
                }
                node.SetActive(true);
                x++;
                print("X: " + x);
            }
            // Clear out the extra null, if one should be present
            if (nextLevel.Count > 0) {
                nextLevel.RemoveAt(nextLevel.Count - 1);
            }
            curLevel = nextLevel.ToArray();
            y++;
            print("Y: " + y);
        } while (nextLevel.Count > 0);
    }

    // Use this for initialization
    void Start () {
        TreeNodeManager.Instance.RunOnReady(new Action(SetupBoard));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
