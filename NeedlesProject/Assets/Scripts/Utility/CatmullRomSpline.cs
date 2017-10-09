using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplinePath
{
    private List<Vector3?> points;

    public int Count
    {
        get { return points.Count - 2; }
    }

    public SplinePath()
    {
        points = new List<Vector3?> { null, null };
    }

    public SplinePath(int capacity)
    {
        points = new List<Vector3?>(capacity) { null, null };
    }

    public SplinePath(IEnumerable<Vector3> collection)
    {
        points = new List<Vector3?>();

        points.Add(null);
        foreach (var item in collection)
        {
            points.Add(item);
        }
        points.Add(null);
    }

    public void Add(Vector3 item)
    {
        points.Insert(points.Count-1, item);
    }

    public void AddRange(IEnumerable<Vector3> collection)
    {
        foreach (var item in collection)
        {
            Add(item);
        }
    }

    public void Remove(int index)
    {
        CheckOutOfRange(index);
        points.RemoveAt(index + 1);
    }

    public void Clear()
    {
        while (IsEmpty())
        {
            Remove(0);
        }
    }

    public bool IsEmpty()
    {
        return Count == 0;
    }

    public void Reverse()
    {
        points.Reverse();
    }

    public Vector3 this[int i]
    {
        set
        {
            CheckOutOfRange(i);
            points[i+1] = value;
        }

        get
        {
            CheckOutOfRange(i);
            return points[i+1].Value;
        }
    }

    private void CheckOutOfRange(int index)
    {
        bool isRange = (0 <= index && index < Count);

        if(!isRange)
        {
            throw new System.IndexOutOfRangeException();
        }
    }

    public List<Vector3?> __ListData()
    {
        return points;
    }

    public override string ToString()
    {
        string output = "";
        for(int i = 1; i < points.Count-1; i++)
        {
            output += points[i].ToString();
            output += "\n";
        }

        return output;
    }
}

/// <summary>catmullromスプライン曲線</summary>
[System.Serializable]
public class CatmullRomSpline
{
    //スプライン曲線が終わった後の処理
    public enum FinishAction
    {
        Pause,
        Stop,
        Repeat,
        Reverse,
    };

    //////////////////
    // 変数(public) /
    ////////////////
    public  FinishAction finishAction;
    public  float        speed;

    ///////////////////
    // 変数(private) /
    /////////////////
    private float        amount;

    private SplinePath   points;

    private bool         isPlay;

    ////////////////
    // プロパティ /
    //////////////
    /// <summary>再生しているか</summary>
    public bool IsPlay
    {
        get { return isPlay; }
    }

    /// <summary>停止状態か</summary>
    public bool IsStop
    {
        get { return (!isPlay) && (IsStartPoint()); }
    }

    /// <summary>一時停止状態か</summary>
    public bool IsPause
    {
        get { return (!isPlay) && (!IsStartPoint()); }
    }

    /// <summary>パスで設定した点の数</summary>
    public int PathNum
    {
        get { return points.Count; }
    }

    ////////////////
    // public関数 /
    //////////////
    /// <summary>コンストラクタ</summary>
    public CatmullRomSpline()
    {
        points       = new SplinePath();
        amount       = 0.0f;
        finishAction = FinishAction.Pause;
        isPlay       = false;
        speed        = 1.0f;
    }

    /// <summary>コンストラクタ</summary>
    public CatmullRomSpline(params Vector3[] path)
    {
        points       = new SplinePath(path);
        amount       = 0.0f;
        finishAction = FinishAction.Pause;
        isPlay       = false;
        speed        = 1.0f;
    }

    /// <summary>パス終了時の処理設定</summary>
    public void SetFinishAction(FinishAction action)
    {
        finishAction = action;
    }

    /// <summary>パスの追加</summary>
    public void AddPath(params Vector3[] path)
    {
        points.AddRange(path);
    }

    /// <summary>再生</summary>
    public void Play(float spd = 1.0f)
    {
        if (IsEndPoint()) { return; }
        speed  = spd;
        isPlay = true;
    }

    /// <summary>停止 ポイントはスタート地点に戻る</summary>
    public void Stop()
    {
        isPlay = false;
        amount = 0.0f;
    }

    /// <summary>一時停止 ポイントはスタート地点に戻らない</summary>
    public void Pause()
    {
        isPlay = false;
    }

