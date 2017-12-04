using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace StageEditor
{
    public class MapData
    {
        private List<List<string>> data;

        ////////////////////////
        // プロパティ(public) /
        //////////////////////

        /// <summary>マップのX方向の大きさ</summary>
        public int SizeX
        { 
            get { return data.Count;      } 
        }

        /// <summary>マップのY方向の大きさ</summary>
        public int SizeY 
        { 
            get 
            { 
                if(SizeX == 0)
                {
                    return 0;
                }
                return data[0].Count;   
            }
        }

        /// <summary>マップのデータが空か</summary>
        public bool IsEmpty
        {
            get { return data.Count == 0; }
        }

        ////////////////////////
        // インデクサ(public) /
        //////////////////////
        public string this [int x ,int y]
        {
            get { return data[x][y];  }
            set { data[x][y] = value; }
        }

        //////////////////
        // 関数(public) /
        ////////////////

        /// <summary>コンストラクタ</summary>
        public MapData()
        {
            data = new List<List<string>>();
        }

        /// <summary>要素の追加</summary>
        public void AddRange(int new_x, int new_y)
        {
            for (int x = 0; x < SizeX; x++)
            {
                for (int y = data[x].Count; y < new_y; y++)
                {
                    data[x].Add("");
                }
            }

            for (int x = SizeX; x < new_x; x++)
            {
                List<string> list = new List<string>();
                for (int y = 0; y < new_y; y++)
                {
                    list.Add("");
                }
                data.Add(list);
            }
        }

        /// <summary>データのサイズを変更</summary>
        public void Clear()
        {
            data.Clear();
        }

        /// <summary>指定したマスが空か</summary>
        public bool IsEmptyMass(int x, int y)
        {
            return data[x][y] == "";
        }
    }
}
