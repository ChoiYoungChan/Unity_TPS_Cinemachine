using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    private AudioSource _audioSource;

    [Header("Footstep")]
    [SerializeField] private AudioClip[] _footstepSound;

    private AudioClip GetRandomFootStep()
    {
        return _footstepSound[Random.Range(0, _footstepSound.Length)];
    }

    private void Step()
    {
        AudioClip clip = GetRandomFootStep();
        _audioSource.PlayOneShot(clip);
    }
}
