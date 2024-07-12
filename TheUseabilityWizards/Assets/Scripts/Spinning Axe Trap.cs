using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningAxeTrap : MonoBehaviour
{
    [SerializeField] float rotationSpeed;
    [SerializeField] int damageAmount;
    [SerializeField] Collider boxCol;

    // Update is called once per frame
    void Update()
    {
        // Rotate the axe and pillar around its local Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger || other.gameObject == this.gameObject)
        {
            return;
        }

        if (other.CompareTag("Player"))
        {
            IDamage dmg = other.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(damageAmount);
            }
        }
    }
}