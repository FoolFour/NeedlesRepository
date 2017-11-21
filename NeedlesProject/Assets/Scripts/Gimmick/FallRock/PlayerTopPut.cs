using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopPut : MonoBehaviour {

    [SerializeField,Tooltip("プレイヤーから離れる距離")]
    public Vector3 m_offset = new Vector3(0,10,0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        var player = GameManagers.Instance.PlayerManager.GetPlayer();
        if (GameManagers.Instance.PlayerManager.GetPlayer())
        {
            var temp = player.transform.position;
            temp += m_offset;
            transform.position = temp;
        }	
	}
}
