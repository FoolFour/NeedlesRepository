using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using IO = System.IO;

public class MaterialSetWindow : EditorWindow
{
    Material[] material = new Material[0];
    Vector2    scrollPos;

    /// <summary>ウィンドウを開く</summary>
    [MenuItem("Window/Material")]
    private static void Open()
    {
        var window = EditorWindow.GetWindow<MaterialSetWindow>("MaterialSet");
        window.Initialize();
    }

    public  void Initialize()
    {
        string path = "./Assets/Editor/mateial";
        if(!IO.File.Exists(path)) { return; }

        using (var rs = new IO.StreamReader(path))
        {
            string str = rs.ReadLine();
            int len = int.Parse(str);

            Array.Resize(ref material, len);
            
            for(int i = 0; i < len; i++)
            {
                str = rs.ReadLine();
                material[i] = AssetDatabase.LoadAssetAtPath<Material>(str);
            }
        }
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        {
            int count = EditorGUILayout.IntField("Size", material.Length);
            if(count != material.Length)
            {
                Array.Resize(ref material, count);
            }

            EditorGUI.indentLevel++;

            for(int i = 0; i < material.Length; i++)
            {
                material[i] =
                    (Material)EditorGUILayout.ObjectField("Element " + i, material[i], typeof(Material), false);
            }

            EditorGUI.indentLevel--;

            if(GUILayout.Button("反映"))
            {
                Save();
            }
        }
        EditorGUILayout.EndScrollView();
    }

    private void Save()
    {
        Array.Resize(ref MaterialDefaultSetter.material, material.Length);
        material.CopyTo(MaterialDefaultSetter.material, 0);

        using (var ws = new IO.StreamWriter("./Assets/Editor/mateial"))
        {
            ws.WriteLine(material.Length);

            for(int i = 0; i < material.Length; i++)
            {
                string path = AssetDatabase.GetAssetPath(material[i]);
                ws.WriteLine(path);
                ws.Flush();
            }
            ws.Flush();
            ws.Close();
        }
    }
}
