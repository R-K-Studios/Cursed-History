using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    public LoadingBar bar;

    public void LoadLevel(int sceneIndex)
    {
        
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneIndex);

        while (!op.isDone)
        {
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            bar.value = progress;
            yield return null;
        }
    }
}
