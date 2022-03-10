using Engine;
using Microsoft.Xna.Framework.Input;

namespace WPF_Roguelike;

public class Ployyer : Choroctr
{
    public PathToTarget Path;
    public int SightRange = 8;

    private float glowSpeed = 3f;
    private float glowTimer = 0f;
    private int glowModifer = 0;
    private bool glowingUp = false;

    public Ployyer(PathToTarget path, ToolMop map, MainGame game) : base(map, "player", game) { Path = path; }
    public override void Update(float dt)
    {
        if (IsTurn)
        {
            if (Input.MouseState.LeftButton == ButtonState.Pressed)
            {
                try
                {
                    var (mX, mY) = Map.PixelToCellPosition(Input.MouseState.X, Input.MouseState.Y);
                    //clicked player
                    if (X == mX && Y == mY)
                    {
                        //if (Input.LastMouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
                        //    player.TakeDamage(11);
                    }
                    else
                    {
                        var cell = Map.CellMap.GetCell(mX, mY);
                        if (cell.IsWalkable)
                        {
                            Path.CreatPath(X, Y, mX, mY, SightRange);
                        }

                        if (Input.WasPressed(Microsoft.Xna.Framework.Input.Keys.Space))
                        {
                            var nextCell = Path.TakeStepForward();
                            if (nextCell != null && nextCell.IsWalkable)
                            {
                                X = nextCell.X;
                                Y = nextCell.Y;
                            }
                        }
                    }
                }
                catch { }
            }
            else Path.Clear();
        }
        glowTimer += dt;
        if (glowTimer > 1f/glowSpeed)
        {
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

        Map.UpdateFOV(X, Y, SightRange - glowModifer);
    }


    public override void Die()
    {
        IsActive = false;
        (Game as MainGame).ParentWindow.GameOverLabel.Content = "Game Over";
    }
}



//if (movesLeft > 0)
//{
//    if (Input.WasPressed(Keys.W))
//    {
//        var tempY = Y - 1;
//        if (tileMap.CellMap.IsWalkable(X, tempY))
//        {
//            movesLeft--;
//            Y = tempY;
//        }
//    }
//    else if (Input.WasPressed(Keys.D))
//    {
//        var tempX = X + 1;
//        if (tileMap.CellMap.IsWalkable(tempX, Y))
//        {
//            movesLeft--;
//            X = tempX;
//        }
//    }
//    else if (Input.WasPressed(Keys.S))
//    {
//        var tempY = Y + 1;
//        if (tileMap.CellMap.IsWalkable(X, tempY))
//        {
//            movesLeft--;
//            Y = tempY;
//        }
//    }
//    else if (Input.WasPressed(Keys.A))
//    {
//        var tempX = X - 1;
//        if (tileMap.CellMap.IsWalkable(tempX, Y))
//        {
//            movesLeft--;
//            X = tempX;
//        }
//    }
//}
