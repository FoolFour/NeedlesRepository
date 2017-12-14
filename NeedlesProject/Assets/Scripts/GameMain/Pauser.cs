using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class Pauser : MonoBehaviour
{
    [SerializeField]
    bool reverse = false;

    static bool        pause;
    public static bool isPause
    {
        get { return pause; }
    }

    static bool         initialized = false;
    static List<Pauser> targets      = new List<Pauser>();   // ポーズ対象のスクリプト

    // ポーズ対象のコンポーネント
    Behaviour[] pauseBehavs        = null;

    ParticleSystem[] pauseParticle = null;

    Rigidbody[]   rgBodies         = null;
    Vector3[]     rgBodyVels       = null;
    Vector3[]     rgBodyAVels      = null;

    Rigidbody2D[] rg2dBodies       = null;
    Vector2[]     rg2dBodyVels     = null;
    float[]       rg2dBodyAVels    = null;

    // 初期化
    protected virtual void Start()
    {
        if(initialized == false)
        {
            initialized = true;
            SceneManager.sceneLoaded += OnSceneLoaded;
            pause = false;
        }
        // ポーズ対象に追加する
        AddTargets();
        if(reverse)
        {
            DisableComponents();
        }
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(mode == LoadSceneMode.Single)
        {
            targets.Clear();
        }
    }

    // 破棄されるとき
    protected virtual void OnDestory()
    {
        // ポーズ対象から除外する
        RemoveTargets();
    }

    protected void AddTargets()
    {
        targets.Add(this);
    }

    protected void RemoveTargets()
    {
        targets.Remove(this);
    }

    // ポーズされたとき
    protected virtual void OnPause()
    {
        if(reverse)
        {
            EnableComponents();
            return;
        }
        DisableComponents();
    }

    // ポーズ解除されたとき
    protected virtual void OnResume()
    {
        if(reverse)
        {
            DisableComponents();
            return;
        }
        EnableComponents();
    }

    private void EnableComponents()
    {
        if (pauseBehavs == null)
        {
            return;
        }

        // ポーズ前の状態にコンポーネントの有効状態を復元
        foreach (var com in pauseBehavs)
        {
            com.enabled = true;
        }

        foreach (var ppa in pauseParticle)
        {
            ppa.Play();
        }

        for (var i = 0; i < rgBodies.Length; ++i)
        {
            rgBodies[i].WakeUp();
            rgBodies[i].velocity = rgBodyVels[i];
            rgBodies[i].angularVelocity = rgBodyAVels[i];
        }

        for (var i = 0; i < rg2dBodies.Length; ++i)
        {
            rg2dBodies[i].WakeUp();
            rg2dBodies[i].velocity = rg2dBodyVels[i];
            rg2dBodies[i].angularVelocity = rg2dBodyAVels[i];
        }

        pauseBehavs = null;

        rgBodies = null;
        rgBodyVels = null;
        rgBodyAVels = null;

        rg2dBodies = null;
        rg2dBodyVels = null;
        rg2dBodyAVels = null;
    }

    private void DisableComponents()
    {
        if (pauseBehavs != null)
        {
            return;
        }

        // 有効なコンポーネントを取得
        pauseBehavs = Array.FindAll(GetComponentsInChildren<Behaviour>(), (obj) => { return obj; });

        //Debug.Log("取得したコンポ―ネント一覧");
        //foreach (var item in pauseBehavs)
        //{
        //    Debug.Log(item);
        //}
        //Debug.Log("取得したコンポーネント一覧終了");

        foreach (var com in pauseBehavs)
        {
            com.enabled = false;
        }

        pauseParticle = Array.FindAll(GetComponentsInChildren<ParticleSystem>(), (obj) => { return obj.isPlaying; });
        foreach (var ppa in pauseParticle)
        {
            ppa.Stop();
        }

        rgBodies = Array.FindAll(GetComponentsInChildren<Rigidbody>(), (obj) => { return !obj.IsSleeping(); });
        rgBodyVels = new Vector3[rgBodies.Length];
        rgBodyAVels = new Vector3[rgBodies.Length];
        for (var i = 0; i < rgBodies.Length; ++i)
        {
            rgBodyVels[i] = rgBodies[i].velocity;
            rgBodyAVels[i] = rgBodies[i].angularVelocity;
            rgBodies[i].Sleep();
        }

        rg2dBodies = Array.FindAll(GetComponentsInChildren<Rigidbody2D>(), (obj) => { return !obj.IsSleeping(); });
        rg2dBodyVels = new Vector2[rg2dBodies.Length];
        rg2dBodyAVels = new float[rg2dBodies.Length];
        for (var i = 0; i < rg2dBodies.Length; ++i)
        {
            rg2dBodyVels[i] = rg2dBodies[i].velocity;
            rg2dBodyAVels[i] = rg2dBodies[i].angularVelocity;
            rg2dBodies[i].Sleep();
        }
    }

    // ポーズ
    public static void Pause()
    {
        pause = true;
        foreach (var obj in targets)
        {
            obj.OnPause();
        }
    }

    // ポーズ解除
    public static void Resume()
    {
        pause = false;
        foreach (var obj in targets)
        {
            obj.OnResume();
        }
    }
}
