using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Camera to Assign")]
    [SerializeField] private GameObject _aimCam;
    [SerializeField] private GameObject _aimCanvas;
    [SerializeField] private GameObject _thirdPeronCam;
    [SerializeField] private GameObject _thirdPersonCanvas;

    [Header("Animator")]
    [SerializeField] private Animator _animator;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("IdleAim", true);
            _animator.SetBool("AimWalk", true);
            _animator.SetBool("isWalk", true);

            _thirdPeronCam.SetActive(false);
            _thirdPersonCanvas.SetActive(false);
            _aimCam.SetActive(true);
            _aimCanvas.SetActive(true);
        }
        else if(Input.GetButton("Fire2"))
        {
            _animator.SetBool("Idle", false);
            _animator.SetBool("IdleAim", true);
            _animator.SetBool("AimWalk", false);
            _animator.SetBool("isWalk", false);

            _thirdPeronCam.SetActive(false);
            _thirdPersonCanvas.SetActive(false);
            _aimCam.SetActive(true);
            _aimCanvas.SetActive(true);
        }
        else
        {
            _animator.SetBool("Idle", true);
            _animator.SetBool("IdleAim", false);
            _animator.SetBool("AimWalk", false);

            _thirdPeronCam.SetActive(true);
            _thirdPersonCanvas.SetActive(true);
            _aimCam.SetActive(false);
            _aimCanvas.SetActive(false);
        }
    }
}
