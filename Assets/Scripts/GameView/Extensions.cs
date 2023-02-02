using UnityEngine;

using Vector2DotNet = System.Numerics.Vector2;
using Vector2Unity = UnityEngine.Vector2;


public static class Vector2UnityExtensions
{
    public static Vector2DotNet ConvertToVector2DotNet(this Vector2Unity vector)
    {
        return new Vector2DotNet(vector.x, vector.y);
    }
}

public static class Vector2DotNetExtensions
{
    public static Vector2Unity ConvertToVector2Unity(this Vector2DotNet vector)
    {
        return new Vector2Unity(vector.X, vector.Y);
    }
}