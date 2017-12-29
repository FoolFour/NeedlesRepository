using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameTimer))]
public class GameTimerCustom : Editor
{
    public override void OnInspectorGUI()
    {
        GameTimer timer = target as GameTimer;

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ステージの経過時間(ポーズを含む)");
            EditorGUILayout.LabelField(timer.gameTime.ToString("F2"));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ステージの経過時間(ポーズを含めない)");
            EditorGUILayout.LabelField(timer.gameTimeNoPauseTime.ToString("F2"));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ポーズの総経過時間");
            EditorGUILayout.LabelField(timer.pauseTime.ToString("F2"));
        EditorGUILayout.EndHorizontal();
    }
}
