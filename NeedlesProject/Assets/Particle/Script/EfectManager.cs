using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EfectManager : MonoBehaviour
{
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    //デバッグ用
    public bool debugStart;

    // Use this for initialization
    void Start()
    {
        ParticlesStop();
        debugStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        //デバッグ用
        if (debugStart == true)
        {
            ParticlesStart();
            debugStart = false;
        }
    }

    //パーティクルスタート
    void ParticlesStart()
    {
        for (int i = 0; i < particles.ToArray().Length; i++)
        {
            particles[i].Play();
        }
    }

    //パーティクル停止
    void ParticlesStop()
    {
        for (int i = 0; i < particles.ToArray().Length; i++)
        {
            particles[i].Stop();
        }
    }
}
