using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : MonoBehaviour, IRespawnMessage
{
    public enum State
    {
        Wait,
        Search,
        Homing,
        BackHome,
        Dead,
    }

    public float m_moveSpeed = 1;
    public float m_radius = 5;
    [Tooltip("見失う距離")]
    public float m_lostDistance = 10;
    public GameObject m_deadEffectPrefab;

    public State m_state = State.Search;
    Rigidbody m_rb;
    Transform m_target;
    Vector3 m_firstPosition;
    Quaternion m_firstRotate;

    // Use this for initialization
    void Start()
    {
        m_state = State.Search;
        m_rb = GetComponent<Rigidbody>();
        m_firstPosition = transform.position;
        m_firstRotate = transform.rotation;
    }

    public void FixedUpdate()
    {
        if (GameManagers.Instance.GameStateManager.GetCurrentGameState() != GameState.Play)
        {
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;
            return;
        }
        switch (m_state)
        {
            case State.Wait: Wait(); break;
            case State.Search: Searching(); break;
            case State.Homing: Homing(); break;
            case State.BackHome: BackHome(); break;
            case State.Dead: Dead(); break;
        }
    }

    public void Wait()
    {
        StartCoroutine(DelayMethod(2, () =>
         {
             m_state = State.Search;
         }));
    }

    void Searching()
    {
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;

        int layer = 1 << 8;
        var cols = Physics.OverlapSphere(transform.position, m_radius, layer);
        foreach (var col in cols)
        {
            if (col.CompareTag("Player"))
            {
                m_target = col.transform;
                m_state = State.Homing;
                return;
            }
        }
    }

    void Homing()
    {
        if (m_target == null)
        {
            m_state = State.BackHome;
            return;
        }
        transform.LookAt(m_target);
        m_rb.velocity = transform.forward * m_moveSpeed;
        var dis = Vector3.Distance(transform.position, m_target.position);
        if(dis > m_lostDistance)
        {
            m_state = State.BackHome;
        }
    }

    void BackHome()
    {
        transform.LookAt(m_firstPosition);
        m_rb.AddForce((transform.forward - m_rb.velocity) * m_moveSpeed, ForceMode.Acceleration);
        var dis = Vector3.Distance(transform.position, m_firstPosition);
        if (dis < 0.1f)
        {
            m_state = State.Search;
        }

        int layer = 1 << 8;
        var cols = Physics.OverlapSphere(transform.position, m_radius, layer);
        foreach (var col in cols)
        {
            if (col.CompareTag("Player"))
            {
                m_target = col.transform;
                m_state = State.Homing;
                return;
            }
        }

    }

    void Dead()
    {
        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var power = collision.transform.position - transform.position;
            power = power.normalized * 10;
            power.y = 3;
            collision.gameObject.GetComponent<Player>().StanMode(power);
            var go = (GameObject)Instantiate(m_deadEffectPrefab, transform.position, Quaternion.identity);
            Destroy(go, 1.0f);
            GetComponent<RemoveComponent>().SwitchActive(false);
            m_state = State.Dead;
        }
        if (collision.gameObject.CompareTag("PlayerArm"))
        {
            var power = collision.transform.position - transform.position;
            power = power.normalized * 10;
            power.y = 3;
            collision.gameObject.GetComponent<NeedleArm>().PlayerStan(power);
            var go = (GameObject)Instantiate(m_deadEffectPrefab, transform.position, Quaternion.identity);
            Destroy(go, 1.0f);
            GetComponent<RemoveComponent>().SwitchActive(false);
            m_state = State.Dead;
        }
    }

    public void RespawnInit()
    {
        m_rb.position = m_firstPosition;
        m_rb.rotation = m_firstRotate;
        GetComponent<RemoveComponent>().SwitchActive(true);
        m_target = null;
        m_state = State.Wait;

        m_rb.velocity = Vector3.zero;
        m_rb.angularVelocity = Vector3.zero;
    }

    private IEnumerator DelayMethod(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
