using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnManager : MonoBehaviour
{

    [SerializeField, TooltipAttribute("初期スポーンポイント")]
    public Transform m_FirstSpawnPoint;

    private Vector3 m_CurrentSpawnPoint;

    public void Start()
    {
        m_CurrentSpawnPoint = m_FirstSpawnPoint.position;
        var startpoint = GameObject.Find("Start");
        if(startpoint) m_CurrentSpawnPoint = startpoint.transform.position;
    }

    public void CurrentSpawnChange(Vector3 point)
    {
        m_CurrentSpawnPoint = point;
    }

    public Vector3 GetCurrentSpawnPoint()
    {
        return m_CurrentSpawnPoint;
    }

    public void ReSpawn()
    {
        var player = GameManagers.Instance.PlayerManager.GetPlayer();
        Camera.main.GetComponent<GameCamera.Camera>().CameraReset(new Vector3(GetCurrentSpawnPoint().x,
                                                            GetCurrentSpawnPoint().y,
                                                            Camera.main.transform.position.z));
        player.gameObject.GetComponent<Player>().Dead();
        player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.gameObject.transform.position = GetCurrentSpawnPoint();

        GameObjectAllInit();

    }
    /// <summary>
    /// ゲーム内のオブジェクトを初期化する
    /// </summary>
    void GameObjectAllInit()
    {
#warning クソ遅いらしいので注意
        var components = GameObject.FindObjectsOfType<Component>();
        foreach (var com in components)
        {
            var respawnobj = com as IRespawnMessage;
            if (respawnobj != null) respawnobj.RespawnInit();
        }
    }
}
