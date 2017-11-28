using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapData
{
    private List<List<string>> data;

    public int SizeX
    { 
        get { return data.Count;    } 
    }

    public int SizeY 
    { 
        get { return data[0].Count; }
    }

    public string this [int x ,int y]
    {
        get { return data[x][y];    }
    }

    public void AddRangeX(int add_x)
    {
        if(add_x <= 0) { return; } 

        for(int x = 0; x < add_x; x++)
        {
            List<string> list = new List<string>();
            //サイズを合わせる
            for(int y = 0; y < SizeY; y++)
            {
                list.Add("");
            }
        }
    }

    public void AddRangeY(int add_y)
    {
        if(add_y <= 0) { return; }
        
        for(int x = 0; x < SizeX; x++)
        {
            for(int y = 0; y < add_y; y++)
            {
                data[x].Add("");
            }
        }
    }

    public void AddRange(int add_x, int add_y)
    {
        AddRangeX(add_x);
        AddRangeY(add_y);
    }

    public void Resize(int new_x, int new_y)
    {
        data.Clear();
        AddRange(new_x, new_y);
    }

    public bool IsEmptyMass(int x, int y)
    {
        return data[x][y] == "";
    }

    public IEnumerable<string> EachElementX(int y)
    {
        for (int x = 0; x < SizeX; x++)
        {
            yield return data[x][y];
        }
    }

    public IEnumerable<string> EachElementY(int x)
    {
        for(int y = 0; y < SizeY; y++)
        {
            yield return data[x][y];
        }
    }
}
