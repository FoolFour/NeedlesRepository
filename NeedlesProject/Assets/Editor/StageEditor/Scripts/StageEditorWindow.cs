using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using IO = System.IO;

public class StageEditorWindow : EditorWindow
{
    //----- ----- -----
    // 変数
    //----- ----- -----

    public string stageName;

    private string saveDirectory = "";
    public  string SaveDirectory  { get { return saveDirectory;    } }


    public Vector2 stageSize = new Vector2(5, 5);

    private BlockData selectedBlock;

    private bool isInitialized = false;

    ///<summary> ツールの状態 </summary>
    public enum ToolState
    {
        Moving,
        Erase
    }
    private ToolState toolState;
    public  ToolState State { get { return toolState; } }

    private const string stageEditorPath = @"Assets\Editor\StageEditor";
    private const string imageDirectory  = stageEditorPath + @"\Images";
    private const string blockDefineDir  = stageEditorPath + @"\StageBlockData";

    //ステージの情報を保存するディレクトリ
    private       string stageDirectory;

    ///<summary> ブロックのデータ </summary>
    public class BlockData
    {
        public string     blockName;
        public string     imageFile;
        public int        priority;
        public GameObject blockPrefab;
    }
    List<BlockData> blockData = new List<BlockData>();

    ///<summary>マップエディタ</summary>
    private StageEditorSubWindow subWindow = null;

    ///<summary>選択中の画像ファイル</summary>
    public string SelectedImage
    {
        get { return selectedBlock.imageFile; }
    }

    //----- ----- -----
    // 関数
    //----- ----- -----

    //////////////////
    // publicの関数 /
    ////////////////

    /// <summary>ウィンドウの初期化</summary>
    public void Initialize()
    {
        //stageDirectory = Application.temporaryCachePath + @"\StageEditor";
        //var info = Format.RichText.Info("Info - ファイルの場所");
        //Debug.Log(info + ":" + stageDirectory);

        //string stageBinaryFile = stageDirectory + @"\Stage.stbf";
        //bool exists = IO.File.Exists(stageBinaryFile);
        //if (exists)
        //{
        //    var fs = new IO.FileStream(stageBinaryFile, IO.FileMode.Open);
        //    var br = new IO.BinaryReader(fs);

        //    stageName = br.ReadString();
        //    stageSize.x = br.ReadInt32();
        //    stageSize.y = br.ReadInt32();
        //    zAxisPutBlockNum = br.ReadInt32();

        //    br.Close();
        //    fs.Close();
        //}

        LoadBlockData();

        isInitialized = true;
    }

    /// <summary>該当するブロックのデータを探す</summary>
    public BlockData FindBlockObject(string filename)
    {
        foreach (var block in blockData)
        {
            if (block.imageFile != filename) { continue; }
            return block;
        }

        return null;
    }

    /// <summary>該当するブロックのデータを探す</summary>
    public BlockData FindBlockObject(int priority)
    {
        foreach (var block in blockData)
        {
            if (block.priority != priority) { continue; }
            return block;
        }

        return null;
    }

    /////////////////
    // private関数 /
    ///////////////

    /// <summary>ウィンドウを開く</summary>
    [MenuItem("Window/StageEditor")]
    private static void Open()
    {
        var window = EditorWindow.GetWindow<StageEditorWindow>("StageEditor");
        window.Initialize();
    }


    /// <summary>ブロックのデータをロード</summary>
    private void LoadBlockData()
    {

        string[] names = System.IO.Directory.GetFiles(blockDefineDir, "*.sbdf");
        foreach (var n in names)
        {
            var fs = new IO.FileStream(n, IO.FileMode.Open);
            var br = new IO.BinaryReader(fs);

            var data = new BlockData
            {
                blockName = br.ReadString(),
                imageFile = br.ReadString(),
                priority = br.ReadInt32()
            };

            string path = br.ReadString();
            data.blockPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

            blockData.Add(data);

            br.Close();
            fs.Close();
        }

        //昇順
        blockData.Sort((a, b) => { return a.priority - b.priority; });

        selectedBlock = blockData[0];
    }

    /// <summary>このウィンドウがアクティブ状態になった時</summary>
    private void OnFocus()
    {
        if (blockData.Count > 0 || !isInitialized) { return; }

        LoadBlockData();
        Repaint();
    }

    private void OnDestroy()
    {
        //var fs = new IO.FileStream(stageDirectory + @"\Stage.stbf", IO.FileMode.Create);
        //var bw = new IO.BinaryWriter(fs);

        //bw.Write(stageName);
        //bw.Write((int)stageSize.x);
        //bw.Write((int)stageSize.y);
        //bw.Write(zAxisPutBlockNum);

        //bw.Close();
        //fs.Close();

        if (subWindow != null)
        {
            subWindow.Close();
        }
    }

#region 描画関連

    /// <summary>描画</summary>
    private void OnGUI()
    {
        DrawStageParametor();
        DrawTools();
        DrawPallet();

        DrawEditStageButton();

        GUILayout.Space(20);

        GUIStyle style = new GUIStyle();
        style.richText = true;
        GUILayout.Label("<size=20><b>ス テ ー ジ エ デ ィ タ Ver0.91</b></size>", style);
        GUILayout.Label("更新履歴");
        GUILayout.Label("0.91 - スクロールができるように");
        GUILayout.Label("0.90 - とりあえず公開");
    }

