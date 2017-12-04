using UnityEngine;
using IO = System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

namespace StageEditor
{
    public class BlockDataFile
    {
        private const string stageEditorPath = @"Assets\Editor\StageEditor";
        private const string blockDefineDir  = stageEditorPath + @"\StageBlockData";

        /// <summary>既定のディレクトリにあるブロックのデータを読み込む</summary>
        public static IEnumerable<BlockData> LoadBlockDatas()
        {
            string[] names = IO.Directory.GetFiles(blockDefineDir, "*.sbdf");
            foreach (var n in names)
            {
                yield return Load(n);
            }
        }

        /// <summary>ブロックデータの読込</summary>
        private static BlockData Load(string name)
        {
            BlockData blockData = null;

            using (var fs = new IO.FileStream(name, IO.FileMode.Open))
            {
                using(var br = new IO.BinaryReader(fs))
                {
                    blockData = new BlockData
                    {
                        blockName = br.ReadString(),
                        imageFile = br.ReadString(),
                        priority  = br.ReadInt32()
                    };

                    string path = br.ReadString();
                    blockData.blockPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                
                    br.Close();
                    fs.Close();
                }
            }

            Debug.Assert(blockData != null, "不正なファイルが検出されました");
            return blockData;
        }
    }
}