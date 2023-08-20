using System;

namespace QTreeStarsMonogame.Models;

internal struct Vector2D
{
    public double X { get; set; }
    public double Y { get; set; }
    public Vector2D(double _x = 0d, double _y = 0d)
    {
        X = _x;
        Y = _y;
    }
    public Vector2D(Vector2D vector)
    {
        X = vector.X;
        Y = vector.Y;
    }
    public static Vector2D operator +(Vector2D _v1, Vector2D _v2) => new(_v1.X + _v2.X, _v1.Y + _v2.Y);
    public static Vector2D operator -(Vector2D _v1, Vector2D _v2) => new(_v1.X - _v2.X, _v1.Y - _v2.Y);
    public static Vector2D operator -(Vector2D _v1) => new(-_v1.X, -_v1.Y);
    public static Vector2D operator *(Vector2D _v1, double d) => new(_v1.X * d, _v1.Y * d);
    public static Vector2D operator /(Vector2D _v1, double d) => new(_v1.X / d, _v1.Y / d);
    public double Abs() => Math.Sqrt(X * X + Y * Y);
    public Vector2D Normalize()
    {
        double l = Abs();
        return this /= l;
    }
    public double DistanceTo(Vector2D _vector) => (_vector - this).Abs();
}
