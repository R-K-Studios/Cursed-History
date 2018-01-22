﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MenuUI {

    public OptionsPanel Opt;
    public TransitionManager tm;
    public PopupManager pop;
    // Update is called once per frame
    
    void Update () {

        if (Input.GetButtonDown("Cancel"))
        {
            if (Opt.IsActive())
            {
                Opt.SetInactive();
            }
            else if (!IsActive())
            {
                if (pop.showPopup)
                {
                    
                }
                else
                {
                    SetActive();
                }
            }
            else
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
