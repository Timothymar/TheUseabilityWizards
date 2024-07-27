using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loadingInterface;
    public Image loadingProgressBar;

    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

   public void StartGame()
    {
        HideMenu();
        //ShowLoadingScreen();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Gameplay"));
        //StartCoroutine(LoadingScreen());
    }

    public void HideMenu()
    {
        mainMenu.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        loadingInterface.SetActive(true);
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        for (int i = 0; i < scenesToLoad.Count; i++)
        {
            totalProgress += scenesToLoad[i].progress;
            loadingProgressBar.fillAmount = totalProgress/scenesToLoad.Count;
            yield return null;
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        // This is for testing.
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // This will only work in release.
        Application.Quit();
#endif
    }

}
