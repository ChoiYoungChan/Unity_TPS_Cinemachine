using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    void HitDamage(float damage) { }

    void Death() { }
}
