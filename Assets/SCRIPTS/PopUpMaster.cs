using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class PopUpMaster : MonoBehaviour {

    private bool curShow = false;
    private string curState = null;
    public GameObject wrapper;
    public StateMachine popupStateMachine;
    public Camera mainCamera;
    public Camera blurCamera;
    public Camera popupCamera;
    public bool showPopup = false;
    public string stateToShow = null;
	

	// Update is called once per frame
	void Update () {
        if (curShow != showPopup) {
            if (showPopup) {
                PopupStartup();
            } else {
                PopupCleanup();
            }
        }
        if (stateToShow.Equals(curState)) {
            ShowState(stateToShow);
        }

    }

    private void PopupStartup() {
        PopupChange(true);
    }

    private void PopupCleanup() {
        PopupChange(false);
        popupStateMachine.Exit();
    }

    private void PopupChange(bool newState) {
        mainCamera.enabled = !newState;
        blurCamera.enabled = newState;
        popupCamera.enabled = newState;
        wrapper.SetActive(newState);
        if (AudioManager.Instance.isActiveAndEnabled) {
            AudioManager.Instance.popupSound(newState);
        }
        curShow = showPopup;
    }

    private void ShowState(string toShow) {
        popupStateMachine.ChangeState(stateToShow);
        curState = stateToShow;
    }

    public bool SetPopup() {
        showPopup = !showPopup;
        return showPopup;
    }

    public bool SetPopup(bool setTo) {
        showPopup = setTo;
        return showPopup;
    }

    public bool StartPopupAtState(string stateName) {
        ChangeState(stateName);
        return SetPopup(true);
    }

    public void ChangeState(string stateName) {
        stateToShow = stateName;
    }

}
