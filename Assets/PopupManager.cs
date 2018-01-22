using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class PopupManager : Singleton<PopupManager> {

    private bool curShow = false;
    private string curState = "";
    public GameObject wrapper;
    public StateMachine[] popupStateMachine;
    public Camera mainCamera;
    public GameObject specialCameras;
    public int curPuzzle = 1;
    public bool showPopup = false;
    public string stateToShow = "";

    private StateMachine curPopStateMachine;

    void Start() {
        curPopStateMachine = popupStateMachine[curPuzzle - 1];
    }

    // Update is called once per frame
    void Update() {
        CheckForEscape();
        if (curShow != showPopup) {
            if (showPopup) {
                PopupStartup();
            } else {
                PopupCleanup();
            }
        } else if (curPopStateMachine.transform.gameObject.activeSelf) {
            if (stateToShow != null && !stateToShow.Equals(curState)) {
                ShowState(stateToShow);
            }
        }

    }

    private void CheckForEscape() {
        if (curShow && Input.GetButtonDown("Cancel")) {
            showPopup = false;
        }
    }

    private void PopupStartup() {
        PopupChange(true);
    }

    private void PopupCleanup() {
        PopupChange(false);
        stateToShow = "";
        curState = "";
        curPopStateMachine.Exit();
    }

    private void PopupChange(bool newState) {
        mainCamera.enabled = !newState;
        specialCameras.SetActive(newState);
        wrapper.SetActive(newState);
        if (FindObjectOfType<AudioManager>() != null) {
            AudioManager.Instance.popupSound(newState);
        }
        curShow = showPopup;
    }

    private void ShowState(string toShow) {
        curPopStateMachine.ChangeState(stateToShow);
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

    public void NextPuzzleGroup() {
        wrapper.GetComponent<StateMachine>().Next();
        curPuzzle++;
        curPopStateMachine = popupStateMachine[curPuzzle - 1];
    }

}
