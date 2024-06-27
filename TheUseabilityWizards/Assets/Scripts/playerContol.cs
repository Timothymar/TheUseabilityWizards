using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerContol : MonoBehaviour, IDamage, IBurnDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;

    // HP
    [Header("----- HP -----")]
    [SerializeField] int HP;
    [SerializeField] int maxHP;

    // Stamina
    [Header("----- Stamina -----")]
    [SerializeField] float Stamina;
    [SerializeField] int maxStamina;
    [SerializeField] float staminaRecoveryAmount;
    [SerializeField] float staminaRecoverySpeed;
    [SerializeField] float staminaSprintDegen;
    [SerializeField] float staminaRecoveryDelay;
    bool staminaRecoveryDelayActive;

    // Potions
    [Header("----- Potions -----")]
    [SerializeField] int potionsHeld;
    List<potions> potionInventory = new List<potions>();

    // Speed
    [Header("----- Speed -----")]
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    bool isSprinting;

    // Jump controls
    [Header("----- Jumping -----")]
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    
    [Header("----- Weapon -----")]
    [SerializeField] List<weaponStats> weaponList = new List<weaponStats>();
    [SerializeField] GameObject weaponModel;
    [SerializeField] int arrowsToShoot;
    [SerializeField] int arrowsShootMax;
    [SerializeField] int arrowsQuiver;
    [SerializeField] int arrowsQuiverMax;
    [SerializeField] float reloadArrowsSpeed;
    [SerializeField] float shootRate;
    [SerializeField] GameObject arrow;


    [Header("----- Burn Damage -----")]
    [SerializeField] private float burnDuration;
    [SerializeField] private float burnInterval;
    [SerializeField] private int burnDamage;
    [SerializeField] private int fireballHits;
    [SerializeField] private int burningThreshold;
    private bool isBurning = false;


    bool isShooting;

    int jumpCount;
    int HPOriginal;
    int selectedWeapon;
    potions potion;

    Vector3 moveDirection;
    Vector3 playerVelocity;



    // Start is called before the first frame update
    void Start()
    {
        // Sets stamina and health and has a recovery for stamina
        //HP = maxHP;
        Stamina = maxStamina;
        StartCoroutine(RecoverStamina());
        updatePlayerHeathUI();
        updatePlayerStaminaUI();
        // Make cursor invisible and lock it to the frame.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        if (!gameManager.instance.isPaused)
        {
            // Line for people to check while they shoot
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 15, Color.blue);

            Movement();
            if (Input.GetButton("Fire1") && !isShooting && Stamina > 0 && weaponList.Count > 0 && arrowsToShoot > 0)
            {
                StartCoroutine(shoot());
                StartCoroutine(StaminaRecoverDelay());
            }

            if (Input.GetButtonDown("Reload"))
            {
                ReloadArrows();
            }

            if (Input.GetButtonDown("UsePotion"))
            {
                UsePotion();
            }

            if (isSprinting)
            {
                Stamina -= staminaSprintDegen * Time.deltaTime;
                if (Stamina <= 0)
                {
                    Stamina = 0;
                    stopSprinting();
                }
                updatePlayerStaminaUI();
            }

            selectWeapon();
        }
        Sprint();
        updatePotionCountUI();
    }

    void Movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;

        controller.Move(moveDirection * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            if (Stamina > 1)
            {
                jumpCount++;
                playerVelocity.y = jumpSpeed;
                Stamina -= 1;
                updatePlayerStaminaUI();
            }
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {

        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
            StartCoroutine(StaminaRecoverDelay());
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            stopSprinting();
        }

    }
    void stopSprinting()
    {
        if (isSprinting)
        {
            speed /= sprintMod;
            isSprinting = false;
            StartCoroutine(StaminaRecoverDelay());
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        arrowsToShoot--;
        
        updateArrowCountUI();
        updateBoltData();
        Instantiate(weaponList[selectedWeapon].arrowType, Camera.main.transform.position, Camera.main.transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;

    }

    IEnumerator RecoverStamina()
    {
        while (true)
        {
            yield return new WaitForSeconds(staminaRecoverySpeed);
            if (!isSprinting && !isShooting && !staminaRecoveryDelayActive && Stamina < maxStamina)
            {
                Stamina += staminaRecoveryAmount;
                updatePlayerStaminaUI();
                Debug.Log("Stamina recovered:" + Stamina);
            }
        }
    }

    IEnumerator StaminaRecoverDelay()
    {
        staminaRecoveryDelayActive = true;
        yield return new WaitForSeconds(staminaRecoveryDelay);
        staminaRecoveryDelayActive = false;
    }

    public void takeDamage(int amt)
    {
        HP -= amt;
        updatePlayerHeathUI();

        if (HP <= 0)
        {
            gameManager.instance.LoseScreen();
        }
    }

    void updatePlayerHeathUI()
    {
        gameManager.instance.playerHP.fillAmount = (float)HP / maxHP;
    }
    void updatePlayerStaminaUI()
    {
        gameManager.instance.playerST.fillAmount = (float)Stamina / maxStamina;
    }
    void ArrowsPickUP()
    { }
    void ReloadArrows()
    {
        StartCoroutine(ReloadArrowsCoroutine());
    }

    IEnumerator ReloadArrowsCoroutine()
    {
        while (arrowsToShoot < arrowsShootMax && arrowsQuiver > 0)
        {
            arrowsToShoot++;
            arrowsQuiver--;
            updateArrowCountUI();
            updateQuiverCountUI();
            updateBoltData();
            yield return new WaitForSeconds(reloadArrowsSpeed);
        }

    }

    public void getWeaponStats(weaponStats weapon)
    {
        weaponList.Add(weapon);
        selectedWeapon = weaponList.Count - 1;

        shootRate = weapon.shootRate;
        reloadArrowsSpeed = weapon.reloadSpeed;
        arrowsToShoot = weaponList[selectedWeapon].arrowsToShoot;
        arrowsShootMax = weaponList[selectedWeapon].arrowsShootMax;
        arrowsQuiver = weaponList[selectedWeapon].arrowsQuiver;
        arrowsQuiverMax = weaponList[selectedWeapon].arrowsQuiverMax;

        updateArrowCountUI();
        updateQuiverCountUI();

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weapon.weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weapon.weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void selectWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedWeapon < weaponList.Count - 1)
        {
            selectedWeapon++;
            changeWeapon();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedWeapon > 0)
        {
            selectedWeapon--;
            changeWeapon();
        }
    }

    void changeWeapon()
    {
        shootRate = weaponList[selectedWeapon].shootRate;
        reloadArrowsSpeed = weaponList[selectedWeapon].reloadSpeed;
        arrowsToShoot = weaponList[selectedWeapon].arrowsToShoot;
        arrowsQuiver = weaponList[selectedWeapon].arrowsQuiver;
        arrowsQuiver = weaponList[selectedWeapon].arrowsQuiver;
        arrowsQuiverMax = weaponList[selectedWeapon].arrowsQuiverMax;

        updateArrowCountUI();
        updateQuiverCountUI();

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weaponList[selectedWeapon].weaponModel.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weaponList[selectedWeapon].weaponModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
    public int GetArrowsToShoot()
    {
        return arrowsToShoot;
    }

    public int GetArrowsQuiver()
    {
        return arrowsQuiver;
    }

    public int GetPotionsHeld()
    {
        return potionsHeld;
    }

    void updateArrowCountUI()
    {
        gameManager.instance.updateArrowCount(arrowsToShoot);
    }

    void updateQuiverCountUI()
    {
        gameManager.instance.updateQuiverCount(arrowsQuiver);
    }

    void updateBoltData()
    {
        
        weaponList[selectedWeapon].arrowsToShoot = arrowsToShoot;
        weaponList[selectedWeapon].arrowsQuiver = arrowsQuiver;
        weaponList[selectedWeapon].arrowsQuiver = arrowsQuiver;
        weaponList[selectedWeapon].arrowsQuiverMax = arrowsQuiverMax;
    }
    public void applyBurnDamage(int damage, float duration, float interval)
    {
        fireballHits += 1;

        // Start Burning if the player was hit enough times with fireball
        if (fireballHits >= burningThreshold && !isBurning)
        {
            StartCoroutine(applyBurnDamageOverTime(damage, duration, interval));
        }
    }

    private IEnumerator applyBurnDamageOverTime(int damage, float duration, float interval)
    {
        isBurning = true;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            takeDamage(damage);
            timeElapsed += interval;
            yield return new WaitForSeconds(interval);
        }

        isBurning = false;
        fireballHits = 0; // Reset fireball hits after burning ends
    }

    public void AddPotion(potions potion)
    {
        potionInventory.Add(potion);
        potionsHeld++;
        updatePotionCountUI();
    }

    public void UsePotion()
    {
        if (potionsHeld > 0)
        {
            potions potion = potionInventory[0];
            HealPlayer(potion.fillAmt);
            potionInventory.RemoveAt(0);
            potionsHeld--;
            updatePotionCountUI();
        }
    }

    void HealPlayer(float amount)
    {
        HP += (int)(maxHP * amount);
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        updatePlayerHeathUI();
    }
    void updatePotionCountUI()
    {
        gameManager.instance.updatePotionCount(potionsHeld);
    }
}