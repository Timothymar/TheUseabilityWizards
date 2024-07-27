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
    
    public GameObject creditsMenu;
    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();

    public void StartGame()
    {
        HideMenu();
        ShowLoadingScreen();
        scenesToLoad.Add(SceneManager.LoadSceneAsync("Gameplay"));
    }

    public void HideMenu()
    {
        mainMenu.SetActive(false);
    }

    public void ShowLoadingScreen()
    {
        loadingInterface.SetActive(true);
    }

    public void Credits()
    {
        if (creditsMenu != null)
        {
            bool isActive = creditsMenu.activeSelf;
            creditsMenu.SetActive(!isActive);
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
