﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace StageInfo
{
    [RequireComponent(typeof(Text))]
    public class Border : MonoBehaviour
    {
        [SerializeField]
        private StageBasicInfoManager info;

        private enum ShowBorder
        {
            Border1,
            Border2
        }
        [SerializeField]
        ShowBorder showInfo;

        private Text text;

        private void Reset()
        {
            info = FindObjectOfType<StageBasicInfoManager>();
        }

        private void Awake()
        {
            text = GetComponent<Text>();
            text.text = "";
        }

        private void Update()
        {
            int time = 0;
            
            if(showInfo == ShowBorder.Border1)
            {
                time = (int)info.NowStageSelectInfo.border1;
            }
            else
            {
                time = (int)info.NowStageSelectInfo.border2;
            }

            var timeSpan = new System.TimeSpan(0, 0, time);
            text.text = new System.DateTime(0).Add(timeSpan).ToString("mm:ss.ff");
        }
    }
}
