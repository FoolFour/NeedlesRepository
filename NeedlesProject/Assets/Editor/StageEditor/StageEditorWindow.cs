using UnityEngine;
using UnityEditor;
using UnityEditor.VersionControl;
using System.Collections;
using System.Collections.Generic;

using IO = System.IO;

public class StageEditorWindow : EditorWindow
{
    //----- ----- -----
    // 変数
    //----- ----- -----

    //ステージ名
    private string stageName;
    public  string StageName { get { return stageName; } }

    //ステージサイズ
    private Vector2 stageSize;
    public  Vector2 StageSize { get { return stageSize; } }

    //奥行きに配置するブロックの数
    private int zAxisPutBlockNum = 1;
    public  int ZAxisPutBlockNum { get { return zAxisPutBlockNum; } }

    //選択しているブロックデータ
    private BlockData selectedBlock;

    //ツールの状態
    public enum ToolState
    {
        Moving,
        Erase
    }
    private ToolState toolState;
    public  ToolState State { get { return toolState; } }


    //画像ディレクトリ
    private const string imageDirectory = @"Assets\Editor\StageEditor\Images";

    private const string blockDefineDir = @"Assets\Editor\StageEditor\StageBlockData";

    private string stageDirectory;

    class BlockData
    {
        public string blockName;
        public string imageFile;
        public int priority;
    }
    List<BlockData> blockData = new List<BlockData>();

    private StageEditorSubWindow subWindow = null;

    public string SelectedImage
    {
        get
        {
            return selectedBlock.imageFile;
        }
    }

    //----- ----- -----
    // 関数
    //----- ----- -----

    [MenuItem("Window/StageEditor")]
    static void Open()
    {
        var window = EditorWindow.GetWindow<StageEditorWindow>("StageEditor");
        window.Initialize();
    }

    public void Initialize()
    {
        stageDirectory = Application.temporaryCachePath + @"\StageEditor";

        //ロード
        bool exists = IO.Directory.Exists(stageDirectory);
        if (!exists)
        {
            IO.Directory.CreateDirectory(stageDirectory);
        }

        string stageBinaryFile = stageDirectory + @"\Stage.stbf";
        exists = IO.File.Exists(stageBinaryFile);
        if (exists)
        {
            var fs = new IO.FileStream(stageBinaryFile, IO.FileMode.Open);
            var br = new IO.BinaryReader(fs);

            stageName        = br.ReadString();
            stageSize.x      = br.ReadInt32();
            stageSize.y      = br.ReadInt32();
            zAxisPutBlockNum = br.ReadInt32();

            br.Close();
            fs.Close();
        }

        string[] names = System.IO.Directory.GetFiles(blockDefineDir, "*.sbdf");
        foreach (var n in names)
        {
            var fs = new IO.FileStream(n, IO.FileMode.Open);
            var br = new IO.BinaryReader(fs);

            var data = new BlockData
            {
                blockName = br.ReadString(),
                imageFile = br.ReadString(),
                priority  = br.ReadInt32()
            };

            blockData.Add(data);

            br.Close();
            fs.Close();
        }

        blockData.Sort((a, b) => { return a.priority - b.priority; });

        selectedBlock = blockData[0];
    }

    private void OnGUI()
    {
        DrawStageParametor();
        DrawTools();
        DrawPallet();

        DrawEditStageButton();
    }

