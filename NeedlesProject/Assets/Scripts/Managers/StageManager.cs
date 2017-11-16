using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {

    [SerializeField,Tooltip("ステージの重力")]
    public Vector3 m_Gravity = new Vector3(0,-9.81f,0);
    [SerializeField, Tooltip("プレイヤーのパンツのマテリアル")]
    public Material m_PlayerPants;
    [SerializeField, Tooltip("プレイヤーのパンツの色")]
    public Color m_PantsColor = Color.green;

	// Use this for initialization
	void Start ()
    {
        Physics.gravity = m_Gravity;
        m_PlayerPants.SetColor("_Color",m_PantsColor);
	}
}
