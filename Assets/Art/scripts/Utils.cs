using UnityEngine;
using Unity.Mathematics;

// 定义四个方向
public enum Dir
{
    None,       // 0
    Up,         // 1
    Down,       // 2
    Left,       // 3
    Right,      // 4
}

public class Utils
{
    // 方向向量，转化为四个方向
    public static Dir Vec2Dir(Vector2 v)
    {
        Dir d = Dir.None;
        if (math.abs(v.x) >= math.abs(v.y))
        {
            d = v.x < 0 ? Dir.Left : Dir.Right;
            return d;
        }
        else if (math.abs(v.y) >= math.abs(v.x))
        {
            d = v.y > 0 ? Dir.Up : Dir.Down;
            return d;
        }
        return d;
    }
}
