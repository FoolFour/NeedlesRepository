using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Debug_DataErase : MonoBehaviour
{
    int num;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Delete))
        {
            num++;
            if(num >= 5)
            {
                PlayerPrefs.DeleteAll();
                Sound.PlaySe("MenuClose");
                Destroy(gameObject);
            }
        }
    }
}
