using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace StageInfo
{
    [RequireComponent(typeof(Text))]
    public class Mission : MonoBehaviour
    {
        [SerializeField]
        private StageSceneInfo info;

        private enum ShowMission
        {
            Mission1,
            Mission2
        }
        [SerializeField]
        ShowMission showInfo;

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
            if(showInfo == ShowMission.Mission1)
            {
                text.text = info.GetSelectStageInfo().mission1;
            }
            else
            {
                text.text = info.GetSelectStageInfo().mission2;
            }
        }
    }
}
