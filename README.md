# Quad Tree beautiful edges

![20230820_040953](https://github.com/kamen4/QuadTreeBeautifulEdges/assets/64691164/9841a882-6d7e-480d-884b-ceed54ed9de4)

Hello, this is my implementation and an example of working with [quad tree](https://en.wikipedia.org/wiki/Quadtree), the project was written using [monogame](https://www.monogame.net/).<br />
In the [Models](https://github.com/kamen4/QuadTreeBeautifulEdges/tree/master/QTreeStarsMonogame/Models) folder you can find the implementation of the [tree class](https://github.com/kamen4/QuadTreeBeautifulEdges/blob/master/QTreeStarsMonogame/Models/QuadTree.cs) (and auxiliary classes).

### Here some variables in [Game1.cs](https://github.com/kamen4/QuadTreeBeautifulEdges/blob/master/QTreeStarsMonogame/Game1.cs) you can play with:
- `Vector2D screenSize;`<br />
  sets the window size
- `int pointsCount;`<br />
  sets the count of points to genrate
- `double connectRadius;`<br />
  sets maximum search radius for points to connect with others
- `double speed;`<br />
  sets maximum speed of points
- `int r;` and `float scale;`<br />
  sets the radius of point (circle) texture and it's rendering scale<br />
  (actual scale in pixels: `rScaled = r * scale / 2f;`)
