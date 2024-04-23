using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private Camera _cam;
    [SerializeField] private float _damage = 10.0f;
    [SerializeField] private float _range = 100.0f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) Shoot();
    }

    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
        {
            Debug.Log("## hit info : " + hit.transform.name);
        }
    }
    
}
