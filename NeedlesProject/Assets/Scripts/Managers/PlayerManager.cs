using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    [SerializeField]
    public GameObject m_NowPlayer = null;

    /// <summary>
    /// プレイヤを登録する
    /// </summary>
    /// <param name="player">登録するプレイヤー</param>
    public void SetPlayer(GameObject player)
    {
        m_NowPlayer = player;
    }

    public GameObject GetPlayer()
    {
        return m_NowPlayer;
    }
}
