﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Result
{
    [RequireComponent(typeof(Text))]
    public class DrawResultUI : MonoBehaviour
    {
        /// <summary>どのパラメーターを描画するか</summary>
        private enum DrawParameter
        {
            StageName,
            Coin,
            Time,
            Mission1,
            Mission2
        }

        //////////////////////////
        // 変数(SerializeField) /
        ////////////////////////
        [SerializeField]
        private ResultData    data;
        
        [SerializeField]
        private DrawParameter drawParameter;

        [SerializeField]
        public  bool          showText;

        /////////////////////////////
        // 変数(NonSerializeField) /
        ///////////////////////////
        private Text          text;

        private string        drawText;


        ///////////////////
        // 関数(private) /
        /////////////////

        private void Reset()
        {
            data = FindObjectOfType<ResultData>();
        }

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        private void Start()
        {
            drawText = GetDrawText();
        }

        private void Update()
        {
            text.text = (showText) ? drawText : "";
        }

        /// <summary>持ってくるデータ</summary>
        private string GetDrawText()
        {
            var p =  drawParameter;
            if (p == DrawParameter.StageName) { return data.stageName; }
            if (p == DrawParameter.Coin     ) { return GetCoin();      }
            if (p == DrawParameter.Time     ) { return GetTime();      }
            if (p == DrawParameter.Mission1 ) { return data.mission1;  }
            if (p == DrawParameter.Mission2 ) { return data.mission2;  }

            throw null;
        }

        private string GetCoin()
        {
            return data.resultGetCoin + "/" + CoinCounting.defaultCoinNum;
        }

        private string GetTime()
        {
            float time = data.clearTime;
            float tmp  = Mathf.Repeat(time, 1.0f);

            int sec      = Mathf.FloorToInt(time);
            int milliSec = Mathf.FloorToInt(tmp * 1000);

            var timeSpan = new System.TimeSpan(0, 0, 0, sec, milliSec);
            return new System.DateTime(0).Add(timeSpan).ToString("mm:ss.ff");
        }
    }
}