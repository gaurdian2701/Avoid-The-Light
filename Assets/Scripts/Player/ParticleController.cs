using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem leftBooster;
    [SerializeField] private ParticleSystem rightBooster;

    public void EnableParticles()
    {
        leftBooster.Play();
        rightBooster.Play();
    }

    public void DisableParticles()
    {
        leftBooster.Stop();
        rightBooster.Stop();
    }
}
