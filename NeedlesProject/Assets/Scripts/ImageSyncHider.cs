using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ImageSyncHider : MonoBehaviour
{
    List<Image> backGrounds;
    
    public void ApplyImageFillAmount(float fillAmount)
    {
        for(int i = 0; i < backGrounds.Count; i++)
        {
            Image item = backGrounds[i];
            item.fillAmount = fillAmount;
        }
    }

    private void Awake()
    {
        int num = transform.childCount;
        backGrounds = new List<Image>(transform.GetComponentsInChildren<Image>());

        backGrounds.ForEach((item)=>
        {
            item.type = Image.Type.Filled;
            item.fillMethod = Image.FillMethod.Vertical;
            item.fillOrigin = 1; // top
        });
    }
}
