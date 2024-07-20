using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectManager : MonoBehaviour
{

    public List<int> PoisonTickTimers = new List<int>();
    public List<int> BurnTickTimers = new List<int>();

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

    public void ApplyBurn(int burnTicks, int burnDamage, Collider other)
    {
        if (BurnTickTimers.Count <= 0)
        {
            BurnTickTimers.Add(burnTicks);
            StartCoroutine(Burned(burnDamage, other));
        }
        else
        {
            BurnTickTimers.Add(burnTicks);
        }
    }

    IEnumerator Burned(int burnDamage, Collider other)
    {
        IDamage dmg = other.GetComponent<IDamage>();
        while (BurnTickTimers.Count > 0)
        {
            for (int i = 0; i < BurnTickTimers.Count; i++)
            {
                BurnTickTimers[i]--;
            }
            dmg.takeDamage(burnDamage);
            BurnTickTimers.RemoveAll(i => i == 0);
            yield return new WaitForSeconds(1);
        }
    }
}
