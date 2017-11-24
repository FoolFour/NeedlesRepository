using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveComponent : MonoBehaviour {

    public void SwitchActive(bool flag)
    {
        var renderer = GetComponent<MeshRenderer>();
        var collider = GetComponent<BoxCollider>();
        if (renderer) renderer.enabled = flag;
        if (collider) collider.enabled = flag;

        var renderers = GetComponentsInChildren<MeshRenderer>();
        var colliders = GetComponentsInChildren<BoxCollider>();
        foreach(var rn in renderers) { rn.enabled = flag; }
        foreach (var co in colliders) { co.enabled = flag; }
    }
}
