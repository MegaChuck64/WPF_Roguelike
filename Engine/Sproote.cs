
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Framework.WpfInterop;

namespace Engine;

public class Sproote : GameObject
{
    public float Scale { get; set; } = 1f;
    public Color Tint { get; set; } = Color.White;
    public Texture2D Texture { get; set; }

    public Sproote(string sprite, WpfGame game) : base(game)
    {
        Texture = Graphics.LoadTexture(sprite);
    }

    public override void Start()
    {
    }

    public override void Update(float dt)
    {
    }

    public override void Draw(SpriteBatch sb)
    {
        float multiplier = Scale * Texture.Width;
        sb.Draw(
            texture: Texture,
            position: new Vector2(X * multiplier, Y * multiplier),
            sourceRectangle: null,
            color: Tint,
            rotation: 0f,
            origin: Vector2.Zero,
            scale: new Vector2(Scale, Scale),
            effects: SpriteEffects.None,
            layerDepth: 0f);
    }
}