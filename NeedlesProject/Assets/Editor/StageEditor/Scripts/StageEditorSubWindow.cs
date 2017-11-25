using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System;

using IO = System.IO;
using Diagnostics = System.Diagnostics;
using System.Linq;

class StageEditorSubWindow : EditorWindow
{
    public StageEditorWindow parent;

    private Vector2 offset = new Vector2();

    private List<List<string>> mapData = new List<List<string>>();

    private Rect rect = new Rect();

    private Vector2 editorOffset = new Vector2(32, 128);

    private Vector2 prevMousePos = new Vector2();

    private Diagnostics.Stopwatch stopwatch = new Diagnostics.Stopwatch();

    private string fileName;

    private const int tileSize = 32;

    private void OnFocus()
    {
        if(!stopwatch.IsRunning)
        {
            stopwatch.Start();
        }

        if (parent == null)
        {
            var text = Format.RichText.Info("Info");
            Debug.Log(text + ": parent is null");

            parent = EditorWindow.GetWindow<StageEditorWindow>();
            return;
        }

        MapFix();
    }

    private void MapFix()
    {
        for (int x = 0; x < mapData.Count; x++)
        {
            for (int y = mapData[x].Count; y < (int)parent.stageSize.y; y++)
            {
                mapData[x].Add("");
            }
        }

        for (int x = mapData.Count; x < (int)parent.stageSize.x; x++)
        {
            List<string> list = new List<string>();
            for (int y = 0; y < (int)parent.stageSize.y; y++)
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
        Debug.Log("初期化");
        subWindow.MapFix();
        return subWindow;
    }

    public void Update()
    {
        if(stopwatch.Elapsed.TotalSeconds > 60.0f * 5)
        {
            Backup();
            stopwatch.Reset();
            stopwatch.Start();
        }
    }

    private void OnGUI()
    {
        MouseDetect();

        if (GUILayout.Button("シーンに反映"))
        {
            Generate(true);
        }

        if(GUILayout.Button("現在のシーンに反映"))
        {
            Generate(false);
        }

        GUILayout.BeginHorizontal();
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Space(4);
                GUILayout.Label("ファイル名");
                GUILayout.Space(3);
                fileName = GUILayout.TextField(fileName);
            }
            GUILayout.EndVertical();

            GUILayout.BeginVertical();
            {
                if(GUILayout.Button("ファイルの保存"))
                {
                    SaveFile();
                }

                if(GUILayout.Button("ファイルの読込"))
                {
                    LoadFile();
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();

        //offset = EditorGUILayout.BeginScrollView(offset, false, false);
        offset.x = GUILayout.HorizontalSlider(offset.x, 0, parent.stageSize.x);
        offset.y = GUILayout.VerticalSlider  (offset.y, 0, parent.stageSize.y);

        for (int x = (int)offset.x; x < mapData.Count; x++)
        {
            for (int y = (int)offset.y; y < mapData[0].Count; y++)
            {
                if (x >= parent.stageSize.x) { continue; }
                if (y >= parent.stageSize.y) { continue; }

                rect.x = (x - (int)offset.x) * tileSize + editorOffset.x;
                rect.y = (y - (int)offset.y) * tileSize + editorOffset.y;
                rect.width  = tileSize;
                rect.height = tileSize;

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
        p2.y = parent.stageSize.y * tileSize + editorOffset.y - (int)offset.y * tileSize;
        for (int x = (int)offset.x + 1; x < parent.stageSize.x; x++)
        {
            p1.x = x * tileSize + editorOffset.x - (int)offset.x * tileSize;
            p2.x = x * tileSize + editorOffset.x - (int)offset.x * tileSize;
            Handles.DrawLine(p1, p2);
        }

        p1.x = editorOffset.x;
        p2.x = parent.stageSize.x * tileSize + editorOffset.x - (int)offset.x * tileSize;
        for (int y = (int)offset.y + 1; y < parent.stageSize.y; y++)
        {
            p1.y = y * tileSize + editorOffset.y - (int)offset.y * tileSize;
            p2.y = y * tileSize + editorOffset.y - (int)offset.y * tileSize;
            Handles.DrawLine(p1, p2);
        }

        DrawFrame();
    }

    private void Backup()
    {
        Debug.Log("ステージデータのバックアップ開始");

        const string directory = "./StageData/Temp";
        if(!IO.Directory.Exists(directory))
        {
            IO.Directory.CreateDirectory(directory);
        }

        DateTime dt = DateTime.Now;
        string saveFileName = dt.ToString("yyyyMMddHHmmss") + ".csv";
        Debug.Log("セーブファイル名：" + saveFileName);
        Save(directory + "/" + saveFileName);

        //11個以上あれば古いファイルから削除する
        var files = IO.Directory.GetFiles(directory).OrderByDescending(f => IO.File.GetLastAccessTime(f));
        int count = 0;
        foreach(string file in files)
        {
            if(count < 10)
            {
                count++;
                continue;
            }

            IO.File.Delete(file);
        }

        Debug.Log("バックアップ完了");
    }

    private void SaveFile()
    {
        const string directory = "./StageData";

        if(!IO.Directory.Exists(directory))
        {
            IO.Directory.CreateDirectory(directory);
        }

        //指定されたファイル名に不正な名前が無いか調べる
        var r = new System.Text.RegularExpressions.Regex(
                    "[\\x00-\\x1f<>:\"/\\\\|?*]" +
                    "|^(CON|PRN|AUX|NUL|COM[0-9]|LPT[0-9]|CLOCK\\$)(\\.|$)" +
                    "|[\\. ]$",
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        if(r.IsMatch(fileName))
        {
            Debug.LogError("ファイル名に不正な文字列を含んでいます\n 別の名前を使ってください");
            return;
        }

        Save(directory + "/" + fileName + ".csv");
    }

    private void Save(string path)
    {
        using (var ws = new IO.StreamWriter(path))
        {

            ws.WriteLine(parent.stageName);
            ws.WriteLine(parent.stageSize.x + "," + parent.stageSize.y + ",");
            ws.Flush();

            for (int x = 0; x < parent.stageSize.x; x++)
            {
                for(int y = 0; y < parent.stageSize.y; y++)
                {
                    var b = parent.FindBlockObject(mapData[x][y]);

                    if(b == null)
                    {
                        ws.Write(",");
                    }
                    else
                    {
                        ws.Write(b.priority + ",");
                    }
                }
                ws.WriteLine();
                ws.Flush();
            }

            ws.Flush();
            ws.Close();
        }
    }

    private void LoadFile()
    {
        const string directory = "./StageData";

        if(!IO.File.Exists(directory + "/" + fileName + ".csv"))
        {
            Debug.LogError("ファイル名が見つかりませんでした");
            return;
        }

        var rs = new IO.StreamReader(directory + "/"+ fileName + ".csv");

        string stageName = rs.ReadLine();

        parent.stageName = stageName;

        string stageSize = rs.ReadLine();

        string[] size = stageSize.Split(',');
        parent.stageSize.x = int.Parse(size[0]);
        parent.stageSize.y = int.Parse(size[1]);


        mapData.Clear();
        MapFix();

        for(int x = 0; x < mapData.Count; x++)
        {
            string tmp = rs.ReadLine();
            string[] stageData = tmp.Split(',');
            for(int y = 0; y < mapData[0].Count; y++)
            {
                if(stageData[y] == "") { continue; }
                int priority = int.Parse(stageData[y]);
                mapData[x][y] = parent.FindBlockObject(priority).imageFile;
            }
        }

        rs.Close();
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

        offset.x = Mathf.Clamp(offset.x, 0.0f, parent.stageSize.x);
        offset.y = Mathf.Clamp(offset.y, 0.0f, parent.stageSize.y);
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

        mousePos += tempOffset * tileSize;

        //補正
        mousePos /= (float)tileSize;
        int x = (int)Mathf.Floor(mousePos.x);
        int y = (int)Mathf.Floor(mousePos.y);

        if (mapData.Count != 0 &&
            x - (int)editorOffset.x / tileSize - (int)tempOffset.x < mapData.Count &&
            y - (int)editorOffset.y / tileSize - (int)tempOffset.y < mapData[0].Count)
        {
            //マップに配置
            mapData[x - (int)editorOffset.x / tileSize][y - (int)editorOffset.y / tileSize] = parent.SelectedImage;
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

        mousePos += tempOffset * tileSize;

        //補正
        mousePos /= (float)tileSize;
        int x = (int)Mathf.Floor(mousePos.x);
        int y = (int)Mathf.Floor(mousePos.y);

        if (mapData.Count != 0 &&
            x - (int)editorOffset.x / tileSize - tempOffset.x < mapData.Count &&
            y - (int)editorOffset.y / tileSize - tempOffset.y < mapData[0].Count)
        {
            //マップから消す
            mapData[x - (int)editorOffset.x / tileSize][y - (int)editorOffset.y / tileSize] = "";
            Repaint();
        }
    }

    private void DrawFrame()
    {
        Vector2 tempOffset = offset;
        tempOffset.x = Mathf.Floor(tempOffset.x);
        tempOffset.y = Mathf.Floor(tempOffset.y);

        //線を描くのに必要な点の定義

        Vector2 top_right    = parent.stageSize * tileSize - tempOffset * tileSize;
        top_right.x += tileSize;
        top_right.y = editorOffset.y;

        Vector2 bottom_left  = parent.stageSize * tileSize - tempOffset * tileSize;
        bottom_left.x = editorOffset.x;
        bottom_left.y += editorOffset.y;

        Vector2 bottom_right = parent.stageSize * tileSize - tempOffset * tileSize;
        bottom_right.x += tileSize;
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

    private void Generate(bool newScene)
    {
        Debug.Log("ステージの保存開始");

        Vector3 createPosition = new Vector3();

        float stageSize_x = parent.stageSize.x;
        float stageSize_y = parent.stageSize.y;

        //シーンを作成
        UnityEngine.SceneManagement.Scene saveScene;
        if(newScene)
        {
            saveScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
        }
        else
        {
            saveScene = EditorSceneManager.GetActiveScene();
        }

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

        string saveDirectory = parent.SaveDirectory;
        if (saveDirectory != "")
        {
            saveDirectory += "\\";
        }

        EditorSceneManager.SaveScene(saveScene, "./Assets/" + saveDirectory + parent.stageName + ".unity");
        Debug.Log("保存が終了しました");
    }
}