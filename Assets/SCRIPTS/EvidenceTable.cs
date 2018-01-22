using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EvidenceTable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler {

    public EvidenceBase evidenceRef;
    public float slowestClickSpeed;
    public float downTime;
    private Vector2 touchOffset;
    private Vector2 initPos;
    private int initLayer;
    public bool firstLook = false;

    void Update()
    {
            if(evidenceRef == null)
            evidenceRef = GetComponentInChildren<EvidenceBase>();

    }


    private Vector2 CurrentTouchPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    
    private void SetDraggedPosition(PointerEventData data) {
        Vector3 p = CurrentTouchPosition() + touchOffset;
        p.z = -200;
        transform.position = p;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        initLayer = gameObject.layer;
        gameObject.layer = 2;
        initPos = transform.position;
        touchOffset = initPos - CurrentTouchPosition();
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data) {
        SetDraggedPosition(data);

    }

    public void OnEndDrag(PointerEventData eventData) {
        gameObject.layer = initLayer;
        transform.localScale = new Vector3(1f, 1f, 1f);
        transform.position = initPos;
    }

    public void OnPointerDown(PointerEventData eventData) {
        downTime = Time.unscaledTime;
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (downTime != -1.0f && eventData.pointerPress != null && eventData.pointerPress.CompareTag("Evidence")) {
            float upTime = Time.unscaledTime;
            print(downTime);
            print(upTime);
            float clickSpeed = upTime - downTime;
            print(clickSpeed);
            print(slowestClickSpeed);
            if (clickSpeed <= slowestClickSpeed) {
                if (!firstLook)
                {
                    GameObject.Find("YarnSpinnerHolder").GetComponent<Yarn.Unity.DialogueRunner>().StartDialogue(evidenceRef.YarnNodes[0]);
                    firstLook = true;
                }
                PopupManager.Instance.StartPopupAtState(evidenceRef.EnlargeID);
            }
        }
        downTime = -1.0f;
    }
}
