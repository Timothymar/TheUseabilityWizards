using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [Header("---------Audio---------")]
    [SerializeField] public AudioClip[] audMaster;
    [SerializeField] public float audMasterVol;
    [SerializeField] public float audMusicVol;
    [SerializeField] public float audSFXVol;

    
    //[SerializeField] AudioMixer audMixer;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;

    float volume;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuTitle;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuOptions;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject bossBar;

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

    public GameObject player;
    public GameObject boss;
    public playerContol playerScript;

    int enemyCount;
    int arrowCount;
    potions potionPickup;

    public bool isPaused = false;
    public bool isStart = false;
    public bool isBoss = false;
    public bool isOptions = false;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        boss = GameObject.FindWithTag("Boss");
        playerScript = player.GetComponent<playerContol>();

        updateArrowCount(playerScript.GetArrowsToShoot());
        updateQuiverCount(playerScript.GetArrowsQuiver());
        //isStart = true;
        // ^ COMMENT/UNCOMMENT THIS LINE AS NEEDED FOR TESTING
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart == true)
        {
            TitleScreen();
        }


        if (Input.GetButtonDown("Cancel"))
        {
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
        menuActive = null;
    }

    public void stateOptions()
    {
        stateUnpause();
        isOptions = !isOptions;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateExOptions()
    {
        isOptions = !isOptions;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isOptions);
        statePause();
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        enemyCounter.text = enemyCount.ToString("F0");
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

    public void TitleScreen()
    {
        menuActive = menuTitle;
        menuActive.SetActive(isStart);
        statePause();
        isStart = !isStart;

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
        stateExOptions();
        menuActive = menuPause;
        menuActive.SetActive(isPaused);
    }

    public void WinScreen()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(isPaused);

    }

    public void LoseScreen()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }

    public void BossHealth()
    {
        bossBar.SetActive(isBoss);
    }

}
