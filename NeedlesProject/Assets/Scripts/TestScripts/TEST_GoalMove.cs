using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_GoalMove : MonoBehaviour
{
    public GameObject m_rocket;

    public void StartEvent()
    {
        Debug.Log("ｲﾍﾞﾝﾄ開始");
        StartCoroutine(GoalEvent());
    }

    IEnumerator GoalEvent()
    {
        for (int i = 0; i < 60; i++)
        {
            m_rocket.transform.position += Vector3.forward * 0.1f;
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < 60; i++)
        {
            m_rocket.transform.position += Vector3.up * 0.1f;
            yield return new WaitForEndOfFrame();
        }

    }
}
