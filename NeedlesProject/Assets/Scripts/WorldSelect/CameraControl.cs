using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour
{
    //////////////////////////
    // 変数(SerializeField)　/
    ////////////////////////

    [SerializeField]
    Camera          mainCamera;
      
    [SerializeField]
    int             current;

    [SerializeField]
    List<Transform> controlPoint;

    /////////////////////////////
    // 変数(NonSerializeField)　/
    ///////////////////////////

    public delegate void OnChangeCompleteHandler(int newControlPoint);
    public event OnChangeCompleteHandler OnChangeComplete;

    //////////////////
    // 関数(public)　/
    ////////////////

    public void ChangeControlPointFlash(int num)
    {
        //アクティブの切り替え
        var tmp_old = controlPoint[current];
        tmp_old.gameObject.SetActive(false);

        current = num;

        var tmp_new = controlPoint[current];
        tmp_new.gameObject.SetActive(true);

        SendOnChangeComplete(num);
    }

    public void ChangeControlPointLiner(int num, float moveTime)
    {
        moveTime = Mathf.Max(moveTime, 0.0f);
        
        //ゼロ除算対策
        if(moveTime == 0.0f)
        {
            ChangeControlPointFlash(num);
            return;
        }

        StartCoroutine(MoveLiner(num, moveTime));
    }

    ///////////////////
    // 関数(private)　/
    /////////////////

    private void OnValidate()
    {
        current = Mathf.Max(0, current);
    }

    private void Reset()
    {
        mainCamera = Camera.main;

        foreach(Transform child in transform)
        {
            controlPoint.Add(child);
        }
    }

    private void Start()
    {
        for(int i = 0; i < controlPoint.Count; i++)
        {
            controlPoint[i].gameObject.SetActive(current == i);
        }
    }

    private void LateUpdate()
    {
        Transform currentPoint = controlPoint[current];

        if(currentPoint.gameObject.activeSelf)
        {
            mainCamera.transform.position = currentPoint.position;
        }
    }

    private IEnumerator MoveLiner(int num, float moveTime)
    {
        var tmp_old = controlPoint[current];
        var tmp_new = controlPoint[num];

        tmp_old.gameObject.SetActive(false);

        //moveTime == 0 の場合の処理はChangeControlPointLinerで対策済み
        for(float t = 0.0f; t < 1.0f; t += Time.deltaTime / moveTime)
        {
            Vector3 p = mainCamera.transform.position;
            p.x = Mathf.SmoothStep(tmp_old.position.x, tmp_new.position.x, t);
            p.y = Mathf.SmoothStep(tmp_old.position.y, tmp_new.position.y, t);
            p.z = Mathf.SmoothStep(tmp_old.position.z, tmp_new.position.z, t);

            mainCamera.transform.position = p;
            yield return null;
        }
        
        tmp_new.gameObject.SetActive(true);

        current = num;
        SendOnChangeComplete(num);
    }

    private void SendOnChangeComplete(int num)
    {
        if(OnChangeComplete != null)
        {
            OnChangeComplete(num);
        }
    }
}
