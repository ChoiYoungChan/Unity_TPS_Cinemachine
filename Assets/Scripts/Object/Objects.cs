using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    public float ObjectHealth { set; get; } = 100.0f;

    public void HitDamage(float damageAmount)
    {
        ObjectHealth -= damageAmount;
        if (ObjectHealth <= 0.0f) Dead();
    }

    private void Dead()
    {
        this.gameObject.SetActive(false);
    }
}
