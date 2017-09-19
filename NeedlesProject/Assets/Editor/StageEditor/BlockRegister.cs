using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using System.Collections;
using System.Collections.Generic;

using IO = System.IO;

public class BlockRegister : EditorWindow
{
    /// <summary> ブロックのID(ファイル保存名) </summary>
    private       string   blockID;

    /// <summary> ブロックの名前 </summary>
    private       string   blockName;

    /// <summary> 画像のファイル </summary>
    private       int      selectImageIndex;

    /// <summary> 画像のファイル </summary>
    private       string   selectImageFile;

    /// <summary> 表示する優先度 </summary>
    private       int      priority;

    /// <summary> 画像ファイル     </summary>
    private       string[] imageNames;

    /// <summary> 画像ディレクトリ </summary>
    private const string   imageDirectory = @"Assets\Editor\StageEditor\Images";

    /// <summary> 出力サイズを保存するディレクトリ </summary>
    private const string   saveDirectory = @"Assets\Editor\StageEditor\StageBlockData";

    [MenuItem("Window/StageEditorOption/BlockRegister")]
    static void Open()
    {
        GetWindow<BlockRegister>("BlockRegister");
    }

    private void OnGUI()
    {
        var labelOption = GUILayout.Width(160);

        EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ブロックID", labelOption);
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            blockID = EditorGUILayout.TextField(blockID);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ブロックの名前", labelOption);
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            blockName = EditorGUILayout.TextArea(blockName);
        EditorGUILayout.EndHorizontal();

        imageNames = IO.Directory.GetFiles(imageDirectory, "*.png");
        int[] value = new int[imageNames.Length];

        for (int i = 0; i < value.Length; i++)
        {
            value[i] = i;
        }

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ブロックの画像", labelOption);
            EditorGUILayout.LabelField(":", GUILayout.Width(10));

            selectImageIndex = EditorGUILayout.IntPopup(selectImageIndex, imageNames, value);
            selectImageFile  = imageNames[selectImageIndex];
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("優先度", labelOption);
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            priority = EditorGUILayout.IntField(priority);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Register"))
        {
            var fs = new IO.FileStream(saveDirectory + "\\" + blockID + ".sbdf", IO.FileMode.Create);
            var bw = new IO.BinaryWriter(fs);

            bw.Write(blockName);
            bw.Write(selectImageFile);
            bw.Write(priority);

            bw.Close();
            fs.Close();

            Debug.Log("保存終了");
        }
    }
}
