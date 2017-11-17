using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinCounting : MonoBehaviour
{
    const int defaultCoinNum = 20;

    int startCoinNum;

    public int coinRest
    {
        get { return transform.childCount; }
    }

    public int playerGetCoinNum
    {
        get { return defaultCoinNum - coinRest; }
    }

    ///////////////////
    // 関数(private) /
    /////////////////

    private void Awake()
    {
        string tmp_name = gameObject.name;
        Debug.Assert(tmp_name == "Coin", "親の名前を「Coin」にしてください");

        startCoinNum = coinRest;
    }

    private void CoinNumTest()
    {
#if UNITY_EDITOR
        //これを満たさない場合ビルド時にエラー
        Debug.Log("コインの枚数チェック");

        if(startCoinNum < defaultCoinNum)
        {
            int tmp = defaultCoinNum - startCoinNum;
            Debug.LogWarning("<color=yellow>警告</color>:コインの枚数が" + tmp + "枚足りません");
            return;
        }

        if(startCoinNum > defaultCoinNum)
        {
            int tmp = startCoinNum   - defaultCoinNum;
            Debug.LogWarning("<color=yellow>警告</color>:コインの枚数が" + tmp + "多いです");
            return;
        }

        Debug.Log("コインの枚数チェック - <color=lightblue>正常終了</color>");
#endif
    }

    private void Update()
    {

    }
}
