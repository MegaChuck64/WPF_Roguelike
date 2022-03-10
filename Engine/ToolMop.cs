
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using RogueSharp;
using RogueSharp.MapCreation;
using RogueSharp.Random;

namespace Engine;

public class ToolMop : GameObject
{

    public IMap CellMap { get; private set; }
    
    public Texture2D FloorSprite { get; private set; }
    public Texture2D WallSprite { get; private set; }
    public Texture2D DoorSprite { get; private set; }

    public Color FloorTintInView { get; set; } = Color.White;
    public Color WallTintInView { get; set; } = Color.White;
    public Color FloorTintOutOfView { get; set; } = Color.DarkGray;
    public Color WallTintOutOfView { get; set; } = Color.DarkGray;

    public float TileScale { get; set; }
    public int TilePixelSize { get; private set; }

    public Cell FovOrigin { get; private set; }
    public Cell DoorCell { get; private set; }
    public int FovRange { get; private set; }

    public ToolMop(
        IMapCreator mapCreator,
        string floorSprite,
        string wallSprite,
        string doorSprite,
        int tilePixelSize,
        float tileScale,
        WpfGame game) : base(game)
    {
        CellMap = mapCreator.CreateMap();
        FloorSprite = Graphics.LoadTexture(floorSprite);
        WallSprite = Graphics.LoadTexture(wallSprite);
        DoorSprite = Graphics.LoadTexture(doorSprite);
        TilePixelSize = tilePixelSize;
        TileScale = tileScale;
        DoorCell = GetRandomEmptyCell();
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

            //todo: gradient here                        

            var tint = GetTint(cell);

            var texture = cell.IsWalkable ? FloorSprite : WallSprite;
            if (cell.X == DoorCell.X && cell.Y == DoorCell.Y && cell.IsInFov)
            {
                texture = DoorSprite;
            }
            var position = CellToPixelPosition(cell);

            sb.Draw(texture, position, null, tint, 0.0f, Vector2.Zero, new Vector2(TileScale, TileScale), SpriteEffects.None, 0f);
        }
    }

    public Color GetTint(Cell cell)
    {
        var tint = cell.IsInFov ?
              (cell.IsWalkable ? FloorTintInView : WallTintInView) :
              (cell.IsWalkable ? FloorTintOutOfView : WallTintOutOfView);

        if (cell.IsInFov)
        {
            var dist = GetDistance(cell.X, cell.Y, FovOrigin.X, FovOrigin.Y);
            var gradient = 1f / (FovRange / (float)dist);
            var multiplier = 1f / (FovRange / MathHelper.Lerp(FovRange, 0f, gradient));

            tint = new Color((int)(tint.R * multiplier), (int)(tint.G * multiplier), (int)(tint.B * multiplier));

            if (tint == Color.Black) tint = FloorTintOutOfView;
        }

        return tint;
    }


    public (int X, int Y) PixelToCellPosition(int x, int y) => (
        (int)(x / (TilePixelSize * TileScale)), 
        (int)(y / (TilePixelSize * TileScale))); 

    public Vector2 CellToPixelPosition(Cell cell) =>
        new(cell.X * TilePixelSize * TileScale, cell.Y * TilePixelSize * TileScale);


    public void UpdateFOV(int x, int y, int radius)
    {
        CellMap.ComputeFov(x,y, radius, true);
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
    
    public Cell GetRandomEmptyCell((int x, int y)? exclude = null)
    {

        while (true)
        {
            int x = Random.GetInt(CellMap.Width - 1);
            int y = Random.GetInt(CellMap.Height - 1);
            if (CellMap.IsWalkable(x, y) )
            {
                if (exclude == null || exclude.Value.x != x || exclude.Value.y != y)
                return (Cell)CellMap.GetCell(x, y);
            }
        }
    }

}