using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class Camera : MonoBehaviour
    {

        public Transform Player;
        Vector3 camerapos;

        // Use this for initialization
        void Start()
        {
            camerapos = Vector3.zero;
        }

        // Update is called once per frame
        void Update()
        {
            if (!Player) return;
            if (Player.GetComponent<Player>().HitCheck() || Player.GetComponent<Player>().IsGround())
            {
                camerapos = Player.position;
            }
            else
            {
                if (camerapos.y < Player.position.y)
                {
                    camerapos.y = Player.position.y;
                }
                camerapos.x = Player.position.x;
            }


            transform.position = Vector3.Lerp(transform.position, camerapos + (Player.up * 0.5f) + new Vector3(0, 3, -20), 0.2f);
        }
    }
}
