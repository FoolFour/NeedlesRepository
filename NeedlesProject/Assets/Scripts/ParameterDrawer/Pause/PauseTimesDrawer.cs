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

	private IEnumerator ApplyData()
	{
		//Pauserの影響でポーズ出現時にtextがenableではない場合があるため
		yield return null;
		text.text = ConvertTime(GetData());
	}

	protected abstract float GetData();
}
