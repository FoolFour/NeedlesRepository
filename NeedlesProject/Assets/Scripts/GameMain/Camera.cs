using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCamera
{
    public class Camera : MonoBehaviour
    {

        private Transform Player;
        [SerializeField, TooltipAttribute("カメラの移動限界")]
        public Vector2 minPositionLimit;
        [SerializeField, TooltipAttribute("カメラの移動限界")]
        public Vector2 maxPositionLimit;
        [SerializeField, TooltipAttribute("正の数字をいれてください")]
        public float zPosition = 20;
        Vector3 camerapos;

        // Use this for initialization
        void Start()
        {
            camerapos = Vector3.zero;
            Player = GameManagers.Instance.PlayerManager.GetPlayer().transform;
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (!Player)
            {
                Player = GameManagers.Instance.PlayerManager.GetPlayer().transform;
                return;
            }

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

            Vector3 temp = Vector3.Max(minPositionLimit,camerapos + (Player.up * 0.5f));
            temp = Vector3.Min(maxPositionLimit, temp);
            transform.position = Vector3.Lerp(transform.position, temp + new Vector3(0, 3, -zPosition), 0.2f);
        }
    }
}
