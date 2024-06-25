using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupItem : MonoBehaviour
{
    [SerializeField] potions item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))         // Doing this as an interface is a better way to do this.
        {
            gameManager.instance.playerScript.getPotionType(item);
            gameManager.instance.potionUsed(item.potionType);
            //gameManager.instance.playerScript.;            // Cheaper to use than 'get component' which is pricey memory wise
            Destroy(gameObject);
        }
    }
}
