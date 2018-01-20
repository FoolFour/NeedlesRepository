using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class PauseTimesDrawer : ParametersDrawerBase
{
	Text      text;

    private void Awake()
    {
		text = GetComponent<Text>();
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
		//Pauser�̉e���Ń|�[�Y�o������text��enable�ł͂Ȃ��ꍇ�����邽��
		yield return null;
		text.text = ConvertTime(GetData());
	}

	protected abstract float GetData();
}