    /// <summary>途中から再生する (Playと同じ挙動)</summary>
    public void Resume(float spd = 1.0f)
    {
        if (IsEndPoint()) { return; }
        speed  = spd;
        isPlay = true;
    }

    /// <summary>パスの開始地点にいるか</summary>
    public bool IsStartPoint()
    {
        return (amount == 0.0f);
    }

    public bool IsStartPoint(float amount)
    {
        return (amount == 0);
    }

    /// <summary>パスの終点地点にいるか</summary>
    public bool IsEndPoint()
    {
        points.__ListData();
        return (amount >= points.__ListData().Count - 3);
    }

    public bool IsEndPoint(float amount)
    {
        points.__ListData();
        return (amount >= points.__ListData().Count - 3);
    }

    /// <summary>スタート地点に戻る</summary>
    public void ReturnStart()
    {
        amount = 0.0f;
    }

    /// <summary>パスの開始地点とパスの終了地点を逆転する</summary>
    public void Reverse()
    {
        points.Reverse();
    }

    /// <summary>更新</summary>
    public void Update(float deltaTime)
    {
        if (!isPlay) { return; }

        int count = points.Count;

        amount += deltaTime * speed;

        if (IsEndPoint())
        {
            OnFinishAction();
        }
    }

    public Vector3 FetchPosition()
    {
        return FetchPoint(amount);
    }

    public Vector3 FetchPosition(float amount)
    {
        amount = Mathf.Clamp(amount, 0.0f, points.Count-1.0f);
        return FetchPoint(amount);
    }

    public Vector3 FetchPosition01(float amount)
    {
        amount = Mathf.Clamp01(amount);

        int pointNumber = points.Count;
        amount *= pointNumber;

        return FetchPoint(amount);
    }

    private void OnFinishAction()
    {
        switch (finishAction)
        {
            case FinishAction.Pause:
                Pause();
                break;

            case FinishAction.Repeat:
                ReturnStart();
                break;

            case FinishAction.Reverse:
                Reverse();
                ReturnStart();
                break;

            case FinishAction.Stop:
                Stop();
                break;

            default:
                Debug.LogError("不正な値です");
                break;
        }
    }

    private Vector3 FetchPoint(float amount)
    {
        Debug.Assert(!points.IsEmpty(), "パスを設定されていません");

        if (points.Count == 1)
        {
            return points[0];
        }

        if (amount == 0.0f)
        {
            return points[0];
        }

        if (IsEndPoint(amount))
        {
            return points[points.Count-1];
        }

        int   i = (int)Mathf.Floor(amount);
        float t = Mathf.Repeat(amount, 1.0f); //Frac

        var p = points.__ListData();

        return FetchPoint(p[i + 0], p[i + 1], p[i + 2], p[i + 3], t);
    }

    Vector3 FetchPoint(Vector3? p0, Vector3? p1, Vector3? p2, Vector3? p3, float t)
    {
        bool isFirstPoint = (p0 == null);
        bool isEndPoint   = (p3 == null);

        float t2 = t * t;
        float t3 = t * t * t;

        if (isFirstPoint && isEndPoint)
        {
            Vector3 h1 = p1.Value;
            Vector3 h2 = p2.Value;

            //2つの点しか登録されていない場合
            return Vector3.Lerp(h1, h2, t);
        }

        if (isFirstPoint)
        {
            Vector3 h1 = p1.Value;
            Vector3 h2 = p2.Value;
            Vector3 h3 = p3.Value;

            //始点とその次の点の間を移動する
            return 0.5f * ((h1 - 2 * h2 + h3) * t2 + (-3 * h1 + 4 * h2 - h3) * t) + h1;
        }

        if (isEndPoint)
        {
            Vector3 h0 = p0.Value;
            Vector3 h1 = p1.Value;
            Vector3 h2 = p2.Value;

            //終点の前の点と終点の間を移動する
            return 0.5f * ((h0 - 2 * h1 + h2) * t2 + (-h0 + h2) * t) + h1;
        }

        {
            Vector3 h0 = p0.Value;
            Vector3 h1 = p1.Value;
            Vector3 h2 = p2.Value;
            Vector3 h3 = p3.Value;

            //p1とp2の間を移動する
            return 0.5f * ((-h0 + 3 * h1 - 3 * h2 + h3) * t3 + (2 * h0 - 5 * h1 + 4 * h2 - h3) * t2 + (-h0 + h2) * t) + h1;
        }
    }

    public override string ToString()
    {
        return points.ToString();
    }
}
