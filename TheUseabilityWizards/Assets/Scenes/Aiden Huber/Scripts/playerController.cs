using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Collission.
    [SerializeField] CharacterController characterController;

    // Status bars.
    [SerializeField] int HP;        // Health
    [SerializeField] int ST;        // Stamina
    [SerializeField] int MA;        // Mana

    int HPOrig;
    int STOrig;
    int MAOrig;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        STOrig = ST;
        MAOrig = MA;
        updatePlayerUI();
        // This next will make cursor invisible and lock it to the window.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHP.fillAmount = (float)HP/HPOrig;
        gameManager.instance.playerST.fillAmount = (float)ST/STOrig;
        gameManager.instance.playerMA.fillAmount = (float)MA/MAOrig;
    }
}
