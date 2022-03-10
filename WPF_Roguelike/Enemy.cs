
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;
using RogueSharp;

namespace WPF_Roguelike;

public class Enemy : Character
{
    private PathToTarget targetPath;
    public Enemy(TileMap tileMap, string spriteAtlas, float scale, WpfGame game, GameObject? owner = null) : base(tileMap, game, owner)
    {

        Scale = scale;
        AttackRange = 2f;
        var animSettings = new AnimationSettings(
            AnimationSettings.AnimStyle.Linear,
            16,
            scale,
            new System.Collections.Generic.List<(int x, int y)> { (26, 9) });
        Sprite = new Sprite(this, spriteAtlas, animSettings);
        Components.Add(Sprite);

        targetPath = new PathToTarget(tileMap.CellMap, "floor", game)
        {
            Tint = new Color(11, 11, 11)
        };

    }

    public void SetTarget(Cell cell, int? limit = null)
    {
        targetPath.CreatPath(X, Y, cell.X, cell.Y, limit);
    }

    public override void Start()
    {
    }

    public override void Update(float dt)
    {
        if (IsTurn)
        {
            var player = (Game as TestGame).player;
            var dist = TileMap.GetDistance(player.X, player.Y, X, Y);

            if (dist <= AttackRange)
            {
                player.TakeDamage(Random.GetInt(20));
            }
            else
            {
                //if (targetPath.IsEnd())
                //{

                //}
                //else
                //{
                targetPath.CreatPath(X, Y, player.X, player.Y);
                var step = targetPath.TakeStepForward();
                if (step != null)
                {
                    if (step.X != X || step.Y != Y)
                    {

                        X = step.X;
                        Y = step.Y;
                    }
                }
                //}
            }

            IsTurn = false;

        }
    }

    public override void Die()
    {
        IsActive = false;
    }
    public override void Draw(SpriteBatch sb)
    {
        var cell = (Cell)Map.CellMap.GetCell(X, Y);
        Sprite.Tint = cell.IsInFov ? Color.Red : Color.Black; // Map.GetTint(cell);
    }

}