    /// <summary>パラメータの名前部分の描画</summary>
    private void DrawParamTitle(string label)
    {
        const int labelWidth = 160;

        EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
        EditorGUILayout.LabelField(":", GUILayout.Width(10));
    }

    /// <summary>ステージの設定部分を描画</summary>
    private void DrawStageParametor()
    {
        EditorGUILayout.Space();

        //ステージ名入力
        EditorGUILayout.BeginHorizontal();
        {
            DrawParamTitle("ステージ名(英字で)");
            stageName = EditorGUILayout.TextField(stageName);
        }
        EditorGUILayout.EndHorizontal();

        //ステージシーンの保存先
        EditorGUILayout.BeginHorizontal();
        {
            DrawParamTitle("ステージデータの保存先");
            saveDirectory = EditorGUILayout.TextField(saveDirectory);
        }
        EditorGUILayout.EndHorizontal();

        //ステージサイズ入力
        EditorGUILayout.BeginHorizontal();
        {
            DrawParamTitle("ステージサイズ");
            EditorGUILayout.LabelField("X:", GUILayout.Width(15));
            stageSize.x = EditorGUILayout.IntField((int)stageSize.x, GUILayout.Width(50));

            EditorGUILayout.LabelField("Y:", GUILayout.Width(15));
            stageSize.y = EditorGUILayout.IntField((int)stageSize.y, GUILayout.Width(50));
        }
        EditorGUILayout.EndHorizontal();

        stageSize.x = Mathf.Max(stageSize.x, 1);
        stageSize.y = Mathf.Max(stageSize.y, 1);
    }

    /// <summary>ツール部分の表示</summary>
    private void DrawTools()
    {
        EditorGUILayout.Space();

        //ツールボックス描画
        string iconImagePath = stageEditorPath + @"\ToolIcons\";

        GUILayoutOption[] option = new GUILayoutOption[4];
        option[0] = GUILayout.MaxWidth(50);
        option[1] = GUILayout.MaxHeight(50);
        option[2] = GUILayout.ExpandWidth(false);
        option[3] = GUILayout.ExpandHeight(false);

        EditorGUILayout.BeginHorizontal();
        {
            const string button = "button";

            //描画ツール
            Texture2D moveToolTex = LoadTexture(iconImagePath + "drawing_tool.png");

            //既に選択されている場合は表示を変える
            bool isMoving = (toolState == ToolState.Moving);
            if (GUILayout.Toggle(isMoving, moveToolTex, button, option))
            {
                toolState = ToolState.Moving;
            }

            //消しゴムツール
            Texture2D eraseToolTex = LoadTexture(iconImagePath + "erase_tool.png");

            //既に選択されている場合は表示を変える
            bool isErase = toolState == ToolState.Erase;
            if (GUILayout.Toggle(isErase, eraseToolTex, button, option))
            {
                toolState = ToolState.Erase;
            }

            switch (toolState)
            {
                case ToolState.Moving:
                    EditorGUILayout.LabelField("描画ツール");
                    break;

                case ToolState.Erase:
                    EditorGUILayout.LabelField("消しゴムツール");
                    break;

                default:
                    Debug.LogError("不正な値");
                    break;
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    /// <summary>パレット(ブロック)の描画</summary>
    private void DrawPallet()
    {
        EditorGUILayout.Space();

        float x = 00.0f;
        float y = 00.0f;
        float w = 50.0f;
        float h = 50.0f;
        float maxW = 300.0f;

        EditorGUILayout.LabelField("pallet");

        EditorGUILayout.BeginVertical();
        foreach (var data in blockData)
        {
            if (x > maxW)
            {
                x = 0.0f;
                y += h;

                EditorGUILayout.EndHorizontal();
            }

            if (x == 0.0f)
            {
                EditorGUILayout.BeginHorizontal();
            }

            GUILayout.FlexibleSpace();

            Texture2D tex = LoadTexture(data.imageFile);
            if (GUILayout.Button(tex, GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
            {
                selectedBlock = data;
                toolState = ToolState.Moving;
            }
            GUILayout.FlexibleSpace();
            x += w;
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();

        //選択されているブロックの表示
        EditorGUILayout.LabelField("Select Blocks");

        if (selectedBlock != null)
        {
            Texture2D selectedTex = LoadTexture(selectedBlock.imageFile);
            GUILayout.Box(selectedTex);

            EditorGUILayout.LabelField("ブロックの名前 : " + selectedBlock.blockName, GUILayout.Width(300));
        }
    }

    /// <summary>ステージエディットボタンの描画</summary>
    private void DrawEditStageButton()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("ステージマップエディタを開く"))
        {
            if (subWindow == null)
            {
                subWindow = StageEditorSubWindow.WillAppear(this);
            }
            else
            {
                subWindow.Focus();
            }
        }
        EditorGUILayout.EndVertical();
    }

#endregion

    /// <summary>テクスチャの読み込み</summary>
    private Texture2D LoadTexture(string name)
    {
        return (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
    }
}
