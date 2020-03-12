using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticleTest : MonoBehaviour
{
    ParticleSystem bloodType_1;
    ParticleSystem bloodType_2;
    ParticleSystem[] particles;

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponentsInChildren<ParticleSystem>();

        bloodType_1 = particles[0];
        bloodType_2 = particles[1];
    }

    public void PlayBloodEffect()
    {
        bloodType_1.Play();
        bloodType_2.Play();
    }
}
