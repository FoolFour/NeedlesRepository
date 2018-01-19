using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using IO = System.IO;

[InitializeOnLoad]
public static class MaterialDefaultSetter
{
    public  static Material[] material;
    private static Material[] originalMaterial;

    static MaterialDefaultSetter()
    {
        material         = new Material[0];
        originalMaterial = new Material[0];

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

        EditorApplication.playmodeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged()
    {
        if(!EditorApplication.isPlaying &&
            EditorApplication.isPlayingOrWillChangePlaymode)
        {
            originalMaterial = new Material[material.Length];
            for (int i = 0; i < material.Length; i++)
            {
                originalMaterial[i] = new Material(material[i]);
            }
        }

        //Editorの再生が終わった時にデフォルトに戻す
        if( EditorApplication.isPlaying && 
           !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            for (int i = 0; i < originalMaterial.Length; i++)
            {
                material[i] = originalMaterial[i];
            }
        }
    }
}
