using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SaveStageName))]
public class SaveStageNameCustom : Editor
{
    public override void OnInspectorGUI()
    {
        SaveStageName saveStageName = target as SaveStageName;

        Undo.RecordObject(saveStageName, "Save stage name");
        EditorGUILayout.LabelField("指定しなければリトライの部分を非アクティブにする");
        saveStageName.nextStage = EditorGUIExtension.SceneField("次のステージ", saveStageName.nextStage, serializedObject);
    }
}
