using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    public List<int> PoisonTickTimers = new List<int>();

    public void ApplyPoison(int poisonTicks, Collider other)
    {
       if (PoisonTickTimers.Count <= 0)
       {
            PoisonTickTimers.Add(poisonTicks);
            StartCoroutine(Poisoned(other));
       }
       else
       {
            PoisonTickTimers.Add(poisonTicks);
       }
    }
    


    IEnumerator Poisoned(Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        while(PoisonTickTimers.Count > 0)
        {
            for (int i = 0; i < PoisonTickTimers.Count; i++)
            {
                PoisonTickTimers[i]--;
            }
            dmg.takeDamage(1);
            PoisonTickTimers.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(1);
        }
    }

}
