using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class weaponStats : ScriptableObject
{
    public GameObject weaponModel;
    public GameObject arrowType;
    [Range(0.5f, 3)] public int reloadSpeed;
    [Range(0.5f, 5)] public float shootRate;

    public int arrowsToShoot;
    public int arrowsShootMax;
    public int arrowsQuiver;
    public int arrowsQuiverMax;
}
