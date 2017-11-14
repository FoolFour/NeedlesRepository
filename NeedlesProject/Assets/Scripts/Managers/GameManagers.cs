using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagers : MonoBehaviour
{

    private static GameManagers sInstance;

    public static GameManagers Instance
    {
        get
        {
            if (sInstance == null)
            {
                sInstance = GameObject.FindObjectOfType<GameManagers>();
            }
            return sInstance;
        }
    }

    public SpawnManager SpawnManager;
    public PlayerManager PlayerManager;

    /// <summary>
    /// コンポーネントが増えたら追加する必要がある
    /// </summary>
    public void Awake()
    {
        SpawnManager = GetComponent<SpawnManager>();
        PlayerManager = GetComponent<PlayerManager>();
    }
}
