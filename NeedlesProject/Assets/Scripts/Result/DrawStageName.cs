using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Result
{
    [RequireComponent(typeof(Image))]
    public class DrawStageName : MonoBehaviour
    {
        [SerializeField]
        private ResultData data;

        private Text       text;

        private void Reset()
        {
            data = FindObjectOfType<ResultData>();
        }

        private void Awake()
        {
            text = GetComponent<Text>();
        }

        private void Update()
        {
            text.text = data.stageName;
        }
    }
}