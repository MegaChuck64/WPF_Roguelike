using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using RogueSharp;
using Point = Microsoft.Xna.Framework.Point;

namespace Engine;

public class TileMap : GameObject
{
    public (int x, int y) FloorLocation { get; set; }
    public (int x, int y) WallLocation { get; set; }
    public (int x, int y) DoorLocation { get; set; }

    public Color FloorTintInView { get; set; } = Color.White;
    //public Color FloorTintOutOfView { get; set; } = Color.DarkGray;
    public Color WallTintInView { get; set; } = Color.White;
    //public Color WallTintOutOfView { get; set; } = Color.DarkGray;

    public Cell FovOrigin { get; private set; }
    public Cell DoorCell { get; private set; }
    public int FovRange { get; private set; }

    public TileAtlas Atlas { get; set; }
    public IMap CellMap { get; private set; }
    public IMapCreator MapCreator { get; set; }

    public TileMap(IMapCreator mapCreator, TileAtlas atlas, WpfGame game, GameObject? owner = null) : base(game, owner)
    {
        Atlas = atlas;
        MapCreator = mapCreator;
        CellMap = MapCreator.CreateMap();
        DoorCell = GetRandomEmptyCell();

    }

    public Cell GetRandomEmptyCell((int x, int y)? exclude = null)
    {

        while (true)
        {
            int x = Random.GetInt(CellMap.Width - 1);
            int y = Random.GetInt(CellMap.Height - 1);
            if (CellMap.IsWalkable(x, y))
            {
                if (exclude == null || exclude.Value.x != x || exclude.Value.y != y)
                    return (Cell)CellMap.GetCell(x, y);
            }
        }
    }





    public override void Start()
    {
    }

    public override void Update(float dt)
    {
    }

    public override void Draw(SpriteBatch sb)
    {

        foreach (Cell cell in CellMap.GetAllCells())
        {
            if (!cell.IsExplored) continue;

            var tint = GetTint(cell);

            var atlasLoc = cell.IsWalkable ? FloorLocation : WallLocation;
            if (cell.X == DoorCell.X && cell.Y == DoorCell.Y && cell.IsInFov)
            {
                atlasLoc = DoorLocation;
            }


            Atlas.Draw(new Point(cell.X, cell.Y), atlasLoc, tint, sb);
        }
    }

    public void RecreateMap()
    {
        CellMap.Clear();
        CellMap = MapCreator.CreateMap();
        DoorCell = GetRandomEmptyCell();

    }
    public Color GetTint(Cell cell)
    {
        var tint = (cell.IsWalkable ? FloorTintInView : WallTintInView);

        if (cell.IsInFov)
        {
            var dist = GetDistance(cell.X, cell.Y, FovOrigin.X, FovOrigin.Y);
            var gradient = 1f / (FovRange / (float)dist);
            var multiplier = 1f / (FovRange / MathHelper.Lerp(FovRange, 0f, gradient));

            tint = new Color((int)(tint.R * multiplier), (int)(tint.G * multiplier), (int)(tint.B * multiplier));

            //if (tint == Color.Black) tint = FloorTintOutOfView;
        }
        else tint = new Color(4, 4, 4);

        return tint;
    }


    public void UpdateFOV(int x, int y, int radius)
    {
        CellMap.ComputeFov(x, y, radius, true);
        foreach (Cell cell in CellMap.GetAllCells())
        {
            if (CellMap.IsInFov(cell.X, cell.Y))
            {
                CellMap.SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
            }
        }

        FovOrigin = (Cell)CellMap.GetCell(x, y);
        FovRange = radius;
    }

    public static int GetDistance(int startX, int startY, int endX, int endY) =>
        (int)Vector2.Distance(new Vector2(startX, startY), new Vector2(endX, endY));


}