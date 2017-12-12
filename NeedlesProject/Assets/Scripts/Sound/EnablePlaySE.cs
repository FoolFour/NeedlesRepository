using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnablePlaySE : MonoBehaviour
{
    public string enableSE;
    public string disableSE;

    private void OnEnable()
    {
        Sound.PlaySe( enableSE);
    }

    private void OnDisable()
    {
        Sound.PlaySe(disableSE);
    }
}
