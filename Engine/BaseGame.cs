using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;
using MonoGame.Framework.WpfInterop.Input;
using System.Collections.Generic;
using System.Windows;

namespace Engine;

public abstract class BaseGame<T> : WpfGame where T : Window
{
    private IGraphicsDeviceService graphicsDeviceManager;

    private SpriteBatch sb;
    private System.DateTime lastTime;
    private int framesRendered;
    public List<GameObject> GameObjects = new();
    
    public int FPS { get; protected set; }

    public T ParentWindow;

    public BaseGame(T window) { ParentWindow = window; }

    protected override void Initialize()
    {
        graphicsDeviceManager = new WpfGraphicsDeviceService(this);
        sb = new SpriteBatch(GraphicsDevice);


        base.Initialize();
        Input.Init(this);
        Graphics.GraphicsDevice = GraphicsDevice;
        Start();
        foreach (var go in GameObjects)
        {
            go.OnStart();
        }

    }

    protected override void Update(GameTime time)
    {

        framesRendered++;

        if ((System.DateTime.Now - lastTime).TotalSeconds >= 1)
        {
            // one second has elapsed 

            FPS = framesRendered;
            framesRendered = 0;
            lastTime = System.DateTime.Now;
        }

        Input.Update();

        var dt = (float)time.ElapsedGameTime.TotalSeconds;
        foreach (var go in GameObjects)
        {
            go.OnUpdate(dt);
        }
        Update(dt);

    }

    protected override void Draw(GameTime time)
    {
        GraphicsDevice.Clear(Color.Black);
        sb.Begin(samplerState: SamplerState.PointClamp);
        foreach (var go in GameObjects)
        {
            go.OnDraw(sb);
        }
        Draw(sb);
        sb.End();
    }

    protected abstract void Start();
    protected abstract void Update(float dt);
    protected abstract void Draw(SpriteBatch sb);
}

