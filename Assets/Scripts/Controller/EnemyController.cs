using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("parameters")]
    [SerializeField] private float _health = 120.0f;
    [SerializeField] private float _presentHealth = 0.0f;
    [SerializeField] private float _damage = 5.00f;
    [SerializeField] private float _speed;


    [Header("Things")]
    [SerializeField] private NavMeshAgent _enemyAgent;
    [SerializeField] private Transform _lookPoint;
    [SerializeField] private GameObject _shootingRaycastArea;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private LayerMask _playerLayer;

    [Header("Shoot")]
    [SerializeField] private float _timebtwShoot;
    private bool isPreviouslyShoot;

    [Header("States")]
    [SerializeField] private float _visionRadius;
    [SerializeField] private float _shootingRadius;
    [SerializeField] private bool _playerInvisionRadius;
    [SerializeField] private bool _playerInshootingRadius;
    [SerializeField] private bool _isPlayer = false;

    private void Awake()
    {
        _enemyAgent = this.GetComponent<NavMeshAgent>();
        _presentHealth = _health;
    }

    private void Update()
    {
        _playerInvisionRadius = Physics.CheckSphere(transform.position, _visionRadius, _playerLayer);
        _playerInshootingRadius = Physics.CheckSphere(transform.position, _shootingRadius, _playerLayer);

        if (_playerInvisionRadius && !_playerInshootingRadius) PursuePlayer();
        if (_playerInvisionRadius && _playerInshootingRadius) ShootPlayer();
    }

    private void PursuePlayer()
    {
        if(_enemyAgent.SetDestination(_playerBody.position))
        {

        }
    }

    private void ShootPlayer()
    {
        _enemyAgent.SetDestination(transform.position);
        this.transform.LookAt(_lookPoint);

        if(!isPreviouslyShoot)
        {
            RaycastHit hit;
            if(Physics.Raycast(_shootingRaycastArea.transform.position, _shootingRaycastArea.transform.forward, 
                                out hit, _shootingRadius))
            {
                Debug.Log("## Shooting : " + hit.transform.name);
                PlayerController player = hit.transform.GetComponent<PlayerController>();
                if (player != null) player.HitDamage(_damage);
            }
        }

        isPreviouslyShoot = true;
        Invoke(nameof(ActiveShooting), _timebtwShoot);
    }

    private void ActiveShooting()
    {
        isPreviouslyShoot = false;
    }

    private void Respawn()
    {
        _enemyAgent.SetDestination(this.transform.position);
        _speed = 0.0f;
        _shootingRadius = 0.0f;
        _visionRadius = 0.0f;
        _playerInvisionRadius = false;
        _playerInshootingRadius = false;
    }

    public void HitDamage(float damage)
    {
        _presentHealth -= damage;
        if (_presentHealth <= 0) Respawn();
    }
}
