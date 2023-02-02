using System;
using System.Numerics;
using System.Collections.Generic;


public static class Utilities 
{
    private static Dictionary<int, Matrix4x4> _rotationMatrices = new Dictionary<int, Matrix4x4>();


    static Utilities()
    {
        for(int i = 0; i < 360; i++)
        {
            _rotationMatrices.Add(i, Matrix4x4.CreateRotationZ((i * MathF.PI) / 180f));
        }
    }
    
    public static Matrix4x4 GetRotationMatrix(int angleInDegrees)
    {
        angleInDegrees = Math.Clamp(angleInDegrees, 0, 359);

        return _rotationMatrices[angleInDegrees];
    }

    public static Vector2 RepeatVector2RangingFrom0To1(Vector2 vector)
    {
        float x = vector.X;
        float y = vector.Y;

        x = x - MathF.Truncate(x);
        y = y - MathF.Truncate(y);

        if (x < 0f) { x = 1 + x; }
        if (y < 0f) { y = 1 + y; }

        return new Vector2(x, y);
    }

    public static Vector2 RepeatVectorWithinScreenBorders(Vector2 vector, float screenHeightToLengthRatio)
    {
        float x = vector.X;
        float y = vector.Y;

        x = x - MathF.Truncate(x);
        y = y - MathF.Truncate(y);

        if (x < 0f) { x = 1 + x; }
        if (y < 0f) { y = screenHeightToLengthRatio + y; }
        else if(y > screenHeightToLengthRatio) { y = screenHeightToLengthRatio - y; }

        return new Vector2(x, y);
    }

    public static int RepeatAngleWithinRangingFrom0To360(float angle)
    {
        angle = angle % 360f;

        if (angle < 0f)
        {
            angle += 360f;
        }

        return (int)angle;
    }

    public static int RepeatInt(int number, int length)
    {
        if(number < 0) { number = length - number; }
        else if(number > length) { number = number - length; }

        return number;
    }
}