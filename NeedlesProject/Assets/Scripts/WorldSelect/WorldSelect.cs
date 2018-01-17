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
    [Range(0.01f, 10.0f)]
    float            pathSpeed;

    [SerializeField, Tooltip("現在のパスの座標からみた移動量")]
    Vector3          offset;

    [SerializeField]
    StageBasicInfoManager info;


    /////////////////////////////
    // 変数(NonserializeField) /
    ///////////////////////////

    /// <summary>スプライン曲線</summary>
    CatmullRomSpline spline;

    /// <summary>タスクの管理</summary>
    TaskLock         task;

    /// <summary>現在選んでいるワールド</summary>
    //int              selectWorldNum;

    /////////////////////////
    /// プロパティ(public) /
    ///////////////////////

    public int SelectWorld
    {
        get { return info.selectWorld + 1; }
    }

    public bool IsChangeAnimation
    {
        get { return task.IsRunning; }
    }

    ///////////////////
    // 関数(public)  /
    /////////////////

    public Vector3[] GetPath()
    {
        return spline.GetPath();
    }

    ///////////////////
    // 関数(private) /
    /////////////////

    private void Start()
    {
        task = gameObject.AddComponent<TaskLock>();

        spline = new CatmullRomSpline();
        foreach (Transform child in pathObj)
        {
            spline.AddPath(child.position);
        }

        transform.position = FetchPosition(info.selectWorld);
    }

    private void Update()
    {
        if (GamePad.IsStickRightInclined(0.5f)) 
        { 
            //ステージの数を越えるか
            if (info.IsMaxWorld) { return; }
            task.Run(NextSelect); 
        }

        if (GamePad.IsStickLeftInclined (0.5f)) 
        { 
            //ステージ0以下は存在しない
            if (info.IsMinWorld) { return; }
            task.Run(PrevSelect); 
        }
    }

    /// <summary>次のワールドを選択</summary>
    private IEnumerator NextSelect()
    {
        Sound.PlaySe("CursorMove");
        float amount = 0;
        while (amount < 1)
        {
            float hoge = pathAnimation.Evaluate(amount);
            hoge = Mathf.Clamp01(hoge);

            transform.position = FetchPosition(info.selectWorld + hoge);

            amount += Time.deltaTime * pathSpeed;
            yield return null;
        }

        info.WorldSelectNext();
        transform.position = FetchPosition(info.selectWorld);
    }

    private IEnumerator PrevSelect()
    {
        Sound.PlaySe("CursorMove");
        float amount = 0;
        while (amount < 1)
        {
            float hoge = pathAnimation.Evaluate(amount);
            hoge = Mathf.Clamp01(hoge);

            transform.position = FetchPosition(info.selectWorld - hoge);

            amount += Time.deltaTime * pathSpeed;
            yield return null;
        }

        info.WorldSelectPrev();
        transform.position = FetchPosition(info.selectWorld);
    }

    private Vector3 FetchPosition(float amount)
    {
        return spline.FetchPosition(amount) + offset;
    }
}
