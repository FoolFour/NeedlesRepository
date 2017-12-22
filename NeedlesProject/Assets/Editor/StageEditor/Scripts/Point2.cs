using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Point2
{
    public int x;
    public int y;

    public Point2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    
    public static Point2 CreateFromFloorVector(Vector2 v)
    {
        int x = Mathf.FloorToInt(v.x);
        int y = Mathf.FloorToInt(v.y);

        return new Point2(x, y);
    }

    public static Point2 operator + (Point2 a, Point2 b)
    {
        a.x += b.x;
        a.y += b.y;
        return a;
    }

    public static Point2 operator - (Point2 a, Point2 b)
    {
        a.x -= b.x;
        a.y -= b.y;
        return a;
    }

    public static Point2 operator * (Point2 a, int s)
    {
        a.x *= s;
        a.y *= s;
        return a;
    }

    public static Point2 operator * (int s, Point2 a)
    {
        a.x *= s;
        a.y *= s;
        return a;
    }

    public static explicit operator Vector2(Point2 p)
    {
        return new Vector2(p.x, p.y);
    }
}
