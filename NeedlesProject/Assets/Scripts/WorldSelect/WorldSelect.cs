using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldSelect : MonoBehaviour
{
    //////////////////////////
    // 変数(SerializeField) /
    ////////////////////////
    [SerializeField, Tooltip("パス生成に使うオブジェクト")]
    Transform        pathObj;

    [SerializeField, Tooltip("パスの移動量設定")]
    AnimationCurve   pathAnimation;

    [SerializeField, Tooltip("パスのスピード")]
    [Range(0, 10)]
    float            pathSpeed;

    [SerializeField, Tooltip("現在のパスの座標からみた移動量")]
    Vector3          offset;


    /////////////////////////////
    // 変数(NonserializeField) /
    ///////////////////////////

    /// <summary>スプライン曲線</summary>
    CatmullRomSpline spline;

    /// <summary>タスクの管理</summary>
    TaskLock         task;

    /// <summary>現在選んでいるワールド</summary>
    int              selectWorldNum;

    /////////////////////////
    /// プロパティ(public) /
    ///////////////////////

    public int SelectWorld
    {
        get { return selectWorldNum + 1; }
    }

    public bool IsChangeAnimation
    {
        get { return task.IsRunning; }
    }

    ///////////////////
    // 関数(private) /
    /////////////////

    private void Awake()
    {
        selectWorldNum = 0;

        spline = new CatmullRomSpline();
        task = gameObject.AddComponent<TaskLock>();
    }

    private void Start()
    {
        foreach (Transform child in pathObj)
        {
            spline.AddPath(child.position);
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow)) { task.Run(NextSelect); }
        if (Input.GetKey(KeyCode. LeftArrow)) { task.Run(PrevSelect); }
    }

    /// <summary>次のワールドを選択</summary>
    private IEnumerator NextSelect()
    {
        //ステージの数を越えるか
        if (selectWorldNum >= spline.PathNum-1) { yield break; }

        float amount = 0;
        while (amount < 1)
        {
            float hoge = pathAnimation.Evaluate(amount);
            hoge = Mathf.Clamp01(hoge);

            transform.position = FetchPosition(selectWorldNum + hoge);

            amount += Time.deltaTime;
            yield return null;
        }

        selectWorldNum++;
        transform.position = FetchPosition(selectWorldNum);
    }

    private IEnumerator PrevSelect()
    {
        //ステージ0以下は存在しない
        if (selectWorldNum <= 0) { yield break; }

        float amount = 0;
        while (amount < 1)
        {
            float hoge = pathAnimation.Evaluate(amount);
            hoge = Mathf.Clamp01(hoge);

            transform.position = FetchPosition(selectWorldNum - hoge);

            amount += Time.deltaTime;
            yield return null;
        }

        selectWorldNum--;
        transform.position = FetchPosition(selectWorldNum);
    }

    private Vector3 FetchPosition(float amount)
    {
        return spline.FetchPosition(amount) + offset;
    }
}
