using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class PauseDataDrawer : ParametersDrawerBase
{
    Text      text;
    protected enum ParameterType
    {
        Name,
        Time
    }

    protected ParameterType parameterType;

    protected bool isShow;

    private void Awake()
    {
        text = GetComponent<Text>();
        string tmp = PlayerPrefs.GetString(PrefsDataName.IsTutorial);

        isShow = (tmp == bool.TrueString) ? false : true;
    }

    private void OnEnable()
    {
        StartCoroutine(ApplyData());
    }

    private void OnDisable()
    {
        text.text = "";
    }

    private IEnumerator ApplyData()
    {
        //Pauserの影響でポーズ出現時にtextがenableではない場合があるため
        yield return null;

        if(!isShow && parameterType == ParameterType.Time)
        {
            text.text = HiddenData();
            yield break;
        }

        if(parameterType == ParameterType.Time)
        {
            text.text = ConvertTime((float)GetData());
        }
        else
        {
            text.text = (string)GetData();
        }
    }
    
    protected abstract object GetData();

    protected virtual string HiddenData()
    {
        return "<size=120>--:--.--</size>";
    }
}
