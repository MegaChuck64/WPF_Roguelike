
using Engine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Framework.WpfInterop;

namespace WPF_Roguelike;

public class Player : Character
{
    public int SightRange = 8;

    private float glowSpeed = 3f;
    private float glowTimer = 0f;
    private int glowModifer = 0;
    private bool glowingUp = false;

    public Player(TileMap tileMap, string spriteAtlas, float scale, WpfGame game, GameObject? owner = null) : base( tileMap, game, owner)
    {
        Scale = scale;
        var animSettings = new AnimationSettings(
            AnimationSettings.AnimStyle.Linear,
            16, 
            scale,
            new System.Collections.Generic.List<(int x, int y)> { (26, 9) });
        Sprite = new Sprite(this, spriteAtlas, animSettings);
        Components.Add(Sprite);
        IsTurn = true;
    }

    public override void Start()
    {
    }

    public override void Update(float dt)
    {
        glowTimer += dt;
        if (glowTimer > 1f / glowSpeed)
        {
            Sprite.AnimSettings.CurrentFrame(true);
            glowTimer = 0f;
            if (glowingUp)
            {
                glowModifer++;
                if (glowModifer >= 1)
                {
                    glowingUp = false;
                }
            }
            else
            {
                glowModifer--;
                if (glowModifer <= -1)
                {
                    glowingUp = true;
                }
            }
        }

        if (IsTurn)
        {
            HandleMovement();
        }
        Map?.UpdateFOV(X, Y, SightRange - glowModifer);

    }

    private void HandleMovement()
    {
        if (Input.WasPressed(Keys.W))
        {
            var tempY = Y - 1;
            if (Map.CellMap.IsWalkable(X, tempY))
            {
                Y = tempY;
            }
            IsTurn = false;
        }
        else if (Input.WasPressed(Keys.D))
        {
            var tempX = X + 1;
            if (Map.CellMap.IsWalkable(tempX, Y))
            {                
                X = tempX;
            }
            IsTurn = false;
        }
        else if (Input.WasPressed(Keys.S))
        {
            var tempY = Y + 1;
            if (Map.CellMap.IsWalkable(X, tempY))
            {
                Y = tempY;
            }
            IsTurn = false;
        }
        else if (Input.WasPressed(Keys.A))
        {
            var tempX = X - 1;
            if (Map.CellMap.IsWalkable(tempX, Y))
            {
                X = tempX;
            }
            IsTurn = false;
        }
    }
    public override void Draw(SpriteBatch sb)
    {
        (Game as TestGame).ParentWindow.EnterButton.Visibility =
            Map.DoorCell.X == X && Map.DoorCell.Y == Y ? 
            System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;        
    }
    public override void Die()
    {
        IsActive = false;
        (Game as TestGame).ParentWindow.GameOverLabel.Content = "Game Over";
    }

}