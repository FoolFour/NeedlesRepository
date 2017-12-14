using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(StageSceneInfo))]
public class StageSceneInfoCustom : Editor
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
        StageSceneInfo info = target as StageSceneInfo;

        var skin         = GUI.skin.box;
        var scrollOption = GUILayout.Height(64);
        const string gridStyle = "PreferencesKeysElement";

        //ワールド選択
        worldSelectScroll = EditorGUILayout.BeginScrollView(worldSelectScroll, skin, scrollOption);
        {
            string[] showText = new string[info.worldList.Count];
            for (int i = 0; i < info.worldList.Count; i++)
            {
                showText[i] = "World " + (i+1);
            }

            selectWorld = GUILayout.SelectionGrid(selectWorld, showText, 1, gridStyle);
            if(selectWorld != prevSelectWorld)
            {
                prevSelectWorld = selectWorld;
                selectStage = 0;
            }
        }
        EditorGUILayout.EndScrollView();


        //ステージ選択
        stageSelectScroll = EditorGUILayout.BeginScrollView(stageSelectScroll, skin, scrollOption);
        {
            var stageNum = info.worldList[selectWorld].Count;
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

        serializedObject.Update();

        Undo.RecordObject(info, "Stage change info");

        stageInfo.stageName = EditorGUILayout.TextField ("ステージ名",       stageInfo.stageName);
        stageInfo.sceneName =                 SceneField("ステージのシーン",  stageInfo.sceneName);
        stageInfo.border1   = EditorGUILayout.FloatField("目標タイム その１", stageInfo.border1  );
        stageInfo.border2   = EditorGUILayout.FloatField("目標タイム その２", stageInfo.border2  );

        serializedObject.ApplyModifiedProperties();
    }

    public string SceneField(string text, string sceneName)
    {
        var oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(sceneName);

        //インスペクタに変更があったかチェックできない為
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        var newScene = EditorGUILayout.ObjectField(text, oldScene, typeof(SceneAsset), false) as SceneAsset;

        if(EditorGUI.EndChangeCheck())
        {
            var    newPath = AssetDatabase.GetAssetPath(newScene);
            return newPath;
        }
        
        return sceneName;
    }
}
