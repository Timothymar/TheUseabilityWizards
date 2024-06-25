using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBurnDamage
{
    void applyBurnDamage(int damage, float duration, float interval);
}