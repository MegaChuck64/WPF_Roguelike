
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueSharp;
using System.Collections.Generic;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Engine;

public class Sprite : IComponent
{
    public GameObject Owner { get; set; }
    public bool IsActive { get; set; } = true;
    public Texture2D TileMap { get; set; }
    public Color Tint { get; set; } = Color.White;
    public AnimationSettings AnimSettings { get; set; }

    public Rectangle DestRect
    {
        get
        {
            var (px, py) = AnimSettings.CellToPixelPosition(Owner.X, Owner.Y);
            return new Rectangle((int)px, (int)py, (int)(AnimSettings.pixelSize * Owner.Scale), (int)(AnimSettings.pixelSize * Owner.Scale));
        }
    }

    public Sprite(GameObject owner, string tileMap, AnimationSettings animSettings) 
    { 
        Owner = owner;
        TileMap = Graphics.LoadTexture(tileMap);
        AnimSettings = animSettings;
    }
    public void Start()
    {
     
    }

    public void Update(float dt)
    {

    }

    public void Draw(SpriteBatch sb)
    {
        if (!IsActive) return;

        sb.Draw(TileMap, DestRect, AnimSettings.CurrentFrame(), Tint, 0f, Vector2.Zero, SpriteEffects.None, 0.0f);
    }


}

public class AnimationSettings
{
    public enum AnimStyle
    {
        Linear,
        PingPong
    }
    public int pixelSize;
    private readonly AnimStyle style;
    private readonly float scale;
    private readonly List<(int x, int y)> frameIndex;
    private int currentFrame = 0;
    private bool ppDirection = false;
    public AnimationSettings(AnimStyle animStyle, int tilePixelSize, float drawScale, List<(int x, int y)> frames)
    {
        pixelSize = tilePixelSize;
        frameIndex = frames;
        style = animStyle;
        scale = drawScale;
    }

    public (int X, int Y) PixelToCellPosition(int x, int y) => (
    (int)(x / (pixelSize * scale)),
    (int)(y / (pixelSize * scale)));

    public Vector2 CellToPixelPosition(int x, int y) =>
        new(x * pixelSize * scale, y * pixelSize * scale);

    public Rectangle CurrentFrame(bool incrementFrame = false)
    {
        var (fx, fy) = frameIndex[currentFrame];

        if (incrementFrame)
        {
            if (ppDirection) currentFrame--;
            else currentFrame++;

            if (currentFrame > frameIndex.Count - 1)
            {
                switch (style)
                {
                    case AnimStyle.Linear:
                        currentFrame = 0;
                        break;
                    case AnimStyle.PingPong:
                        ppDirection = true;
                        currentFrame = frameIndex.Count - 2;
                        break;
                }
            }
            else if (currentFrame < 0)
            {
                currentFrame = 1;
                if (style == AnimStyle.PingPong) ppDirection = false;
            }

        }
        return new Rectangle(fx * pixelSize, fy * pixelSize, pixelSize, pixelSize);
    }


}