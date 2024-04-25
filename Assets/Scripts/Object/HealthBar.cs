using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _healthbarSlider;


    public void FullHealth(float health)
    {
        _healthbarSlider.maxValue = health;
        _healthbarSlider.value = health;
    }

    public void SetHealth(float health)
    {
        _healthbarSlider.value = health;
    }
}
