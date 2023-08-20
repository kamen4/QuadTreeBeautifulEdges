using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System;

namespace QTreeStarsMonogame.Models;

internal class QuadTree
{
    public int NODE_CAPASITY = 4;
    private readonly BoundingBox boundary;
    private List<Vector2D> Points { get; set; }
    QuadTree[]? subtrees; //NW NE SW SE
    public QuadTree(Vector2D _size)
    {
        boundary = new(0, 0, _size.X, _size.Y);
        Points = new();
        subtrees = null;
    }
    private QuadTree(Vector2D _position, Vector2D _dimension)
    {
        boundary = new(_position, _dimension);
        Points = new();
        subtrees = null;
    }
    public bool Insert(Vector2D _point)
    {
        if (!boundary.Contains(_point))
            return false;
        if (subtrees is not null)
        {
            foreach (var s in subtrees)
                s.Insert(_point);
            return true;
        }
        if (Points.Count < NODE_CAPASITY)
        {
            Points.Add(_point);
            return true;
        }
        Subdivide();
        foreach (var p in Points)
            Insert(p);
        Points.Clear();
        return Insert(_point);
    }
    public bool Remove(Vector2D _point)
    {
        if (!boundary.Contains(_point))
            return false;
        if (Points.Remove(_point))
            return true;
        if (subtrees is null)
            return false;
        bool res = false;
        foreach (var s in subtrees)
            res |= s.Remove(_point);
        return res;
    }
    private void Subdivide()
    {
        if (subtrees is not null)
            return;
        subtrees ??= new QuadTree[4];
        var halfDim = boundary.Dimension / 2d;
        subtrees[0] = new QuadTree(boundary.Position, halfDim);
        subtrees[1] = new QuadTree(boundary.Position + new Vector2D(halfDim.X, 0d), halfDim);
        subtrees[2] = new QuadTree(boundary.Position + new Vector2D(0d, halfDim.Y), halfDim);
        subtrees[3] = new QuadTree(boundary.Position + new Vector2D(halfDim.X, halfDim.Y), halfDim);
    }
    public void QueryRange(BoundingBox _range, List<Vector2D> _answ)
    {
        if (!boundary.Intersects(_range))
            return;
        _answ ??= new List<Vector2D>();
        if (subtrees is not null)
            foreach (var s in subtrees)
                s.QueryRange(_range, _answ);
        else
            _answ.AddRange(Points.Where(_range.Contains));
    }
    public void QueryRange(Vector2D _center, double _raduis, List<Vector2D> _answ)
    {
        if (!boundary.Intersects(_center, _raduis))
            return;
        _answ ??= new List<Vector2D>();
        if (subtrees is not null)
            foreach (var s in subtrees)
                s.QueryRange(_center, _raduis, _answ);
        else
            _answ.AddRange(Points.Where(p => _center.DistanceTo(p) <= _raduis));
    }
}
