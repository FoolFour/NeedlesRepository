using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoalSpawner : MonoBehaviour
{
    //////////////////////////
    // 変数(SerializeField)　/
    ////////////////////////

    [SerializeField]
    private GameObject   rocketPrefab;

    [SerializeField]
    private Vector3      spawnPosition;

    [SerializeField]
    private SceneChanger sceneChanger;

#if UNITY_EDITOR
    private Mesh         rocketMesh;
#endif

    private void Reset()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
    }

    private void Awake()
    {
        Vector3 create_pos = transform.position + spawnPosition;
        GameObject instance = Instantiate(rocketPrefab, create_pos, Quaternion.identity);
        var goal = instance.GetComponent<Goal>();

        goal.sceneChanger = sceneChanger;

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
#warning TODO - 出現する位置を可視化する

        //Gizmos.DrawWireMesh()
    }
}
