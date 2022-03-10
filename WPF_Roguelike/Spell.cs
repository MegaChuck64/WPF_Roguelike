using Engine;
using MonoGame.Framework.WpfInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Roguelike;

public abstract class Spell : GameObject
{
    public Sprite Sprite { get; set; }
    public TileMap Map { get; set; }
    

    protected Spell(WpfGame game, GameObject? owner = null) : base(game, owner)
    {

    }
}