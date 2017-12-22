using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ResultNextScene : MonoBehaviour
{
    private void Awake()
    {
        string str = PlayerPrefs.GetString(PrefsDataName.NextSene);

        if (str == "")
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
