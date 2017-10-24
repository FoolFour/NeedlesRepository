using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

class StageEditorSubWindow : EditorWindow
{
    public StageEditorWindow parent;

    private Vector2 offset = new Vector2();

    private List<List<string>> mapData = new List<List<string>>();

    private Rect rect = new Rect();

    private Vector2 editorOffset = new Vector2(32, 64);

    private Vector2 prevMousePos = new Vector2();

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
        Debug.Log("初期化");
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
        offset.x = GUILayout.HorizontalSlider(offset.x, 0, parent.StageSize.x);
        offset.y = GUILayout.VerticalSlider  (offset.y, 0, parent.StageSize.y);

        for (int x = (int)offset.x; x < mapData.Count; x++)
        {
            for (int y = (int)offset.y; y < mapData[0].Count; y++)
            {
                if (x >= parent.StageSize.x) { continue; }
                if (y >= parent.StageSize.y) { continue; }

                rect.x = (x - (int)offset.x) * 32 + editorOffset.x;
                rect.y = (y - (int)offset.y) * 32 + editorOffset.y;
                rect.width = 32;
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
        p2.y = parent.StageSize.y * 32 + 32 - (int)offset.y * 32;
        for (int x = (int)offset.x + 1; x < parent.StageSize.x; x++)
        {
            p1.x = x * 32 + editorOffset.x - (int)offset.x * 32;
            p2.x = x * 32 + editorOffset.x - (int)offset.x * 32;
            Handles.DrawLine(p1, p2);
        }

        p1.x = editorOffset.x;
        p2.x = parent.StageSize.x * 32 + 32 - (int)offset.x * 32;
        for (int y = (int)offset.y + 1; y < parent.StageSize.y; y++)
        {
            p1.y = y * 32 + editorOffset.y - (int)offset.y * 32;
            p2.y = y * 32 + editorOffset.y - (int)offset.y * 32;
            Handles.DrawLine(p1, p2);
        }

        DrawFrame();
    }

    private void MouseDetect()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown)
        {
            if(e.button == 2)
            {
                prevMousePos = e.mousePosition;
            }
        }

        if (e.type == EventType.MouseDrag)
        {
            if(e.button == 2)
            {
                Vector2 delta = e.mousePosition - prevMousePos;
                MoveOffset(-delta / 16.0f);
                prevMousePos = e.mousePosition;
            }
        }


        if (e.type == EventType.MouseDown ||
            e.type == EventType.MouseDrag)
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
        else if(e.type == EventType.ScrollWheel)
        {
            MoveOffset(e.delta);
        }
    }

    private void MoveOffset(Vector2 delta)
    {
        offset += delta;

        offset.x = Mathf.Clamp(offset.x, 0.0f, parent.StageSize.x);
        offset.y = Mathf.Clamp(offset.y, 0.0f, parent.StageSize.y);
        Repaint();
    }

    private void PutBlock(Event e)
    {
        if(e.button == 2) { return; }

        Vector2 mousePos = e.mousePosition;

        if (mousePos.x < editorOffset.x || mousePos.y < editorOffset.y) { return; }

        Vector2 tempOffset = offset;
        tempOffset.x = Mathf.Floor(tempOffset.x);
        tempOffset.y = Mathf.Floor(tempOffset.y);

        mousePos += tempOffset * 32;

        //補正
        mousePos /= 32.0f;
        int x = (int)Mathf.Floor(mousePos.x);
        int y = (int)Mathf.Floor(mousePos.y);

        if (mapData.Count != 0 &&
            x - (int)editorOffset.x / 32 - (int)tempOffset.x < mapData.Count &&
            y - (int)editorOffset.y / 32 - (int)tempOffset.y < mapData[0].Count)
        {
            //マップに配置
            mapData[x - (int)editorOffset.x / 32][y - (int)editorOffset.y / 32] = parent.SelectedImage;
            Repaint();
        }
    }

    private void EraseBlock(Event e)
    {
        if(e.button == 2) { return; }

        Vector2 mousePos = e.mousePosition;

        if (mousePos.x < editorOffset.x || mousePos.y < editorOffset.y) { return; }

        Vector2 tempOffset = offset;
        tempOffset.x = Mathf.Floor(tempOffset.x);
        tempOffset.y = Mathf.Floor(tempOffset.y);

        mousePos += tempOffset * 32;

        //補正
        mousePos /= 32.0f;
        int x = (int)Mathf.Floor(mousePos.x);
        int y = (int)Mathf.Floor(mousePos.y);

        if (mapData.Count != 0 &&
            x - (int)editorOffset.x / 32 - tempOffset.x < mapData.Count &&
            y - (int)editorOffset.y / 32 - tempOffset.y < mapData[0].Count)
        {
            //マップから消す
            mapData[x - (int)editorOffset.x / 32][y - (int)editorOffset.y / 32] = "";
            Repaint();
        }
    }

    private void DrawFrame()
    {
        Vector2 tempOffset = offset;
        tempOffset.x = Mathf.Floor(tempOffset.x);
        tempOffset.y = Mathf.Floor(tempOffset.y);

        //線を描くのに必要な点の定義

        Vector2 top_right    = parent.StageSize * 32 - tempOffset * 32;
        top_right.x += 32;
        top_right.y = editorOffset.y;

        Vector2 bottom_left  = parent.StageSize * 32 - tempOffset * 32;
        bottom_left.x = editorOffset.x;
        bottom_left.y += editorOffset.y;

        Vector2 bottom_right = parent.StageSize * 32 - tempOffset * 32;
        bottom_right.x += 32;
        bottom_right.y += editorOffset.y;

        //枠を描く(内側から黒・白・黒と描く)
        Handles.color = Color.black;
        Handles.DrawLine(top_right, bottom_right);
        Handles.DrawLine(bottom_left, bottom_right);

        top_right.x    += 1;
        bottom_left.y  += 1;

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

                var obj = parent.FindBlockObject(mapData[x][y]).blockPrefab;

                createPosition.x = x;
                createPosition.y = stageSize_y - y - 1;
                var inst = new GameObject();
                inst = PrefabUtility.ConnectGameObjectToPrefab(inst, obj);
                inst.transform.position = createPosition;

                inst.transform.parent = blockParent;
            }
        }

        EditorSceneManager.SaveScene(saveScene, parent.SaveDirectory + "\\" + parent.StageName + ".unity");
        Debug.Log("保存が終了しました");
    }
}