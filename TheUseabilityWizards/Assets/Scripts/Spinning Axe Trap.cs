using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAxeTrap : MonoBehaviour
{
    [SerializeField] float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        // Rotate the axe and pillar around its local Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}