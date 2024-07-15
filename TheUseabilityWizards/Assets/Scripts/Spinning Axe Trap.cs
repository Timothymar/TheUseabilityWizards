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
        Debug.Log("Collision detected with: " + other.gameObject.name);

        if (other.isTrigger || other.gameObject == this.gameObject)
        {
            Debug.Log("Ignored collision with trigger or self.");
            return;
        }

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player collision detected.");

            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                Debug.Log("Applying damage: " + damageAmount);
                dmg.takeDamage(damageAmount);
            }
            else
            {
                Debug.Log("IDamage interface not found on player.");
            }
        }
    }
}