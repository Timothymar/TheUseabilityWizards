using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camraControl : MonoBehaviour
{
    [SerializeField] int sensitivity;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField]  bool invertY;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
            rotX -= mouseY;



        //Clamp the rotX on the x-axis

        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        // Code to rotate cam on the x-axis

        transform.localRotation = Quaternion.Euler(rotX, 0, 0);


        // Rotate the player on the y-axis

        // Short cut for y-axis
        transform.parent.Rotate(Vector3.up * mouseX);

        //transform.parent.Rotate(new Vector3(0, 1, 0)); is long version

    }
}
