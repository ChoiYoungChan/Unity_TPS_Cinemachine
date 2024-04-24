using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private Camera _cam;
    [SerializeField] private PlayerController _player;
    [SerializeField] private float _damage = 10.0f;
    [SerializeField] private float _range = 100.0f;
    [SerializeField] private float _reload = 15.0f;

    [Space(3)]
    [Header("Ammunition and Shooting")]
    [SerializeField] private float _reloadingTime = 0.0f;
    private float _nextTimeShoot = 0.0f;
    private int _maxAmmo = 20;
    private int _mag = 15;
    private int _presentAmmunition;
    private bool _setReloading;



    [Space(3)]
    [Header("Effect")]
    [SerializeField] private ParticleSystem _muzzleFlash;

    private void Awake()
    {
        _presentAmmunition = _maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (_setReloading) return;

        if (_presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= _nextTimeShoot)
        {
            _nextTimeShoot = Time.time + 1.0f / _reload;
            Shoot();
        }
    }

    private void Shoot()
    {
        RaycastHit hit;
        _muzzleFlash.Play();

        if(_mag==0)
        {
            // display ammo is empty
        }

        _presentAmmunition--;

        if(_presentAmmunition == 0)
        {
            _mag--;
        }

        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
        {
            Debug.Log("## hit info : " + hit.transform.name);
            Objects objects = hit.transform.GetComponent<Objects>();

            if (objects != null) objects.HitDamage(_damage);
        }
    }

    private IEnumerator Reload()
    {
        _player.SetPlayerSpeed();
        _setReloading = true;
        Debug.Log("## Reloading");
        yield return new WaitForSeconds(_reloadingTime);
        _player.SetPlayerSpeed(2.0f, 3.0f);
        _presentAmmunition = _maxAmmo;
        _setReloading = false;
    }
}
