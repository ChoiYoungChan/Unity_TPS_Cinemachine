using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacter
{
    public void HitDamage(float damage);

    public void Death();

    public AudioClip GetRandomFootStep();

    public void Step();
}
