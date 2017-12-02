using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRockGenerator : MonoBehaviour {
    [SerializeField, Tooltip("落とすオブジェクト")]
    public GameObject m_RockPrefab;
    [SerializeField, Tooltip("砂埃のエフェクト")]
    public GameObject m_SandEffectPrefab;
    [SerializeField, Tooltip("砂埃の生存時間")]
    public float m_SandEffectLifeTime = 1.5f;
    [SerializeField, Tooltip("ランダムで落石")]
    public bool isRandom = true;
    [SerializeField, Tooltip("確率")]
    public float m_Probability = 30;
    [SerializeField, Tooltip("落石を起こすインターバル")]
    public float m_Interval = 5;

    private float m_Timer = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isRandom)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer < m_Interval) return;
            if (Random.Range(0, 100) < m_Probability && !isInside())
            {
                var go = Instantiate(m_SandEffectPrefab,transform);
                Destroy(go, m_SandEffectLifeTime);
                StartCoroutine(DelayMethod(m_SandEffectLifeTime, () => { RockFall(); }));
            }
            m_Timer = 0;
        }
    }

    bool isInside()
    {
        return Physics.CheckSphere(transform.position, 1.0f);
    }

    public void RockFall()
    {
        Instantiate(m_RockPrefab, transform.position, Quaternion.identity);
    }

    private IEnumerator DelayMethod(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
