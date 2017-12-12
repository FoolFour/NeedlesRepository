using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkEffect : MonoBehaviour {

    public GameObject Spark1;
    public GameObject Spark2;

    public void ActiveChange(bool enable)
    {
        Spark1.SetActive(enable);
        Spark2.SetActive(enable);
    }
}
