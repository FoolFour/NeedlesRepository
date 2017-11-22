using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using System.Collections;
using System.Collections.Generic;

using IO = System.IO;

public class BlockRegister : EditorWindow
{
    /// <summary> ブロックのID(ファイル保存名) </summary>
    private       string     blockID;

    /// <summary> ブロックの名前 </summary>
    private       string     blockName;

    /// <summary> 画像のファイル </summary>
    private       int        selectImageIndex;

    /// <summary> 画像のファイル </summary>
    private       string     selectImageFile;

    /// <summary> 表示する優先度 </summary>
    private       int        priority;

    /// <summary> 表示する </summary>
    private       GameObject prefab;

    /// <summary> 画像ファイル     </summary>
    private       string[]   imageNames;

    /// <summary> 画像ディレクトリ </summary>
    private const string     imageDirectory = @"Assets\Editor\StageEditor\Images";

    /// <summary> 出力サイズを保存するディレクトリ </summary>
    private const string     saveDirectory = @"Assets\Editor\StageEditor\StageBlockData";


    private string loadingFile;

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

        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("プレハブ", labelOption);
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            prefab = (GameObject)EditorGUILayout.ObjectField(prefab, typeof(GameObject), false);
        EditorGUILayout.EndHorizontal();

        //ロード用(変えられるように)
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("ファイルのロード");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ファイル名", labelOption);
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            loadingFile = EditorGUILayout.TextField(loadingFile);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Load"))
        {
            Debug.Log("ファイルのロードを開始します");
            Debug.Log("ロードするファイル名" + saveDirectory + "\\" + loadingFile + ".sbdf");

            var fs = new IO.FileStream(saveDirectory + "\\" + loadingFile + ".sbdf", IO.FileMode.Open);
            var br = new IO.BinaryReader(fs);

            blockName       = br.ReadString();
            selectImageFile = br.ReadString();
            priority = br.ReadInt32();


            //prefab = AssetDatabase.LoadAssetAtPath<GameObject>(br.ReadString());

            br.Close();
            fs.Close();
        }

        EditorGUILayout.Space();


        if (GUILayout.Button("Register"))
        {
            if(!IsUniquePriority())
            {
                Debug.LogError("同じ優先度のブロックがあります\n変更してください");
            }

            SaveBlockData();
        }
    }

    bool IsUniquePriority()
    {
        foreach (var data in BlockDataFile.LoadBlockDatas())
        {
            if(priority == data.priority)
            {
                return false;
            }
        }

        return true;
    }

    void SaveBlockData()
    {
        var saveFile = saveDirectory + "\\" + blockID + ".sbdf";
        using (var fs = new IO.FileStream(saveFile, IO.FileMode.Create))
        {
            using (var bw = new IO.BinaryWriter(fs))
            {
                bw.Write(blockName);
                bw.Write(selectImageFile);
                bw.Write(priority);
                bw.Write(AssetDatabase.GetAssetPath(prefab));

                bw.Close();
                fs.Close();
            }
        }

        Debug.Log("保存終了");
    }
}
