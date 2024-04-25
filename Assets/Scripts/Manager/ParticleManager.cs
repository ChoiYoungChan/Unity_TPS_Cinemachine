using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour, IManager
{
    [Header("Particle")]
    [SerializeField] private ParticleSystem _woody;
    [SerializeField] private ParticleSystem _goal;

    // Start is called before the first frame update
    void Start()
    {
        if (_woody == null) _woody = Resources.Load<ParticleSystem>("Effects/ImpactEffect");
        if (_goal == null) _goal = Resources.Load<ParticleSystem>("Effects/goreEffect");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public ParticleSystem HitEffect()
    {
        return _woody;
    }

    public ParticleSystem HitEnemyEffect()
    {
        return _goal;
    }
}
