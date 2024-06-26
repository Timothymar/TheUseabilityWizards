using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;

    [SerializeField] TMP_Text enemyCounter;

    // Arrow supply
    [SerializeField] TMP_Text arrowSupply;
    [SerializeField] TMP_Text arrowInQuiver;
    

    public Image playerHP;
    public Image playerST;
    // ^ Normally serialized, will fix these later. This is also my reminder to do that.

    public GameObject player;
    public playerContol playerScript;

    int enemyCount;
    int arrowCount;
    potions potionPickup;

    public bool isPaused = false;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerContol>();

        updateArrowCount(playerScript.GetArrowsToShoot());
        updateQuiverCount(playerScript.GetArrowsQuiver());
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

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        enemyCounter.text = enemyCount.ToString("F0");
        //if (enemyCount <= 0)
        //{
        //    DestroyWall.instance.DestroyThisWall();
        //}
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

    //public void updateStatBars(int type)
    //{
    //    int current;
    //    current = playerScript.getStatBarCurrent(type);

    //    if (current == )
    //}


    public int potionUsed(int type)         // Type is potionType. 1 = HP, 2 = ST
    {
        type = gameManager.instance.playerScript.getPotionType(potionPickup);

        if (type == 1)
        {
            int curHP = gameManager.instance.playerScript.getCurHP();
            int maxHP = gameManager.instance.playerScript.getMaxHP();
            if (curHP != maxHP)
            {
                curHP = (int)(curHP + (maxHP * potionPickup.fillAmt));
                return curHP;
            }
        }
        else if (type == 2)
        {
            int curStam = gameManager.instance.playerScript.getCurStamina();
            int maxStam = gameManager.instance.playerScript.getMaxStamina();
            if (curStam != maxStam)
            {
                curStam = (int)(curStam + (maxStam * potionPickup.fillAmt));
                return curStam;
            }
        }
        return 0;
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

}
