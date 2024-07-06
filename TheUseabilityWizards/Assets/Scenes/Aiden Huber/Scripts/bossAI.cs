using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bossAI : MonoBehaviour, IDamage
{
    [Header("----Model/Collision----")]
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] Collider headCol;
    [SerializeField] Collider bodCol;
    [SerializeField] Collider weaponCol;
    [SerializeField] GameObject firedBolt;

    [Header("----Stats----")]
    [SerializeField] int bHP;
    [SerializeField] int maxBHP;
    //[SerializeField] int bStam;
    //[SerializeField] int maxBStam;

    [Header("----Weapon----")]
    bool doesDamage;

    Vector3 playerDir;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.instance.isBoss == true)
        {
            gameManager.instance.BossHealth();
        }
        
        updateBossHealthUI();
    }

    public void takeDamage(int amt)
    {
        bHP -= amt;
        //firedBolt = gameManager.instance.playerScript.getcomp
        //    weaponList[selectedWeapon].arrowType
        //if(gameManager.instance.player.)
        updateBossHealthUI();
    }

    public void updateBossHealthUI()
    {
        gameManager.instance.bossHP.fillAmount = (float)bHP / maxBHP;
    }

    public void weaponColOn()
    {
        weaponCol.enabled = true;
    }
    public void weaponColOff()
    {
        weaponCol.enabled = false;
    }
}
