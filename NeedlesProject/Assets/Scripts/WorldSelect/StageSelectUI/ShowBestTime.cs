using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TimeSpan = System.TimeSpan;
using DateTime = System.DateTime;

namespace StageInfo
{
    [RequireComponent(typeof(Text))]
    public class ShowBestTime : MonoBehaviour
    {
        [SerializeField]
        StageSceneInfo info;

        Text text;
        
        DateTime dateTime = new DateTime(0);

        private void Reset()
        {
            info = FindObjectOfType<StageSceneInfo>();
        }

        private void Awake()
        {
            text = GetComponent<Text>();
            text.text = "";
        }

        private void Update()
        {
            float tmp = info.GetSelectStageInfo().time;
            
            int sec  = Mathf.FloorToInt(tmp);
            int mili = (int)(Mathf.Repeat(tmp, 1.0f) * 1000.0f);

            DateTime dt = dateTime.AddSeconds(sec);
            dt          = dt.AddMilliseconds(mili);

            text.text = dt.ToString("mm:ss.ff");
        }
    }
}