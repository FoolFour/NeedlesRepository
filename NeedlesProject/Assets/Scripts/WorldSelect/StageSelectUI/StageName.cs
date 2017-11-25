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
        StageSceneInfo info;

        Text text;

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
            text.text = info.GetSelectStageInfo().stageName;
        }
    }
}
