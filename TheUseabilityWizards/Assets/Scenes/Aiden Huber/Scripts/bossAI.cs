using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossAI : MonoBehaviour
{
    [Header("----Model/Collision----")]
    [SerializeField] Renderer model;
    [SerializeField] Transform headPos;
    [SerializeField] Collider headCol;
    [SerializeField] Collider bodCol;

    [Header("----Stats----")]
    [SerializeField] int bHP;
    [SerializeField] int maxBHP;


    // Start is called before the first frame update
    void Start()
    {
        updateBossHeathUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amt)
    {
        bHP -= amt;
        updateBossHeathUI();
    }

    void updateBossHeathUI()
    {
        gameManager.instance.bossHP.fillAmount = (float)bHP / maxBHP;
    }
}
