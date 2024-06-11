using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Collission.
    [SerializeField] CharacterController controller;

    // Status bars.
    [SerializeField] int HP;        // Health
    [SerializeField] int ST;        // Stamina

    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] int shootDmg;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;

    bool isShooting;

    int HPOrig;
    int STOrig;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        STOrig = ST;
        updatePlayerUI();
        // Make cursor invisible and lock it to the frame.
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.blue);
        movement();
        sprintBtn();

        if (Input.GetButton("Fire1") && !isShooting)
        {

            // Shoot is a coroutine. You can't call coroutines without calling StartCoroutine.
            StartCoroutine(shoot());
        }
    }

    void movement()
    {
        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDir * speed * Time.deltaTime);


        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    void sprintBtn()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed += sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed -= sprintMod;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        Debug.Log("Shoot");

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
        {
            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDmg);
            }
        }


        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int amt)
    {
        HP -= amt;
        updatePlayerUI();

        //if (HP <= 0)
        //{
        //    gameManager.instance.youLose();
        //}
    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHP.fillAmount = (float)HP/HPOrig;
        gameManager.instance.playerST.fillAmount = (float)ST/STOrig;
    }
}
