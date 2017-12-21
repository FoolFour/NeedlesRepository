using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IO = System.IO;

[CustomEditor(typeof(StageDataFileLoader))]
public class StageDataFileLoaderCustom : Editor
{
    Object nextStageData;

    const string STAGE_DATA_EXT = ".sdf";

    public override void OnInspectorGUI()
    {
        StageDataFileLoader stageDataFile = target as StageDataFileLoader;

        Undo.RecordObject(stageDataFile, "Save stage name");
        EditorGUILayout.LabelField("現在のステージのデータファイル");
        EditorGUILayout.LabelField(".sdfの拡張子しか受け付けない");
        
        serializedObject.Update();

        EditorGUI.BeginChangeCheck();
        Object nowStageData = AssetDatabase.LoadAssetAtPath<Object>(stageDataFile.nowStageData);
        nowStageData  = EditorGUILayout.ObjectField("現在のステージのデータ ", nowStageData, typeof(Object), false);
        
        if(EditorGUI.EndChangeCheck() && nowStageData != null)
        {
            string path = AssetDatabase.GetAssetPath(nowStageData);
            string extension = IO.Path.GetExtension(path);

            if(extension == STAGE_DATA_EXT)
            {
                stageDataFile.nowStageData = path;
            }
            else
            {
                Debug.LogError("拡張子は" + STAGE_DATA_EXT + "のファイルしか受け付けません");
                nowStageData = null;
            }
        }
        
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("指定しなければリトライの部分を非アクティブにする");
        stageDataFile.nextStageScene = EditorGUIExtension.SceneField("次のシーン", stageDataFile.nextStageScene, serializedObject);
    }
}
