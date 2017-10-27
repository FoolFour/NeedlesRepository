using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolEnabled : MonoBehaviour
{
    [SerializeField]
    bool           autoDestroy;

    [SerializeField]
    float          destroyTime;

    ParticlePooler pooler;

    WaitForSeconds waitForSeconds;

    public  void SetPloolerComponent(ParticlePooler pooler)
    {
        this.pooler = pooler;
    }

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(destroyTime);
    }

    private void OnEnable()
    {
        if(autoDestroy)
        {
            StartCoroutine(DisableTimer());
        }
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private IEnumerator DisableTimer()
    {
        yield return waitForSeconds;
        enabled = false;
    }
}
