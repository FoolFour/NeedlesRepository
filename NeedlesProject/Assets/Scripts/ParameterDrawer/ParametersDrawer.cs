using UnityEngine;

public class ParametersDrawerBase : MonoBehaviour
{
	[SerializeField]
	protected StageData data;

	private void Reset()
    {
        data = FindObjectOfType<StageData>();
    }
	
	protected string ConvertTime(float time)
	{
		float frac = Mathf.Repeat(time, 1.0f);

		int sec     = Mathf.FloorToInt(time);
		int milliSec = Mathf.FloorToInt(frac * 1000);

		//C#4より上のバージョンが使えない為
		var   timeSpan = new System.TimeSpan(0, 0, 0, sec, milliSec);
        return new System.DateTime(0).Add(timeSpan).ToString("mm:ss.ff");
	}
}
