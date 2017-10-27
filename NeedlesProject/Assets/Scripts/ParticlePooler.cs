using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlePooler : MonoBehaviour
{
    Dictionary<string, ParticleSystem> particleInstance;

    public  void CreateInstance(GameObject particle)
    {
        var poolEnabled = particle.GetComponent<PoolEnabled>();
        Debug.Assert(poolEnabled != null, "このゲームオブジェクトはPoolEnabledを持っていません");

        var objectName = particle.gameObject.name;
        var search     = particleInstance[objectName];
        if(search == null)
        {
            //新しく生成
            GameObject inst = Instantiate(particle);
            var instParticle = inst.GetComponent<ParticleSystem>();
            particleInstance.Add(objectName, instParticle);
        }
        else
        {
            var instParticle = particleInstance[objectName];

            if (poolEnabled == null)
            {

            }
        }
    }

    private void Awake()
    {
        particleInstance = new Dictionary<string, ParticleSystem>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }
}
