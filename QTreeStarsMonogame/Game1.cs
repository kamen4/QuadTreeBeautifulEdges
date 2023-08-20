using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using QTreeStarsMonogame.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using Color = Microsoft.Xna.Framework.Color;

namespace QTreeStarsMonogame;

public static class Help
{
    private static Texture2D _texture;
    private static Texture2D GetTexture(SpriteBatch spriteBatch)
    {
        if (_texture == null)
        {
            _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _texture.SetData(new[] { Color.White });
        }

        return _texture;
    }

    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color, float thickness = 1f)
    {
        var distance = Vector2.Distance(point1, point2);
        var angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
        DrawLine(spriteBatch, point1, distance, angle, color, thickness);
    }

    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color, float thickness = 1f)
    {
        var origin = new Vector2(0f, 0.5f);
        var scale = new Vector2(length, thickness);
        spriteBatch.Draw(GetTexture(spriteBatch), point, null, color, angle, origin, scale, SpriteEffects.None, 0);
    }
}
public class Game1 : Game
{
    //COMMON VARIBLES
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private QuadTree _quadTree;
    Texture2D tex;
    List<Vector2D> points;
    List<Vector2D> dirs;
    List<(Vector2, Vector2)> lines;
    
    //CONSTANTS
    //size of windows
    readonly Vector2D screenSize = new(800d, 800d);
    //points on screen
    const int pointsCount = 150;
    //maximum search radius for point to connect
    const double connectRadius = 150;
    //max axis spped
    const double speed = 2.5d;
    //texture radius
    const int r = 800;
    //point scale to draw
    const float scale = 0.005f;
    //prev two vars beacome rScaled by "rScaled = r * scale / 2f"
    float rScaled;
    
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }
    protected override void Initialize()
    {
        rScaled = r * scale / 2f;
        tex = new(GraphicsDevice, r, r);
        Color[] colorData = new Color[r * r];
        for (int i = 0; i < r; i++)
            for (int j = 0; j < r; j++)
                if ((i - r / 2d) * (i - r / 2d) + (j - r/2d) * (j - r/2d) < r * r / 4d)
                    colorData[i * r + j] = Color.White;
                else
                    colorData[i * r + j] = Color.Transparent;

        tex.SetData(colorData);

        Window.Title = "STARS";

        Random rnd = new();
        _quadTree = new(screenSize);
        for (int i = 0; i < pointsCount; i++)
            _quadTree.Insert(new(rnd.NextDouble() * screenSize.X, rnd.NextDouble() * screenSize.Y));
        points = new();
        _quadTree.QueryRange(new Models.BoundingBox(new(0, 0), screenSize), points);
        dirs = new();
        for (int i = 0; i < pointsCount; i++)
            dirs.Add(new(rnd.NextDouble() * 2d * speed - speed, rnd.NextDouble() * 2d * speed - speed));

        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = (int)screenSize.X;
        _graphics.PreferredBackBufferHeight = (int)screenSize.Y;
        _graphics.ApplyChanges();
        base.Initialize();
    }
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //generate new tree with moved points
        _quadTree = new(screenSize);
        for (int i = 0; i < pointsCount; i++)
        {
            points[i] += dirs[i];
            points[i] = new(
                points[i].X > screenSize.X ? 0 : points[i].X < 0 ? screenSize.X : points[i].X,
                points[i].Y > screenSize.Y ? 0 : points[i].Y < 0 ? screenSize.Y : points[i].Y
                );
            _quadTree.Insert(points[i]);
        }
        //generate lines
        lines = new();
        foreach (var point in points)
        {
            List<Vector2D> connected = new();
            _quadTree.QueryRange(point, connectRadius, connected);
            foreach (var c in connected)
                lines.Add((new((float)point.X, (float)point.Y), new((float)c.X, (float)c.Y)));
        }
        base.Update(gameTime);
    }
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();
        foreach (var line in lines)
        {
            var len = 1f - Vector2.Distance(line.Item1, line.Item2) / (1.05f * (float)connectRadius);
            _spriteBatch.DrawLine(line.Item1, line.Item2, new Color(len, len, len), 1.5f*rScaled*len);
        }
        foreach (Vector2D p in points ?? new List<Vector2D>())
            _spriteBatch.Draw(tex, new Vector2((float)p.X - rScaled, (float)p.Y - rScaled), null, Color.White, 0f, new(), scale, SpriteEffects.None, 1f);
        _spriteBatch.End();
        base.Draw(gameTime);
    }
}