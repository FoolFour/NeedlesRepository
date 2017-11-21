using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class CoinCountingTest
{
    [Test]
    public void CoinCountingTestSimplePasses()
    {
        // Use the Assert class to test conditions.
        CoinCounting component = GameObject.FindObjectOfType<CoinCounting>();
        if(component == null) { return; }

        int coinNum = component.transform.childCount;
        Assert.AreEqual(CoinCounting.defaultCoinNum, coinNum, "コインが既定の枚数になっていません");

        foreach(Transform child in component.transform)
        {
            Assert.AreEqual("Coin", child.tag, "コインのオブジェクトではないオブジェクトが検出しました");
        }
    }
}
