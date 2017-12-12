using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace StageInfo
{
    [RequireComponent(typeof(Text))]
    public class Border : MonoBehaviour
    {
        [SerializeField]
        private StageSceneInfo info;

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
            info = FindObjectOfType<StageSceneInfo>();
        }

        private void Awake()
        {
            text = GetComponent<Text>();
            text.text = "";
        }

        private void Update()
        {
            const string showText = "秒以内でゴール";
            if(showInfo == ShowBorder.Border1)
            {
                float tmp = info.GetSelectStageInfo().border1;
                
                text.text = tmp + showText;
            }
            else
            {
                float tmp = info.GetSelectStageInfo().border2;

                text.text = tmp + showText;
            }
        }
    }
}
