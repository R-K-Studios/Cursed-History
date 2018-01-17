using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsPanel : MonoBehaviour
{
    //Audio Mixer
    public AudioMixer audioMixer;

    //UI Components
    public CanvasGroup OptionsGroup;

    public Slider MusVol;
    public Slider SFXVol;
    public Toggle MusMute;
    public Toggle SFXMute;
    public Toggle FullScreen;
    public Toggle VSync;

    public Dropdown Resolutions;

    //Bool for if the Menu is on screen
    bool active = false;
    //Bool for if the Menu is transitioning
    bool moving = false;

    //The position of the menu when active
    float activeYPosition;
    Resolution[] resOptions;

    private void Awake()
    {
        //Load PlayerPrefs
     
        //Resolution and Fullscreen preferences should be loaded on startup, though.
    }

    void Start()
    {
        //Gather resolution list
        resOptions = Screen.resolutions;
        Resolutions.ClearOptions();
        List<string> options = new List<string>();
        int currentResIndex = 0;

        //Set list to display in dropdown
        for(int i = 0; i < resOptions.Length; i++)
        {
            string option = resOptions[i].width + " x " + resOptions[i].height;
            options.Add(option);

            if (resOptions[i].width == Screen.currentResolution.width && resOptions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }

        }

        //add options and display current resolution
        Resolutions.AddOptions(options);
        Resolutions.value = currentResIndex;
        Resolutions.RefreshShownValue();



        LoadPrefs();
        //Get the active position
        activeYPosition = transform.localPosition.y;
        //Move it out of the way
        transform.Translate(new Vector3(0, -1, 0));
        //Make it invisible
        OptionsGroup.alpha = 0;
    }
    
    //Is the menu currently on screen?
    public bool IsActive()
    {
        return active;
    }

 


    //Coroutine to fade in and out the menu and move it between positions
    IEnumerator MovePanel()
    {
        moving = true;
        if (active && transform.localPosition.y < activeYPosition)
        {
            while (transform.localPosition.y < activeYPosition)
            {
                transform.Translate(0, .1f, 0);
                OptionsGroup.alpha += .2f;
                yield return null;
            }
            transform.localPosition.Set(transform.localPosition.x, activeYPosition, 0);
            yield return null;
        }
        else if (!active && OptionsGroup.alpha > 0)
        {
            while (OptionsGroup.alpha > 0)
            {
                OptionsGroup.alpha -= .1f;
                transform.Translate(0, -.1f, 0);
                yield return null;
            }
            yield return null;
        }
        moving = false;
        yield return null;
    }

    //Call up the menu
    public void SetActive()
    {
        if (!moving)
        {
            active = true;
            StartCoroutine(MovePanel());
        }
    }

    //Dismiss the menue
    public void SetInactive()
    {
        if (!moving)
        {
            active = false;
            StartCoroutine(MovePanel());
        }
    }

    //Set the Music Volume
    public void SetMusVolume(float value)
    {
        PlayerPrefs.SetFloat("MusVol", value);
        audioMixer.SetFloat("MusicVolume", value);
        Debug.Log("Music Volume Set to : " + value);
        MusMute.isOn = false;

        PlayerPrefs.SetInt("MusMute", 0);
    }

    //Set the Sound Volume
    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVol", value);
        audioMixer.SetFloat("SoundVolume", value);
        Debug.Log("Sound Volume Set to : " + value);
        SFXMute.isOn = false;


        PlayerPrefs.SetInt("SFXMute", 0);
    }

    //Mute the music
    public void SetMusMute(bool value)
    {
        //temporary storage variable
        float vol = MusVol.value;
        if (value)
        {

            PlayerPrefs.SetFloat("MusVol", vol);
            audioMixer.SetFloat("MusicVolume", -80);
        }
        else
        {
            //reset volume
            audioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusVol"));

        }

        //save setting
        PlayerPrefs.SetInt("MusMute", value ? 1 : 0);
    }

    //Mute the sounds
    public void SetSFXMute(bool value)
    {
        float vol = SFXVol.value;
        if (value)
        {
            PlayerPrefs.SetFloat("SFXVol", vol);
            audioMixer.SetFloat("SoundVolume", -80);
        }
        else
        {
            audioMixer.SetFloat("SoundVolume", PlayerPrefs.GetFloat("SFXVol"));
        }

        //save setting
        PlayerPrefs.SetInt("MusMute", value ? 1 : 0);
    }

    //Set FullSccreen
    public void SetFullScreen(bool value)
    {
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
        Debug.Log("Fullscreen Set to : " + value);

        Screen.fullScreen = value;  

    }

    //Set VSync
    public void SetVSync(bool value)
    {
        //TODO: Figure this out
        //PlayerPrefs.SetInt("VSync", value ? 1 : 0);
        //Debug.Log("VSync Set to : " + value);
        
    }
    //Set Resolution
    public void SetRes(int value)
    {
        Resolution res = resOptions[value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen); 
    }

    public void LoadPrefs()
    {
        
        /*
        //Music Volume
        if (PlayerPrefs.HasKey("MusVol"))
            MusVol.value = PlayerPrefs.GetFloat("MusVol");

        SetMusVolume(MusVol.value);

        //Sound Volume
        if (PlayerPrefs.HasKey("SFXVol"))
                SFXVol.value = PlayerPrefs.GetFloat("SFXVol");

        SetSFXVolume(SFXVol.value);
        
        //This doesn't seem to work just right.
        /*
        //Music Mute
        if (!PlayerPrefs.HasKey("MusMute"))
            PlayerPrefs.SetInt("MusMute", 0); //Default Off

        MusMute.isOn = (PlayerPrefs.GetInt("MusMute", 0) == 1) ? false : true;
        SetMusMute(MusMute);
        

        //Sound Mute
        if (!PlayerPrefs.HasKey("SFXMute"))
            PlayerPrefs.SetInt("SFXMute", 0); //Default Off

        SFXMute.isOn = (PlayerPrefs.GetInt("SFXMute", 0) == 1) ? false : true;
        SetSFXMute(SFXMute);
        */


        //Handle these before loading
        //Resolution
        /*
        if (PlayerPrefs.HasKey("ResolutionOption"))
            Resolutions.value = PlayerPrefs.GetInt("ResolutionOption");
        else
            PlayerPrefs.GetInt("ResolutionOption", 3); //Default 0
            
        //FullScreen
        if (PlayerPrefs.HasKey("Fullscreen"))
            FullScreen.isOn = (PlayerPrefs.GetInt("Fullscreen") == 0 ? false : true); //Set the preference
        else
            PlayerPrefs.SetInt("Fullscreen", 1); //Default On

        //VSync
        if (PlayerPrefs.HasKey("VSync"))
            VSync.isOn = (PlayerPrefs.GetInt("VSync") == 0 ? false : true); //Set the preference
        else
            PlayerPrefs.SetInt("VSync", 0); //Default Off

        */

    }

}
