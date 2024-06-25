using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class potions : ScriptableObject
{
    public GameObject potion;
    [Range(0.2f, 0.3f)] public float fillAmt;
    [Range(1, 2)] public int potionType;        // 1 = HP, 2 = ST.
}
