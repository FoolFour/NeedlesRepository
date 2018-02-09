using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(StageBasicInfo))]
public class StageBasicInfoCustom : Editor
{
    //GameObject worldObject;

    int     prevSelectWorld = 0;
    int     selectWorld     = 0;
    int     selectStage     = 0;
    Vector2 worldSelectScroll;
    Vector2 stageSelectScroll;

    public override void OnInspectorGUI()
    {
        //worldObject = EditorGUILayout.ObjectField("WorldObject", worldObject, typeof(GameObject), true) as GameObject;

        EditorGUILayout.LabelField("Stage Config");
        ShowWorldList();
    }

    public void ShowWorldList()
    {
        StageBasicInfo info = target as StageBasicInfo;

        var skin         = GUI.skin.box;
        var scrollOption = GUILayout.Height(64);
        const string gridStyle = "PreferencesKeysElement";

        serializedObject.Update();

        Undo.RecordObject(info, "Stage info changed");

        //ワールド選択
        worldSelectScroll = EditorGUILayout.BeginScrollView(worldSelectScroll, skin, scrollOption);
        {
            string[] showText = new string[info.worldList.Length];
            for (int i = 0; i < info.worldList.Length; i++)
            {
                showText[i] = info.worldList[i].worldName;
            }

            selectWorld = GUILayout.SelectionGrid(selectWorld, showText, 1, gridStyle);
            if(selectWorld != prevSelectWorld)
            {
                prevSelectWorld = selectWorld;
                selectStage = 0;
            }
        }
        EditorGUILayout.EndScrollView();

        info.worldList[selectWorld].worldName = 
            EditorGUILayout.TextField("ワールド名", info.worldList[selectWorld].worldName);

        EditorGUILayout.Space();


        //ステージ選択
        stageSelectScroll = EditorGUILayout.BeginScrollView(stageSelectScroll, skin, scrollOption);
        {
            var stageNum = info.worldList[selectWorld].Length;
            string[] showText = new string[stageNum];
            for (int i_s = 0; i_s < stageNum; i_s++)
            {
                showText[i_s] = info.worldList[selectWorld][i_s].stageName;
            }
            
            selectStage = GUILayout.SelectionGrid(selectStage, showText, 1, gridStyle);
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        //ステージの詳細
        var stageInfo = info.worldList[selectWorld][selectStage];

        stageInfo.stageName  = EditorGUILayout   .TextField ("ステージ名",                 stageInfo.stageName );
        stageInfo.sceneName  = EditorGUIExtension.SceneField("ステージのシーン",           stageInfo.sceneName, serializedObject);
        stageInfo.border1    = EditorGUILayout   .FloatField("目標タイム その１",          stageInfo.border1   );
        stageInfo.border2    = EditorGUILayout   .FloatField("目標タイム その２",          stageInfo.border2   );
        stageInfo.isTutorial = EditorGUILayout   .Toggle    ("チュートリアルのステージか", stageInfo.isTutorial);

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();
        if(GUILayout.Button("ファイルの作成"))
        {
            info.GenerateFile();
        }
    }
}
