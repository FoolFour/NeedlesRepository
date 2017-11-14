using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{

    public GameObject m_PlayerPrefab;

    // Use this for initialization
    void Start()
    {
        GameObject go = Instantiate(m_PlayerPrefab, transform.position, Quaternion.identity);
        GameManagers.Instance.PlayerManager.SetPlayer(go);
    }
}
