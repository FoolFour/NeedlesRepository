using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BlockTiling : TransitionBase
{
    ///////////////////
    // 変数(private)　/
    /////////////////

    [SerializeField]
    private Texture[] ruleTextures;


    private Image     image;
    private Material  material;

    ///////////////////
    // 関数(private)　/
    /////////////////

    protected override void Awake()
    {
        base.Awake();

        image    = GetComponent<Image>();
        material = image.material;

        int useTex = Random.Range(0, ruleTextures.Length);

        material.SetTexture("_RuleTex", ruleTextures[useTex]);

    }

    protected override void ChangeValue(float amount)
    {
        material.SetFloat("_Amount", amount);
    }
}
