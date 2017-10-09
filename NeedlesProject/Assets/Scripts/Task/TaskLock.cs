using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>現在の実行が終わるまで処理を受理しないタスククラス</summary>
public class TaskLock : MonoBehaviour
{
    bool isRun; //ラムダ式が動いていればtrue

    public bool IsRunning
    {
        get { return isRun; }
    }

    /// <summary>実行したい処理</summary>
    public delegate IEnumerator CoroutineTask();

    /// <summary>タスクを実行させる ただし、既に実行しているタスクがあれば実行しない</summary>
    public void Run(CoroutineTask task)
    {
        if (isRun) { return; }
        StartCoroutine(TaskRun(task));
    }

    /// <summary>タスクを走らせる</summary>
    IEnumerator TaskRun(CoroutineTask task)
    {
        isRun = true;
        yield return StartCoroutine(task());
        isRun = false;
    }
}
