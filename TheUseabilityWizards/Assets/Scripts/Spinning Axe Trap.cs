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
        Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);

        if (other.isTrigger || other.gameObject == this.gameObject)
        {
            Debug.Log("Ignoring trigger or self collision.");
            return;
        }

        Debug.Log("Collider is not a trigger and not self.");

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected: " + other.gameObject.name);

            IDamage dmg = other.GetComponent<IDamage>();
            if (dmg != null)
            {
                Debug.Log("IDamage component found, applying damage.");
                dmg.takeDamage(damageAmount);
            }
            else
            {
                Debug.Log("IDamage component not found.");
            }
        }
        else
        {
            Debug.Log("Collider is not tagged as Player.");
        }
    }
}