using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAnimator : MonoBehaviour
{
    
    public ParticleSystem particleEmitter;



    private void PlayParticles()
    {
        particleEmitter.Play();
    }
}
