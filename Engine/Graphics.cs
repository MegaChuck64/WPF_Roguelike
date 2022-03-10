
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace Engine;

public static class Graphics
{
    public static GraphicsDevice GraphicsDevice { get; set; }

    public static Texture2D LoadTexture(string name)
    {
        using var floorFile = File.OpenRead("../../../Content/" + name + ".png");
        return Texture2D.FromStream(GraphicsDevice, floorFile);
    }
}
