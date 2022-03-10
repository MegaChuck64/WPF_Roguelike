
using Engine;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;

namespace WPF_Roguelike;

public abstract class Character : GameObject
{
    public Sprite Sprite { get; set; }
    public TileMap Map { get; set; }
    public bool IsTurn { get; set; }

    public int Health { get; set; }
    public float AttackRange { get; set; }

    protected Character(TileMap tileMap, WpfGame game, GameObject? owner = null) : base(game, owner)
    {
        Map = tileMap;
    }
    
    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            Die();
        }

    }

    public abstract void Die();

    

}