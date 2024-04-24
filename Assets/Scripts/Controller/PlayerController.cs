using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float _playerSpeed = 2.0f;
    [SerializeField] private float _currentSpeed = 0.0f;
    [SerializeField] private float _playerSprintSpeed = 3.0f;
    [SerializeField] private float _currentSprintSpeed = 0.0f;

    [Header("Animator and Gravity")]
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _gravity = -9.81f;

    [Space(3)]
    [Header("Jump & velocity")]
    [SerializeField] private float _jumpRange = 1.0f;
    [SerializeField] private float _turnCalmTime = 0.1f;
    [SerializeField] private float _surfaceDistance = 0.4f;
    [SerializeField] private Transform _surfaceCheck;
    [SerializeField] private LayerMask _surfaceMask;
    private Vector3 _velocity;
    private float _turnCalmVelocity;
    private bool onSurface;

    [Space(3)]
    [Header("Objects")]
    [SerializeField] private Transform _camera;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        SetGravity();

        PlayerMove();
        PlayerJump();
        PlayerSprint();
    }

    private void SetGravity()
    {
        onSurface = Physics.CheckSphere(_surfaceCheck.position, _surfaceDistance, _surfaceMask);

        if(onSurface && _velocity.y < 0) _velocity.y = -2.0f;

        // gravity
        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }

    private void PlayerMove()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalAxis, 0.0f, verticalAxis).normalized;
        var value = direction.magnitude >= 0.1f;
        if (value) {
            _animator.SetBool("isWalk", value);
            _animator.SetBool("isRun", !value);
            _animator.SetBool("Idle", !value);
            _animator.SetTrigger("Jump");
            _animator.SetBool("AimWalk", !value);
            _animator.SetBool("IdleAim", !value);

            Move(direction, _playerSpeed);
            _currentSpeed = _playerSpeed;
        } else {
            _animator.SetBool("Idle", !value);
            _animator.SetTrigger("Jump");
            _animator.SetBool("isWalk", value);
            _animator.SetBool("AimWalk", value);
            _animator.SetBool("IdleAim", value);
            _animator.SetBool("isRun", value);
            _currentSpeed = 0.0f;
        }
    }

    private void PlayerSprint()
    {
        if(Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontalAxis, 0.0f, verticalAxis).normalized;
            var value = direction.magnitude >= 0.1f;

            if (value) {
                _animator.SetBool("isRun", value);
                _animator.SetBool("Idle", !value);
                _animator.SetBool("isWalk", !value);
                _animator.SetBool("IdleAim", !value);

                Move(direction, _playerSprintSpeed);
                _currentSprintSpeed = _playerSprintSpeed;
            } else {
                _animator.SetBool("Idle", value);
                _animator.SetBool("isWalk", value);
                _currentSprintSpeed = 0.0f;
            }
        }
    }

    private void Move(Vector3 direction, float speed)
    {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(this.transform.eulerAngles.y, targetAngle, ref _turnCalmVelocity, _turnCalmTime);
        this.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

        Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        _controller.Move(moveDir.normalized * speed * Time.deltaTime);
    }

    private void PlayerJump()
    {
        if(Input.GetButtonDown("Jump") && onSurface)
        {
            _animator.SetBool("isWalk", false);
            _animator.SetTrigger("Jump");
            _velocity.y = Mathf.Sqrt(_jumpRange * -2 * _gravity);
        } else {
            _animator.ResetTrigger("Jump");
        }
    }

    public void SetPlayerSpeed(float speed = 0.0f, float sprintSpeed = 0.0f)
    {
        this._playerSpeed = speed;
        this._playerSprintSpeed = sprintSpeed;
    }
}
