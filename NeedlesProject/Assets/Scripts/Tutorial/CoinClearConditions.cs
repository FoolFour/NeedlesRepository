using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinClearConditions : IConditions {

    TutorialCoin[] TCoins; 

	// Use this for initialization
	void Start ()
    {
        isClear = false;
        TCoins = GetComponentsInChildren<TutorialCoin>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        foreach(var dead in TCoins)
        {
            if (!dead.IsDead()) return;
        }
        isClear = true;
    }
}
