using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RifleInfo : MonoBehaviour
{
    [SerializeField] private Text _ammoText;
    [SerializeField] private Text _magText;

    public void UpdateAmmoText(int ammo)
    {
        _ammoText.text = "AMMO " + ammo;
    }

    public void UpdateMagText(int mag)
    {
        _magText.text = "MAG " + mag;
    }
}
