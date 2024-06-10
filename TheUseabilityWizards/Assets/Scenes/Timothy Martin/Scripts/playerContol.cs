using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerContol : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    //[SerializeField] Transform playerShootPos;

    [SerializeField] int HP;
    [SerializeField] int Stamina;
    [SerializeField] int maxStamina;
    [SerializeField] float staminaRecovery;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
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
        HPOriginal = HP;
        Stamina = maxStamina;
        StartCoroutine(RecoverStamina());
    }

    // Update is called once per frame
    void Update()
    {
        
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 15, Color.blue);

        Movement();
        Sprint();

        if (Input.GetButtonDown("Fire1") && !isShooting && Stamina > 0)
        {
            StartCoroutine(shoot());
            --Stamina;
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(shoot());
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
                Debug.Log("Stamina recovered:" + Stamina);
            }
            
        }
    }
}
