using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRockController : MonoBehaviour {

    [SerializeField, Tooltip("落とすオブジェクト")]
    public GameObject m_RockPrefab;
    [SerializeField, Tooltip("ランダムで落石")]
    public bool isRandom = true;
    [SerializeField,Tooltip("確率")]
    public float m_Probability = 30;
    [SerializeField, Tooltip("落石を起こすインターバル")]
    public float m_Interval = 5;

    private float m_Timer = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(isRandom)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer < m_Interval) return;
            if (Random.Range(0, 100) < m_Probability) RockFall();
            m_Timer = 0;
        }	
	}

    public void RockFall()
    {
        GameObject go = Instantiate(m_RockPrefab, transform.position, Quaternion.identity);
        Destroy(go, 5);
    }
}
