using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("---------Audio---------")]
    [SerializeField] public AudioClip[] audMaster;
    [SerializeField] public float audMasterVol;
    [SerializeField] public float audMusicVol;
    [SerializeField] public float audSFXVol;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;

    float volume;

    [Header("---------Menus/UI---------")]
    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuPrev;
    [SerializeField] GameObject bossBar;
    [SerializeField] GameObject optionsFirstBtn;
    [SerializeField] GameObject winFirstBtn;
    [SerializeField] GameObject loseFirstBtn;

    [SerializeField] TMP_Text enemyCounter;

    // Arrow supply
    [SerializeField] TMP_Text arrowSupply;
    [SerializeField] TMP_Text arrowInQuiver;

    // Potion
    [SerializeField] TMP_Text potionSupply;

    public Image playerHP;
    public Image playerST;
    public Image bossHP;
    // ^ Normally serialized, will fix these later. This is also my reminder to do that.

    [Header("---------Player/Boss---------")]
    public GameObject player;
    public Vector3 playerStartPos;
    public GameObject boss;
    public playerContol playerScript;

    int enemyCount;
    int arrowCount;
    potions potionPickup;

    [Header("---------Checkpoint---------")]
    [SerializeField] GameObject[] checkptList;
    public Vector3 checkpt;
    public Vector3 prevCP;
    //bool isActive = false;
    bool hasPrev = false;

    public bool isPaused = false;
    public bool isStart = false;
    public bool isBoss = false;
    public bool isOptions = false;
    public bool isCredits = false;


    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerContol>();
        playerStartPos = player.transform.position;
        boss = GameObject.FindWithTag("Boss");
        checkptList = GameObject.FindGameObjectsWithTag("Checkpoint");
        SetCheckpoint();

        updateArrowCount(playerScript.GetArrowsToShoot());
        updateQuiverCount(playerScript.GetArrowsQuiver());
        //isStart = true;
        // ^ COMMENT/UNCOMMENT THIS LINE AS NEEDED FOR TESTING
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (EventSystem.current.gameObject != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }

            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
            else if (menuActive == menuOptions)
            {
                BackButton();
            }
        }
        if (Input.GetButtonDown("Fire2"))
        {
            player.transform.position = checkpt;
        }
    }

    public void SetCheckpoint()
    {
        if (checkpt == playerStartPos)
        {
            if (!hasPrev)
            {
                hasPrev = !hasPrev;
                prevCP = checkpt;
                checkpt = player.transform.position;
            }            
        }
        else
        {
            if (!hasPrev)
            {
                checkpt = playerStartPos;
            }
            else
            {
                checkpt = player.transform.position;
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        if (menuActive != null)
        {
            menuActive = null;
        }
    }

    public void stateOptions()
    {
        if (EventSystem.current.gameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (menuActive == menuPause)
        {
            EventSystem.current.SetSelectedGameObject(optionsFirstBtn);
            menuPrev = menuPause;
            
        }

        isOptions = true;
        Time.timeScale = 0; 
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        menuActive = menuOptions;
        menuActive.SetActive(true);
    }

    public void stateExOptions()
    {
        isOptions = false;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        if (menuActive != null)
        {
            menuActive.SetActive(false);
        }

        menuActive = menuPrev;
        if (menuActive != null)
        {
            menuActive.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void updateArrowCount(int arrowCount)
    {
        arrowCount = playerScript.GetArrowsToShoot();
        arrowSupply.text = arrowCount.ToString("F0");
    }

    public void updateQuiverCount(int quiverCount)
    {
        quiverCount = playerScript.GetArrowsQuiver();
        arrowInQuiver.text = quiverCount.ToString("F0");
    }

    public void updatePotionCount(int potionCount)
    {
        potionCount = playerScript.GetPotionsHeld();
        potionSupply.text = potionCount.ToString("F0");
    }

    public void OptionsScreen()
    {
        stateOptions();
        menuActive = null;
        menuActive = menuOptions;
        menuActive.SetActive(isOptions);
    }

    public void BackButton()
    {
        if (EventSystem.current.gameObject != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
        }

        stateExOptions();

        if (menuPrev == menuPause)
        {
            menuActive = menuPause;
            menuActive.SetActive(true);
        }
        menuPrev = null;
    }

    public void RespawnButton()
    {
        

        foreach (GameObject cp in checkptList)
        {
            if (cp.transform.position == checkpt)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                stateUnpause();
                player.transform.position = checkpt;
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                stateUnpause();
            }
        }

        







        //if (activeCheckPt.GetPos() )
        //{
        //    player.transform.position = playerStartPos;
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //    isStart = false;
        //    stateUnpause();
        //}
        //else if (activeCheckPt.GetCheckpoint() != null)
        //{
        //    player.transform.position = activeCheckPt.GetCheckpoint().transform.position;
        //}
    }

    public void WinScreen()
    {
        statePause();
        EventSystem.current.SetSelectedGameObject(winFirstBtn);
        menuActive = menuWin;
        menuActive.SetActive(isPaused);

    }

    public void LoseScreen()
    {
        statePause();
        EventSystem.current.SetSelectedGameObject(loseFirstBtn);
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }

}
