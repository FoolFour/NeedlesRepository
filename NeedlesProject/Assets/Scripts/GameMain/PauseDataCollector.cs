using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PauseDataCollector : MonoBehaviour
{
    [SerializeField]
    private GameTimer gameTimer;

	StageData stageData;


    ///////////////////
    // 関数(private)　/
    /////////////////

    private void Reset()
    {
        gameTimer = FindObjectOfType<GameTimer>();
    }

	private void Awake()
	{
		stageData = GetComponent<StageData>();
	}

	private void Start()
    {
        if(gameTimer == null)
        {
            gameTimer = FindObjectOfType<GameTimer>();
        }

        stageData.ApplyBorder1(PlayerPrefs.GetFloat(PrefsDataName.Border1));
        stageData.ApplyBorder2(PlayerPrefs.GetFloat(PrefsDataName.Border2));

        stageData.ApplyStageName(PlayerPrefs.GetString(PrefsDataName.StageName));
    }

	private void Update()
	{
		stageData.ApplyTime(gameTimer.gameTimeNoPauseTime);
	}
}
