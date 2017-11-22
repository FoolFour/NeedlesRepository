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
    private Vector2      spawnPosition;

    [SerializeField]
    private SceneChanger sceneChanger;

#if UNITY_EDITOR
    [Header("デバッグ用")]

    [SerializeField]
    [Tooltip("母船のメッシュ")]
    private Mesh         rocketMesh;

    [SerializeField]
    [Tooltip("非選択時にも母船のスポーン位置を表示するか")]
    private bool         showAlways;
#endif

    private void Reset()
    {
        sceneChanger = FindObjectOfType<SceneChanger>();
    }

    private void Awake()
    {
        Vector3 create_pos = transform.position + (Vector3)spawnPosition;
        GameObject instance = Instantiate(rocketPrefab, create_pos, Quaternion.identity);
        var goal = instance.GetComponent<Goal>();

        goal.sceneChanger = sceneChanger;

        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if(showAlways)
        {
            DrawMesh();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(!showAlways)
        {
            DrawMesh();
        }
    }

    private void DrawMesh()
    {
#if UNITY_EDITOR
        Color c = Gizmos.color;
        c.a = 0.05f;
        c.r = 0.6f;
        c.g = 0.75f;
        c.b = 1.0f;
        Gizmos.color = c;

        Vector3 showPosition = transform.position + (Vector3)spawnPosition;
        Gizmos.DrawWireMesh(rocketMesh, showPosition);
#endif
    }
}
