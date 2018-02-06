using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System;

using IO = System.IO;
using Diagnostics = System.Diagnostics;
using System.Linq;

/*
*このクラス内でしていること
   ウィンドウの描画処理
   セーブ・ロード
   バックアップ
   マウスのクリック判定
   シーンにマップデータを転送する
*/

namespace StageEditor
{
    class StageEditorSubWindow : EditorWindow
    {
        //ステージエディターの親ウィンドウ
        public StageEditorWindow parent;

        //ステージエディタのスクロール値
        private int scroll_x;
        private int scroll_y;

        //マップデータ
        private MapData mapData = new MapData();

        //マウスをクリックした座標
        private Vector2 mouseClickPos = new Vector2();

        private int     clickScrollOffset_y;
        private int     clickScrollOffset_x;

        private Diagnostics.Stopwatch stopwatch = new Diagnostics.Stopwatch();

        private string fileName;

        //定数
        private static readonly int TILE_SIZE = 32;

        private static readonly int MAP_VIEW_POS_X      =  32;
        private static readonly int MAP_VIEW_POS_Y      = 128;

        private static readonly int MAP_VIEW_TILE_POS_X = MAP_VIEW_POS_X / TILE_SIZE;
        private static readonly int MAP_VIEW_TILE_POS_Y = MAP_VIEW_POS_Y / TILE_SIZE;


        private void OnFocus()
        {
            //ストップウォッチをリスタート
            if(!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }

            //親ウィンドウが取得できていなかったら取得する
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
            mapData.AddRange(parent.stageSize.x, parent.stageSize.y);
        }

        public static StageEditorSubWindow WillAppear(StageEditorWindow p)
        {
            //初期化
            var subWindow = EditorWindow.GetWindow<StageEditorSubWindow>();
            subWindow.parent = p;
            Debug.Log("初期化");
            subWindow.MapFix();
            return subWindow;
        }

        public void Update()
        {
            //バックアップタイマーが指定の時間になったらバックアップ
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
            DrawTools();
            DrawMap();
            DrawMassLine();
            DrawFrame();
        }

        private void DrawTools()
        {
            if (GUILayout.Button("シーンに反映"))
            {
                Generate(true);
            }

            if(GUILayout.Button("現在のシーンに反映"))
            {
                Generate(false);
            }

            //保存関係の描画
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
        }

        private void DrawMap()
        {
            scroll_x = (int)GUILayout.HorizontalSlider(scroll_x, 0, parent.stageSize.x);
            scroll_y = (int)GUILayout.  VerticalSlider(scroll_y, 0, parent.stageSize.y);

            Rect rect = new Rect();

            for (int x = scroll_x; x < mapData.SizeX; x++)
            {
                for (int y = scroll_y; y < mapData.SizeY; y++)
                {
                    if (x >= parent.stageSize.x) { continue; }
                    if (y >= parent.stageSize.y) { continue; }

                    rect.x = (x - scroll_x) * TILE_SIZE + MAP_VIEW_POS_X;
                    rect.y = (y - scroll_y) * TILE_SIZE + MAP_VIEW_POS_Y;
                    rect.width  = TILE_SIZE;
                    rect.height = TILE_SIZE;

                    if (!mapData.IsEmptyMass(x, y))
                    {
                        var tex = (Texture2D)AssetDatabase.LoadAssetAtPath(mapData[x, y], typeof(Texture2D));
                        GUI.DrawTexture(rect, tex);
                    }
                }
            }
        }

        private void DrawMassLine()
        {
            Vector3 p1 = new Vector3();
            Vector3 p2 = new Vector3();
            Handles.color = Color.white;
            p1.y = MAP_VIEW_POS_Y;
            p2.y = (parent.stageSize.y - scroll_y) * TILE_SIZE + MAP_VIEW_POS_Y;
            for (int x = scroll_x + 1; x < parent.stageSize.x; x++)
            {
                p1.x = (x - scroll_x) * TILE_SIZE + MAP_VIEW_POS_X;
                p2.x = (x - scroll_x) * TILE_SIZE + MAP_VIEW_POS_X;
                Handles.DrawLine(p1, p2);
            }

            p1.x = MAP_VIEW_POS_X;
            p2.x = (parent.stageSize.x - scroll_x) * TILE_SIZE + MAP_VIEW_POS_X ;
            for (int y = scroll_y + 1; y < parent.stageSize.y; y++)
            {
                p1.y = (y - scroll_y) * TILE_SIZE + MAP_VIEW_POS_Y;
                p2.y = (y - scroll_y) * TILE_SIZE + MAP_VIEW_POS_Y;
                Handles.DrawLine(p1, p2);
            }
        }

