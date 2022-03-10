using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Roguelike;

public class TestGame : BaseGame<MainWindow>
{
    public Player player;
    public TileMap map;
    public List<Enemy> enemies = new();
    public int NumEnemies = 1;
    public float Scale = 2f;
    public int turnIndex = 0;
    public int roomCount = 1;

    public Character selectedCharacter;

    private float turnTimer = 0f;
    private float timeBetweenTurns = .25f;
    public TestGame(MainWindow window) : base(window)
    {
    }

    protected override void Start()
    {
        var atlas = new TileAtlas("tileAtlas", 16, Scale);
        var mapCreator = new RoomsMapCreator(22, 18, 10, 7, 3);
        map = new TileMap(mapCreator, atlas, this)
        {
            FloorLocation = (10, 17),
            WallLocation = (16, 0),
            DoorLocation = (3, 6),
            FloorTintInView = new Color(66,66,66),
            WallTintInView = Color.White,
            //FloorTintOutOfView = new Color(1,1,1),
            //WallTintOutOfView = new Color(1, 1, 1),
        };

        player = new Player(map, "tileAtlas", Scale, this)
        {
            Health = 100
        };

        var playerLoc = map.GetRandomEmptyCell();
        player.X = playerLoc.X;
        player.Y = playerLoc.Y;

        GameObjects.Add(map);
        GameObjects.Add(player);
        selectedCharacter = player;
    }

    protected override void Update(float dt)
    {
        if (Input.MouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && 
            Input.KeyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
        {
            var (mouseX, mouseY) = (Input.MouseState.X, Input.MouseState.Y);
            var (tileX, tileY) = map.Atlas.PixelToCellPosition(mouseX, mouseY);
            var chars = new List<Character>();
            chars.AddRange(enemies);
            chars.Add(player);
            var newSel = chars.FirstOrDefault(c => c.X == tileX && c.Y == tileY);
            if (newSel != null)
                selectedCharacter = newSel;
        }

        if (turnIndex > enemies.Count - 1)
        {
            turnIndex = 0;
            player.IsTurn = true;
        }
        else if (!player.IsTurn)
        {
            if (map.CellMap.GetCell(enemies[turnIndex].X, enemies[turnIndex].Y).IsInFov)
            {
                turnTimer += dt;
                if (turnTimer >= timeBetweenTurns)
                {
                    turnTimer = 0f;

                    enemies[turnIndex++].IsTurn = true;
                }
            }
            else turnIndex++;
        }
    }

    protected override void Draw(SpriteBatch sb)
    {
        ParentWindow.FPSLabel.Content = "FPS: " + FPS;
        ParentWindow.SelectedCharacterLabel.Content = selectedCharacter.GetType().Name;
        ParentWindow.AttackRangeLabel.Content = " Range: " + selectedCharacter.AttackRange;
        ParentWindow.HealthLabel.Content = "Health: " + selectedCharacter.Health;
    }

    public void ResetMap()
    {
        roomCount++;
        NumEnemies = roomCount - 1;

        map.RecreateMap();
        var playerLoc = map.GetRandomEmptyCell();
        player.X = playerLoc.X;
        player.Y = playerLoc.Y;
        player.TakeDamage(11);

        GameObjects.RemoveAll(t => t is Enemy);
        enemies.Clear();

        for (int i = 0; i < NumEnemies; i++)
        {
            var enemy = new Enemy(map, "tileAtlas", Scale, this);
            var enemyLoc = map.GetRandomEmptyCell((player.X, player.Y));
            enemy.X = enemyLoc.X;
            enemy.Y = enemyLoc.Y;
            enemies.Add(enemy);
            GameObjects.Add(enemy);

        }
    }

}