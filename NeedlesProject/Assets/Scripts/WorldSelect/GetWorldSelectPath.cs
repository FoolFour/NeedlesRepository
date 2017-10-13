using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>PathDrawerへ情報を送るコンポーネント</summary>
[RequireComponent(typeof(GetWorldSelectPath))]
public class GetWorldSelectPath : MonoBehaviour
{
    [SerializeField]
    WorldSelect worldSelect;

    PathDrawer  pathDrawer;

    private void Reset()
    {
        worldSelect = FindObjectOfType<WorldSelect>();
    }

    private void Awake()
    {
        pathDrawer  = GetComponent<PathDrawer>();
    }

    private void Start()
    {
        pathDrawer.SetPath(worldSelect.GetPath());
    }
}
