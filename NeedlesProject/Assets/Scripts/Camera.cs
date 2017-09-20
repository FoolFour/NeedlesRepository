using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class Camera : MonoBehaviour
    {

        public Transform Player;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.position = Player.position + (Player.up * 0.5f) + new Vector3(0, 3, -10);
        }
    }
}
