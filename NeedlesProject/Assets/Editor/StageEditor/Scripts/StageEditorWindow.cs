using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

using IO = System.IO;

public class StageEditorWindow : EditorWindow
{
    //----- ----- -----
    // 変数
    //----- ----- -----

    private string stageName;
    public  string StageName      { get { return stageName;        } }

    private string saveDirectory;
    public  string SaveDirectory  { get { return saveDirectory;    } }


    private Vector2 stageSize;
    public  Vector2 StageSize     { get { return stageSize;        } }

    private int zAxisPutBlockNum = 1;
    public  int ZAxisPutBlockNum  { get { return zAxisPutBlockNum; } }

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

    private const string imageDirectory = @"Assets\Editor\StageEditor\Images";
    private const string blockDefineDir = @"Assets\Editor\StageEditor\StageBlockData";

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

    [MenuItem("Window/StageEditor")]
    static void Open()
    {
        var window = EditorWindow.GetWindow<StageEditorWindow>("StageEditor");
        window.Initialize();
    }

    public void Initialize()
    {
        stageDirectory = Application.temporaryCachePath + @"\StageEditor";
        var info = Format.RichText.Info("Info - ファイルの場所");
        Debug.Log(info + ":" + stageDirectory);

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

            string path = br.ReadString();
            data.blockPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

            blockData.Add(data);

            br.Close();
            fs.Close();
        }

        blockData.Sort((a, b) => { return a.priority - b.priority; });

        selectedBlock = blockData[0];

        isInitialized = true;
    }

    private void OnFocus()
    {
        if (blockData.Count > 0 || !isInitialized) { return; }

        Initialize();

        Repaint();
    }

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

    void DrawStageParametor()
    {
        int labelWidth = 160;

        EditorGUILayout.Space();

        //ステージ名入力
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("ステージ名(英字で)", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            stageName = EditorGUILayout.TextField(stageName);
        }
        EditorGUILayout.EndHorizontal();

        //ステージシーンの保存先
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("ステージデータの保存先", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            saveDirectory = EditorGUILayout.TextField(saveDirectory);
        }
        EditorGUILayout.EndHorizontal();

        //ステージサイズ入力
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("ステージサイズ", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            EditorGUILayout.LabelField("X:", GUILayout.Width(15));
            stageSize.x = EditorGUILayout.IntField((int)stageSize.x, GUILayout.Width(50));

            EditorGUILayout.LabelField("Y:", GUILayout.Width(15));
            stageSize.y = EditorGUILayout.IntField((int)stageSize.y, GUILayout.Width(50));
        }
        EditorGUILayout.EndHorizontal();

        stageSize.x = Mathf.Max(stageSize.x, 1);
        stageSize.y = Mathf.Max(stageSize.y, 1);


        //Z方向に配置するブロックの数入力
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Z方向に配置するブロックの数", GUILayout.Width(labelWidth));
            EditorGUILayout.LabelField(":", GUILayout.Width(10));
            zAxisPutBlockNum = EditorGUILayout.IntField((int)zAxisPutBlockNum);
        }
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
        //var fs = new IO.FileStream(stageDirectory + @"\Stage.stbf", IO.FileMode.Create);
        //var bw = new IO.BinaryWriter(fs);

        //bw.Write(stageName);
        //bw.Write((int)stageSize.x);
        //bw.Write((int)stageSize.y);
        //bw.Write(zAxisPutBlockNum);

        //bw.Close();
        //fs.Close();

        //if (subWindow != null)
        //{
        //    subWindow.Close();
        //}
    }

    private Texture2D LoadTexture(string name)
    {
        return (Texture2D)AssetDatabase.LoadAssetAtPath(name, typeof(Texture2D));
    }

    public BlockData FindBlockObject(string filename)
    {
        foreach (var block in blockData)
        {
            if (block.imageFile != filename) { continue; }
            return block;
        }

        return null;
    }

    public BlockData FindBlockObject(int priority)
    {
        foreach (var block in blockData)
        {
            if (block.priority != priority) { continue; }
            return block;
        }

        return null;
    }
}

class StageEditorSubWindow : EditorWindow
{
    public StageEditorWindow parent;

    private Vector2 offset = new Vector2();

    private List<List<string>> mapData = new List<List<string>>();

    private Rect rect = new Rect();

    private Vector2 editorOffset = new Vector2(32, 64);

