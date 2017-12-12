using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnManager : MonoBehaviour
{

    [SerializeField, TooltipAttribute("初期スポーンポイント")]
    public Transform m_FirstSpawnPoint;
    [SerializeField, TooltipAttribute("デバッグモード時設定する")]
    private Vector3 m_CurrentSpawnPoint;

    public void Start()
    {
        if(m_FirstSpawnPoint) m_CurrentSpawnPoint = m_FirstSpawnPoint.position;

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
        player.GetComponent<Player>().ExplosionEffect();
        player.GetComponent<Player>().SwitchColliderandRender(false);

        StartCoroutine(DelayMethod(1.0f, () =>
         {
             Camera.main.GetComponent<GameCamera.Camera>().CameraReset(new Vector3(GetCurrentSpawnPoint().x,
                                                            GetCurrentSpawnPoint().y,
                                                            Camera.main.transform.position.z));
             player.gameObject.transform.position = GetCurrentSpawnPoint();
             GameObjectAllInit();

             StartCoroutine(DelayMethod(0.8f, () =>
             {
                 player.GetComponent<Player>().SwitchColliderandRender(true);
                 player.gameObject.GetComponent<Player>().Dead();
                 player.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

             }));
         }));

    }
    /// <summary>
    /// ゲーム内のオブジェクトを初期化する
    /// </summary>
    void GameObjectAllInit()
    {
        var components = GameObject.FindObjectsOfType<Component>();
        foreach (var com in components)
        {
            var respawnobj = com as IRespawnMessage;
            if (respawnobj != null) respawnobj.RespawnInit();
        }
    }

    private IEnumerator DelayMethod(float waitTime, System.Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}
