using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pause
{
    public class ShowTime : MonoBehaviour
    {
        PauseUIData data;

        private void Reset()
        {
            data = FindObjectOfType<PauseUIData>();
        }

        private void Awake()
        {
            if(data == null)
            {
                
            }
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
    }
}