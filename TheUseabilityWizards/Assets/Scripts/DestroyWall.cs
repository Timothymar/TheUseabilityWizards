using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWall : MonoBehaviour
{
    public static DestroyWall instance;

    void Start()
    {
        instance = this;
    }

    public void DestroyThisWall()
    {
        Destroy(gameObject);
    }
}
