using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Parameter")]
    [SerializeField] private Camera _cam;
    [SerializeField] private PlayerController _player;
    [SerializeField] private Animator _animator;
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
    [SerializeField] private GameObject _woodedEffect;

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

        FireAnimation();
    }

    private void Shoot()
    {
        if(_mag==0)
        {
            // display ammo is empty
        }

        _presentAmmunition--;

        if(_presentAmmunition == 0)
        {
            _mag--;
        }
        _muzzleFlash.Play();
        RaycastHit hit;

        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _range))
        {
            Debug.Log("## hit info : " + hit.transform.name);
            Objects objects = hit.transform.GetComponent<Objects>();

            if (objects != null)
            {
                objects.HitDamage(_damage);
                GameObject woodgo = Instantiate(_woodedEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(woodgo, 1.0f);
            }
        }
    }

    private void FireAnimation()
    {
        if (Input.GetButton("Fire1") && Time.time >= _nextTimeShoot)
        {
            _animator.SetBool("Fire", true);
            _animator.SetBool("Idle", false);
            _nextTimeShoot = Time.time + 1.0f / _reload;
            Shoot();
        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("FireWalk", true);
        }
        else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("IdleAim", true);
            _animator.SetBool("FireWalk", true);
            _animator.SetBool("isWalk", true);
            _animator.SetBool("Reloading", false);
        }
        else
        {
            _animator.SetBool("Fire", false);
            _animator.SetBool("Idle", true);
            _animator.SetBool("FireWalk", false);
        }
    }

    private IEnumerator Reload()
    {
        _player.SetPlayerSpeed();
        _setReloading = true;
        Debug.Log("## Reloading");
        _animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(_reloadingTime);
        _animator.SetBool("Reloading", false);
        _presentAmmunition = _maxAmmo;
        _player.SetPlayerSpeed(2.0f, 3.0f);
        _setReloading = false;
    }
}
