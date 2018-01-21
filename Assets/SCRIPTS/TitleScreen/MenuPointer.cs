using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPointer : MonoBehaviour {
    public float[] MenuAngles = new float[3];
    public int selection = 0;
    int LastSelection = -1;

    public Text[] menuOptions;
    public float[] menuStartX;

    float offset = 20;
    float destinationRotation;

    public TransitionManager TM;
    public OptionsPanel Opt;

    // Use this for initialization
    void Start() {
        destinationRotation = MenuAngles[0];
        menuStartX = new float[menuOptions.Length];
        for (int i = 0; i < menuOptions.Length; i++)
        {
            menuStartX[i] = menuOptions[i].rectTransform.localPosition.x;
        }
    }

    // Update is called once per frame
    void Update() {
<<<<<<< HEAD
        if (loadingScreenReady) {
            if (loadingSceneTransitionState == 0) {
                if (loadNewGame) {
                    sceneToLoad = newGameName;
                } else if (loadExistingGame) {
                    // Todo: Add Save/Load 
                } else {
                    print("THIS SHOULDN'T HAPPEN, STUCK IN LOADING SCREEN F O R E V E R");
                }
                foreach (GameObject opts in SceneManager.GetActiveScene().GetRootGameObjects()) {
                    if (opts.name == "Main Camera") {
                        oldCamera = opts.GetComponent<Camera>();
                        break;
                    }
                }
                SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(loadingScreenName));
                loadingSceneTransitionState = 1;
            } else if (loadingSceneTransitionState == 1) {
                rt = new RenderTexture(oldCamera.pixelWidth, oldCamera.pixelHeight, 16, RenderTextureFormat.ARGB32);
                rt.Create();
                oldCamera.targetTexture = rt;
                oldCamera.forceIntoRenderTexture = true;
                foreach (GameObject opts in SceneManager.GetActiveScene().GetRootGameObjects()) {
                    if (opts.name == "FadeImage") {
                        fadeImage = opts.GetComponentInChildren<RawImage>();
                        fadeImage.texture = rt;
                        fadeImage.enabled = true;
                    }
                }
                fadeImage.canvasRenderer.SetAlpha(1f);
                fadeImage.CrossFadeAlpha(0f, fadeTime, true);
                loadingSceneTransitionState = 2;
            } else if (loadingSceneTransitionState == 2) {
                if (fadeImage.canvasRenderer.GetAlpha() == 0) {
                    rt.Release();
                    oldCamera.enabled = false;
                    fadeImage.enabled = false;
                    this.gameObject.GetComponentInParent<Canvas>().transform.Find("Logo").gameObject.SetActive(false); // Kill the partical
                    loadingSceneTransitionState = 3;
                }
            } else if (loadingSceneTransitionState == 3) {
                Scene newScene = SceneManager.GetSceneByName(sceneToLoad);
                if (newScene.isLoaded) {
                    rt = new RenderTexture(oldCamera.pixelWidth, oldCamera.pixelHeight, 16, RenderTextureFormat.ARGB32);
                    rt.Create();
                    foreach (GameObject opts in newScene.GetRootGameObjects()) {
                        if (opts.name == "Main Camera") {
                            oldCamera = opts.GetComponent<Camera>();
                            oldCamera.targetTexture = rt;
                            oldCamera.forceIntoRenderTexture = true;
                            break;
                        }
                    }
                    fadeImage.texture = rt;
                    fadeImage.enabled = true;
                    fadeImage.canvasRenderer.SetAlpha(0f);
                    fadeImage.CrossFadeAlpha(1f, fadeTime, true);
                    loadingSceneTransitionState = 4;
                }
            } else if (loadingSceneTransitionState == 4) {
                Scene newScene = SceneManager.GetSceneByName(sceneToLoad);
                if (fadeImage.canvasRenderer.GetAlpha() == 1) {
                    rt.Release();
                    foreach (GameObject opts in SceneManager.GetActiveScene().GetRootGameObjects()) {
                        if (opts.name == "Main Camera") {
                            opts.GetComponent<Camera>().enabled = false;
                            break;
                        }
                    }
                    fadeImage.enabled = false;
                    oldCamera.targetTexture = null;
                    oldCamera.forceIntoRenderTexture = false;
                    loadingSceneTransitionState = 5;
                    SceneManager.SetActiveScene(newScene);
                }
            } else if (loadingSceneTransitionState == 5) {
                int curSceneIndex = SceneManager.GetActiveScene().buildIndex;
                for (var i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
                    if (i == curSceneIndex) {
                        continue;
                    }
                    SceneManager.UnloadSceneAsync(i);
                }
            }
        }
=======
>>>>>>> refs/remotes/origin/master
        if (!Opt.IsActive())
        {
            if (selection != LastSelection)
            {
                //run coroutine

                LastSelection = selection;
                destinationRotation = MenuAngles[selection];
                StartCoroutine(RotateHand());
            }

            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i != selection)
                {
                    menuOptions[i].rectTransform.localPosition = new Vector2(menuStartX[i], menuOptions[i].rectTransform.localPosition.y);
                    menuOptions[i].color = Color.white;
                }
                else
                {
                    menuOptions[i].rectTransform.localPosition = new Vector2(menuStartX[i] + offset, menuOptions[i].rectTransform.localPosition.y);
                    menuOptions[i].color = Color.yellow;
                }
            }


            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selection++;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selection--;
            }
            else if (Input.GetButtonDown("Jump") && selection == 2)
            {
                Options();
            }


            selection = (selection < 0) ? menuOptions.Length - 1 : (selection >= menuOptions.Length) ? 0 : selection;

        }
    }




    public void NewGame()
    {
        if(!Opt.IsActive())
        TM.NewGame();
    }

    public void Continue()
    {

    }

    public void Options()
    {
        if (!Opt.IsActive())
            Opt.SetActive();
    }


    public void SetSelection(int value)
    {
        selection = Mathf.Abs(value) % 3;
        Debug.Log("boop");
    }


    IEnumerator RotateHand()
    {
        float moveSpeed = 1.5f;

        while (transform.localRotation.z != destinationRotation)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, destinationRotation), moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.localRotation = Quaternion.Euler(0, 0, destinationRotation);
        yield return null;
    }
}
