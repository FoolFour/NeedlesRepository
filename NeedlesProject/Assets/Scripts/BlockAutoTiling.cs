using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BlockAutoTiling : MonoBehaviour {

    public int x = 0;
    public int y = 0;
    public int z = 0;
    MeshRenderer renderer;

    // Use this for initialization
    void Start ()
    {
        renderer = GetComponent<MeshRenderer>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        renderer.materials[x].mainTextureScale = new Vector2(transform.localScale.z, transform.localScale.y);
        renderer.materials[y].mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.z);
        renderer.materials[z].mainTextureScale = new Vector2(transform.localScale.x, transform.localScale.y);
    }
}