    private void OnFocus()
    {
        if (parent == null)
        {
            var text = Format.RichText.Info("Info");
            Debug.Log(text + ": parent is null");
            return;
        }

        for (int x = 0; x < mapData.Count; x++)
        {
            for (int y = mapData[x].Count; y < (int)parent.StageSize.y; y++)
            {
                mapData[x].Add("");
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
        //Debug.Log("ステージエディタウィンドウの初期化開始");
        //Debug.Log("ファイルの読み込み開始");

        //var path = Application.temporaryCachePath + @"\StageEditor\Block.stbd";
        //if (IO.File.Exists(path))
        //{
        //    Load(path);
        //}
        //else
        //{
        //    var str = Format.RichText.Failed("読みこみ失敗");
        //    Debug.Log(str + ": ファイルが見つかりませんでした");
        //    for (int x = 0; x < (int)parent.StageSize.x; x++)
        //    {
        //        List<string> list = new List<string>();
        //        for (int y = 0; y < (int)parent.StageSize.y; y++)
        //        {
        //            list.Add("");
        //        }
        //        mapData.Add(list);
        //    }
        //}

        //Debug.Log("ファイルの読み込み終了");
        //Debug.Log("ステージエディタウィンドウの初期化終了");
    }

    private void Load(string path)
    {
        //var fs = new IO.FileStream(path, IO.FileMode.Open);
        //var br = new IO.BinaryReader(fs);

        //int size_x = br.ReadInt32();
        //int size_y = br.ReadInt32();

        //for (int x = 0; x < size_x; x++)
        //{
        //    List<string> list = new List<string>();
        //    for (int y = 0; y < size_y; y++)
        //    {
        //        int a = br.ReadInt32();
        //        if (a == -1)
        //        {
        //            list.Add("");
        //            continue;
        //        }

        //        var data = parent.FindBlockObject(a);
        //        list.Add(data.imageFile);
        //    }
        //    mapData.Add(list);
        //}

        //br.Close();
        //fs.Close();
    }

    private void OnDestroy()
    {
        //var path = Application.temporaryCachePath + @"\StageEditor\Block.stbd";
        //var fs = new IO.FileStream(path, IO.FileMode.Create);
        //var bw = new IO.BinaryWriter(fs);

        //bw.Write((int)parent.StageSize.x);
        //bw.Write((int)parent.StageSize.y);

        //int x_size = (int)Mathf.Min(parent.StageSize.x, mapData   .Count);
        //int y_size = (int)Mathf.Min(parent.StageSize.y, mapData[0].Count);

        //for (int x = 0; x < x_size; x++)
        //{
        //    for (int y = 0; y < y_size; y++)
        //    {
        //        var d = parent.FindBlockObject(mapData[x][y]);

        //        if (d == null)
        //        {
        //            bw.Write(-1);
        //        }
        //        else
        //        {
        //            bw.Write(d.priority);
        //        }
        //    }
        //}

        //bw.Close();
        //fs.Close();
        //
    }

    private void OnGUI()
    {
        MouseDetect();

        if (GUILayout.Button("シーンに反映"))
        {
            Generate();
        }

        //offset = EditorGUILayout.BeginScrollView(offset, false, false);
        offset.x = (int)GUILayout.HorizontalSlider((int)offset.x, 0, parent.StageSize.x);
        offset.y = (int)GUILayout.VerticalSlider  ((int)offset.y, 0, parent.StageSize.y);

        for (int x = (int)offset.x; x < mapData.Count; x++)
        {
            for (int y = (int)offset.y; y < mapData[0].Count; y++)
            {
                if (x >= parent.StageSize.x) { continue; }
                if (y >= parent.StageSize.y) { continue; }

                rect.x = (x - (int)offset.x) * 32 + editorOffset.x;
                rect.y = (y - (int)offset.y) * 32 + editorOffset.y;
                rect.width  = 32;
                rect.height = 32;

                if (mapData[x][y] != "")
                {
                    var tex = (Texture2D)AssetDatabase.LoadAssetAtPath(mapData[x][y], typeof(Texture2D));
                    GUI.DrawTexture(rect, tex);
                }
            }
        }

        Vector3 p1 = new Vector3();
        Vector3 p2 = new Vector3();
        Handles.color = Color.white;
        p1.y = editorOffset.y;
        p2.y = parent.StageSize.y*32+32 - offset.y * 32;
        for (int x = (int)offset.x+1; x < parent.StageSize.x; x++)
        {
            p1.x = x * 32+ editorOffset.x - offset.x*32;
            p2.x = x * 32+ editorOffset.x - offset.x*32;
            Handles.DrawLine(p1, p2);
        }

        p1.x = editorOffset.x;
        p2.x = parent.StageSize.x*32+32 - offset.x * 32;
        for (int y = (int)offset.y+1; y < parent.StageSize.y; y++)
        {
            p1.y = y * 32+ editorOffset.y - offset.y*32;
            p2.y = y * 32+ editorOffset.y - offset.y*32;
            Handles.DrawLine(p1, p2);
        }

        DrawFrame();
    }

    private void MouseDetect()
    {
        Event e = Event.current;
        if (e.type == EventType.mouseDown ||
            e.type == EventType.mouseDrag)
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

        if (mousePos.x < editorOffset.x || mousePos.y < editorOffset.y) { return; }

        mousePos += offset*32;

        //補正
        mousePos /= 32.0f;
        int x = (int)Mathf.Floor(mousePos.x);
        int y = (int)Mathf.Floor(mousePos.y);

        if (mapData.Count != 0 &&
            x-(int)editorOffset.x/32 - offset.x < mapData.Count &&
            y-(int)editorOffset.y/32 - offset.y < mapData[0].Count)
        {
            //マップに配置
            mapData[x-(int)editorOffset.x/32][y- (int)editorOffset.y / 32] = parent.SelectedImage;
            Repaint();
        }
    }

    private void EraseBlock(Event e)
    {
        Vector2 mousePos = e.mousePosition;

        if (mousePos.x < editorOffset.x || mousePos.y < editorOffset.y) { return; }

        mousePos += offset*32;

        //補正
        mousePos /= 32.0f;
        int x = (int)Mathf.Floor(mousePos.x);
        int y = (int)Mathf.Floor(mousePos.y);

        if (mapData.Count != 0 &&
            x-1 < mapData.Count &&
            y-1 < mapData[0].Count)
        {
            //マップから消す
            mapData[x][y-1] = "";
            Repaint();
        }
    }

    private void DrawFrame()
    {
        //線を描くのに必要な点の定義

        Vector2 top_right = parent.StageSize * 32 - offset * 32;
        top_right.x += 32;
        top_right.y = editorOffset.y;

        Vector2 bottom_left = parent.StageSize * 32 - offset * 32;
        bottom_left.x  = editorOffset.x;
        bottom_left.y  += editorOffset.y;

        Vector2 bottom_right = parent.StageSize * 32 - offset * 32;
        bottom_right.x += 32;
        bottom_right.y += editorOffset.y;

        //枠を描く(内側から黒・白・黒と描く)
        Handles.color = Color.black;
        Handles.DrawLine(top_right, bottom_right);
        Handles.DrawLine(bottom_left, bottom_right);

        top_right.x += 1;
        bottom_left.y += 1;

        bottom_right.x += 1;
        bottom_right.y += 1;

        Handles.color = Color.white;
        Handles.DrawLine(top_right, bottom_right);
        Handles.DrawLine(bottom_left, bottom_right);

        top_right.x += 1;
        bottom_left.y += 1;

        bottom_right.x += 1;
        bottom_right.y += 1;

        Handles.color = Color.black;
        Handles.DrawLine(top_right, bottom_right);
        Handles.DrawLine(bottom_left, bottom_right);
    }

    private void Generate()
    {
        Debug.Log("ステージの保存開始");

        Vector3 createPosition = new Vector3();

        float stageSize_x = parent.StageSize.x;
        float stageSize_y = parent.StageSize.y;

        //シーンを作成
        var saveScene =
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);

        //ブロックのオブジェクトで溢れないように
        Transform blockParent = new GameObject().transform;

        for (int x = 0; x < stageSize_x; x++)
        {
            for (int y = 0; y < stageSize_y; y++)
            {
                if (mapData[x][y] == "") { continue; }

                int createNum = parent.ZAxisPutBlockNum;

                var obj = parent.FindBlockObject(mapData[x][y]).blockPrefab;

                //プレイヤーやゴールのオブジェクトが沢山生成してしまうため
                if (obj.name == "Start" ||
                    obj.name == "Goal")
                {
                    createNum = 1;
                }

                for (int z = 0; z < createNum; z++)
                {
                    createPosition.x = x;
                    createPosition.y = stageSize_y - y - 1;
                    createPosition.z = (float)z - (float)createNum / 2.0f;

                    var inst = Instantiate(obj, createPosition, Quaternion.identity);

                    inst.transform.parent = blockParent;
                }
            }
        }

        EditorSceneManager.SaveScene(saveScene, parent.SaveDirectory + "\\" + parent.StageName + ".unity");
        Debug.Log("保存が終了しました");
    }
}