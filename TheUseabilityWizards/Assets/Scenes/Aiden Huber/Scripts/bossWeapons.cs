using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class bossWeapons : ScriptableObject
{
    public GameObject weaponModel;
    [Range(0.1f, 3)] public float swingSpeed;       // If melee
    [Range(0.1f, 4)] public float fireSpeed;       // If ranged
    [Range(0.1f, 1)] public float DPS;      // Any potential damage over time affects
    [Range(0.5f, 2)] public float attackDmg;      // Single hit damage

    public AudioClip useSound;
    public float useVol;
}
