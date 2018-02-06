using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ApplicationEnd : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(End);
    }

    private void End()
    {
        Application.Quit();
    }
}
