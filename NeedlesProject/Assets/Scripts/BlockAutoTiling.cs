using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        renderer.materials[x].mainTextureScale = new Vector2(transform.localScale.x, 1);
        renderer.materials[y].mainTextureScale = new Vector2(1, transform.localScale.y);
        renderer.materials[z].mainTextureScale = new Vector2(transform.localScale.z, 1);
    }
}
