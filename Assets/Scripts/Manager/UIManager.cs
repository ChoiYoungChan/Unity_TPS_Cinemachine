using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonClass<UIManager>
{
    [SerializeField] RifleInfo _rifleInfo;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAmmoText(int value)
    {
        _rifleInfo.UpdateAmmoText(value);
    }

    public void SetMagText(int value)
    {
        _rifleInfo.UpdateMagText(value);
    }
}
