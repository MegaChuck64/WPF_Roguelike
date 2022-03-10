
using Engine;
using Microsoft.Xna.Framework;

namespace WPF_Roguelike;

public abstract class Choroctr : Sproote
{
    public int Health { get; set; }
    public ToolMop Map { get; set; }

    public bool IsTurn = false;
    public Choroctr(ToolMop map, string sprite, MainGame game) : base(sprite, game) 
    {
        Map = map;
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