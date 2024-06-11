using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerContol : MonoBehaviour
{
    // Collision
    [SerializeField] CharacterController controller;

    // Stats
    [SerializeField] int HP;
    [SerializeField] int maxHP;
    [SerializeField] int Stamina;
    [SerializeField] int maxStamina;
    [SerializeField] float staminaRecovery;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;

    // Jump controls
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] float shootRate;

    [SerializeField] GameObject arrow;

    bool isShooting;

    int jumpCount;
    int HPOriginal;

    Vector3 moveDirection;
    Vector3 playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        // Sets stamina and health and has a recovery for stamina
        HP = maxHP;
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
        // Line for people to check while they shoot
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 15, Color.blue);

        // Movement Controls
        Movement();
        Sprint();

        // Fire controls
        if (Input.GetButton("Fire1") && !isShooting && Stamina > 0)
        {
            StartCoroutine(shoot());
            --Stamina;
            updatePlayerStaminaUI();
        } 
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
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    IEnumerator shoot()
    {

        isShooting = true;

        Instantiate(arrow, Camera.main.transform.position, Camera.main.transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator RecoverStamina()
    {
        while (true)
        {
            yield return new WaitForSeconds(staminaRecovery);
            if (Stamina < maxStamina)
            {
                Stamina += 1;
                updatePlayerStaminaUI();
                Debug.Log("Stamina recovered:" + Stamina);
            }
            
        }
    }
    public void takeDamage(int amt)
    {
        HP -= amt;
        updatePlayerHeathUI();

        //if (HP <= 0)
        //{
        //    gameManager.instance.youLose();
        //}
    }

    void updatePlayerHeathUI()
    {
        gameManager.instance.playerHP.fillAmount = (float)HP / maxHP;
    }
    void updatePlayerStaminaUI()
    {
        gameManager.instance.playerST.fillAmount = (float)Stamina / maxStamina;
    }
}
