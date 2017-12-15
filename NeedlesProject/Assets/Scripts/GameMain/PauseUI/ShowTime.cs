using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Pause
{
    public class ShowTime : MonoBehaviour
    {
        [SerializeField]
        PauseUIData data;

        enum Data
        {
            NowTime,
            BorderTime1,
            BorderTime2,
        }

        [SerializeField]
        Data getData;

        Text text;
        
        string timeText;
        
        private void Reset()
        {
            data = FindObjectOfType<PauseUIData>();
        }

        private void Awake()
        {
            if(data == null)
            {
                data = FindObjectOfType<PauseUIData>();
            }

            text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            StartCoroutine(ApplyData());
        }

        private IEnumerator ApplyData()
        {
            yield return null;
            text.text = GetData();
        }

        private string GetData()
        {
            switch (getData)
            {
                case Data.NowTime:     return GetNowData();
                case Data.BorderTime1: return GetBorderTime1();
                case Data.BorderTime2: return GetBorderTime2();
            }

            throw null;
        }

        private string GetNowData()
        {
            float time     = data.gameTime;
            float tmp      = Mathf.Repeat(time, 1.0f); 

            int   sec      = Mathf.FloorToInt(time);
            int   milliSec = Mathf.FloorToInt(tmp * 1000);

            var   timeSpan = new System.TimeSpan(0, 0, 0, sec, milliSec);
            return new System.DateTime(0).Add(timeSpan).ToString("mm:ss.ff");
        }

        private string GetBorderTime1()
        {
            string output = data.borderTime1.ToString();
            output += "秒以内にゴール";
            return output;
        }

        private string GetBorderTime2()
        {
            string output = data.borderTime2.ToString();
            output += "秒以内にゴール";
            return output;
        }
    }
}