    void DrawStageParametor()
    {
        int labelWidth = 160;

        EditorGUILayout.Space();


        //ステージ名入力
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ステージ名(英字で)", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            stageName = EditorGUILayout.TextField(stageName);
        EditorGUILayout.EndHorizontal();


        //ステージサイズ入力
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ステージサイズ", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            EditorGUILayout.LabelField("X:", GUILayout.Width(15));
            stageSize.x = EditorGUILayout.IntField((int)stageSize.x, GUILayout.Width(50));

            EditorGUILayout.LabelField("Y:", GUILayout.Width(15));
            stageSize.y = EditorGUILayout.IntField((int)stageSize.y, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        stageSize.x = Mathf.Max(stageSize.x, 1);
        stageSize.y = Mathf.Max(stageSize.y, 1);


        //Z方向に配置するブロックの数入力
        EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Z方向に配置するブロックの数", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            zAxisPutBlockNum = EditorGUILayout.IntField((int)zAxisPutBlockNum);
        EditorGUILayout.EndHorizontal();

        zAxisPutBlockNum = Mathf.Max(zAxisPutBlockNum, 1);
    }

    void DrawTools()
    {
        EditorGUILayout.Space();

        //ツールボックス描画
        string iconImagePath = @"Assets\Editor\StageEditor\ToolIcons\";

        GUILayoutOption[] option = new GUILayoutOption[4];
        option[0] = GUILayout.MaxWidth(50);
        option[1] = GUILayout.MaxHeight(50);
        option[2] = GUILayout.ExpandWidth(false);
        option[3] = GUILayout.ExpandHeight(false);

        EditorGUILayout.BeginHorizontal();
            //描画ツール
            Texture2D moveToolTex = LoadTexture(iconImagePath + "drawing_tool.png");
            bool isMoving = toolState == ToolState.Moving;

            if (GUILayout.Toggle(isMoving, moveToolTex, "button", option))
            {
                toolState = ToolState.Moving;
            }

            //消しゴムツール
            Texture2D eraseToolTex = LoadTexture(iconImagePath + "erase_tool.png");
            bool isErase = toolState == ToolState.Erase;

            if (GUILayout.Toggle(isErase, eraseToolTex, "button", option))
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
        EditorGUILayout.EndHorizontal();
    }

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

            Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(data.imageFile, typeof(Texture2D));
            if (GUILayout.Button(tex, GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
            {
                selectedBlock = data;
            }
            GUILayout.FlexibleSpace();
            x += w;
        }
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Select Blocks");

        if (selectedBlock != null)
        {
            Texture2D selectedTex = LoadTexture(selectedBlock.imageFile);
            GUILayout.Box(selectedTex);

            EditorGUILayout.LabelField("ブロックの名前 : " + selectedBlock.blockName, GUILayout.Width(300));
        }
    }

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

    private void OnDestroy()
    {
        var fs = new IO.FileStream(stageDirectory + @"\Stage.stbf", IO.FileMode.Create);
        var bw = new IO.BinaryWriter(fs);

        bw.Write(stageName);
        bw.Write((int)stageSize.x);
        bw.Write((int)stageSize.y);
        bw.Write(zAxisPutBlockNum);

        bw.Close();
        fs.Close();
    }

    private Texture2D LoadTexture(string name)
    {
        return (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
    }
}

class StageEditorSubWindow : EditorWindow
{
    public StageEditorWindow parent;

    private Vector3 offset = new Vector3();

    private List<List<string>> mapData = new List<List<string>>();

    private Rect rect = new Rect();

    private void OnFocus()
    {
        if (parent == null)
        {
            Debug.Log("<color=lightblue>Info:</color> parent is null");
            return;
        }

        for (int x = 0; x < mapData.Count; x++)
        {
            for (int y = mapData[0].Count; y < (int)parent.StageSize.y; y++)
            {
                mapData[0].Add("");
            }
        }

        for (int x = mapData.Count; x < (int)parent.StageSize.x; x++)
        {
            List<string> list = new List<string>();
            for (int y = 0; y < (int)parent.StageSize.y; y++)
            {
                list.Add("");
            }
            mapData.Add(list);
        }
    }

    public static StageEditorSubWindow WillAppear(StageEditorWindow p)
    {
        var subWindow = EditorWindow.GetWindow<StageEditorSubWindow>();
        subWindow.parent = p;
        subWindow.Init();

        return subWindow;
    }

    private void Init()
    {
        Debug.Log("初期化開始");

        for (int x = 0; x < (int)parent.StageSize.x; x++)
        {
            List<string> list = new List<string>();
            for (int y = 0; y < (int)parent.StageSize.y; y++)
            {
                list.Add("");
            }
            mapData.Add(list);
        }
        Debug.Log("初期化終了");
    }

    private void OnGUI()
    {
        MouseDetect();

        for (int x = 0; x < parent.StageSize.x; x++)
        {
            for (int y = 0; y < parent.StageSize.y; y++)
            {
                if (mapData[x][y] == "") { continue; }

                var tex = (Texture2D)AssetDatabase.LoadAssetAtPath(mapData[x][y], typeof(Texture2D));

                rect.x      = x * 32;
                rect.y      = y * 32;
                rect.width  = 32;
                rect.height = 32;

                GUI.DrawTexture(rect, tex);
            }
        }
    }

    private void MouseDetect()
    {
        Event e = Event.current;
        if (e.type == EventType.mouseDrag)
        {
            if (parent.State == StageEditorWindow.ToolState.Moving)
            {
                PutBlock(e);
            }
            else
            {
                EraseBlock(e);
            }
        }
    }

    private void PutBlock(Event e)
    {
        Vector2 mousePos = e.mousePosition;

        if (mousePos.x < 0 || mousePos.y < 0) { return; }

        //補正
        mousePos /= 32.0f;
        int x = (int)Mathf.Floor(mousePos.x);
        int y = (int)Mathf.Floor(mousePos.y);

        if (mapData.Count != 0 &&
            x < mapData.Count &&
            y < mapData[0].Count)
        {
            //マップに配置
            mapData[x][y] = parent.SelectedImage;
            Repaint();
        }
    }

    private void EraseBlock(Event e)
    {
        Vector2 mousePos = e.mousePosition;

        if (mousePos.x < 0 || mousePos.y < 0) { return; }

        //補正
        mousePos /= 32.0f;
        int x = (int)Mathf.Floor(mousePos.x);
        int y = (int)Mathf.Floor(mousePos.y);

        if (mapData.Count != 0 &&
            x < mapData.Count &&
            y < mapData[0].Count)
        {
            //マップから消す
            mapData[x][y] = "";
            Repaint();
        }
    }
}