using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MenuUI {

    public OptionsPanel Opt;
    public TransitionManager tm;
    public PopupManager pop;
    // Update is called once per frame
    
    void Update () {

        if (Input.GetButtonDown("Cancel") && !pop.showPopup)
        {
            if (!IsActive())
            {
                SetActive();
            }
            else if (!Opt.IsActive())
            {
                SetInactive();
            }
        }
	}


    public void Options()
    {
        Opt.SetActive();
    }

    public void Save()
    {

    }
    public void Load()
    {

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void QuitToMenu()
    {
        SetInactive();
        tm.TitleScreen();
    }
}
