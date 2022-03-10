
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using RogueSharp;
using System.Linq;

namespace Engine;

public class PathToTarget : GameObject
{
    public IMap Map { get; set; }
    public Texture2D Texture { get; set; }
    public Color Tint { get; set; } = Color.Blue;
    public float Scale { get; set; } = 1f;

    private Path? path;
    
    private readonly PathFinder pathFinder;


    public PathToTarget(IMap _map, string sprite, WpfGame game) : base(game)
    {
        Map = _map;
        Texture = Graphics.LoadTexture(sprite);
        pathFinder = new PathFinder(Map);
    }

    public void CreatPath(int startX, int startY, int endX, int endY, int? limit = null, bool onlyIfInFOV = false)
    {
        if (onlyIfInFOV)
        {
            if (!Map.GetCell(startX, startY).IsInFov || !Map.GetCell(endX, endY).IsInFov)
                return;
        }

        path = pathFinder.ShortestPath(
            Map.GetCell(startX, startY),
            Map.GetCell(endX, endY));

        if (limit != null)
        {
            var steps = path.Steps.Take(limit.Value);
            path = pathFinder.ShortestPath(
                Map.GetCell(startX, startY),
                steps.Last());
        }
    }

    public bool IsEnd() => path?.End == path?.CurrentStep;

    public Cell TakeStepForward()
    {
        if (path == null || path.End == path.CurrentStep) return null;
        else return (Cell)path.StepForward();
    }

    public Cell TakeStepBackward()
    {
        if (path == null) return null;
        else return (Cell)path.StepBackward();
    }

    public void Clear()
    {
        path = null;
    }
    public override void Start() { }

    public override void Update(float dt) { }

    public override void Draw(SpriteBatch sb)
    {
        if (path != null)
        {
            foreach (var cell in path.Steps)
            {
                var position = new Vector2(cell.X * Texture.Width * Scale, cell.Y * Texture.Width * Scale);

                if (cell.IsInFov)
                    sb.Draw(Texture, position, null, Tint * .2f, 0.0f, Vector2.Zero, new Vector2(Scale, Scale), SpriteEffects.None, 0f);

                //sb.Draw(Texture, new Vector2(cell.X * Texture.Width, cell.Y * Texture.Height),
                //        null, Tint * .2f, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                

            }
        }
    }
}
