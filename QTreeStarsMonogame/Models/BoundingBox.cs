using System.Runtime.Intrinsics.X86;

namespace QTreeStarsMonogame.Models;

internal class BoundingBox
{
    public Vector2D Position { get; private set; }
    public Vector2D Dimension { get; private set; }
    public BoundingBox(Vector2D _position, Vector2D _dimension)
    {
        Position = _position;
        Dimension = _dimension;
    }
    public BoundingBox(double _x, double _y, double _w, double _h)
    {
        Position = new Vector2D(_x, _y);
        Dimension = new Vector2D(_w, _h);
    }

    bool PointInSeg(double _segPos, double _segLen, double _pos) => _pos < _segPos + _segLen && _pos >= _segPos;
    public bool Contains(Vector2D _point) =>
            PointInSeg(Position.X, Dimension.X, _point.X) &&
            PointInSeg(Position.Y, Dimension.Y, _point.Y);
    public bool Intersects(BoundingBox other) =>
            (PointInSeg(Position.X, Dimension.X, other.Position.X) ||
            PointInSeg(other.Position.X, other.Dimension.X, Position.X))
            &&
            (PointInSeg(Position.Y, Dimension.Y, other.Position.Y) ||
            PointInSeg(other.Position.Y, other.Dimension.Y, Position.Y));
    public bool Intersects(Vector2D _center, double _radius) =>
        Intersects(new BoundingBox(_center.X - _radius, _center.Y - _radius, 2 * _radius, 2 * _radius)) &&
        (PointInSeg(Position.X, Dimension.X, _center.X) ||
        PointInSeg(Position.Y, Dimension.Y, _center.Y) ||
        _center.DistanceTo(Position) < _radius ||
        _center.DistanceTo(Position + new Vector2D(0, Dimension.Y)) < _radius ||
        _center.DistanceTo(Position + new Vector2D(Dimension.X, 0)) < _radius ||
        _center.DistanceTo(Position + new Vector2D(Dimension.X, Dimension.Y)) < _radius);

}