        private void DrawFrame()
        {
            //線を描くのに必要な点の定義

            Vector2 top_right = new Vector2
            {
                x = (parent.stageSize.x - scroll_x + 1) * TILE_SIZE,
                y = MAP_VIEW_POS_Y
            };

            Vector2 bottom_left = new Vector2
            {
                x = MAP_VIEW_POS_X,
                y = (parent.stageSize.y - scroll_y) * TILE_SIZE + MAP_VIEW_POS_Y
            };

            Vector2 bottom_right = new Vector2
            {
                x = top_right  .x,
                y = bottom_left.y
            };

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

            top_right.x    += 1;
            bottom_left.y  += 1;

            bottom_right.x += 1;
            bottom_right.y += 1;

            Handles.color = Color.black;
            Handles.DrawLine(top_right, bottom_right);
            Handles.DrawLine(bottom_left, bottom_right);
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
                        var b = parent.FindBlockObject(mapData[x, y]);

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

        //ファイルのロード
        private void LoadFile()
        {
            const string directory = "./StageData";

            //ファイルのチェック
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

            for(int x = 0; x < mapData.SizeX; x++)
            {
                string tmp = rs.ReadLine();
                string[] stageData = tmp.Split(',');
                for(int y = 0; y < mapData.SizeY; y++)
                {
                    if(stageData[y] == "") { continue; }
                    int priority = int.Parse(stageData[y]);
                    mapData[x, y] = parent.FindBlockObject(priority).imageFile;
                }
            }

            rs.Close();
        }

        /// <summary>マウスのクリック判定</summary>
        private void MouseDetect()
        {
            Event e = Event.current;

            const int WHEEL_CLICK = 2;

            if(e.button == WHEEL_CLICK)
            {
                if (e.type == EventType.MouseDown)
                {
                    mouseClickPos = e.mousePosition;
                    clickScrollOffset_x = scroll_x;
                    clickScrollOffset_y = scroll_y;
                }

                if (e.type == EventType.MouseDrag)
                {
                    Vector2 delta = e.mousePosition - mouseClickPos;
                    int x = (int)-delta.x / 16 + clickScrollOffset_x;
                    int y = (int)-delta.y / 16 + clickScrollOffset_y;
                    SetOffset(x, y);
                }
            }
            else if (e.type == EventType.MouseDown ||
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
                //微妙な誤差が入っている可能性があるため
                int delta_x = 0;
                if(e.delta.x < -0.01f || 0.01f < e.delta.x)
                {
                    delta_x = (int)Mathf.Sign(e.delta.x);
                }

                int delta_y = 0;
                if(e.delta.y < -0.01f || 0.01f < e.delta.y)
                {
                    delta_y = (int)Mathf.Sign(e.delta.y);
                }
                MoveOffset(delta_x, delta_y);
            }
        }

        private void MoveOffset(int x, int y)
        {
            scroll_x = Mathf.Clamp(scroll_x + x, 0, parent.stageSize.x);
            scroll_y = Mathf.Clamp(scroll_y + y, 0, parent.stageSize.y);
            Repaint();
        }

        private void SetOffset(int x, int y)
        {
            scroll_x = Mathf.Clamp(x, 0, parent.stageSize.x);
            scroll_y = Mathf.Clamp(y, 0, parent.stageSize.y);
            Repaint();
        }

        private void PutBlock(Event e)
        {
            const int WHEEL_BUTTON = 2;
            if(e.button == WHEEL_BUTTON) { return; }

            UpdateBlockData(e.mousePosition, parent.SelectedImage);
        }

        private void EraseBlock(Event e)
        {
            const int WHEEL_BUTTON = 2;
            if(e.button == WHEEL_BUTTON) { return; }

            UpdateBlockData(e.mousePosition, "");
        }

        private void UpdateBlockData(Vector2 mousePosition, string blockData)
        {
            if (!IsInsideMapArea(mousePosition)) { return; }
            if (mapData.IsEmpty)                 { return; }

            //補正
            int mouse_x = ((int)mousePosition.x - MAP_VIEW_POS_X) / TILE_SIZE + scroll_x;
            int mouse_y = ((int)mousePosition.y - MAP_VIEW_POS_Y) / TILE_SIZE + scroll_y;

            if (mouse_x < mapData.SizeX &&
                mouse_y < mapData.SizeY)
            {
                //マップから消す
                mapData[mouse_x, mouse_y] = blockData;
                Repaint();
            }
        }

        private bool IsInsideMapArea(Vector2 mousePosition)
        {
            if(mousePosition.x < MAP_VIEW_POS_X) { return false; }
            if(mousePosition.y < MAP_VIEW_POS_Y) { return false; }
            return true;
        }

        private void Generate(bool newScene)
        {
            Debug.Log("ステージの保存開始");

            Vector3 createPosition = new Vector3();

            int stageSize_x = parent.stageSize.x;
            int stageSize_y = parent.stageSize.y;

            //シーンを作成
            UnityEngine.SceneManagement.Scene saveScene;
            if(newScene)
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
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
                    if (mapData.IsEmptyMass(x, y)) { continue; }

                    var obj = parent.FindBlockObject(mapData[x, y]).blockPrefab;

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
}