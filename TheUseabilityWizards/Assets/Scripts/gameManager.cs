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

    
    //[SerializeField] AudioMixer audMixer;
    [SerializeField] Slider volSlider;

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
        isStart = true;
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerContol>();

        updateArrowCount(playerScript.GetArrowsToShoot());
        updateQuiverCount(playerScript.GetArrowsQuiver());
        

        //if (isStart == true)
        //{
        //    TitleScreen();
        //}
    }

    // Update is called once per frame
    void Update()
    {
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
        if (isStart == true)
        {
            isStart = false;
        }
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(isPaused);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        enemyCounter.text = enemyCount.ToString("F0");
        if (enemyCount <= 0)
        {
            DestroyWall.instance.DestroyThisWall();
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

    public void TitleScreen()
    {
        statePause();
        menuActive = menuTitle;
        menuActive.SetActive(isPaused);
    }

    public void OptionsScreen()
    {
        //statePause();
        menuActive = menuOptions;
        menuActive.SetActive(isPaused);
    }

    public void BackButton()
    {
        menuActive = menuPause;
        menuPause.SetActive(isPaused);
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
