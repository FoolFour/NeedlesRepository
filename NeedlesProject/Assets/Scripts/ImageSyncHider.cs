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
        backGrounds = new List<Image>(num * 3);

        foreach (Transform child in transform)
        {
            foreach(Transform imageObj in child)
            {
                Image image = imageObj.GetComponent<Image>();
                backGrounds.Add(image);
            }
        }
    }
}
