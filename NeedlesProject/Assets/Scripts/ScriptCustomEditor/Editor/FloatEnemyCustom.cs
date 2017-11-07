using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(FloatEnemy))]
public class FloatEnemyCustom : Editor
{
    //整数に丸めるか
    private bool isSnap;

    public override void OnInspectorGUI()
    {
        FloatEnemy enemy = target as FloatEnemy;

        enemy.isHitBlock = EditorGUILayout.Toggle("Hit Block", enemy.isHitBlock);

        enemy.movespeed = EditorGUILayout.FloatField("Move Speed", enemy.movespeed);
        EditorGUILayout.Space();
        float min = -20;
        float max = 20;

        EditorGUILayout.LabelField("移動範囲");
        enemy.min = EditorGUILayout.FloatField(" Min", enemy.min);
        enemy.max = EditorGUILayout.FloatField(" Max", enemy.max);
        isSnap = EditorGUILayout.Toggle(" Snap", isSnap);
        EditorGUILayout.MinMaxSlider(ref enemy.min, ref enemy.max, min, max);

        enemy.min = Mathf.Max(enemy.min, min);
        enemy.max = Mathf.Min(enemy.max, max);
        if (isSnap)
        {
            enemy.min = Mathf.Round(enemy.min);
            enemy.max = Mathf.Round(enemy.max);
        }

        enemy.leftright = (FloatEnemy.LR)EditorGUILayout.EnumPopup("Move Direction", enemy.leftright);
    }
}
