using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EditorGUIExtension
{
    public static string SceneField(string text, string sceneName, SerializedObject serializedObject)
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
