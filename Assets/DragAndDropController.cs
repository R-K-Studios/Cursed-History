using UnityEngine;
using System.Collections;
public class DragAndDropController : MonoBehaviour {
    private bool draggingItem = false;
    private GameObject draggedObject;
    private Vector2 touchOffset;
    private Vector2 initPos;
    private ContactFilter2D noFilter = new ContactFilter2D();

    private void Start() {
        noFilter.NoFilter();
    }

    void Update() {
        if (HasInput) {
            DragOrPickUp();
        } else {
            if (draggingItem)
                DropItem();
        }
    }

    Vector2 CurrentTouchPosition {
        get {
            Vector2 inputPos;
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return inputPos;
        }
    }

    private void DragOrPickUp() {
        var inputPosition = CurrentTouchPosition;

        if (draggingItem) {
            Vector3 p = inputPosition + touchOffset;
            p.z = -200;
            draggedObject.transform.position = p;
        } else {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0) {
                var hit = touches[0];
                if (hit.transform != null) {
                    draggedObject = hit.transform.gameObject;
                    if (draggedObject.tag != "Evidence") {
                        draggedObject = null;
                        return;
                    }
                    draggingItem = true;
                    initPos = hit.transform.position;
                    touchOffset =  initPos - inputPosition;
                    draggedObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }
            }
        }
    }

    private bool HasInput {
        get {
            // returns true if either the mouse button is down or at least one touch is felt on the screen
            return Input.GetMouseButton(0);
        }
    }

    void DropItem() {
        draggingItem = false;
        Collider2D draggedCollider = draggedObject.GetComponent<Collider2D>();
        if (draggedObject != null) {
            Collider2D[] results = new Collider2D[1];
            int numResults = draggedCollider.OverlapCollider(noFilter, results);
            if (numResults != 0) {
                GameObject node = results[0].gameObject;
                TreeNode nodeScript = node.GetComponent<TreeNode>();
                if (nodeScript != null) {
                    nodeScript.HandleDrop(draggedObject.transform.GetChild(0).gameObject);
                }
            }
        }
        draggedObject.transform.localScale = new Vector3(1f, 1f, 1f);
        draggedObject.transform.position = initPos;
    }
}