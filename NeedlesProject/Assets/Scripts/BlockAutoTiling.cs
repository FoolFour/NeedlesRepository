using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BlockAutoTiling : MonoBehaviour {

    [Tooltip("materialが３つのみの場合こちらを使う")]
    public int front = 0;
    [Tooltip("materialが３つのみの場合こちらを使う")]
    public int top = 0;
    [Tooltip("materialが３つのみの場合こちらを使う")]
    public int left = 0;

    [Tooltip("materialが４つ以降の場合こちらを使う")]
    public int back = -1;
    [Tooltip("materialが４つ以降の場合こちらを使う")]
    public int bottom = -1;
    [Tooltip("materialが４つ以降の場合こちらを使う")]
    public int right = -1;
    MeshRenderer renderer;

    // Use this for initialization
    void Start ()
    {
        renderer = GetComponent<MeshRenderer>();
        if (left != -1) renderer.materials[left].mainTextureScale = new Vector2(transform.localScale.z, transform.localScale.y);
        if (top != -1) renderer.materials[top].mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.z);
        if (front != -1) renderer.materials[front].mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);

        if (back != -1) renderer.materials[back].mainTextureScale = new Vector2(transform.localScale.z, transform.localScale.y);
        if (bottom != -1) renderer.materials[bottom].mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.z);
        if (right != -1) renderer.materials[right].mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
}
