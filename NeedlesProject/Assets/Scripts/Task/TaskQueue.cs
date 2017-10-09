using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>予約した処理を順番に処理するタスククラス</summary>
public class TaskQueue : MonoBehaviour
{
    //////////////////
    // 変数(public) /
    ////////////////

    /// <summary>ラムダ式が動いていればtrue</summary>
    bool isRun;

    /// <summary>キュー</summary>
    Queue<CoroutineTask> queue = new Queue<CoroutineTask>();

    /// <summary>実行したい処理</summary>
    public delegate IEnumerator CoroutineTask();



    ////////////////////////
    // プロパティ(public) /
    //////////////////////

    /// <summary>キューに入っている要素の数</summary>
    public int Count
    {
        get { return queue.Count; }
    }

    /// <summary>キューが空かどうか</summary>
    public bool IsEmpty
    {
        get { return queue.Count == 0; }
    }



    //////////////////
    // 関数(public) /
    ////////////////

    /// <summary>キューに要素を追加する</summary>
    public void Add(CoroutineTask task)
    {
        queue.Enqueue(task);
    }

    /// <summary>キューの中身を空にする</summary>
    public void Clear()
    {
        queue.Clear();
    }

    /// <summary> キューの先頭にある処理を実行 </summary>
    public void RunNext()
    {
        if (isRun) { return; }
        if (queue.Count == 0) { return; }
        StartCoroutine(RunTask(queue.Dequeue()));
    }

    /// <summary>キューに入っている処理を先頭から全て実行</summary>
    public void RunAll()
    {
        //余計な処理が発生してしまうため
        if (queue.Count == 0) { return; }

        StartCoroutine(RunAllTask());
    }



    ///////////////////
    // 関数(private) /
    /////////////////

    /// <summary>タスクを実行する</summary>
    IEnumerator RunTask(CoroutineTask task)
    {
        isRun = true;
        yield return task();
        isRun = false;
    }

    /// <summary>キューの中身を全て実行</summary>
    IEnumerator RunAllTask()
    {
        while (IsEmpty)
        {
            yield return StartCoroutine(RunTask(queue.Dequeue()));
        }
    }
}
