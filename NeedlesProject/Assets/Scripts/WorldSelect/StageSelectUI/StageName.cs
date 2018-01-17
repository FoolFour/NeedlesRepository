using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace StageInfo
{
    [RequireComponent(typeof(Text))]
    public class StageName : MonoBehaviour
    {
        [SerializeField]
        StageBasicInfoManager info;

        Text text;

        private void Reset()
        {
            info = FindObjectOfType<StageBasicInfoManager>();
        }

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        private void OnEnable()
        {
            text.text = "";
        }

        private void Update()
        {
            text.text = info.NowStageSelectInfo.stageName;
        }
    }
}
