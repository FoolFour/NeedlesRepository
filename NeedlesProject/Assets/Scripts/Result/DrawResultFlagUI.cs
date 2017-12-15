using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Result
{
    public class DrawResultFlagUI : MonoBehaviour
    {
        private enum DrawFlag
        {
            NewRecord,
            Border1,
            Border2
        }

        [SerializeField]
        private ResultData data;

        [SerializeField]
        private DrawFlag   drawFlag;

        private bool       showFlag;

        private UIBehaviour image;

        private void Reset()
        {
            data  = FindObjectOfType<ResultData>();
        }

        private void Awake()
        {
            image = GetComponent<UIBehaviour>();
        }

        private void Start()
        {
            showFlag = GetFlag();
        }

        private void Update()
        {
            image.enabled = showFlag;
        }

        private bool GetFlag()
        {
            if(drawFlag == DrawFlag.NewRecord) { return data.newRecord; }
            if(drawFlag == DrawFlag.Border1)   { return data.border1ClearFlag; }
            if(drawFlag == DrawFlag.Border2)   { return data.border2ClearFlag; }
            
            throw null;
        }
    }
}