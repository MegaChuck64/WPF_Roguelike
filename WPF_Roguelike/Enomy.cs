
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;

namespace WPF_Roguelike;

public class Enomy : Choroctr
{
    public Enomy(ToolMop map, string sprite, MainGame game) : base(map, sprite, game)
    {
    }

    public override void Draw(SpriteBatch sb)
    {
        if (!Map.CellMap.GetCell(X, Y).IsInFov) return;

            Tint = Map.GetTint((Cell)Map.CellMap.GetCell(X, Y));
        var dist = Vector2.Distance(new Vector2(X, Y), new Vector2(Map.FovOrigin.X, Map.FovOrigin.Y));
        if (dist <= 2)
        {
            Tint = Color.White;
        }
        base.Draw(sb);
    }
    public override void Die()
    {
        IsActive = false;
    }
}