using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseStageNameDrawer : PauseDataDrawer
{
    private void Start()
    {
        parameterType = ParameterType.Name;
    }

    protected override object GetData()
    {
        return data.stageName;
    }
}
