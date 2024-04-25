using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour, ICharacter
{
    [Header("parameters")]
    [SerializeField] private float _health = 120.0f;
    [SerializeField] private float _presentHealth = 0.0f;
    [SerializeField] private float _damage = 5.00f;
    [SerializeField] private float _speed;


    [Header("Things")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _lookPoint;
    [SerializeField] private GameObject _shootingRaycastArea;
    [SerializeField] private Transform _enemyBody;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private Transform _spawn;
    [SerializeField] private Transform _playerCharacter;

    [Header("Animation and effect")]
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _muzzleflash;

    [Header("Shoot")]
    [SerializeField] private float _timebtwShoot;
    private bool isPreviouslyShoot;

    [Header("States")]
    [SerializeField] private float _visionRadius;
    [SerializeField] private float _shootingRadius;
    [SerializeField] private bool _enemyInvisionRadius;
    [SerializeField] private bool _enemyInshootingRadius;
    [SerializeField] private bool _isPlayer = false;


    [Space(3)]
    [Header("Sound")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _footstepSound;
    [SerializeField] private AudioClip _shootingSound;

    private void Awake()
    {
        _agent = this.GetComponent<NavMeshAgent>();
        _presentHealth = _health;
    }

    private void Update()
    {
        _enemyInvisionRadius = Physics.CheckSphere(transform.position, _visionRadius, _enemyLayer);
        _enemyInshootingRadius = Physics.CheckSphere(transform.position, _shootingRadius, _enemyLayer);

        if (_enemyInvisionRadius && !_enemyInshootingRadius) PursueEnemy();
        if (_enemyInvisionRadius && _enemyInshootingRadius) ShootEnemy();
    }

    private void PursueEnemy()
    {
        if (_agent.SetDestination(_enemyBody.position))
        {
            _animator.SetBool("IsRunning", true);
            _animator.SetBool("IsShooting", false);
        }
        else
        {
            _animator.SetBool("IsRunning", false);
            _animator.SetBool("IsShooting", false);
        }
    }

    private void ShootEnemy()
    {
        _agent.SetDestination(transform.position);
        this.transform.LookAt(_lookPoint);

        if (!isPreviouslyShoot)
        {
            _muzzleflash.Play();
            _audioSource.PlayOneShot(_shootingSound);

            RaycastHit hit;
            if (Physics.Raycast(_shootingRaycastArea.transform.position, _shootingRaycastArea.transform.forward,
                                out hit, _shootingRadius))
            {
                Debug.Log("## Shooting : " + hit.transform.name);
                EnemyController enemy = hit.transform.GetComponent<EnemyController>();
                if (enemy != null) enemy.HitDamage(_damage);

                _animator.SetBool("IsRunning", false);
                _animator.SetBool("IsShooting", true);
            }
            isPreviouslyShoot = true;
            Invoke(nameof(ActiveShooting), _timebtwShoot);
        }
    }

    private void ActiveShooting()
    {
        isPreviouslyShoot = false;
    }

    public AudioClip GetRandomFootStep()
    {
        return _footstepSound[Random.Range(0, _footstepSound.Length)];
    }

    public void Step()
    {
        AudioClip clip = GetRandomFootStep();
        _audioSource.PlayOneShot(clip);
    }

    public void Death()
    {
        _animator.SetBool("IsDeath", true);
        _animator.SetBool("IsRunning", false);
        _animator.SetBool("IsShooting", false);

        _agent.SetDestination(this.transform.position);
        _speed = 0.0f;
        _shootingRadius = 0.0f;
        _visionRadius = 0.0f;
        _enemyInvisionRadius = false;
        _enemyInshootingRadius = false;

        Debug.Log("## " + this.transform.name + " Dead ");
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        ScoreManager.Instance.SetEnemyKills(1);
        StartCoroutine(Respawn());
    }

    public void HitDamage(float damage)
    {
        _presentHealth -= damage;
        if (_presentHealth <= 0) Death();
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5.0f);
        Debug.Log("## Spawn");
        this.gameObject.GetComponent<CapsuleCollider>().enabled = true;

        _presentHealth = 120.0f;
        _speed = 1.0f;
        _shootingRadius = 10.0f;
        _visionRadius = 100.0f;
        _enemyInvisionRadius = true;
        _enemyInshootingRadius = false;

        _animator.SetBool("IsDeath", false);
        _animator.SetBool("IsRunning", true);

        _playerCharacter.transform.position = _spawn.transform.position;
        PursueEnemy();
    }
}
