using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 特定の機能を付けたり消したりする
/// </summary>
public class ComponentFlash : MonoBehaviour,IRespawnMessage {

    [Tooltip("点滅させるビヘイビア")]
    public Behaviour[] m_components;
    [Tooltip("点滅させるパーティクル")]
    public ParticleSystem[] m_particles;
    [Tooltip("点滅させる当たり判定")]
    public Collider[] m_colliders;

    [Tooltip("処理を遅らせる時間")]
    public float m_delayTime = 0;
    [Tooltip("実体化する時間")]
    public float m_existTime = 5;
    [Tooltip("透明化している時間")]
    public float m_notExistTime = 3;

    private float m_timer = 0;
    private bool isdelay = true;
    private bool isActive = true;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_timer += Time.deltaTime;
        if (!isdelay)
        {
            float Time = 0;
            if (isActive) Time = m_existTime;
            else Time = m_notExistTime;

            if (m_timer >= Time)
            {
                isActive = !isActive;
                SwitchScript(isActive);
                m_timer = 0;
            }
        }
        if (m_timer >= m_delayTime && isdelay)
        {
            m_timer = 0;
            isdelay = false;
        }
    }

    /// <summary>
    /// 反対にする
    /// </summary>
    /// <param name="flag"></param>
    void SwitchScript(bool flag)
    {
        foreach(var com in m_components)
        {
            com.enabled = flag;
        }

        foreach (var par in m_particles)
        {
            if (flag) par.Play();
            else par.Stop();
        }

        foreach (var collder in m_colliders)
        {
            collder.enabled = flag;
        }
    }

    public void RespawnInit()
    {
        SwitchScript(true);
        m_timer = 0;
        isdelay = true;
        isActive = true;
    }
}
